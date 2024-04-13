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
    public class Crosshair : IPlottable, IHasLine, IHasColor
    {
        public bool IsVisible { get; set; } = true;
        public int XAxisIndex { get; set; } = 0;
        public int YAxisIndex { get; set; } = 0;
        public string Label { get; set; } = null;

        public HLine HorizontalLine = new();

        public VLine VerticalLine = new();

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
            get => HorizontalLine.LineStyle;
        }

        /// <summary>
        /// Sets the line width for vertical and horizontal lines
        /// </summary>
        public double LineWidth
        {
            set
            {
                HorizontalLine.LineWidth = value;
                VerticalLine.LineWidth = value;
            }
            get => (float)HorizontalLine.LineWidth;
        }

        [Obsolete("Get HorizontalLine.PositionLabelFont and VerticalLine.PositionLabelFont instead.", true)]
        public Drawing.Font LabelFont { get; set; }

        [Obsolete("Get HorizontalLine.PositionLabelBackground and VerticalLine.PositionLabelBackground instead.", true)]
        public Color LabelBackgroundColor { get; set; }

        [Obsolete("Get HorizontalLine.PositionLabel and VerticalLine.PositionLabel instead.", true)]
        public bool PositionLabel { get; set; }

        /// <summary>
        /// Set color for horizontal and vertical lines and their position label backgrounds
        /// </summary>
        public Color Color
        {
            get => HorizontalLine.Color;
            set
            {
                HorizontalLine.Color = value;
                VerticalLine.Color = value;
                HorizontalLine.PositionLabelBackground = value;
                VerticalLine.PositionLabelBackground = value;
            }
        }

        public Color LineColor { get => Color; set { Color = value; } }

        /// <summary>
        /// If true, AxisAuto() will ignore the position of this line when determining axis limits
        /// </summary>
        public bool IgnoreAxisAuto { get; set; } = false;

        public Crosshair()
        {
            LineStyle = LineStyle.Dash;
            LineWidth = 1;
            Color = Color.FromArgb(200, Color.Red);
            HorizontalLine.PositionLabel = true;
            VerticalLine.PositionLabel = true;
        }

        public AxisLimits GetAxisLimits()
        {
            return IgnoreAxisAuto ? AxisLimits.NoLimits : new(X, X, Y, Y);
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

        public void ValidateData(bool deep = false) { }

        public void Render(PlotDimensions dims, Bitmap bmp, bool lowQuality = false)
        {
            if (IsVisible == false)
                return;

            HorizontalLine.Render(dims, bmp, lowQuality);
            VerticalLine.Render(dims, bmp, lowQuality);
        }

        [Obsolete("Use VerticalLine.PositionFormatter()")]
        public bool IsDateTimeX
        {
            get => isDateTimeX;
            set
            {
                isDateTimeX = value;
                VerticalLine.PositionFormatter = value ?
                    position => DateTime.FromOADate(position).ToString(stringFormatX) :
                    position => position.ToString(stringFormatX);
            }
        }

        [Obsolete]
        private bool isDateTimeX = false;

        [Obsolete("Use VerticalLine.PositionFormatter()")]
        public string StringFormatX
        {
            get => stringFormatX;
            set
            {
                stringFormatX = value;
                VerticalLine.PositionFormatter = isDateTimeX ?
                    position => DateTime.FromOADate(position).ToString(stringFormatX) :
                    position => position.ToString(stringFormatX);
            }
        }

        [Obsolete]
        private string stringFormatX = "F2";

        [Obsolete("Use VerticalLine.IsVisible")]
        public bool IsVisibleX
        {
            get => VerticalLine.IsVisible;
            set => VerticalLine.IsVisible = value;
        }

        [Obsolete("Use HorizontalLine.PositionFormatter()")]
        public bool IsDateTimeY
        {
            get => isDateTimeY;
            set
            {
                isDateTimeY = value;
                HorizontalLine.PositionFormatter = value ?
                    position => DateTime.FromOADate(position).ToString(stringFormatY) :
                    (position) => position.ToString(stringFormatY);
            }
        }

        [Obsolete]
        private bool isDateTimeY = false;

        [Obsolete("Use HorizontalLine.PositionFormat()")]
        public string StringFormatY
        {
            get => stringFormatY;
            set
            {
                stringFormatY = value;
                HorizontalLine.PositionFormatter = isDateTimeY ?
                    position => DateTime.FromOADate(position).ToString(stringFormatY) :
                    position => position.ToString(stringFormatY);
            }
        }

        [Obsolete]
        private string stringFormatY = "F2";

        [Obsolete("Use HorizontalLine.IsVisible")]
        public bool IsVisibleY
        {
            get => HorizontalLine.IsVisible;
            set => HorizontalLine.IsVisible = value;
        }
    }
}
