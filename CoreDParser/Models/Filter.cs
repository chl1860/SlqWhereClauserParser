using System;
using System.Collections.Generic;
using System.Text;

namespace CoreDParser.Models
{
    public class Filter
    {
        public string field { get; set; }

        public string op { get; set; }

        public string data { get; set; }
    }
}
