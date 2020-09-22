using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using ScottPlot.Config;

namespace ScottPlot
{
    public class PlottableImage : Plottable
    {
        public double x;
        public double y;
        public double rotation;
        public Image image;
        public ImageAlignment alignment;
        public Color frameColor;
        public int frameSize;
        public string label;

        public override string ToString()
        {
            return $"PlottableImage Size(\"{image.Size}\") at ({x}, {y})";
        }

        public override Config.AxisLimits2D GetLimits()
        {
            double[] limits = { x, x, y, y };

            // TODO: use features of 2d axis
            return new Config.AxisLimits2D(limits);
        }

        public override void Render(Settings settings)
        {
            if (image is null)
                throw new Exception("Image cannot be null");

            PointF defaultPoint = settings.GetPixel(x, y);
            PointF textLocationPoint = new PointF();

            if (rotation == 0)
            {
                switch (alignment)
                {
                    case ImageAlignment.lowerCenter:
                        textLocationPoint.Y = defaultPoint.Y - image.Height;
                        textLocationPoint.X = defaultPoint.X - image.Width / 2;
                        break;
                    case ImageAlignment.lowerLeft:
                        textLocationPoint.Y = defaultPoint.Y - image.Height;
                        textLocationPoint.X = defaultPoint.X;
                        break;
                    case ImageAlignment.lowerRight:
                        textLocationPoint.Y = defaultPoint.Y - image.Height;
                        textLocationPoint.X = defaultPoint.X - image.Width;
                        break;
                    case ImageAlignment.middleLeft:
                        textLocationPoint.Y = defaultPoint.Y - image.Height / 2;
                        textLocationPoint.X = defaultPoint.X;
                        break;
                    case ImageAlignment.middleRight:
                        textLocationPoint.Y = defaultPoint.Y - image.Height / 2;
                        textLocationPoint.X = defaultPoint.X - image.Width;
                        break;
                    case ImageAlignment.upperCenter:
                        textLocationPoint.Y = defaultPoint.Y;
                        textLocationPoint.X = defaultPoint.X - image.Width / 2;
                        break;
                    case ImageAlignment.upperLeft:
                        textLocationPoint = defaultPoint;
                        break;
                    case ImageAlignment.upperRight:
                        textLocationPoint.Y = defaultPoint.Y;
                        textLocationPoint.X = defaultPoint.X - image.Width;
                        break;
                    case ImageAlignment.middleCenter:
                        textLocationPoint.Y = defaultPoint.Y - image.Height / 2;
                        textLocationPoint.X = defaultPoint.X - image.Width / 2;
                        break;
                }
            }
            else
            {
                // ignore alignment if rotation is used
                textLocationPoint = new PointF(defaultPoint.X, defaultPoint.Y);
            }

            settings.gfxData.TranslateTransform((int)textLocationPoint.X, (int)textLocationPoint.Y);
            settings.gfxData.RotateTransform((float)rotation);

            if (frameSize > 0)
            {
                using (var framePen = new Pen(frameColor, frameSize * 2))
                {
                    Rectangle frameRect = new Rectangle(0, 0, image.Width - 1, image.Height - 1);
                    settings.gfxData.DrawRectangle(framePen, frameRect);
                }
            }

            settings.gfxData.DrawImage(image, new PointF(0, 0));
            settings.gfxData.ResetTransform();
        }

        public override int GetPointCount()
        {
            return 1;
        }

        public override LegendItem[] GetLegendItems()
        {
            return null; // don't show this in the legend
        }
    }
}
