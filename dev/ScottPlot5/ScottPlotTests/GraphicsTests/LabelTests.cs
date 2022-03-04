using Microsoft.Maui.Graphics;
using Microsoft.Maui.Graphics.Skia;
using NUnit.Framework;

namespace ScottPlotTests.GraphicsTests
{
    public class LabelTests
    {
        readonly static BitmapExportContext TestContext = new SkiaBitmapExportContext(1, 1, 1.0f);
        static ICanvas Canvas => TestContext.Canvas;

        [Test]
        public void Test_Label_MeasureString_Returns_Positive_Number()
        {
            ScottPlot.TextLabel lbl1 = new("testing");
            ScottPlot.PixelSize lbl1Size = lbl1.Measure(Canvas);

            Assert.NotZero(lbl1Size.Width);
            Assert.NotZero(lbl1Size.Height);
        }

        [Test]
        public void Test_Label_MeasureString_Respects_Font_Size()
        {
            ScottPlot.TextLabel lbl1 = new("testing");
            ScottPlot.PixelSize lbl1Size = lbl1.Measure(Canvas);

            ScottPlot.TextLabel lbl2 = new("testing") { FontSize = lbl1.FontSize / 2 };
            ScottPlot.PixelSize lbl2Size = lbl2.Measure(Canvas);

            Assert.Greater(lbl1Size.Width, lbl2Size.Width);
            Assert.Greater(lbl1Size.Height, lbl2Size.Height);
        }

        [Test]
        public void Test_Label_MeasureString_Respects_Text_Content()
        {
            ScottPlot.TextLabel lbl1 = new("XX");
            ScottPlot.PixelSize lbl1Size = lbl1.Measure(Canvas);

            ScottPlot.TextLabel lbl2 = new("X");
            ScottPlot.PixelSize lbl2Size = lbl2.Measure(Canvas);

            Assert.Greater(lbl1Size.Width, lbl2Size.Width);
            Assert.AreEqual(lbl1Size.Height, lbl2Size.Height);
        }
    }
}
