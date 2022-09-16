using ScottPlot.Axis;
using ScottPlot.Style;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScottPlot.Plottables
{
    public class Legend : IPlottable
    {
        private const int HorizontalPadding = 5;
        private const int VerticalPadding = 5;
        private const int SymbolWidth = 20;
        public bool IsVisible { get; set; } = true;
        public IAxes Axes { get; set; } = Axis.Axes.Default;
        public AxisLimits GetAxisLimits() => AxisLimits.NoLimits;

        public Corner Position { get; set; } = Corner.BottomRight; // TODO: Should we support positions that aren't corners?
        public IList<LegendItem> LegendItems { get; set; }
        public SKFont Font { get; set; } = new(); // TODO: Do we want our own abstraction for this? It wraps a native object and implements IDisposable
        public Stroke Border { get; set; } = new(Colors.DarkGray, 1);
        public Fill Background { get; set; } = new Fill(Colors.White);


        public Legend(IList<LegendItem> legendItems)
        {
            LegendItems = legendItems;
        }

        public void Render(SKSurface surface)
        {
            PixelSize size = Measure();

            Pixel topLeftCorner = GetTopLeftCorner(size);

            surface.Canvas.Translate(topLeftCorner.X, topLeftCorner.Y);


            using SKPaint paint = new();
            paint.SetFill(Background);

            SKRect border = new(0, 0, size.Width, size.Height);
            surface.Canvas.DrawRect(border, paint);

            paint.SetStroke(Border);
            surface.Canvas.DrawRect(border, paint);

            surface.Canvas.Translate(Border.Width, Border.Width);

            float y = VerticalPadding;
            foreach(var curr in LegendItems)
            {
                RenderLegendItem(surface, curr, y);
                y += Measure(curr).Height;
            }
        }

        private void RenderLegendItem(SKSurface surface, LegendItem item, float y)
        {
            using ScopedTransform transform = new(surface.Canvas);
            using SKPaint paint = new(Font);
            paint.SetFill(new(Colors.Black));

            surface.Canvas.Translate(HorizontalPadding, 0);

            float top = y;

            RenderLegendSymbol(surface, item, new(0, top, SymbolWidth, top + paint.TextSize));
            surface.Canvas.DrawText(item.Label, new(HorizontalPadding + SymbolWidth, top + paint.TextSize), paint);
            
            y += Measure(item, false).Height;
            foreach(var curr in item.Children)
            {
                RenderLegendItem(surface, curr, y);
                y += Measure(curr).Height;
            }
        }

        private void RenderLegendSymbol(SKSurface surface, LegendItem item, SKRect rect)
        {
            using SKPaint paint = new();
            if (item.Line.HasValue)
            {
                paint.SetStroke(item.Line.Value);

                surface.Canvas.DrawLine(new(rect.Left, rect.MidY), new(rect.Right, rect.MidY), paint);
            }

            if (item.Marker.HasValue)
            {
                paint.Style = SKPaintStyle.Fill;
                paint.Color = item.Marker.Value.Color.ToSKColor();

                if (item.Marker.Value.Shape == MarkerShape.Circle)
                {
                    surface.Canvas.DrawCircle(new(rect.MidX, rect.MidY), item.Marker.Value.Size / 2, paint);
                }
                else
                {
                    throw new NotImplementedException();
                }
            }

            if (item.Fill.HasValue)
            {
                paint.SetFill(item.Fill.Value);

                surface.Canvas.DrawRect(rect, paint);
            }
        } 
 
        public PixelSize Measure(LegendItem item, bool includeChildren = true)
        {
            using SKPaint paint = new(Font);

            PixelSize labelRect = Drawing.MeasureString(item.Label ?? "", paint);

            float width = HorizontalPadding + SymbolWidth + HorizontalPadding + labelRect.Width + HorizontalPadding;
            float height = 2 * VerticalPadding + labelRect.Height;

            if (item.Children.Count > 0)
            {
                width += HorizontalPadding;
            }

            if (includeChildren)
            {
                for (int i = 0; i < item.Children.Count; i++)
                {
                    var childSize = Measure(item.Children[i]);
                    width = Math.Max(width, childSize.Width);
                    height += childSize.Height;
                }
            }

            return new(width, height);
        }

        // I'm a Javascript developer, this was bound to happen someday
        public PixelSize Measure() =>
            LegendItems
                .Select((LegendItem item) => Measure(item))
                .Aggregate(
                    new PixelSize(Border.Width * 2, Border.Width * 2),
                    (a, b) => 
                        new(
                            Math.Max(a.Width, b.Width),
                            a.Height + b.Height)
                        );

        private Pixel GetTopLeftCorner(PixelSize size)
        {
            const int margin = 5;

            return Position switch
            {
                Corner.TopLeft => new Pixel(Axes.DataRect.Left, Axes.DataRect.Top) + new Pixel(margin, margin),
                Corner.TopRight => new Pixel(Axes.DataRect.Right - size.Width, Axes.DataRect.Top) + new Pixel(-margin, margin),
                Corner.BottomLeft => new Pixel(Axes.DataRect.Left, Axes.DataRect.Bottom - size.Height) + new Pixel(margin, -margin),
                Corner.BottomRight => new Pixel(Axes.DataRect.Right - size.Width, Axes.DataRect.Bottom - size.Height) + new Pixel(-margin, -margin),
                _ => throw new NotImplementedException(),
            };
        }
    }
}
