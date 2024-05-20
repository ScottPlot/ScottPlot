namespace ScottPlot.Panels;

public class TitlePanel : IPanel
{
    public bool IsVisible { get; set; } = true;

    public Edge Edge => Edge.Top;

    public bool ShowDebugInformation { get; set; } = false;
    public float MinimumSize { get; set; } = 0;
    public float MaximumSize { get; set; } = float.MaxValue;

    public TitlePanel()
    {
        LabelStyle.Rotation = 0;
    }

    [Obsolete("Reference LabelStyle")]
    public Label Label => LabelStyle;

    public Label LabelStyle { get; } = new()
    {
        Text = string.Empty,
        FontSize = 16,
        Bold = true,
        Alignment = Alignment.LowerCenter,
    };

    public string LabelText { get => LabelStyle.Text; set => LabelStyle.Text = value; }

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

        if (string.IsNullOrWhiteSpace(LabelStyle.Text))
            return 0;

        using SKPaint paint = new();

        return LabelStyle.Measure(LabelStyle.Text, paint).Height + VerticalPadding;
    }

    public void Render(RenderPack rp, float size, float offset)
    {
        if (!IsVisible)
            return;

        using SKPaint paint = new();

        PixelRect panelRect = GetPanelRect(rp.DataRect, size, offset);

        Pixel labelPoint = new(panelRect.HorizontalCenter, panelRect.Bottom);

        if (ShowDebugInformation)
        {
            Drawing.DrawDebugRectangle(rp.Canvas, panelRect, labelPoint, LabelStyle.ForeColor);
        }

        LabelStyle.Render(rp.Canvas, labelPoint, paint);
    }
}
