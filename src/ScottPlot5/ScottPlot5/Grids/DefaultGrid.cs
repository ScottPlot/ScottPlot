namespace ScottPlot.Grids;

public class DefaultGrid(IXAxis xAxis, IYAxis yAxis) : IGrid
{
    public bool IsVisible { get; set; } = true;
    public bool IsBeneathPlottables { get; set; } = true;
    public IXAxis XAxis { get; set; } = xAxis;
    public IYAxis YAxis { get; set; } = yAxis;
    public GridStyle XAxisStyle { get; set; } = new();
    public GridStyle YAxisStyle { get; set; } = new();

    public Color MajorLineColor
    {
        set
        {
            XAxisStyle.MajorLineStyle.Color = value;
            YAxisStyle.MajorLineStyle.Color = value;
        }
    }

    public Color MinorLineColor
    {
        set
        {
            XAxisStyle.MinorLineStyle.Color = value;
            YAxisStyle.MinorLineStyle.Color = value;
        }
    }

    public float MajorLineWidth
    {
        set
        {
            XAxisStyle.MajorLineStyle.Width = value;
            YAxisStyle.MajorLineStyle.Width = value;
        }
    }

    public float MinorLineWidth
    {
        set
        {
            XAxisStyle.MinorLineStyle.Width = value;
            YAxisStyle.MinorLineStyle.Width = value;
        }
    }

    public void Render(RenderPack rp)
    {
        if (!IsVisible)
            return;

        var minX = Math.Min(XAxis.Min, XAxis.Max);
        var maxX = Math.Max(XAxis.Min, XAxis.Max);
        var minY = Math.Min(YAxis.Min, YAxis.Max);
        var maxY = Math.Max(YAxis.Min, YAxis.Max);

        var xTicks = XAxis.TickGenerator.Ticks.Where(x => x.Position >= minX && x.Position <= maxX);
        var yTicks = YAxis.TickGenerator.Ticks.Where(x => x.Position >= minY && x.Position <= maxY);

        XAxisStyle.Render(rp, XAxis, xTicks);
        YAxisStyle.Render(rp, YAxis, yTicks);
    }
}
