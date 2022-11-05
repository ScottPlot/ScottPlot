using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScottPlot.Style.Hatches
{
    public  class Grid : IHatch
    {
        protected SKBitmap CreateBitmap(Color backgroundColor, Color hatchColor)
        {
            var bmp = new SKBitmap(20, 20);
            using var paint = new SKPaint()
            {
                Color = hatchColor.ToSKColor(),
                IsStroke = true,
                StrokeWidth = 3
            };
            using var path = new SKPath();
            using var canvas = new SKCanvas(bmp);

            canvas.Clear(backgroundColor.ToSKColor());
            canvas.DrawRect(0, 0, 20, 20, paint);

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
