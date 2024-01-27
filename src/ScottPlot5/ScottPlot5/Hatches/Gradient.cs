using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScottPlot.Hatches
{
    public enum GradiantType
    {
        Linear,
        Radial,
        Sweep,
        TwoPointConical
    }
    public class Gradient : IHatch
    {
        public GradiantType Type { get; set; } = GradiantType.Linear;

        public Gradient(GradiantType type = GradiantType.Linear)
        {
            Type = type;
        }
        /// <summary>
        /// Get or set the start angle in degrees for sweep gradient
        /// </summary>
        public float StartAngle { get; set; } = 0f;
        /// <summary>
        /// Get or set the end angle in degrees for sweep gradient
        /// </summary>
        public float EndAngle { get; set; } = 360f;
        /// <summary>
        /// Get or set how the shader should handle drawing outside the original bounds.
        /// </summary>
        public SKShaderTileMode TileMode { get; set; } = SKShaderTileMode.Clamp;

        /// <summary>
        /// Start of linear gradient
        /// </summary>
        public Alignment AlignmentStart { get; set; } = Alignment.UpperLeft;
        /// <summary>
        /// End of linear gradient
        /// </summary>
        public Alignment AlignmentEnd { get; set; } = Alignment.LowerRight;

        public SKShader GetShader(Color backgroundColor, Color hatchColor, PixelRect rect)
        {
            return Type switch
            {
                GradiantType.Radial => SKShader.CreateRadialGradient(
                    new SKPoint(rect.HorizontalCenter, rect.VerticalCenter),
                    Math.Max(rect.Width, rect.Height) / 2.0f,
                    new SKColor[] { backgroundColor.ToSKColor(), hatchColor.ToSKColor() },
                    TileMode
                    ),

                GradiantType.Sweep => SKShader.CreateSweepGradient(
                    new SKPoint(rect.HorizontalCenter, rect.VerticalCenter),
                    new SKColor[] { backgroundColor.ToSKColor(), hatchColor.ToSKColor() },
                    null,
                    TileMode,
                    StartAngle, EndAngle
                    ),

                GradiantType.TwoPointConical => SKShader.CreateTwoPointConicalGradient(
                    rect.TopLeft.ToSKPoint(), Math.Min(rect.Width, rect.Height),
                    rect.BottomRight.ToSKPoint(), Math.Min(rect.Width, rect.Height),
                    new SKColor[] { backgroundColor.ToSKColor(), hatchColor.ToSKColor() },
                    null,
                    TileMode
                    ),

                _ => SKShader.CreateLinearGradient(
                    rect.GetAlignedPixel(AlignmentStart).ToSKPoint(), rect.GetAlignedPixel(AlignmentEnd).ToSKPoint(),
                    new SKColor[] { backgroundColor.ToSKColor(), hatchColor.ToSKColor() },
                    TileMode)
            };
        }
    }
}
