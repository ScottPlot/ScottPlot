namespace ScottPlot;

public class BackgroundStyle : IDisposable
{
    public Color Color { get; set; } = Colors.White;

    private SKBitmap? SKBitmap { get; set; } = null;
    public ImagePosition ImagePosition { get; set; } = ImagePosition.Stretch;
    public bool AntiAlias = true;

    public Image? _Image;
    public Image? Image
    {
        get => _Image; set
        {
            _Image = value;

            if (value is not null)
            {
                byte[] bytes = value.GetImageBytes();
                SKBitmap = SKBitmap.Decode(bytes); // TODO: SKImage instead?
            }
        }
    }

    public void Dispose()
    {
        SKBitmap?.Dispose();
    }

    public PixelRect GetImageRect(PixelRect targetRect)
    {
        return Image is null
            ? PixelRect.Zero
            : ImagePosition.GetRect(Image.Size, targetRect);
    }

    public void Render(SKCanvas canvas, PixelRect target)
    {
        using SKPaint paint = new() { Color = Color.ToSKColor() };
        Drawing.FillRectangle(canvas, target, paint);

        if (Image is not null)
        {
            PixelRect imgRect = ImagePosition.GetRect(Image.Size, target);
            Image.Render(canvas, imgRect, paint, AntiAlias);
        }
    }
}
