using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScottPlot.Hatches
{
    public class Square : IHatch
    {
        static Square()
        {
            bmp = CreateBitmap();
        }
        private static SKBitmap bmp;
        private static SKBitmap CreateBitmap()
        {
            var bitmap = new SKBitmap(20, 20);
            using var paint = new SKPaint() { Color = Colors.White.ToSKColor() };
            using var path = new SKPath();
            using var canvas = new SKCanvas(bitmap);

            canvas.Clear(Colors.Black.ToSKColor());
            canvas.DrawRect(new SKRect(0, 0, 10, 10), paint);

            return bitmap;
        }

        public SKShader GetShader(Color backgroundColor, Color hatchColor, PixelRect rect)
        {
            return SKShader.CreateBitmap(
                bmp,
                SKShaderTileMode.Repeat,
                SKShaderTileMode.Repeat,
                SKMatrix.CreateScale(0.5f, 0.5f))
                    .WithColorFilter(Drawing.GetMaskColorFilter(hatchColor, backgroundColor));
        }
    }
}
