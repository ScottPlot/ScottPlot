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

    /// <summary>
    /// When set to false the grid will be rendered on top of plottables instead of beneath them.
    /// </summary>
    public bool IsBeneathPlottables { get; set; } = true;

    /// <summary>
    /// Fill the region between every other pair of major grid lines with this color.
    /// </summary>
    public Color FillColor1 { get; set; } = Colors.Transparent;

    /// <summary>
    /// Fill the region between every other pair of major grid lines with this color.
    /// </summary>
    public Color FillColor2 { get; set; } = Colors.Transparent;

    private bool FillColorEnabled => FillColor1 != Colors.Transparent || FillColor2 != Colors.Transparent;

    public void Render(RenderPack rp, IAxis axis, IEnumerable<Tick> ticks)
    {
        if (!IsVisible)
        {
            return;
        }

        float[] xTicksMinor = ticks
            .Where(x => !x.IsMajor)
            .Select(x => axis.GetPixel(x.Position, rp.DataRect))
            .Take(MaximumNumberOfGridLines)
            .ToArray();

        float[] xTicksMajor = ticks
            .Where(x => x.IsMajor)
            .Select(x => axis.GetPixel(x.Position, rp.DataRect))
            .Take(MaximumNumberOfGridLines)
            .ToArray();

        int oddTick = 0; // 0 for even, 1 for odd.

        var Ticks = ticks
            .Where(x => x.IsMajor)
            .Take(MaximumNumberOfGridLines)
            .Take(2) // need only first 2
            .ToArray();
        if (Ticks.Length >= 2 && (Ticks[1].Position - Ticks[0].Position != 0))
            oddTick = (int)(Ticks[0].Position / (Ticks[1].Position - Ticks[0].Position)) % 2;

        RenderGridLines(rp, xTicksMinor, axis.Edge, MinorLineStyle);
        RenderAlternatingFill(rp, xTicksMajor, axis.Edge, oddTick);
        RenderGridLines(rp, xTicksMajor, axis.Edge, MajorLineStyle);
    }

    private void RenderAlternatingFill(RenderPack rp, float[] positions, Edge edge, int oddTick)
    {
        if (!FillColorEnabled)
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
            Color fillColor = (i + oddTick) % 2 == 0 ? FillColor1 : FillColor2;
            PixelRect rect = new(starts.ElementAt(i - 1), ends.ElementAt(i));
            Drawing.FillRectangle(rp.Canvas, rect, fillColor);
        }
    }

    private static void RenderGridLines(RenderPack rp, float[] positions, Edge edge, LineStyle lineStyle)
    {
        if (!lineStyle.CanBeRendered)
            return;

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
