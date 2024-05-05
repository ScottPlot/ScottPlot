using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScottPlot.Plottables
{
    public class Coxcomb : PieBase
    {

        public Coxcomb(IList<PieSlice> slices)
        {
            Slices = slices;
        }

        public override AxisLimits GetAxisLimits()
        {
            double radius = ShowSliceLabels
                ? SliceLabelDistance + Padding
                : 1 + Padding;

            return new AxisLimits(-radius, radius, -radius, radius);
        }

        public override void Render(RenderPack rp)
        {
            double total = Slices.Sum(s => s.Value);
            float[] sliceSizes = Slices.Select(x => (float)(x.Value / total)).ToArray();

            Pixel origin = Axes.GetPixel(Coordinates.Origin);

            using SKPath path = new();
            using SKPaint paint = new() { IsAntialias = true };

            var rotationPerSlice = 360f / Slices.Count;
            using SKAutoCanvasRestore _ = new(rp.Canvas);
            rp.Canvas.Translate(origin.X, origin.Y);


            for (int i = 0; i < Slices.Count; i++)
            {
                rp.Canvas.RotateDegrees(rotationPerSlice);

                float degrees1 = rotationPerSlice / 2;
                float degrees2 = rotationPerSlice / 2;

                SKPoint ptInner = GetRotatedPoint(sliceSizes[i], degrees1); // Unlike piecharts this is unique (there's no donut coxcomb charts)
                SKPoint ptOuterHome = GetRotatedPoint(sliceSizes[i], degrees1);
                SKPoint ptOuterRotated = GetRotatedPoint(sliceSizes[i], degrees2);

                float minX = Math.Abs(Axes.GetPixelX(sliceSizes[i]) - origin.X);
                float minY = Math.Abs(Axes.GetPixelY(sliceSizes[i]) - origin.Y);
                var radius = Math.Min(minX, minY);
                var rect = new SKRect(-radius, -radius, radius, radius);

                if (rotationPerSlice != 360)
                {
                    path.MoveTo(ptInner);
                    path.LineTo(ptOuterHome);
                    path.ArcTo(rect, -rotationPerSlice / 2, rotationPerSlice, false);
                    path.Close();
                }
                else
                {
                    path.AddOval(rect);
                }

                Slices[i].Fill.ApplyToPaint(paint, new PixelRect(origin, radius));
                paint.Shader = paint.Shader?.WithLocalMatrix(SKMatrix.CreateRotationDegrees(-rotationPerSlice));
                rp.Canvas.DrawPath(path, paint);

                LineStyle.ApplyToPaint(paint);
                rp.Canvas.DrawPath(path, paint);

                path.Reset();
            }

            if (ShowSliceLabels)
            {
                for (int i = 0; i < Slices.Count; i++)
                {
                    double x = Math.Cos(rotationPerSlice * Math.PI / 180) * SliceLabelDistance;
                    double y = -Math.Sin(rotationPerSlice * Math.PI / 180) * SliceLabelDistance;
                    Pixel px = Axes.GetPixel(new Coordinates(x, y));
                    Slices[i].LabelStyle.Render(rp.Canvas, px, paint);
                }
            }
        }
    }
}
