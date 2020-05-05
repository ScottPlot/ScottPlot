using ScottPlot.Config;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace ScottPlot
{
    public class PlottableScaleBar : Plottable
    {
        readonly double sizeX;
        readonly double sizeY;
        readonly double padPx;
        readonly double thickness;
        readonly Color color;
        readonly string fontName;
        readonly double fontSize;
        readonly FontStyle fontStyle = FontStyle.Regular;

        public PlottableScaleBar(double sizeX, double sizeY, double thickness, double fontSize, Color color, double padPx)
        {
            this.sizeX = sizeX;
            this.sizeY = sizeY;
            this.thickness = thickness;
            this.fontSize = fontSize;
            this.color = color;
            this.padPx = padPx;
            fontName = Fonts.GetDefaultFontName();
        }

        public override string ToString()
        {
            return $"PlottableScaleBar ({sizeX}, {sizeY})";
        }

        public override LegendItem[] GetLegendItems()
        {
            return null;
        }

        public override AxisLimits2D GetLimits()
        {
            return new AxisLimits2D();
        }

        public override int GetPointCount()
        {
            return 1;
        }

        public override void Render(Settings settings)
        {
            float widthPx = (float)(sizeX * settings.xAxisScale);
            float heightPx = (float)(sizeY * settings.yAxisScale);

            using (var font = new Font(fontName, (float)fontSize, fontStyle))
            using (var brush = new SolidBrush(color))
            using (var pen = new Pen(color, (float)thickness))
            {
                PointF cornerPoint = settings.GetPixel(settings.axes.x.max, settings.axes.y.min);
                cornerPoint.X -= (float)padPx;
                cornerPoint.Y -= (float)padPx;

                string xLabel = sizeX.ToString();
                string yLabel = sizeY.ToString();
                var xLabelSize = Drawing.GDI.MeasureString(settings.gfxData, xLabel, font);
                var yLabelSize = Drawing.GDI.MeasureString(settings.gfxData, yLabel, font);
                cornerPoint.X -= yLabelSize.Width;
                cornerPoint.Y -= yLabelSize.Height;

                PointF horizPoint = new PointF(cornerPoint.X - widthPx, cornerPoint.Y);
                PointF vertPoint = new PointF(cornerPoint.X, cornerPoint.Y - heightPx);
                PointF horizMidPoint = new PointF((cornerPoint.X + horizPoint.X) / 2, cornerPoint.Y);
                PointF vertMidPoint = new PointF(cornerPoint.X, (cornerPoint.Y + vertPoint.Y) / 2);

                settings.gfxData.DrawLines(pen, new PointF[] { horizPoint, cornerPoint, vertPoint });
                settings.gfxData.DrawString(xLabel, font, brush, horizMidPoint.X - xLabelSize.Width, cornerPoint.Y);
                settings.gfxData.DrawString(yLabel, font, brush, cornerPoint.X, vertMidPoint.Y - xLabelSize.Height / 2);
            }
        }
    }
}
