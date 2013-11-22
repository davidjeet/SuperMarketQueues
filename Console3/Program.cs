using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Console3
{
    using System.Runtime.InteropServices;
    using System.Threading;

    using EasyNetQ;

    using Messages;

    class Program
    {
        private static IBus bus1;
        private static readonly string SUBSCRIPTIONID = "st7";

        private static bool publishMode = true;
        private static bool subscriberMode = true;
        private static bool quitMode = true;

        static void Main(string[] args)
        {
            bus1 = RabbitHutch.CreateBus(@"host=localhost;username=cadev;password=CustomerAccount!");

            var taskList = new List<Task>();
            for (var counter=0; counter<3; counter++)
            {
                var task = Task.Factory.StartNew(() => FireUpSubscriberThread(counter));
                taskList.Add(task);
                Thread.Sleep(1000);
            }

            var publishTask = Task.Factory.StartNew(() => FireUpPublisherThread());
            taskList.Add(publishTask);
            
           // SetUpSubcribers();
           // SetUpPublisher();

            do
            {
                Thread.Sleep(1000);
                var key = Console.ReadKey();
                switch (char.ToLower(key.KeyChar))
                {
                    case 'p':
                        publishMode = false;
                        Console.WriteLine("============== > Publishing stopped < =========================");
                        break;
                    case 's':
                        subscriberMode = false;
                        Console.WriteLine("============== > Subscribing stopped < =========================");
                        break;
                    case 'q':
                        publishMode = false;
                        subscriberMode = false;
                        quitMode = false;
                        Console.WriteLine("============== > Quitting application < =========================");
                        break;
                }

            } while (quitMode);
            
            bus1.Dispose();
            Console.WriteLine("Shutting down services...");
            Task.WaitAll(taskList.ToArray());            
            Console.WriteLine("Press any key to exit program");
            Console.ReadKey();
        }

        private static void FireUpSubscriberThread(int j)
        {
            Console.WriteLine("**** Starting thread {0} *****", j);

            bus1.Subscribe<ITestInterface>(SUBSCRIPTIONID, handler, x => x.WithTopic("Q." + j));

            do
            {
                Thread.Sleep(5000);
                Console.WriteLine("******** Thread {0} reporting in at {1} *********", j, DateTime.Now.ToShortTimeString());
            }
            while (subscriberMode);
        }

        private static void FireUpPublisherThread()
        {
            int i = 1;
            do
            {
                var message = new MyMessage
                              {
                                  Text = i.ToString(),
                                  Description = Guid.NewGuid().ToString(),
                                  CreatedOn = DateTime.Now
                              };
                string val = string.Format("Q.{0}", (i%3));
                bus1.Publish<ITestInterface>(message, val);
                Thread.Sleep(2000);
                i++;
            }
            while (publishMode);
        }


        private static void SetUpPublisher()
        {
            int i = 0;
            var message = new MyMessage
            {
                Text = i.ToString(),
                Description = Guid.NewGuid().ToString(),
                CreatedOn = DateTime.Now
            };
            bus1.Publish(message as ITestInterface);
            message.Text = (i++).ToString();
            bus1.Publish(message, "Q.0");
            Thread.Sleep(2000);
            message.Text = (i++).ToString();
            bus1.Publish(message, "Q.1");
            Thread.Sleep(2000);
            message.Text = (i++).ToString();
            bus1.Publish<ITestInterface>(message as ITestInterface, "Q.2");
            Thread.Sleep(2000);

            Console.WriteLine("Publishers set up and and waiting...");
            Thread.Sleep(5000);
        }


        private static void SetUpSubcribers()
        {
            bus1.Subscribe<ITestInterface>(SUBSCRIPTIONID, handler1, x => x.WithTopic("Q.1*"));
            bus1.Subscribe<ITestInterface>(SUBSCRIPTIONID, handler2, x => x.WithTopic("Q.2*"));
            bus1.Subscribe<ITestInterface>(SUBSCRIPTIONID, handler3, x => x.WithTopic("Q.3*"));

            Console.WriteLine("Subscribers set up and and waiting...");
            Thread.Sleep(5000);
        }

        static void handler(ITestInterface msg)
        {
            Console.WriteLine("==========================================================================");
            Console.WriteLine("Results: {0} | {1} | {2}", msg.Text, msg.Description, msg.CreatedOn);
            Console.WriteLine("==========================================================================");
            Thread.Sleep(5000);
        }

        static void handler1(ITestInterface msg)
        {
            Console.WriteLine("=====================================");
            Console.WriteLine("Text(1):" + msg.Text);
            Console.WriteLine("=====================================");

        }

        static void handler2(ITestInterface msg)
        {
            Console.WriteLine("=====================================");
            Console.WriteLine("Description(2):" + msg.Text + msg.Description);
            Console.WriteLine("=====================================");

        }

        static void handler3(ITestInterface msg)
        {
            Console.WriteLine("=====================================");
            Console.WriteLine("CreatedOn(3):" + msg.Text + msg.CreatedOn);
            Console.WriteLine("=====================================");

        }



    }
}
