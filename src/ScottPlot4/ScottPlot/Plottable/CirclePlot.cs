using System;
using System.Drawing;

namespace ScottPlot.Plottable
{
    public class CirclePlot : IPlottable
    {
        /// <summary>
        /// Horizontal center of the circle (axis units)
        /// </summary>
        double X { get; }

        /// <summary>
        /// Vertical center of the circle (axis units)
        /// </summary>
        double Y { get; }

        /// <summary>
        /// Radius of the circle (axis units)
        /// </summary>
        public double Radius { get; set; }

        /// <summary>
        /// Thickness of circle outline (pixel units)
        /// </summary>
        public Color LineColor { get; set; } = Color.Black;

        /// <summary>
        /// Thickness of circle outline (pixel units)
        /// </summary>
        public double LineWidth { get; set; } = 2;

        /// <summary>
        /// Style of circle outline (pixel units)
        /// </summary>
        public LineStyle LineStyle { get; set; } = LineStyle.Solid;

        /// <summary>
        /// Represents a circle centered at (x, y) with a given radius
        /// </summary>
        public CirclePlot(double x, double y, double radius)
        {
            X = x;
            Y = y;
            Radius = radius;
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
            using var pen = ScottPlot.Drawing.GDI.Pen(LineColor, LineWidth, LineStyle);

            // Use 'dims' methods to convert between axis coordinates and pixel positions
            float xPixel = dims.GetPixelX(X);
            float yPixel = dims.GetPixelY(Y);

            // Use 'dims' to determine how large the radius is in pixel units
            float xRadiusPixels = dims.GetPixelX(X + Radius) - xPixel;
            float yRadiusPixels = dims.GetPixelY(Y + Radius) - yPixel;

            // Render data by drawing on the Graphics object
            gfx.DrawEllipse(pen, xPixel - xRadiusPixels, yPixel - yRadiusPixels, xRadiusPixels * 2, yRadiusPixels * 2);
        }
    }
}
