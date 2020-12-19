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
        public double X;

        /// <summary>
        /// Vertical position (in pixel units) relative to the data area
        /// </summary>
        public double Y;

        /// <summary>
        /// Text displayed in the annotation
        /// </summary>
        public string Label;

        public readonly Drawing.Font Font = new Drawing.Font();
        public Color FontColor { set => Font.Color = value; }
        public string FontName { set => Font.Name = value; }
        public float FontSize { set => Font.Size = value; }
        public bool FontBold { set => Font.Bold = value; }
        public Alignment Alignment { set => Font.Alignment = value; }
        public float Rotation { set => Font.Rotation = value; }

        public bool Background = true;
        public Color BackgroundColor = Color.Yellow;

        public bool Shadow = true;
        public Color ShadowColor = Color.FromArgb(25, Color.Black);

        public bool Border = true;
        public float BorderWidth = 1;
        public Color BorderColor = Color.Black;

        public bool IsVisible { get; set; } = true;
        public int XAxisIndex { get; set; } = 0;
        public int YAxisIndex { get; set; } = 0;

        public override string ToString() => $"PlottableAnnotation at ({X} px, {Y} px)";
        public LegendItem[] GetLegendItems() => null;
        public AxisLimits GetAxisLimits() => new AxisLimits(double.NaN, double.NaN, double.NaN, double.NaN);

        public void ValidateData(bool deep = false)
        {
            if (double.IsNaN(X) || double.IsInfinity(X))
                throw new InvalidOperationException("xPixel must be a valid number");

            if (double.IsNaN(Y) || double.IsInfinity(Y))
                throw new InvalidOperationException("xPixel must be a valid number");
        }

        // TODO: the negative coordiante thing is silly. Use alignment fields to control this behavior.

        public void Render(PlotDimensions dims, Bitmap bmp, bool lowQuality = false)
        {
            using (var gfx = GDI.Graphics(bmp, lowQuality))
            using (var font = GDI.Font(Font))
            using (var fontBrush = new SolidBrush(Font.Color))
            using (var shadowBrush = new SolidBrush(ShadowColor))
            using (var backgroundBrush = new SolidBrush(BackgroundColor))
            using (var borderPen = new Pen(BorderColor, BorderWidth))
            {
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
        }
    }
}
