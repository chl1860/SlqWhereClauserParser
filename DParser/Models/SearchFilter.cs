using System.Collections.Generic;

namespace DParser.Models
{
    public class SearchFilter
    {
        public string groupOp { get; set; }

        public IList<Filter> rules { get; set; }
    }
}
