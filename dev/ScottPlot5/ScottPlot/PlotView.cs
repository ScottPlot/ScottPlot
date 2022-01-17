using Microsoft.Maui.Graphics;

namespace ScottPlot;

/// <summary>
/// This object is passed to plottables when rendering
/// </summary>
public class PlotView
{
    public readonly AxisLimits2D AxisLimits;
    public readonly RectangleF DataAreaRect;

    public PlotView()
    {
        AxisLimits = new AxisLimits2D(-10, 10, -10, 10);
    }

    public PlotView(AxisLimits2D limits, RectangleF data)
    {
        AxisLimits = limits;
        DataAreaRect = data;
    }

    public Coordinate GetCoordinate(float x, float y) => new(GetCoordinateX(x), GetCoordinateY(y));

    public Pixel GetPixel(double x, double y) => new(GetPixelX(x), GetPixelY(y));

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

    // TODO: distribute this logic into the functions above
    private static float GetPixel(double value, bool inverted, double max, double min, double pxPerUnit, double pxOffset)
    {
        double unitsFromMin = inverted ? max - value : value - min;
        double pxFromMin = unitsFromMin * pxPerUnit;
        double pixel = pxOffset + pxFromMin;
        return (float)pixel;
    }

    public double GetCoordinateX(float xPixel) =>
        GetCoordinate(
            pixel: xPixel,
            inverted: false,
            dataSizePx: DataAreaRect.Width,
            min: AxisLimits.XMin,
            unitsPerPx: AxisLimits.XSpan / DataAreaRect.Width,
            pxOffset: DataAreaRect.X);

    public double GetCoordinateY(float yPixel) =>
        GetCoordinate(
            pixel: yPixel,
            inverted: true,
            dataSizePx: DataAreaRect.Height,
            min: AxisLimits.YMin,
            unitsPerPx: AxisLimits.YSpan / DataAreaRect.Height,
            pxOffset: DataAreaRect.Y);

    // TODO: distribute this logic into the functions above
    private static double GetCoordinate(float pixel, bool inverted, double dataSizePx, double min, double unitsPerPx, double pxOffset)
    {
        double pxFromMin = inverted ? dataSizePx + pxOffset - pixel : pixel - pxOffset;
        return pxFromMin * unitsPerPx + min;
    }
}