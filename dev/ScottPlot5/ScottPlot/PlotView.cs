using Microsoft.Maui.Graphics;

namespace ScottPlot;

/// <summary>
/// This object represents all aspect of a figure's dimensions and coordinate system.
/// It is injected into the Draw() method of Plottable objects.
/// It is stateless (modifications return copies).
/// </summary>
public struct PlotView
{
    public readonly CoordinateRect AxisLimits;
    public readonly RectangleF DataRect;
    public readonly RectangleF FigureRect;

    public bool HasDataArea => DataRect.Width > 0 && DataRect.Height > 0;

    public bool HasFigureArea => FigureRect.Width > 0 && FigureRect.Height > 0;

    public PlotView(CoordinateRect limits, RectangleF figure, RectangleF data)
    {
        AxisLimits = limits;
        FigureRect = figure;
        DataRect = data;
    }

    public override string ToString() => $"AxisLimits={AxisLimits} FigureRect={FigureRect} DataRect={DataRect}";

    #region layout modifications

    public PlotView WithSize(float width, float height)
    {
        RectangleF newFigureArea = new(0, 0, width, height);

        float padLeft = DataRect.Left;
        float padRight = FigureRect.Width - DataRect.Right;
        float padBottom = FigureRect.Height - DataRect.Bottom;
        float padTop = DataRect.Top;

        RectangleF newDataArea = new(
            x: padLeft,
            y: padTop,
            width: width - padRight - padLeft,
            height: height - padTop - padBottom);

        return new PlotView(AxisLimits, newFigureArea, newDataArea);
    }

    public PlotView WithPadding(float left, float right, float bottom, float top)
    {
        RectangleF newDataArea = new(
            x: left,
            y: top,
            width: FigureRect.Width - left - right,
            height: FigureRect.Height - top - bottom);
        return new PlotView(AxisLimits, FigureRect, newDataArea);
    }

    #endregion

    #region axis modifications

    public PlotView WithPan(Pixel px1, Pixel px2) => WithPan(GetCoordinate(px1) - GetCoordinate(px2));

    public PlotView WithPan(Coordinate delta) => new(AxisLimits + delta, FigureRect, DataRect);

    public PlotView WithAxisLimits(double xMin, double xMax, double yMin, double yMax) =>
        new(new CoordinateRect(xMin, xMax, yMin, yMax), FigureRect, DataRect);

    #endregion

    #region pixel/coordinate translation

    public Coordinate GetCoordinate(Pixel pixel) => new(GetCoordinateX(pixel.X), GetCoordinateY(pixel.Y));

    public Coordinate GetCoordinate(float x, float y) => new(GetCoordinateX(x), GetCoordinateY(y));

    public Pixel GetPixel(Coordinate coordinate) => new(GetPixelX(coordinate.X), GetPixelY(coordinate.Y));

    public Pixel GetPixel(double x, double y) => new(GetPixelX(x), GetPixelY(y));

    public float GetPixelX(double x) =>
        GetPixel(
            value: x,
            inverted: false,
            max: AxisLimits.XMax,
            min: AxisLimits.XMin,
            pxPerUnit: DataRect.Width / AxisLimits.XSpan,
            pxOffset: DataRect.X);

    public float GetPixelY(double y) =>
        GetPixel(
            value: y,
            inverted: true,
            max: AxisLimits.YMax,
            min: AxisLimits.YMin,
            pxPerUnit: DataRect.Height / AxisLimits.YSpan,
            pxOffset: DataRect.Y);

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
            dataSizePx: DataRect.Width,
            min: AxisLimits.XMin,
            unitsPerPx: AxisLimits.XSpan / DataRect.Width,
            pxOffset: DataRect.X);

    public double GetCoordinateY(float yPixel) =>
        GetCoordinate(
            pixel: yPixel,
            inverted: true,
            dataSizePx: DataRect.Height,
            min: AxisLimits.YMin,
            unitsPerPx: AxisLimits.YSpan / DataRect.Height,
            pxOffset: DataRect.Y);

    // TODO: distribute this logic into the functions above
    private static double GetCoordinate(float pixel, bool inverted, double dataSizePx, double min, double unitsPerPx, double pxOffset)
    {
        double pxFromMin = inverted ? dataSizePx + pxOffset - pixel : pixel - pxOffset;
        return pxFromMin * unitsPerPx + min;
    }

    #endregion
}