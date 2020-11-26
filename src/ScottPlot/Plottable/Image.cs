using System.ComponentModel;
using System.Drawing;
using ScottPlot.Drawing;
using System;
using System.Data;

namespace ScottPlot.Plottable
{
    public class Image : IPlottable
    {
        public double x;
        public double y;
        public double rotation;
        public System.Drawing.Image image;
        public Alignment alignment;
        public Color frameColor;
        public int frameSize;
        public string label;
        public bool IsVisible { get; set; } = true;
        public int HorizontalAxisIndex { get; set; } = 0;
        public int VerticalAxisIndex { get; set; } = 0;

        public override string ToString() => $"PlottableImage Size(\"{image.Size}\") at ({x}, {y})";
        public AxisLimits GetAxisLimits() => new AxisLimits(x, x, y, y);
        public LegendItem[] GetLegendItems() => null;

        public void ValidateData(bool deep = false)
        {
            if (double.IsNaN(x) || double.IsInfinity(x))
                throw new InvalidOperationException("x must be a real value");

            if (double.IsNaN(y) || double.IsInfinity(y))
                throw new InvalidOperationException("y must be a real value");

            if (double.IsNaN(rotation) || double.IsInfinity(rotation))
                throw new InvalidOperationException("rotation must be a real value");

            if (image is null)
                throw new InvalidOperationException("image cannot be null");
        }

        private PointF TextLocation(PointF input)
        {
            switch (alignment)
            {
                case Alignment.LowerCenter:
                    return new PointF(input.X - image.Width / 2, input.Y - image.Height);
                case Alignment.LowerLeft:
                    return new PointF(input.X, input.Y - image.Height);
                case Alignment.LowerRight:
                    return new PointF(input.X - image.Width, input.Y - image.Height);
                case Alignment.MiddleLeft:
                    return new PointF(input.X, input.Y - image.Height / 2);
                case Alignment.MiddleRight:
                    return new PointF(input.X - image.Width, input.Y - image.Height / 2);
                case Alignment.UpperCenter:
                    return new PointF(input.X - image.Width / 2, input.Y);
                case Alignment.UpperLeft:
                    return new PointF(input.X, input.Y);
                case Alignment.UpperRight:
                    return new PointF(input.X - image.Width, input.Y);
                case Alignment.MiddleCenter:
                    return new PointF(input.X - image.Width / 2, input.Y - image.Height / 2);
                default:
                    throw new InvalidEnumArgumentException();
            }
        }

        public void Render(PlotDimensions dims, Bitmap bmp, bool lowQuality = false)
        {
            PointF defaultPoint = new PointF(dims.GetPixelX(x), dims.GetPixelY(y));
            PointF textLocationPoint = (rotation == 0) ? TextLocation(defaultPoint) : defaultPoint;

            using (Graphics gfx = GDI.Graphics(bmp, dims, lowQuality))
            using (var framePen = new Pen(frameColor, frameSize * 2))
            {
                gfx.TranslateTransform((int)textLocationPoint.X, (int)textLocationPoint.Y);
                gfx.RotateTransform((float)rotation);

                if (frameSize > 0)
                    gfx.DrawRectangle(framePen, new Rectangle(0, 0, image.Width - 1, image.Height - 1));

                gfx.DrawImage(image, new PointF(0, 0));
                gfx.ResetTransform();
            }
        }
    }
}
