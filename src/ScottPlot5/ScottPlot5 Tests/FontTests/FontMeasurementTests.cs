using SkiaSharp;

namespace ScottPlotTests.FontTests;

internal class FontMeasurementTests
{
    [Test]
    public void Test_String_Measurement()
    {
        SKBitmap bmp = new(500, 500);
        using SKCanvas canvas = new(bmp);
        canvas.Clear(SKColors.Navy);

        string[] fonts = { "Times New Roman", "Consolas", "Impact", "Arial Narrow" };

        float yOffset = 20;
        foreach (string font in fonts)
        {
            Label lbl = new()
            {
                Text = "Hello, World",
                Font = new() { Name = font, Size = 64, Color = Colors.White },
            };

            Pixel px = new(20, yOffset);
            lbl.Draw(canvas, px);

            PixelRect rect = lbl.GetRectangle(px);
            Drawing.DrawRectangle(canvas, rect, Colors.Yellow);

            yOffset += 100;
        }

        bmp.SaveTestImage();
        Assert.Pass();
    }
}
