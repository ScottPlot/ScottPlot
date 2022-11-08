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
        public IList<Bar> Bars { get; set; }
        public string? Label { get; set; }
        public Fill Fill { get; set; }
    }

    public class BarPlot : IPlottable
    {
        public bool IsVisible { get; set; } = true;
        public IAxes Axes { get; set; } = Axis.Axes.Default;
        public string? Label { get; set; }
        public IList<BarSeries> Series { get; set; }
        public double BarWidth { get; set; } = 0.8;
        public Orientation Orientation { get; set; } = Orientation.Vertical;
        public Stroke Stroke { get; set; } = new();

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

        // TODO: Multiple bars on the same coordinate
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

            limits.Rect.XMin -= BarWidth / 2;
            limits.Rect.XMax += BarWidth / 2;
            limits.Rect.YMin -= BarWidth / 2;
            limits.Rect.YMax += BarWidth / 2;

            return limits;
        }

        // TODO: Multiple bars on the same coordinate
        public void Render(SKSurface surface)
        {
            using var paint = new SKPaint();

            foreach (var s in Series)
            {
                foreach (var bar in s.Bars)
                {
                    var rect = GetRect(bar);

                    paint.SetFill(s.Fill);
                    surface.Canvas.DrawRect(rect, paint);

                    paint.SetStroke(Stroke);
                    surface.Canvas.DrawRect(rect, paint);
                }
            }
        }

        private SKRect GetRect(Bar bar)
        {
            return Orientation switch
            {
                // Left, top, right, bottom
                Orientation.Vertical => new SKRect(
                        Axes.GetPixelX(bar.Position - BarWidth / 2),
                        Axes.GetPixelY(bar.Value),
                        Axes.GetPixelX(bar.Position + BarWidth / 2),
                        Axes.GetPixelY(bar.ValueBase)
                    ),
                Orientation.Horizontal => new SKRect(
                        Axes.GetPixelX(bar.ValueBase),
                        Axes.GetPixelY(bar.Position - BarWidth / 2),
                        Axes.GetPixelX(bar.Value),
                        Axes.GetPixelY(bar.Position + BarWidth / 2)
                    ),
                _ => throw new NotImplementedException(),
            };
        }
    }
}
