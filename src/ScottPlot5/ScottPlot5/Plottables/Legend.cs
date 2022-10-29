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
        public IEnumerable<LegendItem> LegendItems => Enumerable.Empty<LegendItem>();

        public IPosition Position = new CornerPosition(Corner.BottomRight);
        public IEnumerable<LegendItem> Items { get; set; }
        public Font Font { get; set; } = new(); // TODO: Do we want our own abstraction for this? It wraps a native object and implements IDisposable
        public Stroke Border { get; set; } = new(Colors.DarkGray, 1);
        public Fill Background { get; set; } = new Fill(Colors.White);


        public Legend(IEnumerable<LegendItem> legendItems)
        {
            Items = legendItems;
        }

        public void Render(SKSurface surface)
        {
            using SKFont font = Font.GetFont();
            using SKPaint paint = new(font);
            
            PixelSize size = Measure(paint);

            Pixel topLeftCorner = Position.GetPosition(Axes.DataRect, size);

            surface.Canvas.Translate(topLeftCorner.X, topLeftCorner.Y);


            paint.SetFill(Background);

            SKRect border = new(0, 0, size.Width, size.Height);
            surface.Canvas.DrawRect(border, paint);

            paint.SetStroke(Border);
            surface.Canvas.DrawRect(border, paint);

            surface.Canvas.Translate(Border.Width, Border.Width);

            float y = VerticalPadding;
            foreach (var curr in Items)
            {
                RenderLegendItem(surface, paint, curr, y);
                y += Measure(curr, paint).Height;
            }
        }

        private void RenderLegendItem(SKSurface surface, SKPaint paint, LegendItem item, float y)
        {
            if (ShouldHide(item))
            {
                return;
            }

            using ScopedTransform transform = new(surface.Canvas);
            paint.SetFill(new(Colors.Black));

            surface.Canvas.Translate(HorizontalPadding, 0);

            float top = y;

            RenderLegendSymbol(surface, item, new(0, top, SymbolWidth, top + paint.TextSize));

            float x = HasSymbol(item) ? HorizontalPadding + SymbolWidth : 0;

            surface.Canvas.DrawText(item.Label ?? "", new(x, top + paint.TextSize), paint);

            y += Measure(item, paint, false).Height;
            foreach (var curr in item.Children)
            {
                RenderLegendItem(surface, paint, curr, y);
                y += Measure(curr, paint).Height;
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

                Drawing.DrawMarkers(surface, item.Marker.Value, EnumerableHelpers.One<Pixel>(new(rect.MidX, rect.MidY)));
            }

            if (item.Fill.HasValue)
            {
                paint.SetFill(item.Fill.Value);

                surface.Canvas.DrawRect(rect, paint);
            }
        }

        public PixelSize Measure(LegendItem item, SKPaint paint, bool includeChildren = true)
        {
            if (ShouldHide(item))
            {
                return PixelSize.Zero;
            }

            PixelSize labelRect = Drawing.MeasureString(item.Label ?? "", paint);

            float width = HorizontalPadding + SymbolWidth + HorizontalPadding + labelRect.Width + HorizontalPadding;
            float height = string.IsNullOrEmpty(item.Label) ? VerticalPadding / 2 : 2 * VerticalPadding + labelRect.Height;
            var childrenArray = item.Children.ToArray(); // Avoids enumerating the children enumerable multiple times

            if (childrenArray.Length > 0)
            {
                width += HorizontalPadding;
            }

            if (includeChildren)
            {
                for (int i = 0; i < childrenArray.Length; i++)
                {
                    var childSize = Measure(childrenArray[i], paint);
                    width = Math.Max(width, HorizontalPadding + childSize.Width);
                    height += childSize.Height;
                }
            }

            return new(width, height);
        }

        // I'm a Javascript developer, this was bound to happen someday
        public PixelSize Measure(SKPaint paint) =>
            Items
                .Select((LegendItem item) => Measure(item, paint))
                .Aggregate(
                    new PixelSize(Border.Width * 2, Border.Width * 2),
                    (a, b) =>
                        new(
                            Math.Max(a.Width, b.Width),
                            a.Height + b.Height)
                        );

        private bool ShouldHide(LegendItem item) => string.IsNullOrEmpty(item.Label) && !(item.Children?.Any(c => !ShouldHide(c)) ?? false);

        private bool HasSymbol(LegendItem item) => item.Line.HasValue || item.Marker.HasValue || item.Fill.HasValue;
    }
}
