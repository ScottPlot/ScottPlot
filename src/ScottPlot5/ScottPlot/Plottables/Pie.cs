using ScottPlot.Style;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScottPlot.Plottables
{
    public class PieSlice
    {
        public string? Label { get; set; }
        public double Value { get; set; }
        public Fill Fill { get; set; }
    }

    public class Pie : PlottableBase
    {
        public IList<PieSlice> Slices { get; set; }
        public Stroke Stroke { get; set; } = new() { Width = 0 };

        public Pie(IList<PieSlice> slices)
        {
            Slices = slices;
        }

        public override AxisLimits GetAxisLimits()
        {
            return new AxisLimits(-1, 1, -1, 1);
        }

        public override void Render(SKSurface surface, PixelRect dataRect)
        {
            // TODO: Maybe this can be done in a call to base.Render()?
            if (XAxis is null || YAxis is null)
                throw new InvalidOperationException("Both axes must be set before first render");

            surface.Canvas.ClipRect(dataRect.ToSKRect()); // TODO: This too?


            double total = Slices.Sum(s => s.Value);
            var origin = new Pixel(XAxis.GetPixel(0, dataRect), YAxis.GetPixel(0, dataRect));
            
            // TODO: Bring back PxPerUnit?
            double radius = Math.Min(Math.Abs(XAxis.GetPixel(1, dataRect) - origin.X), Math.Abs(YAxis.GetPixel(1, dataRect) - origin.Y));
            SKRect rect = new((float)(origin.X - radius), (float)(origin.Y - radius), (float)(origin.X + radius), (float)(origin.Y + radius));

            using SKPath path = new();
            using SKPaint paint = new() { IsAntialias = true };

            double start = 0;
            foreach (var slice in Slices)
            {
                double sweep = slice.Value / total * 360;

                path.MoveTo(origin.X, origin.Y);
                path.ArcTo(rect, (float)start, (float)sweep, false);
                path.Close();

                paint.SetFill(slice.Fill);
                surface.Canvas.DrawPath(path, paint);

                paint.SetStroke(Stroke);
                surface.Canvas.DrawPath(path, paint);

                path.Reset();

                start += sweep;
            }
        }
    }
}
