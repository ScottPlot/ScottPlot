using ScottPlot.DataSources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScottPlot.Plottables
{
    public class FunctionPlot : IPlottable
    {
        public bool IsVisible { get; set; } = true;
        public IAxes Axes { get; set; } = new Axes();

        public string? Label { get; set; }
        public LineStyle LineStyle { get; } = new();
        IFunctionSource Source { get; set; }
        public FunctionPlot(IFunctionSource source)
        {
            Source = source;
        }

        private double MinX => Math.Min(Source.RangeX.Min.FiniteCoallesce(Axes.XAxis.Min), Axes.XAxis.Min);
        private double MaxX => Math.Max(Source.RangeX.Max.FiniteCoallesce(Axes.XAxis.Max), Axes.XAxis.Max);

        public IEnumerable<LegendItem> LegendItems => EnumerableExtensions.One(
            new LegendItem
            {
                Label = Label,
                Marker = MarkerStyle.None,
                Line = LineStyle,
            });

        public AxisLimits GetAxisLimits()
        {
            var xMin = Source.RangeX.Min.FiniteCoallesce(double.NaN);
            var xMax = Source.RangeX.Max.FiniteCoallesce(double.NaN);

            if (!double.IsNaN(xMin) && !double.IsNaN(xMax))
            {
                var yRange = Source.GetRangeY(new(xMin, xMax));
                return new AxisLimits(xMin, xMax, yRange.Min, yRange.Max);
            }

            return new AxisLimits(xMin, xMax, double.NaN, double.NaN);
        }

        public void Render(RenderPack rp)
        {
            var unitsPerPixel = Axes.XAxis.GetCoordinateDistance(1, rp.DataRect);

            using SKPath path = new();
            bool penIsDown = false;
            for (double x = MinX; x <= MaxX; x += unitsPerPixel)
            {
                double y = Source.Get(x);
                if (y.IsInfiniteOrNaN())
                {
                    penIsDown = false; // Picking up pen allows us to skip over regions where the function is undefined
                    continue;
                }

                var px = Axes.GetPixel(new(x, y));

                if (penIsDown)
                {
                    path.LineTo(px.ToSKPoint());
                }
                else
                {
                    path.MoveTo(px.ToSKPoint());
                    penIsDown = true;
                }
            }

            using SKPaint paint = new();
            LineStyle.ApplyToPaint(paint);

            rp.Canvas.DrawPath(path, paint);
        }
    }
}
