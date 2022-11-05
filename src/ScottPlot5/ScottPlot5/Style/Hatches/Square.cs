using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScottPlot.Style.Hatches
{
    public class Square : IHatch
    {
        protected SKBitmap CreateBitmap(Color backgroundColor, Color hatchColor)
        {
            var bitmap = new SKBitmap(20, 20);
            using var paint = new SKPaint() { Color = hatchColor.ToSKColor() };
            using var path = new SKPath();
            using var canvas = new SKCanvas(bitmap);

            canvas.Clear(backgroundColor.ToSKColor());
            canvas.DrawRect(new SKRect(0, 0, 10, 10), paint);

            return bitmap;
        }

        public SKShader GetShader(Color backgroundColor, Color hatchColor)
        {
            return SKShader.CreateBitmap(
                CreateBitmap(backgroundColor, hatchColor),
                SKShaderTileMode.Repeat,
                SKShaderTileMode.Repeat,
                SKMatrix.CreateScale(0.5f, 0.5f));
        }
    }
}
