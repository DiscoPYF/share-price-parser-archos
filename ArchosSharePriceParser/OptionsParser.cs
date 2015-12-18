using ArchosSharePriceParser.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArchosSharePriceParser
{
    public static class OptionsParser
    {
        private enum OperationResult
        {
            Success,
            Failure
        }

        private class TransitObject
        {
            public int Index { get; set; }

            public string[] Args { get; set; }

            public OptionSet OptionSet { get; set; }
        }

        #region Property

        private readonly static Dictionary<string, Func<TransitObject, OperationResult>> knownOptions;

        #endregion

        #region Constructor

        static OptionsParser()
        {
            knownOptions = new Dictionary<string, Func<TransitObject, OperationResult>>()
            {
                { "-a", HandleAverage },
                { "--average", HandleAverage },
                { "-e", HandleEvolution },
                { "--evolution", HandleEvolution }
            };
        }

        #endregion

        #region Method

        public static OptionSet GetOptions(string[] args)
        {
            OptionSet result;
            TransitObject transit;
            string stringValue;

            result = null;
            transit = new TransitObject();
            transit.Args = args;
            transit.OptionSet = new OptionSet();
            for (transit.Index = 0; transit.Index < args.Length; transit.Index++)
            {
                // Is option found ?
                if (knownOptions.ContainsKey(args[transit.Index]))
                { // Yes

                    // Is option well formatted ?
                    if (knownOptions[args[transit.Index]].Invoke(transit) == OperationResult.Failure)
                        // No
                        return (result);
                }
                else // No
                    break;
            }

            // Finding the input file path
            stringValue = args[transit.Index];
            if (!string.IsNullOrWhiteSpace(stringValue))
            {
                transit.OptionSet.InputFilePath = stringValue;
                result = transit.OptionSet;
            }
            return (result);
        }

        private static OperationResult HandleAverage(TransitObject transit)
        {
            transit.OptionSet.Average = true;
            return (OperationResult.Success);
        }

        private static OperationResult HandleEvolution(TransitObject transit)
        {
            OperationResult result;
            string stringValue;
            int interval;

            result = OperationResult.Failure;
            transit.OptionSet.Evolution = true;
            stringValue = transit.Args[transit.Index + 1];
            if (!string.IsNullOrWhiteSpace(stringValue))
            {
                if (int.TryParse(stringValue, out interval))
                {
                    transit.OptionSet.Interval = interval;
                    transit.Index++;
                    result = OperationResult.Success;
                }
            }
            return (result);
        }

        #endregion
    }
}
