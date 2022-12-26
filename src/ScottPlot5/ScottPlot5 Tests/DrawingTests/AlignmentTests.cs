using SkiaSharp;

namespace ScottPlotTests.DrawingTests;

internal class AlignmentTests
{
    [Test]
    public void Test_Rectange_Alignment()
    {
        SKBitmap bmp = new(500, 500);
        using SKCanvas canvas = new(bmp);
        canvas.Clear(SKColors.Navy);

        PixelSize size = new(40, 20);

        int y = 20;
        foreach (Alignment2 alignment in Enum.GetValues(typeof(Alignment2)))
        {
            Pixel pixel = new(250, 20 + y);
            PixelRect rect = Drawing.DrawText(canvas, pixel, alignment.ToString(), 32, Colors.White, alignment);
            Drawing.DrawRectangle(canvas, rect, Colors.Yellow);
            Drawing.DrawCircle(canvas, pixel, Colors.White);
            y += 50;
        }

        bmp.SaveTestImage();
    }
}
