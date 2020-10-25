using System.ComponentModel;
using System.Drawing;
using ScottPlot.Config;
using ScottPlot.Drawing;
using ScottPlot.Renderable;

namespace ScottPlot.Plottable
{
    public class Image : IRenderable, IHasAxisLimits, IValidatable
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

        public override string ToString() => $"PlottableImage Size(\"{image.Size}\") at ({x}, {y})";

        public string ErrorMessage(bool deepValidation = false)
        {
            if (image is null)
                return "image must not be null";

            return null;
        }

        public AxisLimits2D GetLimits() => new AxisLimits2D(new double[] { x, x, y, y });

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

            using (Graphics gfx = GDI.Graphics(bmp, lowQuality))
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
