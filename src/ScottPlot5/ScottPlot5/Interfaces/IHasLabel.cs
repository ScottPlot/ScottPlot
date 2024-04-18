namespace ScottPlot;

interface IHasLabel
{
    Label LabelStyle { get; }

    float LabelOffsetX { get; set; }
    float LabelOffsetY { get; set; }
    float LabelRotation { get; set; }
    float LabelPadding { set; }
    PixelPadding LabelPixelPadding { get; set; }

    string LabelText { get; set; }
    Alignment LabelAlignment { get; set; }

    string LabelFontName { get; set; }
    float LabelFontSize { get; set; }
    float? LabelLineSpacing { get; set; }
    bool LabelItalic { get; set; }
    bool LabelBold { get; set; }

    Color LabelFontColor { get; set; }
    Color LabelBackgroundColor { get; set; }

    float LabelBorderWidth { get; set; }
    Color LabelBorderColor { get; set; }

    Color LabelShadowColor { get; set; }
    PixelOffset LabelShadowOffset { get; set; }
}
