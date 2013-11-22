using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyNetQ;

namespace ConsoleApplication2
{
    using System.Threading;

    using Messages;

    class Program
    {
        private static IBus bus;

        static void Main(string[] args)
        {
            using (bus = RabbitHutch.CreateBus(@"host=localhost;username=cadev;password=CustomerAccount!"))
            {
                SubscribeToStuff();
                do
                {                    
                    Thread.Sleep(1000);
                }
                while (true);
            }           
        }

        private static void SubscribeToStuff()
        {
            bus.Subscribe<ITestInterface>("st7", PrintToScreen);
        }

        private static void PrintToScreen(ITestInterface msg)
        {
            Console.WriteLine("************************************");
            Console.WriteLine("Id:  {0}", msg.Text);
            Console.WriteLine("Description:  {0}", msg.Description);
            Console.WriteLine("Created On:  {0}", msg.CreatedOn);
            Console.WriteLine("************************************");
        }
    }
}
