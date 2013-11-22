using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyNetQ;

namespace ConsoleApplication1
{
    using System.Threading;

    using Messages;

    class Program
    {
        private static IBus bus;

        static void Main(string[] args)
        {
            bus = RabbitHutch.CreateBus(@"host=localhost;username=cadev;password=CustomerAccount!");
            for(int i=0; i< 10;i++) {
                PublishStuff(i);
                Thread.Sleep(2000);
            }
            bus.Dispose();
            Console.ReadKey();
        }


        private static void PublishStuff(int j)
        {
            var message = new MyMessage { Text = j.ToString(), 
                                          Description = Guid.NewGuid().ToString(), CreatedOn = DateTime.Now };
            Console.WriteLine("====================================================");
            Console.WriteLine("Description:  {0}", message.Description);
            Console.WriteLine("Publishing a message at: " + message.CreatedOn.TimeOfDay.TotalSeconds);
            Console.WriteLine("****************************************************");

            bus.Publish<ITestInterface>(message);
        }
    }
}
