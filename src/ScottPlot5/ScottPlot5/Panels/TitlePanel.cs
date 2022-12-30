using ScottPlot.LayoutSystem;

namespace ScottPlot.Panels;

public class TitlePanel : IPanel
{
    public Edge Edge => Edge.Top;

    public bool ShowDebugInformation { get; set; } = false;

    public Label Label { get; } = new()
    {
        Text = string.Empty,
        FontName = FontService.SansFontName,
        FontSize = 16,
        Bold = true,
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
        if (string.IsNullOrWhiteSpace(Label.Text))
            return 0;
        else
            return Label.Measure().Height + VerticalPadding;
    }

    public void Render(SKSurface surface, PixelRect dataRect, float size, float offset)
    {
        PixelRect panelRect = GetPanelRect(dataRect, size, offset);

        Pixel labelPoint = new(panelRect.HorizontalCenter, panelRect.Bottom);

        if (ShowDebugInformation)
        {
            Drawing.DrawDebugRectangle(surface.Canvas, panelRect, labelPoint, Label.Color);
        }

        Label.Draw(surface.Canvas, labelPoint);
    }
}
