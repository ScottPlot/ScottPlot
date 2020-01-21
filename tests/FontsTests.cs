using NUnit.Framework;
using ScottPlot.Config;

namespace ScottPlotTests
{
    [TestFixture]
    public class FontsTests
    {
        [Test]
        public void GetDefaultFontName_SegoeUIFontPresent_ReturnSegoeFont()
        {
            string[] presentFonts = new string[] { "newFont ar", "SOME font", "AnotherOne", "Arial", "Segoe UI", "Comic Sans", "onemorefont" };
            string expected = "Segoe UI";
            string result = Fonts.GetDefaultFontName(presentFonts);
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void GetDefaultFontName_SegoeUIFontNotPresent_ReturnDifferentFont()
        {
            string[] presentFonts = new string[] { "newFont ar", "SOME font", "AnotherOne", "Arial", "Comic Sans", "onemorefont" };
            string expected = "Segoe UI";
            string result = Fonts.GetDefaultFontName(presentFonts);
            Assert.AreNotEqual(expected, result);
        }

        [Test]
        public void GetDefaultFontName_NoSegoeUIButSegoeUIBlackPresent_ReturnSegoeUIBlack()
        {
            string[] presentFonts = new string[] { "newFont ar", "SOME font", "AnotherOne", "Segoe UI Black", "Arial", "Comic Sans", "onemorefont" };
            string expected = "Segoe UI Black";
            string result = Fonts.GetDefaultFontName(presentFonts);
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void GetDefaultFontName_NoSegoeUIButSomeSansPresent_ReturnSansFont()
        {
            string[] presentFonts = new string[] { "newFont ar", "SOME font", "AnotherOne", "Arial", "Comic Sans", "onemorefont" };
            string expected = "Comic Sans";
            string result = Fonts.GetDefaultFontName(presentFonts);
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void GetDefaultFontName_NoSegoeUIButSomeSansandDejavuPresent_ReturnDejavuFont()
        {
            string[] presentFonts = new string[] { "newFont ar", "SOME font", "AnotherOne", "Arial", "Comic Sans", "onemorefont", "Fictional Dejavu" };
            string expected = "Fictional Dejavu";
            string result = Fonts.GetDefaultFontName(presentFonts);
            Assert.AreEqual(expected, result);
        }
    }
}
