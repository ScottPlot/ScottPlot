namespace ScottPlot
{
    public enum ErrorAction
    {
        Render,
        SkipRender,
        DebugLog,
        ShowErrorOnPlot,
        ThrowException
    }

    public enum Style
    {
        Default,
        Control,
        Blue1,
        Blue2,
        Blue3,
        Light1,
        Light2,
        Gray1,
        Gray2,
        Black,
        Seaborn
    }

    public enum TextAlignment
    {
        upperLeft,
        upperRight,
        upperCenter,
        middleLeft,
        middleRight,
        lowerLeft,
        lowerRight,
        lowerCenter,
        middleCenter
    }

    public enum ImageAlignment
    {
        upperLeft,
        upperRight,
        upperCenter,
        middleLeft,
        middleRight,
        lowerLeft,
        lowerRight,
        lowerCenter,
        middleCenter
    }

    public enum LineStyle
    {
        None,
        Solid,
        Dash,
        DashDot,
        DashDotDot,
        Dot
    }

    public enum FillType
    {
        NoFill,
        FillAbove,
        FillBelow,
        FillAboveAndBelow
    }

    public enum legendLocation
    {
        none,
        upperLeft,
        upperRight,
        upperCenter,
        middleLeft,
        middleRight,
        lowerLeft,
        lowerRight,
        lowerCenter,
    }

    public enum shadowDirection
    {
        none,
        upperLeft,
        upperRight,
        lowerLeft,
        lowerRight,
    }
}
