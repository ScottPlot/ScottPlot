using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScottPlot.Hatches
{
    public class Gradient : IHatch
    {
        public SKShader GetShader(Color backgroundColor, Color hatchColor, float width, float height)
        {
            Console.WriteLine($"Width: {width} - Height: {height}");
            return SKShader.CreateLinearGradient(
                new SKPoint(0,0), new SKPoint(width, height),
                new[] {backgroundColor.ToSKColor(), hatchColor.ToSKColor()},
                SKShaderTileMode.Clamp);
        }
    }
}
