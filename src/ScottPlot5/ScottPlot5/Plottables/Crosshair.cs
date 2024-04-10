namespace ScottPlot.Plottables;

public class Crosshair : IPlottable, IRenderLast
{
    public bool IsVisible { get; set; } = true;

    private IAxes axes = new Axes();
    public IAxes Axes
    {
        get
        {
            return axes;
        }
        set
        {
            axes = value;
            horizontalLine.Axes = value;
            verticalLine.Axes = value;
        }
    }

    public IEnumerable<LegendItem> LegendItems
    {
        get
        {
            return horizontalLine.LegendItems.Concat(verticalLine.LegendItems);
        }
    }

    public AxisLimits GetAxisLimits() => new(X, X, Y, Y);

    private readonly HorizontalLine horizontalLine = new();
    private readonly VerticalLine verticalLine = new ();

    public Coordinates Position { get => new(X, Y); set { X = value.X; Y = value.Y; } }

    public double X
    {
        get
        {
            return verticalLine.X;
        }
        set
        {
            verticalLine.X = value;
        }
    }

    public double Y
    {
        get
        {
            return horizontalLine.Y;
        }
        set
        {
            horizontalLine.Y = value;
        }
    }

    public bool VerticalLineIsVisible
    {
        get
        {
            return verticalLine.IsVisible;
        }
        set
        {
            verticalLine.IsVisible = value;
        }
    }

    public bool HorizontalLineIsVisible
    {
        get
        {
            return horizontalLine.IsVisible;
        }
        set
        {
            horizontalLine.IsVisible = value;
        }
    }

    public readonly LineStyle LineStyle = new();

    private float fontSize;
    public float FontSize
    {
        get
        {
            return fontSize;
        }
        set
        {
            fontSize = value;
            horizontalLine.FontSize = value;
            verticalLine.FontSize = value;
        }
    }

    private bool fontBold;
    public bool FontBold
    {
        get
        {
            return fontBold;
        }
        set
        {
            fontBold = value;
            horizontalLine.FontBold = value;
            verticalLine.FontBold = value;
        }
    }

    private string fontName;
    public string FontName
    {
        get
        {
            return fontName;
        }
        set
        {
            fontName = value;
            horizontalLine.FontName = value;
            verticalLine.FontName = value;
        }
    }

    private Color fontColor;
    public Color FontColor
    {
        get
        {
            return fontColor;
        }
        set
        {
            fontColor = value;
            horizontalLine.FontColor = value;
            verticalLine.FontColor = value;
        }
    }

    public string TextX { get => verticalLine.Text; set => verticalLine.Text = value; }
    public string TextY { get => horizontalLine.Text; set => horizontalLine.Text = value; }

    public Crosshair()
    {
        horizontalLine.Axes = Axes;
        verticalLine.Axes = Axes;
        horizontalLine.LineStyle = LineStyle;
        verticalLine.LineStyle = LineStyle;
    }

    public void Render(RenderPack rp)
    {
        if (!IsVisible)
            return;

        horizontalLine.Render(rp);
        verticalLine.Render(rp);
    }

    public void RenderLast(RenderPack rp)
    {
        if (!IsVisible)
            return;

        horizontalLine.RenderLast(rp);
        verticalLine.RenderLast(rp);
    }
}
