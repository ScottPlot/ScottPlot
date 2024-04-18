using SkiaSharp;

namespace ScottPlotTests.RenderTests;

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

            Label lbl = new()
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
            Label lbl = new()
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
    public void Test_MultilineLabel_AlignmentWithRotation()
    {
        SKBitmap bmp = new(600, 600);
        using SKCanvas canvas = new(bmp);
        canvas.Clear(SKColors.Navy);

        PixelSize size = new(40, 20);

        Alignment[,] alignmentMatrix = AlignmentExtensions.AlignmentMatrix;

        for (int y = 0; y < alignmentMatrix.GetLength(0); y++)
        {
            for (int x = 0; x < alignmentMatrix.GetLength(1); x++)
            {
                Alignment alignment = alignmentMatrix[y, x];

                Pixel pixel = new(100 + x * 200, 100 + y * 200);
                Label label = new()
                {
                    Text = alignment.ToString()
                        .Replace("Upper", "Upper\n")
                        .Replace("Middle", "Middle\n")
                        .Replace("Lower", "Lower\n"),
                    Alignment = alignment,
                    FontSize = 32,
                    ForeColor = Colors.White.WithOpacity(.5),
                    BackgroundColor = Colors.White.WithAlpha(.1),
                    BorderColor = Colors.Yellow,
                    BorderWidth = 1,
                    PointSize = 5,
                    PointColor = Colors.White,
                    Rotation = 45,
                };

                label.Render(canvas, pixel);
            }
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
            Label lbl = new()
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

    [Test]
    public void Test_Label_Multiline()
    {
        SKSurface surface = Drawing.CreateSurface(400, 300);
        SKCanvas canvas = surface.Canvas;
        canvas.Clear(SKColors.Navy);

        Label lbl = new()
        {
            Text = "One\nTwo",
            ForeColor = Colors.White.WithAlpha(.5),
            FontSize = 22,
            PointSize = 5,
            PointColor = Colors.White,
            BorderWidth = 1,
            BorderColor = Colors.Yellow,
        };

        lbl.Render(canvas, 200, 100);

        lbl.Rotation = 45;

        lbl.Render(canvas, 200, 200);

        surface.SaveTestImage();
    }
}
