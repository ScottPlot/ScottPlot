using SkiaSharp;

namespace ScottPlotTests.FontTests;

internal class FontMeasurementTests
{
    [Test]
    public void Test_String_Measurement()
    {
        string text = "Hello, World";
        Font font = FontService.GetSystemDefaultMonospaceFont(16);
        PixelSize size = FontService.Measure(font, text);
        Console.WriteLine(size);

        // draw what we found
        SKBitmap bmp = new(500, 500);
        using SKCanvas canvas = new(bmp);
        canvas.Clear(SKColors.Navy);

        Alignment alignment = Alignment.UpperLeft;
        DrawTestString(canvas, text, "Times New Roman", new Pixel(20, 20), alignment);
        DrawTestString(canvas, text, "Consolas", new Pixel(20, 120), alignment);
        DrawTestString(canvas, text, "Impact", new Pixel(20, 220), alignment);
        DrawTestString(canvas, text, "Arial Narrow", new Pixel(20, 320), alignment);

        bmp.SaveTestImage();
    }

    [Test]
    public void Test_String_Alignment()
    {
        string text = "Hello, World";
        Font font = FontService.GetSystemDefaultMonospaceFont(16);
        PixelSize size = FontService.Measure(font, text);
        Console.WriteLine(size);

        // draw what we found
        SKBitmap bmp = new(500, 500);
        using SKCanvas canvas = new(bmp);
        canvas.Clear(SKColors.Navy);

        DrawTestString(canvas, text, "Times New Roman", new Pixel(20, 20), Alignment.UpperLeft);
        DrawTestString(canvas, text, "Times New Roman", new Pixel(480, 120), Alignment.UpperRight);

        bmp.SaveTestImage();
    }

    private void DrawTestString(SKCanvas canvas, string text, string fontName, Pixel pixel, Alignment alignment)
    {
        Label2 lbl = new()
        {
            Text = text,
            FontName = fontName,
            FontSize = 64,
            Alignment = alignment,
        };

        PixelRect rect = lbl.GetRectangle(pixel);

        Drawing.Fillectangle(canvas, rect, Colors.Yellow.WithOpacity(.2));
        lbl.Draw(canvas, pixel);
        Drawing.DrawRectangle(canvas, rect, Colors.Yellow);
        Drawing.DrawCircle(canvas, pixel, Colors.White);
    }
}
