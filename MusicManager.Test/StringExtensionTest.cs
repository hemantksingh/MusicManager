using NUnit.Framework;

namespace MusicManager.Test
{
    [TestFixture]
    class StringExtensionTest
    {
        [Test]
        public void CleanString_WithPeriod_PeriodRemoved()
        {
            string stringToClean = " string With White spaces... ";
            string expectedString = "string With White spaces";

            Assert.AreEqual(expectedString, stringToClean.Remove(".".ToCharArray()),
                "Cleaned string should not be trimmed");
        }

        [Test]
        public void CleanString_WithDash_DashRemoved()
        {
            string stringToClean = "- www.Songs.pk";
            string expectedString = "www.Songs.pk";

            Assert.AreEqual(expectedString, stringToClean.Remove("-".ToCharArray()),
                "The cleaned string should not contain dashes '-'.");
        }

        [Test]
        public void CleanString_WithSquareBrackets_SquareBracketsRemoved()
        {
            string stringToClean = "[Songs.PK] De Dana Dan - 01 - Rishte Naate";
            string expectedString = "Songs.PK De Dana Dan - 01 - Rishte Naate";

            Assert.AreEqual(expectedString, stringToClean.Remove("[]".ToCharArray()),
                "The cleaned string should not have brackets '[]'.");
        }

        [Test]
        public void CleanString_MultipleCharacters_AllChractersRemoved()
        {
            string stringToClean = "[Songs.PK] De Dana Dan - 01 - Rishte Naate";
            string expectedString = "SongsPK De Dana Dan  01  Rishte Naate";

            Assert.AreEqual(expectedString, stringToClean.Remove("[].-".ToCharArray()),
                "The cleaned string should not have any of these characters '[].-'.");
        }
        
        [Test]
        public void CleanString_WithOnlyWhitespaces_ReturnsEmptyString()
        {
            string stringToClean = "  ";
            string expectedString = string.Empty;

            Assert.AreEqual(expectedString, stringToClean.Remove(" ".ToCharArray()),
                "Cleaned string should be empty.");
        }

        [Test]
        public void CleanString_WithPeriodDontTrim_IsUntrimmedPeriodRemoved()
        {
            string stringToClean = " string With White spaces... ";
            string expectedString = " string With White spaces ";

            Assert.AreEqual(expectedString, stringToClean.Remove(".".ToCharArray(), shouldTrim: false),
                "Cleaned string should not be trimmed");
        }

        [Test]
        public void ReplaceString_WithoutMatch_NothingReplaced()
        {
            string inputString = "This is a string";
            string expectedString = "This is a string";

            string actualString = inputString.Replace("www.Songs.PK", string.Empty);
            
            Assert.AreEqual(expectedString, actualString, 
                "If there is no match found while replacing a string, nothing should be replaced.");
        }
    }
}
