namespace ScottPlot.Panels;

public abstract class PanelBase : IPanel
{
    public bool IsVisible { get; set; } = true;
    public Edge Edge { get; set; }
    public bool ShowDebugInformation { get; set; } = false;
    public float MinimumSize { get; set; } = 0;
    public float MaximumSize { get; set; } = float.MaxValue;

    public abstract float Measure();
    public abstract void Render(RenderPack rp, float size, float offset);

    public PixelRect GetPanelRect(PixelRect dataRect, float size, float offset)
    {
        return Edge switch
        {
            Edge.Left => new(
                left: dataRect.Left - size - offset,
                right: dataRect.Left - offset,
                bottom: dataRect.Bottom,
                top: dataRect.Top),
            Edge.Right => new(
                left: dataRect.Right + offset,
                right: dataRect.Right + size + offset,
                bottom: dataRect.Bottom,
                top: dataRect.Top),
            Edge.Bottom => new(
                left: dataRect.Left,
                right: dataRect.Right,
                bottom: dataRect.Bottom + size + offset,
                top: dataRect.Bottom + offset),
            Edge.Top => new(
                left: dataRect.Left,
                right: dataRect.Right,
                bottom: dataRect.Top - offset,
                top: dataRect.Top - size - offset),
            _ => throw new NotImplementedException($"{Edge}")
        };
    }
}
