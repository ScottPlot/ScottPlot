using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScottPlot.Style.Hatches
{
    public class Dots : IHatch
    {
        protected SKBitmap CreateBitmap(Color backgroundColor, Color hatchColor)
        {
            var bmp = new SKBitmap(20, 20);
            using var paint = new SKPaint() { Color = hatchColor.ToSKColor() };
            using var path = new SKPath();
            using var canvas = new SKCanvas(bmp);

            paint.IsAntialias = true; // AA is especially important for circles, it seems to do little for the other shapes

            canvas.Clear(backgroundColor.ToSKColor());
            canvas.DrawCircle(5, 5, 5, paint);

            return bmp;
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
