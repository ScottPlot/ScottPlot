namespace ScottPlot.Legends;

public static class Rendering
{
    public static void Render(Legend legend, RenderPack rp)
    {
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

    public static void RenderLegend(Legend legend, SKCanvas canvas, LegendPack lp)
    {
        using SKPaint paint = new();

        // render the legend panel
        Drawing.FillRectangle(canvas, lp.LegendShadowRect, legend.ShadowFill.Color);
        Drawing.FillRectangle(canvas, lp.LegendRect, legend.BackgroundFill.Color);
        Drawing.DrawRectangle(canvas, lp.LegendRect, legend.OutlineStyle.Color, legend.OutlineStyle.Width);

        // render all items inside the legend
        float xOffset = lp.Offset.X;
        float prevHeight = 0;
        float yOffset = lp.Offset.Y;
        foreach (ItemSizeAndChildren item in lp.SizedItems)
        {
            bool isHorizontal = legend.Orientation == Orientation.Horizontal;
            bool isWider = (xOffset + item.Size.WithChildren.Width) > lp.LegendRect.Right;

            if (isHorizontal && isWider)
            {
                yOffset += prevHeight;
                xOffset = lp.Offset.X;
                prevHeight = 0;
            }

            if (item.Item.CustomFontStyle is not null)
            {
                item.Item.CustomFontStyle.ApplyToPaint(paint);
            }
            else
            {
                legend.Font.ApplyToPaint(paint);
            }

            RenderItem(
                canvas: canvas,
                paint: paint,
                sizedItem: item,
                x: xOffset,
                y: yOffset,
                symbolWidth: legend.SymbolWidth,
                symbolPadRight: legend.SymbolLabelSeparation,
                itemPadding: legend.ItemPadding);

            if (legend.Orientation == Orientation.Horizontal)
            {
                xOffset += item.Size.WithChildren.Width;
                prevHeight = Math.Max(prevHeight, item.Size.WithChildren.Height);
            }
            else
            {
                yOffset += item.Size.WithChildren.Height;
            }
        }
    }

    /// <summary>
    /// Render a leger item: its label, symbol, and all its children
    /// </summary>
    private static void RenderItem(SKCanvas canvas, SKPaint paint,
        ItemSizeAndChildren sizedItem, float x, float y,
        float symbolWidth, float symbolPadRight, PixelPadding itemPadding)
    {
        LegendItem item = sizedItem.Item;

        SKPoint textPoint = new(x, y + paint.TextSize);
        float ownHeight = sizedItem.Size.OwnSize.Height;

        if (item.HasArrow)
        {
            RenderArrow(
                canvas: canvas,
                item: item,
                x: x,
                y: y + itemPadding.Bottom,
                height: ownHeight - itemPadding.Vertical,
                symbolWidth: symbolWidth);

            textPoint.X += symbolWidth + symbolPadRight;
        }
        else if (item.HasSymbol)
        {
            RenderSymbol(
                canvas: canvas,
                item: item,
                x: x,
                y: y + itemPadding.Bottom,
                height: ownHeight - itemPadding.Vertical,
                symbolWidth: symbolWidth);

            textPoint.X += symbolWidth + symbolPadRight;
        }

        using SKAutoCanvasRestore _ = new(canvas);
        if (!string.IsNullOrEmpty(item.Label))
        {
            canvas.DrawText(item.Label, textPoint, paint);
            canvas.Translate(itemPadding.Left, 0);
        }

        y += ownHeight;
        foreach (var curr in sizedItem.Children)
        {
            RenderItem(canvas, paint, curr, x, y, symbolWidth, symbolPadRight, itemPadding);
            y += curr.Size.WithChildren.Height;
        }
    }

    /// <summary>
    /// Render just the symbol of a legend
    /// </summary>
    public static void RenderSymbol(
        SKCanvas canvas,
        LegendItem item,
        float x,
        float y,
        float height,
        float symbolWidth)
    {
        // TODO: make LegendSymbol its own object that include size and padding

        PixelRect rect = new(x, x + symbolWidth, y + height, y);

        using SKPaint paint = new();

        if (item.Line is not null && item.Line.Width > 0)
        {
            item.Line.ApplyToPaint(paint);
            canvas.DrawLine(new(rect.Left, rect.VerticalCenter), new(rect.Right, rect.VerticalCenter), paint);
        }

        if (item.Marker.IsVisible)
        {
            Pixel px = new(rect.HorizontalCenter, rect.VerticalCenter);
            Drawing.DrawMarker(canvas, paint, px, item.Marker);
        }

        if (item.Fill.HasValue)
        {
            item.Fill.ApplyToPaint(paint, rect);
            canvas.DrawRect(rect.ToSKRect(), paint);
        }
    }

    public static void RenderArrow(
        SKCanvas canvas,
        LegendItem item,
        float x,
        float y,
        float height,
        float symbolWidth)
    {
        using SKPaint paint = new();
        item.Line.ApplyToPaint(paint);
        paint.Style = SKPaintStyle.StrokeAndFill;

        using SKPath path = PathStrategies.Arrows.GetPath([
                new RootedPixelVector(new(x, y + height / 2), new(symbolWidth, 0))
            ], new ArrowStyle()
            {
                Anchor = ArrowAnchor.Tail,
                LineStyle = item.Line
            }, maxBladeWidth: height / 2);

        canvas.DrawPath(path, paint);
    }
}
