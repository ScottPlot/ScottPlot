using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using ScottPlot.Config;
using ScottPlot.Drawing;
using ScottPlot.Renderable;

namespace ScottPlot.Plottable
{
    public class FunctionPlot : IRenderable, IHasLegendItems, IHasAxisLimits, IValidatable
    {
        public Func<double, double?> function;

        // TODO: Capitalize these fields
        public double lineWidth = 1;
        public LineStyle lineStyle = LineStyle.Solid;
        public string label;
        public Color color = Color.Black;
        public bool IsVisible { get; set; } = true;

        public FunctionPlot(Func<double, double?> function)
        {
            this.function = function;
        }

        public AxisLimits2D GetLimits()
        {
            double max = double.NegativeInfinity;
            double min = double.PositiveInfinity;

            foreach (double x in DataGen.Range(-10, 10, .1))
            {
                double? y = function(x);
                if (y != null)
                {
                    max = Math.Max(max, y.Value);
                    min = Math.Min(min, y.Value);
                }
            }

            // TODO: X limits should probably be null or NaN
            return new AxisLimits2D(-10, 10, min, max);
        }

        public int PointCount { get; private set; }

        public void Render(PlotDimensions dims, Bitmap bmp, bool lowQuality = false)
        {
            List<double> xList = new List<double>();
            List<double> yList = new List<double>();

            PointCount = (int)dims.DataWidth;
            for (int columnIndex = 0; columnIndex < dims.DataWidth; columnIndex++)
            {
                double x = columnIndex * dims.UnitsPerPxX + dims.XMin;
                try
                {
                    double? y = function(x);

                    if (y is null)
                        throw new NoNullAllowedException();

                    if (double.IsNaN(y.Value) || double.IsInfinity(y.Value))
                        throw new ArithmeticException("not a real number");

                    xList.Add(x);
                    yList.Add(y.Value);
                }
                catch (Exception e) //Domain error, such log(-1) or 1/0
                {
                    Debug.WriteLine($"Y({x}) failed because {e}");
                    continue;
                }
            }

            // create a temporary scatter plot and use it for rendering
            double[] xs = xList.ToArray();
            double[] ys = yList.ToArray();
            var scatter = new ScatterPlot(xs, ys)
            {
                color = color,
                lineWidth = lineWidth,
                markerSize = 0,
                label = label,
                markerShape = MarkerShape.none,
                lineStyle = lineStyle
            };
            scatter.Render(dims, bmp, lowQuality);
        }

        public string ErrorMessage(bool deepValidation = false)
        {
            if (function is null)
                return "function cannot be null";

            return null;
        }

        public override string ToString()
        {
            string label = string.IsNullOrWhiteSpace(this.label) ? "" : $" ({this.label})";
            return $"PlottableFunction{label} displaying {PointCount} points";
        }

        public LegendItem[] LegendItems
        {
            get
            {
                var singleLegendItem = new LegendItem()
                {
                    label = label,
                    color = color,
                    lineStyle = lineStyle,
                    lineWidth = lineWidth,
                    markerShape = MarkerShape.none
                };
                return new LegendItem[] { singleLegendItem };
            }
        }
    }
}
