using ScottPlot.Axis;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ScottPlot.Plottables
{
    public struct Box
    {
        public double Position { get; set; }
        public double BoxMin { get; set; }
        public double BoxMiddle { get; set; }
        public double BoxMax { get; set; }
        public double? WhiskerMin { get; set; }
        public double? WhiskerMax { get; set; }
    }

    public class BoxSeries
    {
        public IList<Box> Boxes { get; set; } = Array.Empty<Box>();
        public string? Label { get; set; }
        public FillStyle Fill { get; set; } = new();
        public LineStyle Stroke { get; set; } = new();
    }

    public class BoxPlot : IPlottable
    {
        public bool IsVisible { get; set; } = true;
        public IAxes Axes { get; set; } = Axis.Axes.Default;
        public string? Label { get; set; }
        public IList<BoxSeries> Series { get; set; }
        public Orientation Orientation { get; set; } = Orientation.Vertical;

        public double Padding { get; set; } = 0.05;
        private double MaxBoxWidth => 1 - Padding * 2;

        public bool GroupBoxesWithSameXPosition = true;

        public BoxPlot(IList<BoxSeries> series)
        {
            Series = series;
        }

        public IEnumerable<LegendItem> LegendItems => EnumerableExtensions.One(
            new LegendItem
            {
                Label = Label,
                Children = Series.Select(boxSeries => new LegendItem
                {
                    Label = boxSeries.Label,
                    Fill = boxSeries.Fill
                })
            });

        public AxisLimits GetAxisLimits()
        {
            AxisLimits limits = new(double.PositiveInfinity, double.NegativeInfinity, double.PositiveInfinity, double.NegativeInfinity);

            foreach (var s in Series)
            {
                foreach (var b in s.Boxes)
                {
                    if (Orientation == Orientation.Vertical)
                    {
                        limits.ExpandX(b.Position);
                        limits.ExpandY(b.BoxMin);
                        limits.ExpandY(b.BoxMiddle);
                        limits.ExpandY(b.BoxMax);
                        limits.ExpandY(b.WhiskerMin ?? limits.Rect.YMin);
                        limits.ExpandY(b.WhiskerMax ?? limits.Rect.YMin);
                    }
                    else
                    {
                        limits.ExpandY(b.Position);
                        limits.ExpandX(b.BoxMin);
                        limits.ExpandX(b.BoxMiddle);
                        limits.ExpandX(b.BoxMax);
                        limits.ExpandX(b.WhiskerMin ?? limits.Rect.YMin);
                        limits.ExpandX(b.WhiskerMax ?? limits.Rect.YMin);
                    }
                }
            }

            limits.Rect.XMin -= MaxBoxWidth / 2;
            limits.Rect.XMax += MaxBoxWidth / 2;
            limits.Rect.YMin -= MaxBoxWidth / 2;
            limits.Rect.YMax += MaxBoxWidth / 2;


            return limits;
        }

        public void Render(SKSurface surface)
        {
            using var paint = new SKPaint();
            var boxesByXCoordinate = Series
                .SelectMany(s => s.Boxes.Select(b => (Box: b, Series: s)))
                .ToLookup(t => t.Box.Position);

            int maxPerXCoordinate = boxesByXCoordinate.Max(g => g.Count());
            double widthPerGroup = 1 - (maxPerXCoordinate + 1) * Padding;
            double boxWidth = (1 - Padding) * widthPerGroup / maxPerXCoordinate;

            foreach (IGrouping<double, (Box Box, BoxSeries Series)>? group in boxesByXCoordinate)
            {
                int boxesInGroup = group.Count();
                int i = 0;
                foreach (var t in group)
                {
                    double boxWidthAndPadding = boxWidth + Padding;
                    double groupWidth = boxWidthAndPadding * boxesInGroup;

                    double newPosition = GroupBoxesWithSameXPosition ?
                        group.Key - groupWidth / 2 + (i + 0.5) * boxWidthAndPadding :
                        group.Key;

                    DrawBox(surface, paint, t.Box, t.Series, newPosition, boxWidth);
                    if (t.Box.WhiskerMin.HasValue)
                        DrawWhisker(surface, paint, t.Box, t.Series, newPosition, boxWidth, t.Box.WhiskerMin.Value);

                    if (t.Box.WhiskerMax.HasValue)
                        DrawWhisker(surface, paint, t.Box, t.Series, newPosition, boxWidth, t.Box.WhiskerMax.Value);
                    i++;
                }
            }
        }

        public (PixelRect, PixelRect) GetRects(Box box, double x, double width)
        {
            if (Orientation == Orientation.Vertical)
            {
                var topLeft = Axes.GetPixel(new Coordinates(x - width / 2, box.BoxMax));
                var midRight = Axes.GetPixel(new Coordinates(x + width / 2, box.BoxMiddle));
                var botLeft = Axes.GetPixel(new Coordinates(x - width / 2, box.BoxMin));

                var topRect = new PixelRect(topLeft, midRight);
                var botRect = new PixelRect(midRight, botLeft);

                return (topRect, botRect);
            }
            else
            {
                var topLeft = Axes.GetPixel(new Coordinates(box.BoxMin, x - width / 2));
                var midRight = Axes.GetPixel(new Coordinates(box.BoxMiddle, x + width / 2));
                var botLeft = Axes.GetPixel(new Coordinates(box.BoxMax, x - width / 2));

                var topRect = new PixelRect(topLeft, midRight);
                var botRect = new PixelRect(midRight, botLeft);

                return (topRect, botRect);
            }
        }

        public void DrawBox(SKSurface surface, SKPaint paint, Box box, BoxSeries series, double x, double width)
        {
            (PixelRect topRect, PixelRect botRect) = GetRects(box, x, width);

            series.Fill.ApplyToPaint(paint);
            surface.Canvas.DrawRect(topRect.ToSKRect(), paint);
            surface.Canvas.DrawRect(botRect.ToSKRect(), paint);

            series.Stroke.ApplyToPaint(paint);
            // Done individually with DrawLine rather than with DrawRect to avoid double-stroking the middle line
            surface.Canvas.DrawLine(topRect.TopLeft.ToSKPoint(), topRect.TopRight.ToSKPoint(), paint);
            surface.Canvas.DrawLine(topRect.BottomLeft.ToSKPoint(), topRect.BottomRight.ToSKPoint(), paint);
            surface.Canvas.DrawLine(botRect.BottomLeft.ToSKPoint(), botRect.BottomRight.ToSKPoint(), paint);

            surface.Canvas.DrawLine(topRect.TopLeft.ToSKPoint(), botRect.BottomLeft.ToSKPoint(), paint);
            surface.Canvas.DrawLine(topRect.TopRight.ToSKPoint(), botRect.BottomRight.ToSKPoint(), paint);
        }

        private void DrawWhisker(SKSurface surface, SKPaint paint, Box box, BoxSeries series, double x, double boxWidth, double value)
        {
            Coordinates whiskerBase = value > box.BoxMax ? new(x, box.BoxMax) : new(x, box.BoxMin);
            Coordinates whiskerTip = new(x, value);

            Pixel whiskerBasePx = Axes.GetPixel(whiskerBase);
            Pixel whiskerTipPx = Axes.GetPixel(whiskerTip);

            series.Stroke.ApplyToPaint(paint);
            surface.Canvas.DrawLine(whiskerBasePx.ToSKPoint(), whiskerTipPx.ToSKPoint(), paint);

            float whiskerWidth = Math.Max((float)Axes.XAxis.GetPixelDistance(boxWidth, surface.GetPixelRect()) / 5, 20);
            Pixel whiskerLeft = whiskerTipPx + new Pixel(-whiskerWidth / 2, 0);
            Pixel whiskerRight = whiskerTipPx + new Pixel(whiskerWidth / 2, 0);

            surface.Canvas.DrawLine(whiskerLeft.ToSKPoint(), whiskerRight.ToSKPoint(), paint);
        }
    }
}
