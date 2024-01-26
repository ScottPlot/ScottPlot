using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScottPlot.Hatches
{
    public class Grid : IHatch
    {
        public bool Rotate { get; set; }

        static Grid()
        {
            bmp = CreateBitmap();
        }
        public Grid(bool rotate = false)
        {
            Rotate = rotate;
        }

        private static SKBitmap bmp;
        private static SKBitmap CreateBitmap()
        {
            var bmp = new SKBitmap(20, 20);
            using var paint = new SKPaint()
            {
                Color = Colors.White.ToSKColor(),
                IsStroke = true,
                StrokeWidth = 3
            };
            using var path = new SKPath();
            using var canvas = new SKCanvas(bmp);

            canvas.Clear(Colors.Black.ToSKColor());
            canvas.DrawRect(0, 0, 20, 20, paint);

            return bmp;
        }

        public SKShader GetShader(Color backgroundColor, Color hatchColor, PixelRect rect)
        {
            var rotationMatrix = Rotate ? SKMatrix.CreateRotationDegrees(45) : SKMatrix.Identity;

            return SKShader.CreateBitmap(
                bmp,
                SKShaderTileMode.Repeat,
                SKShaderTileMode.Repeat,
                SKMatrix.CreateScale(0.5f, 0.5f)
                    .PostConcat(rotationMatrix))
                    .WithColorFilter(Drawing.GetMaskColorFilter(hatchColor, backgroundColor));
        }
    }
}
