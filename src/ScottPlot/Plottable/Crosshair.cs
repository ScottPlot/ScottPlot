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

        /// <summary>
        /// X position (axis units) of the vertical line
        /// </summary>
        public double X { get; set; } = 0;

        /// <summary>
        /// X position (axis units) of the horizontal line
        /// </summary>
        public double Y { get; set; } = 0;

        /// <summary>
        /// Controls the conversion from X position to a text.
        /// https://docs.microsoft.com/en-us/dotnet/standard/base-types/standard-numeric-format-strings
        /// </summary>
        public string StringFormatX = "F2";

        /// <summary>
        /// If true, the X position will be converted to DateTime before applying the format string
        /// </summary>
        public bool IsDateTimeX = false;

        /// <summary>
        /// This function will be used to generate labels from X position when IsDateTimeX is false.
        /// By default, it simply calls the ToString method with StringFormatX.
        /// You can set this function to apply a custom formatter.
        /// </summary>
        public Func<double, string> NumericStringFormatterX;

        /// <summary>
        /// This function will be used to generate labels from X position when IsDateTimeX is true.
        /// By default, it simply calls the ToString method with StringFormatX.
        /// You can set this function to apply a custom formatter.
        /// </summary>
        public Func<DateTime, string> DateTimeStringFormatterX;

        /// <summary>
        /// If false, the vertical line marking the X position will not be rendered.
        /// </summary>
        public bool IsVisibleX = true;

        /// <summary>
        /// Controls the conversion from Y position to a text.
        /// https://docs.microsoft.com/en-us/dotnet/standard/base-types/standard-numeric-format-strings
        /// </summary>
        public string StringFormatY = "F2";

        /// <summary>
        /// If true, the Y position will be converted to DateTime before applying the format string
        /// </summary>
        public bool IsDateTimeY = false;

        /// <summary>
        /// This function will be used to generate labels from Y position when IsDateTimeY is false.
        /// By default, it simply calls the ToString method with StringFormatY.
        /// You can set this function to apply a custom formatter.
        /// </summary>
        public Func<double, string> NumericStringFormatterY;

        /// <summary>
        /// This function will be used to generate labels from Y position when IsDateTimeY is true.
        /// By default, it simply calls the ToString method with StringFormatY.
        /// You can set this function to apply a custom formatter.
        /// </summary>
        public Func<DateTime, string> DateTimeStringFormatterY;

        /// <summary>
        /// If false, the horizontal line marking the Y position will not be rendered.
        /// </summary>
        public bool IsVisibleY = true;

        /// <summary>
        /// Control whether the label for the X crosshair appears on the top or bottom edge of the plot
        /// </summary>
        public bool XLabelOnTop = false;

        /// <summary>
        /// Control whether the label for the Y crosshair appears on the left or right edge of the plot
        /// </summary>
        public bool YLabelOnRight = false;

        public LineStyle LineStyle = LineStyle.Dash;
        public float LineWidth = 1;
        public Color LineColor = Color.FromArgb(150, Color.Red);
        public Drawing.Font LabelFont = new();
        public Color LabelBackgroundColor;

        public Crosshair()
        {
            LabelFont.Bold = true;
            LabelFont.Color = Color.White;
            LabelBackgroundColor = Color.Black;

            NumericStringFormatterX = X => X.ToString(StringFormatX);
            NumericStringFormatterY = Y => Y.ToString(StringFormatY);

            DateTimeStringFormatterX = X => X.ToString(StringFormatX);
            DateTimeStringFormatterY = Y => Y.ToString(StringFormatY);
        }

        public AxisLimits GetAxisLimits() => new(double.NaN, double.NaN, double.NaN, double.NaN);

        public LegendItem[] GetLegendItems() => null;

        public void ValidateData(bool deep = false) { }

        public void Render(PlotDimensions dims, Bitmap bmp, bool lowQuality = false)
        {
            if (IsVisible == false)
                return;

            using var gfx = Drawing.GDI.Graphics(bmp, dims, lowQuality, clipToDataArea: false);
            using var pen = Drawing.GDI.Pen(LineColor, LineWidth, LineStyle);
            using var fnt = Drawing.GDI.Font(LabelFont);
            using var fillBrush = Drawing.GDI.Brush(LabelBackgroundColor);
            using var fontBrush = Drawing.GDI.Brush(LabelFont.Color);

            RenderVerticalLine(dims, gfx, pen, fnt, fillBrush, fontBrush);
            RenderHorizontalLine(dims, gfx, pen, fnt, fillBrush, fontBrush);
        }

        private void RenderHorizontalLine(PlotDimensions dims, Graphics gfx, Pen pen, Font fnt, Brush fillBrush, Brush fontBrush)
        {
            if (!IsVisibleY)
                return;

            if (Y < dims.YMin || Y > dims.YMax)
                return;

            float yPixel = dims.GetPixelY(Y);
            gfx.DrawLine(pen, dims.DataOffsetX, yPixel, dims.DataOffsetX + dims.DataWidth, yPixel);

            string yLabel = IsDateTimeY ? DateTimeStringFormatterY(DateTime.FromOADate(Y)) : NumericStringFormatterY(Y);

            SizeF yLabelSize = Drawing.GDI.MeasureString(yLabel, LabelFont);

            float xPos = YLabelOnRight ? dims.DataOffsetX + dims.DataWidth : dims.DataOffsetX - yLabelSize.Width;
            float yPos = yPixel - yLabelSize.Height / 2;

            RectangleF xLabelRect = new(xPos, yPos, yLabelSize.Width, yLabelSize.Height);
            gfx.FillRectangle(fillBrush, xLabelRect);
            var sf = Drawing.GDI.StringFormat(HorizontalAlignment.Left, VerticalAlignment.Middle);
            gfx.DrawString(yLabel, fnt, fontBrush, xPos, yPixel, sf);
        }

        private void RenderVerticalLine(PlotDimensions dims, Graphics gfx, Pen pen, Font fnt, Brush fillBrush, Brush fontBrush)
        {
            if (!IsVisibleX)
                return;

            if (X < dims.XMin || X > dims.XMax)
                return;

            float xPixel = dims.GetPixelX(X);
            gfx.DrawLine(pen, xPixel, dims.DataOffsetY, xPixel, dims.DataOffsetY + dims.DataHeight);

            string xLabel = IsDateTimeX ? DateTimeStringFormatterX(DateTime.FromOADate(X)) : NumericStringFormatterX(X);
            SizeF xLabelSize = Drawing.GDI.MeasureString(xLabel, LabelFont);

            float xPos = xPixel - xLabelSize.Width / 2;
            float yPos = XLabelOnTop ? dims.DataOffsetY - xLabelSize.Height : dims.DataOffsetY + dims.DataHeight;

            RectangleF xLabelRect = new(xPos, yPos, xLabelSize.Width, xLabelSize.Height);
            gfx.FillRectangle(fillBrush, xLabelRect);
            var sf = Drawing.GDI.StringFormat(HorizontalAlignment.Center, VerticalAlignment.Upper);
            gfx.DrawString(xLabel, fnt, fontBrush, xPixel, yPos, sf);
        }
    }
}
