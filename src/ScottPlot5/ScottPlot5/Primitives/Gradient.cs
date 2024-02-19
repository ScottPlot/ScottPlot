namespace ScottPlot;

public class Gradient(GradientType gradientType = GradientType.Linear) : IHatch
{
    /// <summary>
    /// Describes the geometry of a color gradient used to fill an area
    /// </summary>
    public GradientType GradientType { get; set; } = gradientType;

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

    /// <summary>
    /// Colors used for the gradient, or null to use the Hatch colors.
    /// </summary>
    public Color[] Colors { get; set; } = null!;

    /// <summary>
    /// Get or set the positions (in the range of 0..1) of each corresponding color, 
    /// or null to evenly distribute the colors.
    /// </summary>
    public float[] ColorPositions { get; set; } = null!;

    public SKShader GetShader(Color backgroundColor, Color hatchColor, PixelRect rect)
    {
        SKColor[] colors = Colors?.Length > 1
            ? Colors.Select(x => x.ToSKColor()).ToArray()
            : [backgroundColor.ToSKColor(), hatchColor.ToSKColor()];

        return GradientType switch
        {
            GradientType.Radial => SKShader.CreateRadialGradient(
                center: new SKPoint(rect.HorizontalCenter, rect.VerticalCenter),
                radius: Math.Max(rect.Width, rect.Height) / 2.0f,
                colors: colors,
                colorPos: ColorPositions,
                mode: TileMode
                ),

            GradientType.Sweep => SKShader.CreateSweepGradient(
                center: new SKPoint(rect.HorizontalCenter, rect.VerticalCenter),
                colors: colors,
                colorPos: ColorPositions,
                tileMode: TileMode,
                startAngle: StartAngle,
                endAngle: EndAngle
                ),

            GradientType.TwoPointConical => SKShader.CreateTwoPointConicalGradient(
                start: rect.TopLeft.ToSKPoint(),
                startRadius: Math.Min(rect.Width, rect.Height),
                end: rect.BottomRight.ToSKPoint(),
                endRadius: Math.Min(rect.Width, rect.Height),
                colors: colors,
                colorPos: ColorPositions,
                mode: TileMode
                ),

            _ => SKShader.CreateLinearGradient(
                start: rect.GetAlignedPixel(AlignmentStart).ToSKPoint(),
                end: rect.GetAlignedPixel(AlignmentEnd).ToSKPoint(),
                colors: colors,
                colorPos: ColorPositions,
                mode: TileMode
                ),
        };
    }
}
