using ScottPlot.Plottable;
using System;
using System.Drawing;

namespace ScottPlot.Plottable
{
    public class CirclePlot : IPlottable
    {
        double X { get; }

        double Y { get; }

        // radius
        public float Radius { get; set; }

        /// <summary>
        /// Color of the circle
        /// </summary>
        public Color Color { get; set; } = Color.Black;

        /// <summary>
        /// Thickness of circle line
        /// </summary>
        public double LineWidth { get; set; } = 2;

        /// <summary>
        /// Style of the circle line
        /// </summary>
        public LineStyle LineStyle { get; set; } = LineStyle.Solid;

        /// <summary>
        /// Creates a circle at position xs, ys with radius size
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="radius"></param>        
        /// <param name="lineWidth"></param>
        /// <param name="lineStyle"></param>
        public CirclePlot(double x, double y, float radius, double lineWidth = 2, LineStyle lineStyle = LineStyle.Solid)
        {
            X = x;
            Y = y;
            Radius = radius;
            LineWidth = lineWidth;
            LineStyle = lineStyle;
        }

        // These default values are fine for most cases
        public bool IsVisible { get; set; } = true;
        public int XAxisIndex { get; set; } = 0;
        public int YAxisIndex { get; set; } = 0;
        public void ValidateData(bool deep = false) { }

        // Return an empty array for plottables that do not appear in the legend
        public LegendItem[] GetLegendItems()
            => Array.Empty<LegendItem>();

        // This method returns the bounds of the data
        public AxisLimits GetAxisLimits()
        {
            double xMin = X - Radius;
            double xMax = X + Radius;
            double yMin = Y - Radius;
            double yMax = Y + Radius;
            return new AxisLimits(xMin, xMax, yMin, yMax);
        }

        // This method describes how to plot the data on the cart.
        public void Render(PlotDimensions dims, System.Drawing.Bitmap bmp, bool lowQuality = false)
        {
            // Use ScottPlot's GDI helper functions to create System.Drawing objects
            using var gfx = ScottPlot.Drawing.GDI.Graphics(bmp, dims, lowQuality);
            using var pen = ScottPlot.Drawing.GDI.Pen(Color, LineWidth, LineStyle);

            // Use 'dims' methods to convert between axis coordinates and pixel positions
            float xPixel = dims.GetPixelX(X);
            float yPixel = dims.GetPixelY(Y);

            // Render data by drawing on the Graphics object
            gfx.DrawEllipse(pen, xPixel, yPixel, Radius * 2, Radius * 2);
        }
    }
}
