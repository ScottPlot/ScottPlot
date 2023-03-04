using System;
using System.Drawing;

namespace ScottPlot.Plottable
{
    public class Ellipse : IPlottable
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
        /// Horizontal radius (axis units)
        /// </summary>
        public double RadiusX { get; set; }

        /// <summary>
        /// Vertical radius (axis units)
        /// </summary>
        public double RadiusY { get; set; }

        /// <summary>
        /// Outline color
        /// </summary>
        public Color LineColor { get; set; } = Color.Black;

        /// <summary>
        /// Outline thickness (pixel units)
        /// </summary>
        public double LineWidth { get; set; } = 2;

        /// <summary>
        /// Outline line style
        /// </summary>
        public LineStyle LineStyle { get; set; } = LineStyle.Solid;

        /// <summary>
        /// Create an ellipse centered at (x, y) with the given horizontal and vertical radius
        /// </summary>
        public Ellipse(double x, double y, double xRadius, double yRadius)
        {
            X = x;
            Y = y;
            RadiusX = xRadius;
            RadiusY = yRadius;
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
            return new AxisLimits(
                xMin: X - RadiusX,
                xMax: X + RadiusX, 
                yMin: Y - RadiusY, 
                yMax: Y + RadiusY);
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
            float xRadiusPixels = dims.GetPixelX(X + RadiusX) - xPixel;
            float yRadiusPixels = dims.GetPixelY(Y + RadiusY) - yPixel;

            // Render data by drawing on the Graphics object
            gfx.DrawEllipse(pen, xPixel - xRadiusPixels, yPixel - yRadiusPixels, xRadiusPixels * 2, yRadiusPixels * 2);
        }
    }
}
