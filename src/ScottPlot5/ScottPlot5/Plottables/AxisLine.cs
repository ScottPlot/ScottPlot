using ScottPlot.Interfaces;

namespace ScottPlot.Plottables;

public abstract class AxisLine : IPlottable, IRenderLast, IHoldLineStyle
{
    public bool IsVisible { get; set; } = true;

    public IAxes Axes { get; set; } = new Axes();

    public readonly Label Label = new();
    public float FontSize { get => Label.FontSize; set => Label.FontSize = value; }
    public bool FontBold { get => Label.Bold; set => Label.Bold = value; }
    public string FontName { get => Label.FontName; set => Label.FontName = value; }
    public Color FontColor { get => Label.ForeColor; set => Label.ForeColor = value; }
    public string Text { get => Label.Text; set => Label.Text = value; }

    public LineStyle LineStyle { get; set; } = new();
    public float LineWidth { get => LineStyle.Width; set => LineStyle.Width = value; }
    public LinePattern LinePattern { get => LineStyle.Pattern; set => LineStyle.Pattern = value; }
    public Color LineColor { get => LineStyle.Color; set => LineStyle.Color = value; }

    public bool LabelOppositeAxis { get; set; } = false;

    public bool IsDraggable { get; set; } = false;

    public Color Color
    {
        get => LineStyle.Color;
        set
        {
            LineStyle.Color = value;
            Label.BackColor = value;
        }
    }

    public double Position { get; set; } = 0;

    public IEnumerable<LegendItem> LegendItems
    {
        get
        {
            return LegendItem.Single(new LegendItem()
            {
                Label = Label.Text,
                Line = LineStyle,
            });
        }
    }

    public abstract bool IsUnderMouse(CoordinateRect rect);

    public abstract AxisLimits GetAxisLimits();

    public abstract void Render(RenderPack rp);

    public abstract void RenderLast(RenderPack rp);
}
