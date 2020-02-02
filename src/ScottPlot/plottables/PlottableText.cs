using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace ScottPlot
{
    public class PlottableText : Plottable
    {
        public double x;
        public double y;
        public double rotation;
        public string text;
        public Brush brush, frameBrush;
        public Font font;
        public TextAlignment alignment;
        public bool frame;
        public Color frameColor;

        public PlottableText(string text, double x, double y, Color color, string fontName, double fontSize, bool bold, string label, TextAlignment alignment, double rotation, bool frame, Color frameColor)
        {
            this.text = text ?? throw new Exception("Text cannot be null");
            this.x = x;
            this.y = y;
            this.rotation = rotation;
            this.label = label;
            this.alignment = alignment;
            this.frame = frame;
            this.frameColor = frameColor;

            brush = new SolidBrush(color);
            frameBrush = new SolidBrush(frameColor);

            FontStyle fontStyle = (bold == true) ? FontStyle.Bold : FontStyle.Regular;
            font = new Font(fontName, (float)fontSize, fontStyle);

            pointCount = 1;
        }

        public override string ToString()
        {
            return $"PlottableText \"{text}\" at ({x}, {y})";
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

            SizeF stringSize = settings.gfxData.MeasureString(text, font);

            switch (alignment)
            {
                case TextAlignment.lowerCenter:
                    textLocationPoint.Y = defaultPoint.Y - stringSize.Height;
                    textLocationPoint.X = defaultPoint.X - stringSize.Width / 2;
                    break;
                case TextAlignment.lowerLeft:
                    textLocationPoint.Y = defaultPoint.Y - stringSize.Height;
                    textLocationPoint.X = defaultPoint.X;
                    break;
                case TextAlignment.lowerRight:
                    textLocationPoint.Y = defaultPoint.Y - stringSize.Height;
                    textLocationPoint.X = defaultPoint.X - stringSize.Width;
                    break;
                case TextAlignment.middleLeft:
                    textLocationPoint.Y = defaultPoint.Y - stringSize.Height / 2;
                    textLocationPoint.X = defaultPoint.X;
                    break;
                case TextAlignment.middleRight:
                    textLocationPoint.Y = defaultPoint.Y - stringSize.Height / 2;
                    textLocationPoint.X = defaultPoint.X - stringSize.Width;
                    break;
                case TextAlignment.upperCenter:
                    textLocationPoint.Y = defaultPoint.Y;
                    textLocationPoint.X = defaultPoint.X - stringSize.Width / 2;
                    break;
                case TextAlignment.upperLeft:
                    textLocationPoint = defaultPoint;
                    break;
                case TextAlignment.upperRight:
                    textLocationPoint.Y = defaultPoint.Y;
                    textLocationPoint.X = defaultPoint.X - stringSize.Width;
                    break;
                case TextAlignment.middleCenter:
                    textLocationPoint.Y = defaultPoint.Y - stringSize.Height / 2;
                    textLocationPoint.X = defaultPoint.X - stringSize.Width / 2;
                    break;
            }

            //textLocationPoint.Y -= stringSize.Height / 2;

            settings.gfxData.TranslateTransform((int)textLocationPoint.X, (int)textLocationPoint.Y);
            settings.gfxData.RotateTransform((float)rotation);
            if (frame)
            {
                Rectangle stringRect = new Rectangle(0, 0, (int)stringSize.Width, (int)stringSize.Height);
                settings.gfxData.FillRectangle(frameBrush, stringRect);
            }
            settings.gfxData.DrawString(text, font, brush, new PointF(0, 0));
            settings.gfxData.ResetTransform();
        }
    }
}
