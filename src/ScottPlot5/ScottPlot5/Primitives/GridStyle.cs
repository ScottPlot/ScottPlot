namespace ScottPlot;

public class GridStyle
{
    public bool IsVisible { get; set; } = true;

    public LineStyle MajorLineStyle { get; set; } = new()
    {
        Width = 1,
        Color = Colors.Black.WithOpacity(.1)
    };

    public LineStyle MinorLineStyle { get; set; } = new()
    {
        Width = 0,
        Color = Colors.Black.WithOpacity(.05)
    };

    public int MaximumNumberOfGridLines { get; set; } = 1_000;
    public bool IsBeneathPlottables { get; set; } = true;

    public void Render(RenderPack rp, IAxis axis, IEnumerable<Tick> ticks)
    {
        if (!IsVisible)
            return;

        if (MinorLineStyle.IsVisible && MinorLineStyle.Width > 0)
        {
            float[] xTicksMinor = ticks
                .Where(x => !x.IsMajor)
                .Select(x => axis.GetPixel(x.Position, rp.DataRect))
                .Take(MaximumNumberOfGridLines)
                .ToArray();

            RenderGridLines(rp, xTicksMinor, axis.Edge, MinorLineStyle);
        }

        if (MajorLineStyle.IsVisible && MajorLineStyle.Width > 0)
        {
            float[] xTicksMajor = ticks
                .Where(x => x.IsMajor)
                .Select(x => axis.GetPixel(x.Position, rp.DataRect))
                .Take(MaximumNumberOfGridLines)
                .ToArray();

            RenderGridLines(rp, xTicksMajor, axis.Edge, MajorLineStyle);
        }
    }

    private static void RenderGridLines(RenderPack rp, float[] positions, Edge edge, LineStyle lineStyle)
    {
        Pixel[] starts = new Pixel[positions.Length];
        Pixel[] ends = new Pixel[positions.Length];

        for (int i = 0; i < positions.Length; i++)
        {
            float px = positions[i];

            starts[i] = edge.IsHorizontal()
                ? new Pixel(px, rp.DataRect.Bottom)
                : new Pixel(rp.DataRect.Left, px);

            ends[i] = edge.IsHorizontal()
                ? new Pixel(px, rp.DataRect.Top)
                : new Pixel(rp.DataRect.Right, px);
        }

        Drawing.DrawLines(rp.Canvas, starts, ends, lineStyle.Color, lineStyle.Width, antiAlias: true, lineStyle.Pattern);
    }
}
