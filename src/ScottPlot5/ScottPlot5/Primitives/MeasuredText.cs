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
    /// Width of each line of text in pixel units.
    /// </summary>
    public float[] LineWidths { get; init; }

    /// <summary>
    /// Recommended vertical offset when calling SKCanvas.DrawText().
    /// See https://github.com/ScottPlot/ScottPlot/issues/3700 for details.
    /// </summary>
    public required float VerticalOffset { get; init; }

    /// <summary>
    /// Distance below the baseline the rendered font may occupy.
    /// </summary>
    public required float Bottom { get; init; }

    /// <summary>
    /// Width of the entire text. 
    /// Equals the length of the widest line.
    /// </summary>
    public float Width => Size.Width;

    /// <summary>
    /// Height of the entire text.
    /// </summary>
    public float Height => Size.Height;

    /// <summary>
    /// Return a rectangle representing the bounding box of the entire text
    /// with the alignment point centered at the origin.
    /// </summary>
    public PixelRect Rect(Alignment alignment)
    {
        float xOffset = Width * alignment.HorizontalFraction();
        float yOffset = Height * alignment.VerticalFraction();
        return new PixelRect(Size).WithDelta(-xOffset, yOffset - Height);
    }
}
