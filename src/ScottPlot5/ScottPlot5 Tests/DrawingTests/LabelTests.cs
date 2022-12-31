using ScottPlot.Extensions;
using SkiaSharp;

namespace ScottPlotTests.DrawingTests;

internal class LabelTests
{
    [Test]
    public void Test_Label_Alignment()
    {
        SKBitmap bmp = new(500, 500);
        using SKCanvas canvas = new(bmp);
        canvas.Clear(SKColors.Navy);

        PixelSize size = new(40, 20);

        int y = 20;
        foreach (Alignment alignment in Enum.GetValues(typeof(Alignment)))
        {
            Pixel pixel = new(250, 20 + y);
            Label lbl = new()
            {
                Text = alignment.ToString(),
                Alignment = alignment,
                Font = new() { Size = 32, Color = Colors.White.WithOpacity(.5) },
                Border = new() { Color = Colors.Yellow, Width = 1 },
                PointSize = 5,
            };

            lbl.Draw(canvas, pixel);

            y += 50;
        }

        bmp.SaveTestImage();
    }

    [Test]
    public void Test_Label_Rotation()
    {
        SKBitmap bmp = new(500, 500);
        using SKCanvas canvas = new(bmp);
        canvas.Clear(SKColors.Navy);

        PixelSize size = new(40, 20);

        for (int i = 0; i < 360; i += 45)
        {
            float radius = 100;
            float x = (float)Math.Cos(i.ToRadians()) * radius;
            float y = (float)Math.Sin(i.ToRadians()) * radius;
            Pixel center = bmp.CenterPixel().WithDelta(x, y);

            Label lbl = new()
            {
                Text = $"R{i}",
                Font = new() { Size = 32, Color = Colors.White.WithOpacity(.5) },
                Rotation = i,
                PointSize = 5,
                Border = new() { Color = Colors.Yellow },
                BackgroundColor = Colors.White.WithOpacity(.2),
            };
            lbl.Draw(canvas, center);
        }

        bmp.SaveTestImage();
    }

    [Test]
    public void Test_Label_Alignment_With_Rotation()
    {
        SKBitmap bmp = new(500, 500);
        using SKCanvas canvas = new(bmp);
        canvas.Clear(SKColors.Navy);

        PixelSize size = new(40, 20);

        int y = 20;
        foreach (Alignment alignment in Enum.GetValues(typeof(Alignment)))
        {
            Pixel pixel = new(250, 20 + y);
            Label lbl = new()
            {
                Text = alignment.ToString(),
                Alignment = alignment,
                Font = new() { Size = 32, Color = Colors.White.WithOpacity(.5) },
                Border = new() { Color = Colors.Yellow },
                PointSize = 5,
                Rotation = 30,
            };

            lbl.Draw(canvas, pixel);

            y += 50;
        }

        bmp.SaveTestImage();
    }
}
