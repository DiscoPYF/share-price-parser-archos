using ArchosSharePriceParser.Model;
using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ArchosSharePriceParser
{


    class Program
    {
        static void Main(string[] args)
        {
            OptionSet options;

            if (args.Length < 2)
                PrintUsage();
            else
            {
                options = OptionsParser.GetOptions(args);
                if (options == null)
                    PrintUsage();
                else
                {
                    using (TextFieldParser parser = new TextFieldParser(options.InputFilePath))
                    {
                        parser.CommentTokens = new string[] { "#" };
                        parser.SetDelimiters(new string[] { "," });
                        parser.HasFieldsEnclosedInQuotes = false;
                        parser.TextFieldType = FieldType.Delimited;

                        // Skipping header line
                        Console.WriteLine(parser.ReadLine());

                        if (options.Average)
                            ParseAverage(parser);
                        else if (options.Evolution)
                            ParseEvolution(parser, options.Interval);
                    }
                }
            }
            Console.ReadKey();
        }

        public static void PrintUsage()
        {
            Console.WriteLine("Usage: ArchosSharePriceParser.exe [-a | --average] [-e | --evolution interval] INPUT_CSV_FILE");
        }

        public static void ParseAverage(TextFieldParser parser)
        {
            string[] fields;
            DateTime lastDateTime;
            DateTime dateTime;
            AverageEntry averageEntry;
            int i;
            CultureInfo info;

            lastDateTime = default(DateTime);
            averageEntry = new AverageEntry();
            i = 1;
            info = new CultureInfo("en");
            while (!parser.EndOfData)
            {
                fields = parser.ReadFields();

                averageEntry.OpenPrice += float.Parse(fields[1], info);
                averageEntry.ClosePrice += float.Parse(fields[2], info);
                averageEntry.HighPrice += float.Parse(fields[3], info);
                averageEntry.LowPrice += float.Parse(fields[4], info);
                averageEntry.Volume += int.Parse(fields[5], info);
                dateTime = DateTime.Parse(fields[0]);

                // Counter has been reset ? (To prevent writing on the very first cycle)
                if (i == 1)
                    // Yes
                    lastDateTime = dateTime;

                if (lastDateTime.Month != dateTime.Month ||
                    lastDateTime.Year != dateTime.Year)
                {
                    averageEntry.Month = dateTime.Month;
                    averageEntry.Year = dateTime.Year;
                    Console.WriteLine(string.Format(info, "{0}-{1},{2},{3},{4},{5},{6}", averageEntry.Month, averageEntry.Year,
                                                                               averageEntry.OpenPrice / i, averageEntry.ClosePrice / i,
                                                                               averageEntry.HighPrice / i, averageEntry.LowPrice / i,
                                                                               averageEntry.Volume / i));
                    averageEntry.OpenPrice = averageEntry.ClosePrice = averageEntry.HighPrice = averageEntry.LowPrice = 0.0F;
                    averageEntry.Volume = 0;
                    i = 0;
                }
                i++;
            }
        }

        public static void ParseEvolution(TextFieldParser parser, int interval)
        {
            int i;
            string line;

            i = 1;
            while (!parser.EndOfData)
            {
                line = parser.ReadLine();
                if (i == 1)
                    Console.WriteLine(line);
                else if (i >= interval)
                    i = 0;
                i++;
            }
        }
    }
}
