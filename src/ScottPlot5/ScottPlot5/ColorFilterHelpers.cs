using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScottPlot
{
    internal static class ColorFilterHelpers
    {
        public static SKColorFilter GetMaskColorFilter(Color foreground, Color? background = null)
        {
            background ??= Colors.Black;

            float redDifference = foreground.Red - background.Value.Red;
            float greenDifference = foreground.Green - background.Value.Green;
            float blueDifference = foreground.Blue - background.Value.Blue;
            float alphaDifference = foreground.Alpha - background.Value.Alpha;

            // See https://learn.microsoft.com/en-us/xamarin/xamarin-forms/user-interface/graphics/skiasharp/effects/color-filters for an explanation of this matrix
            // 
            // Essentially, this matrix maps all gray colours to a line from `background.Value` to `foreground`. Black and white are at the extremes on this line, 
            // so they get mapped to `background.Value` and `foreground` respectively
            var mat = new float[] {
                redDifference / 255, 0, 0, 0, background.Value.Red / 255.0f,
                0, greenDifference / 255, 0, 0, background.Value.Green / 255.0f,
                0, 0, blueDifference / 255, 0, background.Value.Blue / 255.0f,
                0, 0, 0, alphaDifference / 255, background.Value.Alpha / 255.0f,
            };

            var filter = SKColorFilter.CreateColorMatrix(mat);
            return filter;
        }
    }
}
