namespace ScottPlot;

public enum ImageScalingStyle
{
    /// <summary>
    /// As-is, no scaling. 
    /// </summary>
    None, 
    /// <summary>
    /// Stretch image to fill drawing rect.
    /// </summary>
    StetchToFill,
    /// <summary>
    /// Scale image to fill drawing rect, retaining the original aspect ratio.
    /// </summary>
    FillRetainAspect
} 