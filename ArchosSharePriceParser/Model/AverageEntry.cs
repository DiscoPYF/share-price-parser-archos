using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArchosSharePriceParser.Model
{
    public class AverageEntry
    {
        public int Month { get; set; }

        public int Year { get; set; }

        public float OpenPrice { get; set; }

        public float ClosePrice { get; set; }

        public float HighPrice { get; set; }

        public float LowPrice { get; set; }

        public int Volume { get; set; }
    }
}
