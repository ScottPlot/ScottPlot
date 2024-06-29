namespace ScottPlot;

public class GridStyle
{
    public bool IsVisible { get; set; } = true;

    public LineStyle MajorLineStyle { get; set; } = new()
    {
        Width = 1,
        Color = Colors.Black.WithOpacity(.1),
        AntiAlias = false,
    };

    public LineStyle MinorLineStyle { get; set; } = new()
    {
        Width = 0,
        IsVisible = true,
        Color = Colors.Black.WithOpacity(.05),
        AntiAlias = false,
    };

    public int MaximumNumberOfGridLines { get; set; } = 1_000;
    public bool IsBeneathPlottables { get; set; } = true;

    public Color? AlternatingColor { get; set; } = null;

    public void Render(RenderPack rp, IAxis axis, IEnumerable<Tick> ticks)
    {
        if (!IsVisible)
        {
            return;
        }

        if (MinorLineStyle.CanBeRendered)
        {
            float[] xTicksMinor = ticks
                .Where(x => !x.IsMajor)
                .Select(x => axis.GetPixel(x.Position, rp.DataRect))
                .Take(MaximumNumberOfGridLines)
                .ToArray();

            RenderGridLines(rp, xTicksMinor, axis.Edge, MinorLineStyle);
        }

        if (MajorLineStyle.CanBeRendered)
        {
            float[] xTicksMajor = ticks
                .Where(x => x.IsMajor)
                .Select(x => axis.GetPixel(x.Position, rp.DataRect))
                .Take(MaximumNumberOfGridLines)
                .ToArray();

            RenderGridBackground(rp, xTicksMajor, axis.Edge);
            RenderGridLines(rp, xTicksMajor, axis.Edge, MajorLineStyle);
        }
    }

    private void RenderGridBackground(
        RenderPack rp, float[] positions, Edge edge)
    {
        if (AlternatingColor is null)
        {
            return;
        }

        float[] pos = [
            edge.IsHorizontal() ? rp.DataRect.Left : rp.DataRect.Bottom,
            .. positions,
            edge.IsHorizontal() ? rp.DataRect.Right : rp.DataRect.Top];

        IEnumerable<Pixel> starts = pos
            .Select(px => edge.IsHorizontal()
                ? new Pixel(px, rp.DataRect.Bottom)
                : new Pixel(rp.DataRect.Left, px));
        IEnumerable<Pixel> ends = pos
            .Select(px => edge.IsHorizontal()
                ? new Pixel(px, rp.DataRect.Top)
                : new Pixel(rp.DataRect.Right, px));

        for (int i = 1; i < starts.Count(); i++)
        {
            if (i % 2 == 0)
            {
                continue;
            }

            Drawing.FillRectangle(
                rp.Canvas,
                new PixelRect(starts.ElementAt(i - 1), ends.ElementAt(i)),
                AlternatingColor.Value);
        }
    }

    private static void RenderGridLines(RenderPack rp, float[] positions, Edge edge, LineStyle lineStyle)
    {
        var starts = new Pixel[positions.Length];
        var ends = new Pixel[positions.Length];

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

        using SKPaint paint = new();
        lineStyle.Render(rp.Canvas, starts, ends, paint);
    }
}
