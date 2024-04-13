using System;

namespace ScottPlot;

public enum LineStyle
{
    None,
    Solid,
    Dash,
    DashDot,
    DashDotDot,
    Dot,
    Custom,
}

public static class LineStylePatterns
{
    public static float[] Dash { get; set; } = { 8.0F, 4.0F };
    public static float[] DashDot { get; set; } = { 8.0F, 4.0F, 2.0F, 4.0F };
    public static float[] DashDotDot { get; set; } = { 8.0F, 4.0F, 2.0F, 4.0F, 2.0F, 4.0F };
    public static float[] Dot { get; set; } = { 2.0F, 4.0F };
    public static float[] Custom { get; set; } = { 4.0F, 8.0F, 4.0F };

    public static float[] GetDashPattern(LineStyle lineStyle)
    {
        if (lineStyle == LineStyle.Dash)
            return Dash;
        else if (lineStyle == LineStyle.DashDot)
            return DashDot;
        else if (lineStyle == LineStyle.DashDotDot)
            return DashDotDot;
        else if (lineStyle == LineStyle.Dot)
            return Dot;
        else if (lineStyle == LineStyle.Custom)
            return Custom;
        else
            throw new NotImplementedException($"{lineStyle} does not have a defined pattern");
    }
}
