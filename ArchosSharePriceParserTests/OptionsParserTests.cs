using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArchosSharePriceParser;
using ArchosSharePriceParser.Model;

namespace ArchosSharePriceParserTests
{
    [TestClass]
    public class OptionsParserTests
    {
        private const string inputFilePathTest = "testFilePath";

        [TestMethod]
        public void GetOptions_ValidAverage_ReturnParsedOptions()
        {
            // arrange
            string[] args;
            OptionSet expected;
            OptionSet actual;

            args = new string[]
            {
                "-a", inputFilePathTest
            };
            expected = new OptionSet()
            {
                Average = true,
                Evolution = false,
                InputFilePath = inputFilePathTest
            };

            // act
            actual = OptionsParser.GetOptions(args);

            // assert
            Assert.IsTrue(expected.Average == actual.Average &&
                expected.Evolution == actual.Evolution &&
                expected.InputFilePath.Equals(actual.InputFilePath) &&
                expected.Interval == actual.Interval);
        }
    }
}
