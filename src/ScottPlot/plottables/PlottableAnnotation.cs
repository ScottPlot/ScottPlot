using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using ScottPlot.Config;

namespace ScottPlot
{
    public class PlottableAnnotation : Plottable
    {
        public double xPixel;
        public double yPixel;
        public string label;

        Font font;
        Brush fontBrush;
        bool fill;
        Brush fillBrush;
        Brush shadowBrush;
        Pen pen;
        bool shadow;


        public PlottableAnnotation(double xPixel, double yPixel, string label,
            double fontSize, string fontName, Color fontColor,
            bool fill, Color fillColor,
            double lineWidth, Color lineColor,
            bool shadow)
        {
            this.xPixel = xPixel;
            this.yPixel = yPixel;
            this.label = label;
            this.shadow = shadow;

            font = new Font(Fonts.GetValidFontName(fontName), (float)fontSize, FontStyle.Regular, GraphicsUnit.Pixel);
            fontBrush = new SolidBrush(fontColor);

            this.fill = fill;
            fillBrush = new SolidBrush(fillColor);
            shadowBrush = new SolidBrush(Color.FromArgb(50, 0, 0, 0));

            pen = new Pen(lineColor, (float)lineWidth);
        }

        public override string ToString()
        {
            return $"PlottableAnnotation at ({xPixel} px, {yPixel} px)";
        }

        public override int GetPointCount()
        {
            return 1;
        }

        public override AxisLimits2D GetLimits()
        {
            return new AxisLimits2D();
        }

        public override LegendItem[] GetLegendItems()
        {
            return null;
        }

        public override void Render(Settings settings)
        {
            if (label is null)
                return;

            SizeF size = Drawing.GDI.MeasureString(settings.gfxData, label, font);

            double x = (xPixel >= 0) ? xPixel : settings.bmpData.Width + xPixel - size.Width;
            double y = (yPixel >= 0) ? yPixel : settings.bmpData.Height + yPixel - size.Height;

            PointF location = new PointF((float)x, (float)y);

            if (fill && shadow)
                settings.gfxData.FillRectangle(shadowBrush, location.X + 5, location.Y + 5, size.Width, size.Height);

            if (fill)
                settings.gfxData.FillRectangle(fillBrush, location.X, location.Y, size.Width, size.Height);

            if (pen.Width > 0)
                settings.gfxData.DrawRectangle(pen, location.X, location.Y, size.Width, size.Height);

            settings.gfxData.DrawString(label, font, fontBrush, location);
        }
    }
}
