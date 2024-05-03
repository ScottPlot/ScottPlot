using ScottPlot;
using ScottPlot.WinForms;
using SkiaSharp;

namespace WinForms_Demo.Demos;

public partial class LabelDemo : Form, IDemoWindow
{
    public string Title => "Font Styling";

    public string Description => "A tool to facilitate evaluating different fonts " +
        "and the customization options for size, alignment, line height, and more.";

    public LabelDemo()
    {
        InitializeComponent();

        SizeChanged += (s, e) => RegenerateImage();
        tbSize.ValueChanged += (s, e) => RegenerateImage();
        tbAlignment.ValueChanged += (s, e) => RegenerateImage();
        tbPadding.ValueChanged += (s, e) => RegenerateImage();
        tbRotation.ValueChanged += (s, e) => RegenerateImage();
        Load += (s, e) => RegenerateImage();
    }

    public void RegenerateImage()
    {
        SKImageInfo info = new(pictureBox1.Width, pictureBox1.Height);
        SKSurface surface = SKSurface.Create(info);
        SKCanvas canvas = surface.Canvas;
        canvas.Clear(SKColors.White);

        ScottPlot.Label label = new()
        {
            ForeColor = Colors.Black,
            BorderColor = Colors.Gray,
            BorderWidth = 1,
            Text = "Testing",
            FontSize = tbSize.Value,
            Alignment = (Alignment)tbAlignment.Value,
            Rotation = tbRotation.Value,
            Padding = tbPadding.Value,
        };

        PixelSize size = new(pictureBox1.Width, pictureBox1.Height);
        PixelRect rect = new(size);

        LineStyle ls = new()
        {
            Width = 1,
            Color = Colors.Magenta,
        };

        using SKPaint paint = new();
        Drawing.DrawCircle(canvas, rect.Center, 5, ls, paint);
        label.Render(canvas, rect.Center, paint);

        ScottPlot.Image img2 = new(surface);
        var oldImage = pictureBox1.Image;
        pictureBox1.Image = img2.GetBitmap();
        oldImage?.Dispose();
    }
}
