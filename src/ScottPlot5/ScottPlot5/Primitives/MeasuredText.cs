namespace ScottPlot;

public readonly struct MeasuredText
{
    /// <summary>
    /// Size of the entire multiline label in pixels.
    /// Width is the largest value returned by paint.MeasureText().
    /// Height is the line height multiplied by the number of lines.
    /// </summary>
    public PixelSize Size { get; init; }

    /// <summary>
    /// Vertical spacing between each line.
    /// This is the value returned by GetFontMetrics().
    /// </summary>
    public float LineHeight { get; init; }

    /// <summary>
    /// Recommended vertical offset when calling SKCanvas.DrawText().
    /// See https://github.com/ScottPlot/ScottPlot/issues/3700 for details.
    /// </summary>
    public required float VerticalOffset { get; init; }

    public required float Bottom { get; init; }

    public float Width => Size.Width;
    public float Height => Size.Height;

    public PixelRect Rect(Alignment alignment)
    {
        float xOffset = Width * alignment.HorizontalFraction();
        float yOffset = Height * alignment.VerticalFraction();
        return new PixelRect(Size).WithDelta(-xOffset, yOffset - Height);
    }
}
