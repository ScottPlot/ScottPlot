using ScottPlot.Drawing;
using System;
using System.Drawing;

namespace ScottPlot.Plottable
{
    /// <summary>
    /// Text placed at a location relative to the data area that does not move when the axis limits change
    /// </summary>
    public class Annotation : IPlottable
    {
        /// <summary>
        /// Horizontal location (in pixel units) relative to the data area
        /// </summary>
        public double X { get; set; }

        /// <summary>
        /// Vertical position (in pixel units) relative to the data area
        /// </summary>
        public double Y { get; set; }

        /// <summary>
        /// Text displayed in the annotation
        /// </summary>
        public string Label { get; set; }

        public readonly Drawing.Font Font = new();

        public bool Background { get; set; } = true;
        public Color BackgroundColor { get; set; } = Color.Yellow;

        public bool Shadow { get; set; } = true;
        public Color ShadowColor { get; set; } = Color.FromArgb(25, Color.Black);

        public bool Border { get; set; } = true;
        public float BorderWidth { get; set; } = 1;
        public Color BorderColor { get; set; } = Color.Black;

        public bool IsVisible { get; set; } = true;
        public int XAxisIndex { get; set; } = 0;
        public int YAxisIndex { get; set; } = 0;

        public override string ToString() => $"PlottableAnnotation at ({X} px, {Y} px)";

        public void ValidateData(bool deep = false)
        {
            if (double.IsNaN(X) || double.IsInfinity(X))
                throw new InvalidOperationException("xPixel must be a valid number");

            if (double.IsNaN(Y) || double.IsInfinity(Y))
                throw new InvalidOperationException("xPixel must be a valid number");
        }

        public void Render(PlotDimensions dims, Bitmap bmp, bool lowQuality = false)
        {
            if (!IsVisible)
                return;

            using var gfx = GDI.Graphics(bmp, dims, lowQuality, false);
            using var font = GDI.Font(Font);
            using var fontBrush = new SolidBrush(Font.Color);
            using var shadowBrush = new SolidBrush(ShadowColor);
            using var backgroundBrush = new SolidBrush(BackgroundColor);
            using var borderPen = new Pen(BorderColor, BorderWidth);

            SizeF size = GDI.MeasureString(gfx, Label, font);

            double x = (X >= 0) ? X : dims.DataWidth + X - size.Width;
            double y = (Y >= 0) ? Y : dims.DataHeight + Y - size.Height;
            PointF location = new PointF((float)x + dims.DataOffsetX, (float)y + dims.DataOffsetY);

            if (Background && Shadow)
                gfx.FillRectangle(shadowBrush, location.X + 5, location.Y + 5, size.Width, size.Height);

            if (Background)
                gfx.FillRectangle(backgroundBrush, location.X, location.Y, size.Width, size.Height);

            if (Border)
                gfx.DrawRectangle(borderPen, location.X, location.Y, size.Width, size.Height);

            gfx.DrawString(Label, font, fontBrush, location);
        }

        public AxisLimits GetAxisLimits() => AxisLimits.NoLimits;

        public LegendItem[] GetLegendItems() => LegendItem.None;
    }
}
