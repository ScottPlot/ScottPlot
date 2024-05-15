namespace ScottPlot.AxisPanels.Experimental;

/// <summary>
/// A simple custom Y axis which includes a subtitle/sub-label.
/// Note that <see cref="Measure"/> and <see cref="Render"/> are called whenever the plot needs to be rendered
/// so be wary of heavy code that will slow down the rendering if you are using the chart in a user interface
/// where there is zooming, panning, etc.
/// </summary>
public class LeftAxisWithSubtitle : YAxisBase
{
    public string SubLabelText { get => SubLabelStyle.Text; set => SubLabelStyle.Text = value; }

    public Label SubLabelStyle { get; set; } = new() { Rotation = -90, };

    public override Edge Edge => Edge.Left;

    public LeftAxisWithSubtitle()
    {
        TickGenerator = new TickGenerators.NumericAutomatic();
    }

    // Override measure to tell the layout engine how much space the axis needs to render properly.
    public override float Measure()
    {
        if (!IsVisible)
            return 0;

        if (!Range.HasBeenSet)
            return SizeWhenNoData;

        using SKPaint paint = new();
        float maxTickLabelWidth = TickGenerator.Ticks.Length > 0
            ? TickGenerator.Ticks.Select(x => TickLabelStyle.Measure(x.Label, paint).Width).Max()
            : 0;

        // Add the sub-label to the required size of the axis, as well as the main axis label.
        float axisLabelHeight = LabelStyle.Measure(LabelText, paint).LineHeight
                                + SubLabelStyle.Measure(SubLabelText, paint).LineHeight
                                + PaddingBetweenTickAndAxisLabels.Horizontal
                                + PaddingOutsideAxisLabels.Horizontal;

        return maxTickLabelWidth + axisLabelHeight;
    }

    // Override render to actually render things in the space determined by Measure.
    public override void Render(RenderPack rp, float size, float offset)
    {
        if (!IsVisible)
            return;

        // Use the properties on this class and the sub-class to determine what to render, and how.
        // You can put whatever you want here! Check out the implementation of the base axis classes for ideas
        // and how to draw shapes, text, etc. 

        PixelRect panelRect = GetPanelRect(rp.DataRect, size, offset);
        float x = panelRect.Left + PaddingOutsideAxisLabels.Horizontal;
        Pixel labelPoint = new(x, rp.DataRect.VerticalCenter);

        using SKPaint paint = new();
        LabelAlignment = Alignment.UpperCenter;
        LabelStyle.Render(rp.Canvas, labelPoint, paint);

        float labelHeight = LabelStyle.Measure().LineHeight;
        Pixel subLabelPoint = new(x + labelHeight, rp.DataRect.VerticalCenter);

        using SKPaint paint2 = new();
        SubLabelStyle.Alignment = Alignment.UpperCenter;
        SubLabelStyle.Render(rp.Canvas, subLabelPoint, paint2);

        DrawTicks(rp, TickLabelStyle, panelRect, TickGenerator.Ticks, this, MajorTickStyle, MinorTickStyle);
        DrawFrame(rp, panelRect, Edge, FrameLineStyle);
    }
}
