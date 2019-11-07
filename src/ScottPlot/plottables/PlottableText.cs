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
        public Brush brush;
        public Font font;
        public TextAlignment alignment;

        public PlottableText(string text, double x, double y, Color color, string fontName, double fontSize, bool bold, string label, TextAlignment alignment, double rotation)
        {
            this.text = text ?? throw new Exception("Text cannot be null");
            this.x = x;
            this.y = y;
            this.rotation = rotation;
            this.label = label;
            this.alignment = alignment;
            brush = new SolidBrush(color);
            FontStyle fontStyle = (bold == true) ? FontStyle.Bold : FontStyle.Regular;
            font = new Font(fontName, (float)fontSize, fontStyle);

            pointCount = 1;
        }

        public override string ToString()
        {
            return $"PlottableText \"{text}\" at ({x}, {y})";
        }

        public override double[] GetLimits()
        {
            return new double[] { x, x, y, y };
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
            settings.gfxData.DrawString(text, font, brush, new PointF(0, 0));
            settings.gfxData.ResetTransform();
        }

        public override void SaveCSV(string filePath)
        {
            throw new NotImplementedException();
        }
    }
}
