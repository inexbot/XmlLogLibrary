using System;
using System.Collections.Generic;
using System.Text;

namespace XmlLogLibrary
{
    public class LogBase
    {
        public object data { get; set; }
        public string type { get; set; }
        public string kind { get; set; }
        public string datetime { get; set; }
    }
}