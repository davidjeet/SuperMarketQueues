using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messages
{

    public interface ITestInterface
    {
        string Text { get; set; }
        string Description { get; set; }
        DateTime CreatedOn { get; set; }
    }

    public class MyMessage : ITestInterface
    {
        public string Text { get; set; }
        public string Description { get; set; }
        public DateTime CreatedOn { get; set; }
    }

}
