using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace ScottPlot.Plottables
{
    public class Text : IPlottable
    {
        // interface stuff
        public bool visible { get; set; } = true;
        public int pointCount { get { return 1; } }
        public string label { get; set; }
        public Color color { get; set; }
        public MarkerShape markerShape { get; set; }
        public LineStyle lineStyle { get; set; }

        // properties
        public double x;
        public double y;
        public double rotation;
        public string text;
        public Brush brush, frameBrush;
        public Font font;
        public TextAlignment alignment;
        public bool frame;
        public Color frameColor;

        public Text(string text, double x, double y, Color color, string fontName, double fontSize, bool bold, string label, TextAlignment alignment, double rotation, bool frame, Color frameColor)
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
        }

        public override string ToString()
        {
            return $"PlottableText \"{text}\" at ({x}, {y})";
        }

        public Config.AxisLimits2D GetLimits()
        {
            double[] limits = { x, x, y, y };

            // TODO: use features of 2d axis
            return new Config.AxisLimits2D(limits);
        }

        public void Render(Context renderContext)
        {
            PointF defaultPoint = renderContext.GetPixel(x, y);
            PointF textLocationPoint = new PointF();

            SizeF stringSize = renderContext.gfxData.MeasureString(text, font);

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

            renderContext.gfxData.TranslateTransform((int)textLocationPoint.X, (int)textLocationPoint.Y);
            renderContext.gfxData.RotateTransform((float)rotation);
            if (frame)
            {
                Rectangle stringRect = new Rectangle(0, 0, (int)stringSize.Width, (int)stringSize.Height);
                renderContext.gfxData.FillRectangle(frameBrush, stringRect);
            }
            renderContext.gfxData.DrawString(text, font, brush, new PointF(0, 0));
            renderContext.gfxData.ResetTransform();
        }
    }
}
