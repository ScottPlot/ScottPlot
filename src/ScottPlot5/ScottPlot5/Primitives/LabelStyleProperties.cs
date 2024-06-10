namespace ScottPlot;

/// <summary>
/// Classes with a <see cref="LabelStyle"/> can inherit this
/// to include shortcuts to its most commonly used properties.
/// </summary>
public abstract class LabelStyleProperties : IHasLabel
{
    public abstract Label LabelStyle { get; set; }

    public float LabelOffsetX { get => LabelStyle.OffsetX; set => LabelStyle.OffsetX = value; }
    public float LabelOffsetY { get => LabelStyle.OffsetY; set => LabelStyle.OffsetY = value; }
    public float LabelRotation { get => LabelStyle.Rotation; set => LabelStyle.Rotation = value; }
    public float LabelPadding { set => LabelStyle.Padding = value; }
    public PixelPadding LabelPixelPadding { get => LabelStyle.PixelPadding; set => LabelStyle.PixelPadding = value; }
    public PixelRect LabelLastRenderPixelRect => LabelStyle.LastRenderPixelRect;

    public string LabelText { get => LabelStyle.Text; set => LabelStyle.Text = value; }
    public Alignment LabelAlignment { get => LabelStyle.Alignment; set => LabelStyle.Alignment = value; }

    public string LabelFontName { get => LabelStyle.FontName; set => LabelStyle.FontName = value; }
    public float LabelFontSize { get => LabelStyle.FontSize; set => LabelStyle.FontSize = value; }
    public float? LabelLineSpacing { get => LabelStyle.LineSpacing; set => LabelStyle.LineSpacing = value; }
    public bool LabelItalic { get => LabelStyle.Italic; set => LabelStyle.Italic = value; }
    public bool LabelBold { get => LabelStyle.Bold; set => LabelStyle.Bold = value; }

    public Color LabelFontColor { get => LabelStyle.ForeColor; set => LabelStyle.ForeColor = value; }
    public Color LabelBackgroundColor { get => LabelStyle.BackgroundColor; set => LabelStyle.BackgroundColor = value; }

    public float LabelBorderWidth { get => LabelStyle.BorderWidth; set => LabelStyle.BorderWidth = value; }
    public Color LabelBorderColor { get => LabelStyle.BorderColor; set => LabelStyle.BorderColor = value; }

    public Color LabelShadowColor { get => LabelStyle.ShadowColor; set => LabelStyle.ShadowColor = value; }
    public PixelOffset LabelShadowOffset { get => LabelStyle.ShadowOffset; set => LabelStyle.ShadowOffset = value; }
}
