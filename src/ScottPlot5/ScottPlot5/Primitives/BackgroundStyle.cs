namespace ScottPlot.Primitives;

public class BackgroundStyle : IDisposable
{
    public SKBitmap? Image { get; set; } = null;
    public ImageScalingStyle ImageScaling { get; set; } = ImageScalingStyle.FillRetainAspect;
    public Color Color { get; set; } = Colors.White;

    public void Dispose()
    {
        Image?.Dispose();
    }
}
