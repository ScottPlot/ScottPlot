using ScottPlot.Drawing;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Xml;

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
        public Color Color { get => LineColor; set => LineColor = value; }
        public Color LineColor { get; set; } = Color.Black;
        public FillType FillType { get; set; } = FillType.NoFill;
        public Color FillColor { get; set; } = Color.FromArgb(50, Color.Black);
        public double XMin { get; set; } = double.NegativeInfinity;
        public double XMax { get; set; } = double.PositiveInfinity;
        public AxisLimits AxisLimits { get; set; } = AxisLimits.NoLimits;
        public AxisLimits GetAxisLimits() => AxisLimits;

        public FunctionPlot(Func<double, double?> function)
        {
            Function = function;
        }

        public int PointCount { get; private set; }

        private PointF[] GetPoints(PlotDimensions dims)
        {
            List<PointF> points = new();

            double xStart = XMin.IsFinite() ? XMin : dims.XMin;
            double xEnd = XMax.IsFinite() ? XMax : dims.XMax;
            double width = xEnd - xStart;

            PointCount = (int)(width * dims.PxPerUnitX) + 1;

            for (int columnIndex = 0; columnIndex < PointCount; columnIndex++)
            {
                double x = columnIndex * dims.UnitsPerPxX + xStart;
                double? y = Function(x);

                if (y is null)
                {
                    Debug.WriteLine($"Y({x}) failed because y was null");
                    continue;
                }

                if (double.IsNaN(y.Value) || double.IsInfinity(y.Value))
                {
                    Debug.WriteLine($"Y({x}) failed because y was not a real number");
                    continue;
                }

                float xPx = dims.GetPixelX(x);
                float yPx = dims.GetPixelY(y.Value);
                points.Add(new PointF(xPx, yPx));
            }

            return points.ToArray();
        }

        public void Render(PlotDimensions dims, Bitmap bmp, bool lowQuality = false)
        {
            PointF[] points = GetPoints(dims);
            using var gfx = GDI.Graphics(bmp, dims, lowQuality);
            using var penLine = GDI.Pen(LineColor, LineWidth, LineStyle, true);
            if (FillType == FillType.FillAbove || FillType == FillType.FillBelow)
            {
                bool above = FillType == FillType.FillAbove;
                GDI.FillToInfinity(dims, gfx, points.First().X, points.Last().X, points, above, FillColor, FillColor);
            }
            gfx.DrawLines(penLine, points);
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
            var singleItem = new LegendItem(this)
            {
                label = Label,
                color = Color,
                lineStyle = LineStyle,
                lineWidth = LineWidth,
                markerShape = MarkerShape.none
            };
            return LegendItem.Single(singleItem);
        }
    }
}
