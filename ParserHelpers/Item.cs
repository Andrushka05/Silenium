using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserHelpers
{
    public class Item
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public List<string> Photos { get; set; }
        public string Url { get; set; }
        public string Price { get; set; }
        public string Article { get; set; }
        public string Catalog { get; set; }
    }
}
