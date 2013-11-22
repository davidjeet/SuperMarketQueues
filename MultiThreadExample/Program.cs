using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace MultiThreadExample
{
    using Messages;

    class Program
    {
        public static void Write1(int i)
        {
            Console.WriteLine(i);
            Thread.Sleep(100);
        }

        public static string Write2(MyMessage msg, int i)
        {
            Thread.Sleep(new Random().Next(1, 500));
            Console.WriteLine("Source: {0} Target {1}", msg.Text, i);
            Console.WriteLine("==================================================");
            if (i % 2 == 0) msg.CreatedOn = DateTime.Now.AddYears(10);
            return msg.Text;
        }

        static void Main(string[] args)
        {
            var taskList = new List<Task<string>>();
            var list = new List<MyMessage>();
            for (var j = 0; j < 5; j++)
            {
                list.Add(new MyMessage { Text = j.ToString(), CreatedOn = DateTime.Now, Description = "xxx" });
            }

            for (int index = 0; index < 5; index++)
            {
                Thread.Sleep(100);
                int val = index;
                Task<string> runningTask = Task.Factory.StartNew(() => Write2(list[val], val));
                var e= runningTask.Result;
                taskList.Add(runningTask);  

            }

            Task.WaitAll(taskList.ToArray());
            //Thread.Sleep(2000);
            for (var k = 0; k < 5; k++)
            {
                Console.WriteLine("{0} : {1}", k, list[k].CreatedOn.Year);
            }
            Console.WriteLine("*******************************************************************");

            for (var k = 0; k < 5; k++)
            {
                Console.WriteLine("TaskId:{0} - Status:{1}", taskList[k].Id, taskList[k].Status);
                
            }


            Console.ReadKey();
        }
    }
}
