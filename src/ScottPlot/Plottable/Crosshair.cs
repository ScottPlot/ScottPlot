using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScottPlot.Plottable
{
    /// <summary>
    /// The Crosshair plot type draws a vertical and horizontal line to label a point
    /// on the plot and displays the coordinates of that point in labels that overlap
    /// the axis tick labels. 
    /// 
    /// This plot type is typically used in combination with
    /// MouseMove events to track the location of the mouse and/or with plot types that
    /// have GetPointNearest() methods.
    /// </summary>
    public class Crosshair : IPlottable
    {
        public bool IsVisible { get; set; } = true;
        public int XAxisIndex { get; set; } = 0;
        public int YAxisIndex { get; set; } = 0;

        public readonly HLine HorizontalLine = new();

        public readonly VLine VerticalLine = new();

        /// <summary>
        /// X position (axis units) of the vertical line
        /// </summary>
        public double X { get => VerticalLine.X; set => VerticalLine.X = value; }

        /// <summary>
        /// X position (axis units) of the horizontal line
        /// </summary>
        public double Y { get => HorizontalLine.Y; set => HorizontalLine.Y = value; }

        /// <summary>
        /// Sets style for horizontal and vertical lines
        /// </summary>
        public LineStyle LineStyle
        {
            set
            {
                HorizontalLine.LineStyle = value;
                VerticalLine.LineStyle = value;
            }
        }

        /// <summary>
        /// Sets the line width for vertical and horizontal lines
        /// </summary>
        public float LineWidth
        {
            set
            {
                HorizontalLine.LineWidth = value;
                VerticalLine.LineWidth = value;
            }
        }

        /// <summary>
        /// Sets visibility of the text labels for each line drawn over the axis
        /// </summary>
        public bool PositionLabel
        {
            set
            {
                HorizontalLine.PositionLabel = value;
                VerticalLine.PositionLabel = value;
            }
        }

        /// <summary>
        /// Sets color for horizontal and vertical lines and their position label backgrounds
        /// </summary>
        public Color Color
        {
            set
            {
                HorizontalLine.Color = value;
                VerticalLine.Color = value;
                HorizontalLine.PositionLabelBackground = value;
                VerticalLine.PositionLabelBackground = value;
            }
        }

        public Crosshair()
        {
            LineStyle = LineStyle.Dash;
            LineWidth = 1;
            Color = Color.FromArgb(200, Color.Red);
            VerticalLine.PositionLabel = true;
            HorizontalLine.PositionLabel = true;
            PositionLabel = true;
        }

        public AxisLimits GetAxisLimits() => new(double.NaN, double.NaN, double.NaN, double.NaN);

        public LegendItem[] GetLegendItems() => null;

        public void ValidateData(bool deep = false) { }

        public void Render(PlotDimensions dims, Bitmap bmp, bool lowQuality = false)
        {
            if (IsVisible == false)
                return;

            HorizontalLine.Render(dims, bmp, lowQuality);
            VerticalLine.Render(dims, bmp, lowQuality);
        }
    }
}
