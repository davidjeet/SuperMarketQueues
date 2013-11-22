using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication2
{
    public class Processor
    {
        private Queue<Customer> queue; 

        public int Length
        {
            get
            {
                return queue.Count;
            }
        }

        public Processor()
        {
               queue = new Queue<Customer>();  
        }
    }
}
