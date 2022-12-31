using ScottPlot.LayoutSystem;

namespace ScottPlot.Panels;

public class TitlePanel : IPanel
{
    public bool IsVisible { get; set; } = true;

    public Edge Edge => Edge.Top;

    public bool ShowDebugInformation { get; set; } = false;

    public Label Label { get; } = new()
    {
        Text = string.Empty,
        Font = new() { Size = 16, Bold = true },
        Alignment = Alignment.LowerCenter,
    };

    /// <summary>
    /// Extra space to add above the title text so the title does not touch the edge of the image
    /// </summary>
    public float VerticalPadding = 10;

    public PixelRect GetPanelRect(PixelRect dataRect, float size, float offset)
    {
        return new PixelRect(
            left: dataRect.Left,
            right: dataRect.Right,
            bottom: dataRect.Top - offset,
            top: dataRect.Top - offset - size);
    }

    public float Measure()
    {
        if (!IsVisible)
            return 0;

        if (string.IsNullOrWhiteSpace(Label.Text))
            return 0;

        return Label.Measure().Height + VerticalPadding;
    }

    public void Render(SKSurface surface, PixelRect dataRect, float size, float offset)
    {
        if (!IsVisible)
            return;

        PixelRect panelRect = GetPanelRect(dataRect, size, offset);

        Pixel labelPoint = new(panelRect.HorizontalCenter, panelRect.Bottom);

        if (ShowDebugInformation)
        {
            Drawing.DrawDebugRectangle(surface.Canvas, panelRect, labelPoint, Label.Font.Color);
        }

        Label.Draw(surface.Canvas, labelPoint);
    }
}
