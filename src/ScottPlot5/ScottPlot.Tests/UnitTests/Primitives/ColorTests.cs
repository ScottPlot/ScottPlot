using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScottPlot.Tests.UnitTests.Primitives
{
    internal class ColorTests
    {
        [Test]
        public void Test_Color_Constructor()
        {
            Color color = new(13, 17, 23);
            Assert.That(color.Red, Is.EqualTo(13));
            Assert.That(color.Green, Is.EqualTo(17));
            Assert.That(color.Blue, Is.EqualTo(23));
            Assert.That(color.Alpha, Is.EqualTo(255));
        }

        [Test]
        public void Test_Color_ConstructorWithAlpha()
        {
            Color color = new(13, 17, 23, 27);
            Assert.That(color.Red, Is.EqualTo(13));
            Assert.That(color.Green, Is.EqualTo(17));
            Assert.That(color.Blue, Is.EqualTo(23));
            Assert.That(color.Alpha, Is.EqualTo(27));
        }

        [Test]
        public void Test_Color_ToARGB()
        {
            // MediumVioletRed 0xFFC71585 is RGB (199, 21, 133)
            Color color = new(199, 21, 133);
            Assert.That(color.ARGB, Is.EqualTo(0xFFC71585));
        }

        [Test]
        public void Test_Color_FromARGB()
        {
            // MediumVioletRed 0xFFC71585 is RGB (199, 21, 133)
            Color color = Color.FromARGB(0xFFC71585);
            Assert.That(color.Red, Is.EqualTo(199));
            Assert.That(color.Green, Is.EqualTo(21));
            Assert.That(color.Blue, Is.EqualTo(133));
            Assert.That(color.Alpha, Is.EqualTo(255));
        }

        [Test]
        public void Test_Color_WithRed()
        {
            Color color = new Color(12, 34, 56, 78).WithRed(99);
            Assert.That(color.Red, Is.EqualTo(99));
            Assert.That(color.Green, Is.EqualTo(34));
            Assert.That(color.Blue, Is.EqualTo(56));
            Assert.That(color.Alpha, Is.EqualTo(78));
        }

        [Test]
        public void Test_Color_WithGreen()
        {
            Color color = new Color(12, 34, 56, 78).WithGreen(99);
            Assert.That(color.Red, Is.EqualTo(12));
            Assert.That(color.Green, Is.EqualTo(99));
            Assert.That(color.Blue, Is.EqualTo(56));
            Assert.That(color.Alpha, Is.EqualTo(78));
        }

        [Test]
        public void Test_Color_WithBlue()
        {
            Color color = new Color(12, 34, 56, 78).WithBlue(99);
            Assert.That(color.Red, Is.EqualTo(12));
            Assert.That(color.Green, Is.EqualTo(34));
            Assert.That(color.Blue, Is.EqualTo(99));
            Assert.That(color.Alpha, Is.EqualTo(78));
        }

        [Test]
        public void Test_Color_WithAlpha()
        {
            Color color = new Color(12, 34, 56, 78).WithAlpha(99);
            Assert.That(color.Red, Is.EqualTo(12));
            Assert.That(color.Green, Is.EqualTo(34));
            Assert.That(color.Blue, Is.EqualTo(56));
            Assert.That(color.Alpha, Is.EqualTo(99));
        }

        [Test]
        public void Test_Color_ToSKColor()
        {
            SKColor color = new Color(12, 34, 56, 78).ToSKColor();
            Assert.That(color.Red, Is.EqualTo(12));
            Assert.That(color.Green, Is.EqualTo(34));
            Assert.That(color.Blue, Is.EqualTo(56));
            Assert.That(color.Alpha, Is.EqualTo(78));
        }

        [Test]
        public void Test_Color_ToHex()
        {
            Color color = new(12, 34, 56);
            Assert.That(color.ToStringRGB(), Is.EqualTo("#0C2238"));
        }

        [Test]
        public void Test_Colors_ColorValues()
        {
            Assert.That(Colors.Orange.ToStringRGB, Is.EqualTo("#FFA500"));
            Assert.That(Colors.Chocolate.ToStringRGB, Is.EqualTo("#D2691E"));
            Assert.That(Colors.GoldenRod.ToStringRGB, Is.EqualTo("#DAA520"));
        }

        [Test]
        public void Test_Colors_WebColors_HasColors()
        {
            Assert.That(new NamedColors.WebColors().GetAllColors(), Is.Not.Empty);
        }

        [Test]
        public void Test_Colors_WebColors_ColorValues()
        {
            Assert.That(NamedColors.WebColors.Orange.ToStringRGB, Is.EqualTo("#FFA500"));
            Assert.That(NamedColors.WebColors.Chocolate.ToStringRGB, Is.EqualTo("#D2691E"));
            Assert.That(NamedColors.WebColors.GoldenRod.ToStringRGB, Is.EqualTo("#DAA520"));
        }

        [Test]
        public void Test_Colors_XKCD_HasColors()
        {
            Assert.That(new NamedColors.XkcdColors().GetAllColors(), Is.Not.Empty);
        }

        [Test]
        public void Test_Colors_XKCD_ColorValues()
        {
            Assert.That(NamedColors.XkcdColors.Orange.ToStringRGB, Is.EqualTo("#F97306"));
            Assert.That(NamedColors.XkcdColors.Darkblue.ToStringRGB, Is.EqualTo("#030764"));
            Assert.That(NamedColors.XkcdColors.BabyPoopGreen.ToStringRGB, Is.EqualTo("#8F9805"));
        }
    }
}
