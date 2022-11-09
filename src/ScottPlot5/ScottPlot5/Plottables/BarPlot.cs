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
    public class Bar
    {
        public double Position { get; set; }
        public double Value { get; set; }
        public double ValueBase { get; set; }
    }

    public class BarSeries
    {
        public IList<Bar> Bars { get; set; } = Array.Empty<Bar>();
        public string? Label { get; set; }
        public Fill Fill { get; set; }
    }

    public class BarPlot : IPlottable
    {
        public bool IsVisible { get; set; } = true;
        public IAxes Axes { get; set; } = Axis.Axes.Default;
        public string? Label { get; set; }
        public IList<BarSeries> Series { get; set; }
        public double Padding { get; set; } = 0.05;
        private double MaxBarWidth => 1 - Padding * 2;
        public Orientation Orientation { get; set; } = Orientation.Vertical;
        public Stroke Stroke { get; set; } = new();
        public bool GroupBarsWithSameXPosition = true; // Disable for stacked bar charts

        public BarPlot(IList<BarSeries> series)
        {
            Series = series;
        }

        public IEnumerable<LegendItem> LegendItems => EnumerableHelpers.One(
            new LegendItem
            {
                Label = Label,
                Children = Series.Select(s => new LegendItem
                {
                    Label = s.Label,
                    Fill = s.Fill
                })
            });

        public AxisLimits GetAxisLimits()
        {
            AxisLimits limits = new(double.PositiveInfinity, double.NegativeInfinity, double.PositiveInfinity, double.NegativeInfinity);

            foreach (var s in Series)
            {
                foreach (var b in s.Bars)
                {
                    if (Orientation == Orientation.Vertical)
                    {
                        limits.Expand(b.Position, b.Value);
                        limits.ExpandY(b.ValueBase);
                    }
                    else
                    {
                        limits.Expand(b.Value, b.Position);
                        limits.ExpandX(b.ValueBase);
                    }
                }
            }

            limits.Rect.XMin -= MaxBarWidth / 2;
            limits.Rect.XMax += MaxBarWidth / 2;
            limits.Rect.YMin -= MaxBarWidth / 2;
            limits.Rect.YMax += MaxBarWidth / 2;

            return limits;
        }

        public void Render(SKSurface surface)
        {
            using var paint = new SKPaint();
            var barsByXCoordinate = Series
                .SelectMany(s => s.Bars.Select(b => (Bar: b, Series: s)))
                .ToLookup(t => t.Bar.Position);

            int maxPerXCoordinate = GroupBarsWithSameXPosition ? barsByXCoordinate.Max(g => g.Count()) : 1;
            double widthPerGroup = 1 - (maxPerXCoordinate + 1) * Padding;
            double barWidth = widthPerGroup / maxPerXCoordinate;

            foreach (IGrouping<double, (Bar Bar, BarSeries Series)>? group in barsByXCoordinate)
            {
                int barsInGroup = group.Count();
                int i = 0;
                foreach (var t in group)
                {
                    double barWidthAndPadding = barWidth + Padding;
                    double groupWidth = barWidthAndPadding * barsInGroup;

                    double newPosition = GroupBarsWithSameXPosition ?
                        group.Key - groupWidth / 2 + (i + 0.5) * barWidthAndPadding :
                        group.Key;


                    var rect = GetRect(t.Bar, newPosition, barWidth);

                    paint.SetFill(t.Series.Fill);
                    surface.Canvas.DrawRect(rect, paint);

                    paint.SetStroke(Stroke);
                    surface.Canvas.DrawRect(rect, paint);

                    i++;
                }
            }
        }

        private SKRect GetRect(Bar bar, double pos, double barWidth)
        {
            return Orientation switch
            {
                // Left, top, right, bottom
                Orientation.Vertical => new SKRect(
                        Axes.GetPixelX(pos - barWidth / 2),
                        Axes.GetPixelY(bar.Value),
                        Axes.GetPixelX(pos + barWidth / 2),
                        Axes.GetPixelY(bar.ValueBase)
                    ),
                Orientation.Horizontal => new SKRect(
                        Axes.GetPixelX(bar.ValueBase),
                        Axes.GetPixelY(pos - barWidth / 2),
                        Axes.GetPixelX(bar.Value),
                        Axes.GetPixelY(pos + barWidth / 2)
                    ),
                _ => throw new NotImplementedException(),
            };
        }
    }
}
