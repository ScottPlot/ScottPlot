namespace ScottPlot;

public class BackgroundStyle : IDisposable
{
    public Color Color { get => FillStyle.Color; set => FillStyle.Color = value; }
    public FillStyle FillStyle { get; set; } = new FillStyle()
    {
        IsVisible = true,
        Color = Colors.White,
    };

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

    public void Render(RenderPack rp, PixelRect target)
    {
        Drawing.FillRectangle(rp.Canvas, target, rp.Paint, FillStyle);

        if (Image is not null)
        {
            PixelRect imgRect = ImagePosition.GetRect(Image.Size, target);
            Image.Render(rp.Canvas, imgRect, rp.Paint, AntiAlias);
        }
    }
}
