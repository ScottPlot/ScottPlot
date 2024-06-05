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
        using SKPaint paint = new();

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

            lbl.Render(canvas, pixel, paint);

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
        using SKPaint paint = new();

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

            lbl.Render(canvas, center, paint);
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
        using SKPaint paint = new();

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

            lbl.Render(canvas, pixel, paint);

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
        using SKPaint paint = new();

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

                label.Render(canvas, pixel, paint);
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

        using SKPaint paint = new();
        string[] fonts = ["Times New Roman", "Consolas", "Impact", "Arial Narrow", "MiSsInG fOnT"];

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
            lbl.Render(canvas, px, paint);

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
        using SKPaint paint = new();

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

        lbl.Render(canvas, new(200, 100), paint);

        lbl.Rotation = 45;

        lbl.Render(canvas, new(200, 200), paint);

        surface.SaveTestImage();
    }

    [Test]
    public void Test_Label_Rounded()
    {
        SKBitmap bmp = new(200, 150);
        using SKCanvas canvas = new(bmp);
        canvas.Clear(SKColors.Navy);
        using SKPaint paint = new();

        Label lbl = new()
        {
            Text = $"Hello",
            FontSize = 32,
            ForeColor = Colors.White.WithOpacity(.5),
            BorderColor = Colors.Yellow.WithAlpha(.5),
            BackgroundColor = Colors.White.WithAlpha(.2),
            BorderWidth = 2,
            Padding = 10,
            BorderRadius = 15,
        };

        lbl.Render(canvas, new(50, 50), paint);

        bmp.SaveTestImage();
    }

    [Test]
    public void Test_Label_AntiAlias()
    {
        SKBitmap bmp = new(200, 200);
        using SKCanvas canvas = new(bmp);
        canvas.Clear(SKColors.White);
        using SKPaint paint = new();

        Label lbl1 = new()
        {
            Text = $"Default",
            BorderColor = Colors.Black,
            BorderWidth = 1,
            Padding = 3,
            ShadowColor = Colors.Black.WithAlpha(.5),
            ShadowOffset = new(5, 5),
            BackgroundColor = Colors.White,
        };

        Label lbl2 = new()
        {
            Text = $"AntiAliasBackground = false",
            BorderColor = Colors.Black,
            BorderWidth = 1,
            Padding = 3,
            ShadowColor = Colors.Black.WithAlpha(.5),
            ShadowOffset = new(5, 5),
            BackgroundColor = Colors.White,
            AntiAliasBackground = false,
        };

        Label lbl3 = new()
        {
            Text = $"AntiAliasText = false",
            BorderColor = Colors.Black,
            BorderWidth = 1,
            Padding = 3,
            ShadowColor = Colors.Black.WithAlpha(.5),
            ShadowOffset = new(5, 5),
            BackgroundColor = Colors.White,
            AntiAliasText = false,
        };

        lbl1.Render(canvas, new(25, 50), paint);
        lbl2.Render(canvas, new(25, 100), paint);
        lbl3.Render(canvas, new(25, 150), paint);

        bmp.SaveTestImage();
    }


    [Test]
    public void Test_Label_Offset()
    {
        SKBitmap bmp = new(500, 500);
        Test_Label_Offset(bmp, "X:{0}, Y:{1}");
        bmp.SaveTestImage();
    }

    [Test]
    public void Test_Label_MultiLineOffset()
    {
        SKBitmap bmp = new(500, 500);
        Test_Label_Offset(bmp, "X:{0}\nY:{1}");
        bmp.SaveTestImage();
    }

    private static void Test_Label_Offset(SKBitmap bmp, string format)
    {
        using SKCanvas canvas = new(bmp);
        canvas.Clear(SKColors.Navy);

        using SKPaint paint = new();

        Pixel center = new(bmp.Width / 2, bmp.Height / 2);
        float offset = 150f;

        for (int y = -1; y < 2; y++)
        {
            for (int x = -1; x < 2; x++)
            {
                float offsetX = offset * x;
                float offsetY = offset * y;

                Label lbl = new()
                {
                    Text = string.Format(format, offsetX, offsetY),
                    Alignment = Alignment.MiddleCenter,
                    FontSize = 24,
                    ForeColor = Colors.White.WithOpacity(.5),
                    PointSize = 5,
                    BorderColor = Colors.Yellow,
                    PointColor = Colors.White,
                    BorderWidth = 1,
                    OffsetX = offsetX,
                    OffsetY = offsetY,
                };

                lbl.Render(canvas, center, paint);
            }
        }
    }
}
