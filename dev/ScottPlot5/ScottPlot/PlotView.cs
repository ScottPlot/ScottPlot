using Microsoft.Maui.Graphics;

namespace ScottPlot;

/// <summary>
/// This object is passed to plottables when rendering
/// </summary>
public class PlotView
{
    public readonly AxisLimits2D AxisLimits;
    public readonly RectangleF DataAreaRect;

    public PlotView(AxisLimits2D limits, RectangleF data)
    {
        AxisLimits = limits;
        DataAreaRect = data;
    }

    public (float x, float y) GetPixelXY(double x, double y) =>
        (GetPixelX(x), GetPixelY(y));

    public float GetPixelX(double x) =>
        GetPixel(
            value: x,
            inverted: false,
            max: AxisLimits.XMax,
            min: AxisLimits.XMin,
            pxPerUnit: DataAreaRect.Width / AxisLimits.XSpan,
            pxOffset: DataAreaRect.X);

    public float GetPixelY(double y) =>
        GetPixel(
            value: y,
            inverted: true,
            max: AxisLimits.YMax,
            min: AxisLimits.YMin,
            pxPerUnit: DataAreaRect.Height / AxisLimits.YSpan,
            pxOffset: DataAreaRect.Y);

    private static float GetPixel(double value, bool inverted, double max, double min, double pxPerUnit, double pxOffset)
    {
        double unitsFromMin = inverted ? max - value : value - min;
        double pxFromMin = unitsFromMin * pxPerUnit;
        double pixel = pxOffset + pxFromMin;
        return (float)pixel;
    }
}