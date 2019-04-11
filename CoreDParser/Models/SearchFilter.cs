using System;
using System.Collections.Generic;
using System.Text;

namespace CoreDParser.Models
{
    public class SearchFilter
    {
        public string groupOp { get; set; }

        public IList<Filter> rules { get; set; }
    }
}
