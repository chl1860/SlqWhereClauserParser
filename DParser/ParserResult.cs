using Core.Models;

namespace DParser
{
    public class ParserResult
    {
        public SearchFilter AndFilters { get; set; }
        public SearchFilter OrFilters { get; set; }
    }
}
