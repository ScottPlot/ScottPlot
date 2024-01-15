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
            LabelExperimental lbl = new()
            {
                Text = alignment.ToString(),
                Alignment = alignment,
                FontSize = 32,
                ForeColor = Colors.White.WithOpacity(.5),
                BorderColor = Colors.Yellow,
                BorderWidth = 1,
                PointSize = 5,
                PointColor = Colors.White,
            };

            lbl.Render(canvas, pixel);

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
            float x = (float)Math.Cos(i * Math.PI / 180) * radius;
            float y = (float)Math.Sin(i * Math.PI / 180) * radius;
            Pixel center = new(bmp.Width / 2 + x, bmp.Height / 2 + y);

            LabelExperimental lbl = new()
            {
                Text = $"R{i}",
                FontSize = 32,
                ForeColor = Colors.White.WithOpacity(.5),
                Rotation = i,
                PointSize = 5,
                BorderColor = Colors.Yellow,
                PointColor = Colors.White,
                BorderWidth = 1,
            };

            lbl.Render(canvas, center);
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
            LabelExperimental lbl = new()
            {
                Text = alignment.ToString(),
                Alignment = alignment,
                FontSize = 32,
                ForeColor = Colors.White.WithOpacity(.5),
                BorderColor = Colors.Yellow,
                BorderWidth = 1,
                PointSize = 5,
                PointColor = Colors.White,
                Rotation = 30,
            };

            lbl.Render(canvas, pixel);

            y += 50;
        }

        bmp.SaveTestImage();
    }

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
            LabelExperimental lbl = new()
            {
                Text = "Hello, World",
                FontName = font,
                FontSize = 64,
                ForeColor = Colors.White,
                BorderColor = Colors.Yellow,
                BorderWidth = 1,
            };

            Pixel px = new(20, yOffset);
            lbl.Render(canvas, px);

            yOffset += 100;
        }

        bmp.SaveTestImage();
        Assert.Pass();
    }
}
