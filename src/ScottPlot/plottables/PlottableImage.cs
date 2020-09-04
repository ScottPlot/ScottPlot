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
        public Brush frameBrush;
        public TextAlignment alignment;
        public bool frame;
        public Color frameColor;
        public int framePadding;
        public string label;

        public PlottableImage(Image image, double x, double y, string label, TextAlignment alignment, double rotation, bool frame, Color frameColor, int frameSize)
        {
            this.image = image ?? throw new Exception("Image cannot be null");
            this.x = x;
            this.y = y;
            this.rotation = rotation;
            this.label = label;
            this.alignment = alignment;
            this.frame = frame;
            this.frameColor = frameColor;
            if (frameSize < 0)
            {
                throw new Exception("Frame padding cannot be lower than 0");
            }
            this.framePadding = frameSize;

            frameBrush = new SolidBrush(frameColor);
        }

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
            PointF defaultPoint = settings.GetPixel(x, y);
            PointF textLocationPoint = new PointF();

            if (rotation == 0)
            {
                switch (alignment)
                {
                    case TextAlignment.lowerCenter:
                        textLocationPoint.Y = defaultPoint.Y - image.Height;
                        textLocationPoint.X = defaultPoint.X - image.Width / 2;
                        break;
                    case TextAlignment.lowerLeft:
                        textLocationPoint.Y = defaultPoint.Y - image.Height;
                        textLocationPoint.X = defaultPoint.X;
                        break;
                    case TextAlignment.lowerRight:
                        textLocationPoint.Y = defaultPoint.Y - image.Height;
                        textLocationPoint.X = defaultPoint.X - image.Width;
                        break;
                    case TextAlignment.middleLeft:
                        textLocationPoint.Y = defaultPoint.Y - image.Height / 2;
                        textLocationPoint.X = defaultPoint.X;
                        break;
                    case TextAlignment.middleRight:
                        textLocationPoint.Y = defaultPoint.Y - image.Height / 2;
                        textLocationPoint.X = defaultPoint.X - image.Width;
                        break;
                    case TextAlignment.upperCenter:
                        textLocationPoint.Y = defaultPoint.Y;
                        textLocationPoint.X = defaultPoint.X - image.Width / 2;
                        break;
                    case TextAlignment.upperLeft:
                        textLocationPoint = defaultPoint;
                        break;
                    case TextAlignment.upperRight:
                        textLocationPoint.Y = defaultPoint.Y;
                        textLocationPoint.X = defaultPoint.X - image.Width;
                        break;
                    case TextAlignment.middleCenter:
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
            if (frame && framePadding > 0)
            {
                Rectangle imageRect = new Rectangle(
                    -framePadding,
                    -framePadding,
                    image.Width + (framePadding * 2),
                    image.Height + (framePadding * 2)
                    );
                settings.gfxData.FillRectangle(frameBrush, imageRect);
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
