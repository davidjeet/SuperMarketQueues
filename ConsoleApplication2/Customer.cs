using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleApplication2
{
    using Messages;


    //public class QueueLength
    //{
    //    private readonly Object _Lock = new Object();
    //    private int state;

    //    public int GetState()
    //    {
    //        lock (_Lock)
    //        {
    //            return this.state;
    //        }
    //    }

    //    public void UpdateState()
    //    {
    //        lock (_Lock)
    //        {
    //            this.state++;
    //        }
    //    }
    //}

    public class Customer
    {
        public void HandleMessage(MyMessage message)
        {
            
        }
    }
}
