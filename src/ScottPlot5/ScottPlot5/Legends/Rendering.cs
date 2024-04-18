namespace ScottPlot.Legends;

public static class Rendering
{
    public static bool ShowDebugLines { get; set; } = false;

    public static void Render(Legend legend, RenderPack rp)
    {
        if (legend.GetItems().Length == 0)
            return;

        LegendLayout lp = LegendLayoutFactory.LegendOnPlot(legend, rp.DataRect);
        RenderLegend(legend, rp.Canvas, lp);
    }

    public static void AsSvg(Legend legend, Stream svgStream, int maxWidth = 0, int maxHeight = 0)
    {
        if (svgStream is null)
            throw new NullReferenceException($"invalid Stream");

        LegendLayout lp = LegendLayoutFactory.StandaloneLegend(legend, maxWidth, maxHeight);

        SKRect rect = new(0, 0, lp.LegendRect.Width, lp.LegendRect.Height);
        using SKCanvas canvas = SKSvgCanvas.Create(rect, svgStream);
        RenderLegend(legend, canvas, lp);
    }

    public static Image GetImage(Legend legend, int maxWidth = 0, int maxHeight = 0)
    {
        LegendLayout lp = LegendLayoutFactory.StandaloneLegend(legend, maxWidth, maxHeight);

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
        LegendLayout lp = LegendLayoutFactory.StandaloneLegend(legend, 0, 0);

        int width = (int)Math.Ceiling(lp.LegendRect.Width);
        int height = (int)Math.Ceiling(lp.LegendRect.Height);

        using SvgImage svg = new(width, height);

        RenderLegend(legend, svg.Canvas, lp);

        return svg.GetXml();
    }

    private static void RenderLegend(Legend legend, SKCanvas canvas, LegendLayout lp)
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
            PixelRect labelRect = lp.LabelRects[i];
            PixelRect symbolRect = lp.SymbolRects[i];
            PixelRect symbolFillRect = symbolRect.Contract(0, symbolRect.Height * .2f);
            PixelRect symbolFillOutlineRect = symbolFillRect.Expand(1 - item.OutlineWidth);
            PixelLine symbolLine = new(symbolRect.RightCenter, symbolRect.LeftCenter);

            if (ShowDebugLines)
            {
                Drawing.DrawRectangle(canvas, symbolRect, Colors.Black.WithAlpha(.2), 1);
                Drawing.DrawRectangle(canvas, labelRect, Colors.Black.WithAlpha(.2), 1);
            }

            item.LabelStyle.Render(canvas, labelRect.LeftCenter, paint);
            item.LineStyle.Render(canvas, symbolLine, paint);
            item.FillStyle.Render(canvas, symbolFillRect, paint);
            item.OutlineStyle.Render(canvas, symbolFillOutlineRect, paint);
            item.MarkerStyle.Render(canvas, symbolRect.Center, paint);
            item.ArrowStyle.Render(canvas, symbolLine, paint);
        }
    }
}
