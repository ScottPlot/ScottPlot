using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using ScottPlot.Config;
using ScottPlot.Drawing;

namespace ScottPlot
{
    public class PlottableImage : Plottable, IPlottable
    {
        public double x;
        public double y;
        public double rotation;
        public Image image;
        public ImageAlignment alignment;
        public Color frameColor;
        public int frameSize;
        public string label;

        public override string ToString() => $"PlottableImage Size(\"{image.Size}\") at ({x}, {y})";

        public string ValidationErrorMessage { get; private set; }
        public bool IsValidData(bool deepValidation = false)
        {
            if (image is null)
            {
                ValidationErrorMessage = "image must not be null";
                return false;
            }

            ValidationErrorMessage = "";
            return true;
        }

        public override AxisLimits2D GetLimits() => new AxisLimits2D(new double[] { x, x, y, y });

        public override int GetPointCount() => 1;

        public override LegendItem[] GetLegendItems() => null;

        private PointF TextLocation(PointF input)
        {
            switch (alignment)
            {
                case ImageAlignment.lowerCenter:
                    return new PointF(input.X - image.Width / 2, input.Y - image.Height);
                case ImageAlignment.lowerLeft:
                    return new PointF(input.X, input.Y - image.Height);
                case ImageAlignment.lowerRight:
                    return new PointF(input.X - image.Width, input.Y - image.Height);
                case ImageAlignment.middleLeft:
                    return new PointF(input.X, input.Y - image.Height / 2);
                case ImageAlignment.middleRight:
                    return new PointF(input.X - image.Width, input.Y - image.Height / 2);
                case ImageAlignment.upperCenter:
                    return new PointF(input.X - image.Width / 2, input.Y);
                case ImageAlignment.upperLeft:
                    return new PointF(input.X, input.Y);
                case ImageAlignment.upperRight:
                    return new PointF(input.X - image.Width, input.Y);
                case ImageAlignment.middleCenter:
                    return new PointF(input.X - image.Width / 2, input.Y - image.Height / 2);
                default:
                    throw new InvalidEnumArgumentException();
            }
        }

        public override void Render(Settings settings) => throw new InvalidOperationException("use new Render()");

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
