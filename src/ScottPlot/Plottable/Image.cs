using System.ComponentModel;
using System.Drawing;
using ScottPlot.Drawing;
using System;
using System.Data;

namespace ScottPlot.Plottable
{
    /// <summary>
    /// Display a Bitmap at X/Y coordinates in unit space
    /// </summary>
    public class Image : IPlottable
    {
        public bool IsVisible { get; set; } = true;
        public double X;
        public double Y;
        public double Rotation;
        public Bitmap Bitmap;
        public Alignment Alignment;
        public Color BorderColor;
        public float BorderSize;
        public string Label;
        public int XAxisIndex { get; set; } = 0;
        public int YAxisIndex { get; set; } = 0;

        public override string ToString() => $"PlottableImage Size(\"{Bitmap.Size}\") at ({X}, {Y})";
        public AxisLimits GetAxisLimits() => new AxisLimits(X, X, Y, Y);
        public LegendItem[] GetLegendItems() => null;

        public void ValidateData(bool deep = false)
        {
            if (double.IsNaN(X) || double.IsInfinity(X))
                throw new InvalidOperationException("x must be a real value");

            if (double.IsNaN(Y) || double.IsInfinity(Y))
                throw new InvalidOperationException("y must be a real value");

            if (double.IsNaN(Rotation) || double.IsInfinity(Rotation))
                throw new InvalidOperationException("rotation must be a real value");

            if (Bitmap is null)
                throw new InvalidOperationException("image cannot be null");
        }

        private PointF TextLocation(PointF input)
        {
            switch (Alignment)
            {
                case Alignment.LowerCenter:
                    return new PointF(input.X - Bitmap.Width / 2, input.Y - Bitmap.Height);
                case Alignment.LowerLeft:
                    return new PointF(input.X, input.Y - Bitmap.Height);
                case Alignment.LowerRight:
                    return new PointF(input.X - Bitmap.Width, input.Y - Bitmap.Height);
                case Alignment.MiddleLeft:
                    return new PointF(input.X, input.Y - Bitmap.Height / 2);
                case Alignment.MiddleRight:
                    return new PointF(input.X - Bitmap.Width, input.Y - Bitmap.Height / 2);
                case Alignment.UpperCenter:
                    return new PointF(input.X - Bitmap.Width / 2, input.Y);
                case Alignment.UpperLeft:
                    return new PointF(input.X, input.Y);
                case Alignment.UpperRight:
                    return new PointF(input.X - Bitmap.Width, input.Y);
                case Alignment.MiddleCenter:
                    return new PointF(input.X - Bitmap.Width / 2, input.Y - Bitmap.Height / 2);
                default:
                    throw new InvalidEnumArgumentException();
            }
        }

        public void Render(PlotDimensions dims, Bitmap bmp, bool lowQuality = false)
        {
            PointF defaultPoint = new PointF(dims.GetPixelX(X), dims.GetPixelY(Y));
            PointF textLocationPoint = (Rotation == 0) ? TextLocation(defaultPoint) : defaultPoint;

            using (Graphics gfx = GDI.Graphics(bmp, dims, lowQuality))
            using (var framePen = new Pen(BorderColor, BorderSize * 2))
            {
                gfx.TranslateTransform((int)textLocationPoint.X, (int)textLocationPoint.Y);
                gfx.RotateTransform((float)Rotation);

                if (BorderSize > 0)
                    gfx.DrawRectangle(framePen, new Rectangle(0, 0, Bitmap.Width - 1, Bitmap.Height - 1));

                gfx.DrawImage(Bitmap, new PointF(0, 0));
                gfx.ResetTransform();
            }
        }
    }
}
