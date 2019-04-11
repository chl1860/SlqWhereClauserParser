using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DParser.Models
{
    public class Filter
    {
        public string field { get; set; }

        public string op { get; set; }

        public string data { get; set; }
    }

}
