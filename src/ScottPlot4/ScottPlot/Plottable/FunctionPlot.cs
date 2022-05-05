using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;

namespace ScottPlot.Plottable
{
    /// <summary>
    /// A function plot displays a curve using a function (Y as a function of X)
    /// </summary>
    public class FunctionPlot : IPlottable, IHasLine, IHasColor
    {
        /// <summary>
        /// The function to translate an X to a Y (or null if undefined)
        /// </summary>
        public Func<double, double?> Function;

        // customizations
        public bool IsVisible { get; set; } = true;
        public int XAxisIndex { get; set; } = 0;
        public int YAxisIndex { get; set; } = 0;
        public double LineWidth { get; set; } = 1;
        public LineStyle LineStyle { get; set; } = LineStyle.Solid;
        public string Label { get; set; }
        public Color Color { get; set; } = Color.Black;
        public Color LineColor { get; set; } = Color.Black;

        public FunctionPlot(Func<double, double?> function)
        {
            Function = function;
        }

        public AxisLimits GetAxisLimits()
        {
            double max = double.NegativeInfinity;
            double min = double.PositiveInfinity;

            foreach (double x in DataGen.Range(-10, 10, .1))
            {
                double? y = Function(x);
                if (y != null)
                {
                    max = Math.Max(max, y.Value);
                    min = Math.Min(min, y.Value);
                }
            }

            // TODO: should X limits be null or NaN?
            return new AxisLimits(-10, 10, min, max);
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
                    double? y = Function(x);

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
                Color = Color,
                LineWidth = LineWidth,
                MarkerSize = 0,
                Label = Label,
                MarkerShape = MarkerShape.none,
                LineStyle = LineStyle
            };
            scatter.Render(dims, bmp, lowQuality);
        }

        public void ValidateData(bool deepValidation = false)
        {
            if (Function is null)
                throw new InvalidOperationException("function cannot be null");
        }

        public override string ToString()
        {
            string label = string.IsNullOrWhiteSpace(this.Label) ? "" : $" ({this.Label})";
            return $"PlottableFunction{label} displaying {PointCount} points";
        }

        public LegendItem[] GetLegendItems()
        {
            var singleLegendItem = new LegendItem(this)
            {
                label = Label,
                color = Color,
                lineStyle = LineStyle,
                lineWidth = LineWidth,
                markerShape = MarkerShape.none
            };
            return new LegendItem[] { singleLegendItem };
        }
    }
}
