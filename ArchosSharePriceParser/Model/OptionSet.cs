using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArchosSharePriceParser.Model
{
    public class OptionSet
    {
        public bool Average { get; set; }

        public bool Evolution { get; set; }

        public int Interval { get; set; }

        public string InputFilePath { get; set; }
    }
}
