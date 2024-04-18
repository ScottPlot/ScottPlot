namespace ScottPlot.Legends;

public static class Rendering
{
    public static void Render(Legend legend, RenderPack rp)
    {
        if (legend.GetItems().Length == 0)
            return;

        LegendPack lp = LegendPackFactory.LegendOnPlot(legend, rp.DataRect);
        RenderLegend(legend, rp.Canvas, lp);
    }

    public static void AsSvg(Legend legend, Stream svgStream, int maxWidth = 0, int maxHeight = 0)
    {
        if (svgStream is null)
            throw new NullReferenceException($"invalid Stream");

        LegendPack lp = LegendPackFactory.StandaloneLegend(legend, maxWidth, maxHeight);

        SKRect rect = new(0, 0, lp.LegendRect.Width, lp.LegendRect.Height);
        using SKCanvas canvas = SKSvgCanvas.Create(rect, svgStream);
        RenderLegend(legend, canvas, lp);
    }

    public static Image GetImage(Legend legend, int maxWidth = 0, int maxHeight = 0)
    {
        LegendPack lp = LegendPackFactory.StandaloneLegend(legend, maxWidth, maxHeight);

        SKImageInfo info = new(
            width: Math.Max(1, (int)Math.Ceiling(lp.LegendRect.Width)),
            height: Math.Max(1, (int)Math.Ceiling(lp.LegendRect.Height)),
            colorType: SKColorType.Rgba8888,
            alphaType: SKAlphaType.Premul);

        using SKSurface surface = SKSurface.Create(info)
            ?? throw new NullReferenceException($"invalid SKImageInfo");

        RenderLegend(legend, surface.Canvas, lp);

        return new Image(surface);
    }

    public static string GetSvgXml(Legend legend)
    {
        LegendPack lp = LegendPackFactory.StandaloneLegend(legend, 0, 0);

        int width = (int)Math.Ceiling(lp.LegendRect.Width);
        int height = (int)Math.Ceiling(lp.LegendRect.Height);

        using SvgImage svg = new(width, height);

        RenderLegend(legend, svg.Canvas, lp);

        return svg.GetXml();
    }

    private static void RenderLegend(Legend legend, SKCanvas canvas, LegendPack lp)
    {
        using SKPaint paint = new();

        // render the legend panel
        PixelRect shadowRect = lp.LegendRect.WithOffset(legend.ShadowOffset);
        Drawing.FillRectangle(canvas, shadowRect, paint, legend.ShadowFill);
        Drawing.FillRectangle(canvas, lp.LegendRect, paint, legend.BackgroundFill);
        Drawing.DrawRectangle(canvas, lp.LegendRect, paint, legend.OutlineStyle);

        // render items inside the legend
        for (int i = 0; i < lp.LegendItems.Length; i++)
        {
            LegendItem item = lp.LegendItems[i];
            PixelRect rect = lp.LegendItemRects[i];
            RenderLegendItem(item, rect, canvas, paint);
        }
    }

    private static void RenderLegendItem(LegendItem item, PixelRect rect, SKCanvas canvas, SKPaint paint)
    {
        Drawing.DrawRectangle(canvas, rect, Colors.Black.WithAlpha(.2), 1);
        item.LabelStyle.Render(canvas, rect.LeftCenter, paint);
    }
}
