using SkiaSharp;
using SkiaSharp.Views.Desktop;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing.Drawing2D;

namespace ScottPlotSkia
{
    public static class SkaiExtensions
    {
        public static SKStrokeJoin ToSKStrokeJoin(this LineJoin join)
        {
            switch (join)
            {
                case LineJoin.Miter:
                    return SKStrokeJoin.Miter;
                case LineJoin.Bevel:
                    return SKStrokeJoin.Bevel;
                case LineJoin.Round:
                    return SKStrokeJoin.Round;
                default:
                    return SKStrokeJoin.Miter;
            }
        }

        public static SKStrokeCap ToSKStrokeCap(this LineCap cap)
        {
            switch (cap)
            {
                case LineCap.Flat:
                    return SKStrokeCap.Butt;
                case LineCap.Square:
                    return SKStrokeCap.Square;
                case LineCap.Round:
                    return SKStrokeCap.Round;
                default:
                    return SKStrokeCap.Butt;
            }
        }
        public static SKPaint ToSKPaint(this Pen pen, bool AntiAlias = false)
        {
            SKPaint result = new SKPaint();
            result.Color = pen.Color.ToSKColor();
            result.StrokeWidth = pen.Width;
            result.StrokeJoin = pen.LineJoin.ToSKStrokeJoin();
            result.StrokeCap = pen.StartCap.ToSKStrokeCap();
            result.StrokeMiter = pen.MiterLimit;
            result.IsAntialias = AntiAlias;
            if (pen.DashStyle != DashStyle.Solid)
            {
                result.PathEffect = SKPathEffect.CreateDash(pen.DashPattern.Select(x => x * pen.Width).ToArray(), 0);
            }
            return result;
        }

        public static SKPaint ToSKPaint(this Brush brush, bool AntiAlias = false)
        {
            SKPaint result = new SKPaint();
            var solidBrush = brush as SolidBrush;
            if (solidBrush == null)
                throw new ArgumentOutOfRangeException("Unsupported brush type: " + brush.GetType().ToString());
            result.Color = solidBrush.Color.ToSKColor();
            result.IsStroke = true;
            result.IsAntialias = AntiAlias;
            result.Style = SKPaintStyle.Fill;
            return result;
        }
    }
}
