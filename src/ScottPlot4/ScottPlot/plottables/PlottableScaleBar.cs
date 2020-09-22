using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using ScottPlot.Config;

namespace ScottPlot
{
    public class PlottableScaleBar : Plottable
    {
        readonly double sizeX;
        readonly double sizeY;
        readonly string labelX;
        readonly string labelY;
        readonly double padPx;
        readonly double thickness;
        readonly Color color;
        readonly string fontName;
        readonly double fontSize;
        readonly FontStyle fontStyle = FontStyle.Regular;

        public PlottableScaleBar(double sizeX, double sizeY, string labelX, string labelY,
            double thickness, double fontSize, Color color, double padPx)
        {
            this.sizeX = sizeX;
            this.sizeY = sizeY;
            this.labelX = (labelX is null) ? sizeX.ToString() : labelX;
            this.labelY = (labelY is null) ? sizeY.ToString() : labelY;
            this.thickness = thickness;
            this.fontSize = fontSize;
            this.color = color;
            this.padPx = padPx;
            fontName = Fonts.GetDefaultFontName();
        }

        public override string ToString()
        {
            return $"PlottableScaleBar ({labelX}, {labelY})";
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

                var xLabelSize = Drawing.GDI.MeasureString(settings.gfxData, labelX, font);
                var yLabelSize = Drawing.GDI.MeasureString(settings.gfxData, labelY, font);
                cornerPoint.X -= yLabelSize.Width * 1.2f;
                cornerPoint.Y -= yLabelSize.Height;

                PointF horizPoint = new PointF(cornerPoint.X - widthPx, cornerPoint.Y);
                PointF vertPoint = new PointF(cornerPoint.X, cornerPoint.Y - heightPx);
                PointF horizMidPoint = new PointF((cornerPoint.X + horizPoint.X) / 2, cornerPoint.Y);
                PointF vertMidPoint = new PointF(cornerPoint.X, (cornerPoint.Y + vertPoint.Y) / 2);

                settings.gfxData.DrawLines(pen, new PointF[] { horizPoint, cornerPoint, vertPoint });
                settings.gfxData.DrawString(labelX, font, brush, horizMidPoint.X, cornerPoint.Y, settings.misc.sfNorth);
                settings.gfxData.DrawString(labelY, font, brush, cornerPoint.X, vertMidPoint.Y, settings.misc.sfWest);
            }
        }
    }
}
