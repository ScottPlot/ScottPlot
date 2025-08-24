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

    public LabelStyle SubLabelStyle { get; set; } = new() { Rotation = -90, };

    public override Edge Edge => Edge.Left;

    public LeftAxisWithSubtitle()
    {
        TickGenerator = new TickGenerators.NumericAutomatic();
    }

    // Override measure to tell the layout engine how much space the axis needs to render properly.
    public override float Measure(Paint paint)
    {
        if (!IsVisible)
            return 0;

        if (!Range.HasBeenSet)
            return SizeWhenNoData;

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

        PixelRect panelRect = GetPanelRect(rp.DataRect, size, offset, rp.Paint);
        float x = panelRect.Left + PaddingOutsideAxisLabels.Horizontal;
        Pixel labelPoint = new(x, rp.DataRect.VerticalCenter);

        LabelAlignment = Alignment.UpperCenter;
        LabelStyle.Render(rp.Canvas, labelPoint, rp.Paint);

        float labelHeight = LabelStyle.Measure(rp.Paint).LineHeight;
        Pixel subLabelPoint = new(x + labelHeight, rp.DataRect.VerticalCenter);

        SubLabelStyle.Alignment = Alignment.UpperCenter;
        SubLabelStyle.Render(rp.Canvas, subLabelPoint, rp.Paint);

        DrawTicks(rp, TickLabelStyle, panelRect, TickGenerator.Ticks, this, MajorTickStyle, MinorTickStyle);
        DrawFrame(rp, panelRect, Edge, FrameLineStyle);
    }
}
