using System;

namespace ScottPlot;

/// <summary>
/// This object defines the layout and axis limits of a plot figure.
/// It holds the minimum necessary state required to reproduce a plot.
/// </summary>
public class PlotInfo
{
    public readonly PixelRect FigureRect;
    public float Width => FigureRect.Width;
    public float Height => FigureRect.Height;

    public readonly PixelRect DataRect;

    public readonly CoordinateRect AxisLimits;

    public readonly float DisplayScale = 1.0f;

    public double PxPerUnitX => DataRect.Width / AxisLimits.Width;
    public double PxPerUnitY => DataRect.Height / AxisLimits.Height;
    public double UnitsPerPxX => AxisLimits.Width / DataRect.Width;
    public double UnitsPerPxY => AxisLimits.Height / DataRect.Height;

    public PlotInfo(PixelSize figureSize, PixelRect dataRect, CoordinateRect axisLimits)
    {
        FigureRect = new PixelRect(figureSize);
        DataRect = dataRect;
        AxisLimits = axisLimits;
    }

    public Coordinate GetCoordinate(Pixel px) => new(GetCoordinateX(px.X), GetCoordinateY(px.Y));
    public double GetCoordinateX(float xPixel) => GetCoordinate(xPixel, false, DataRect.Width, AxisLimits.XMin, UnitsPerPxX, DataRect.Left);
    public double GetCoordinateY(float yPixel) => GetCoordinate(yPixel, true, DataRect.Height, AxisLimits.YMin, UnitsPerPxY, DataRect.Top);

    public Pixel GetPixel(Coordinate coordinate) => new(GetPixelX(coordinate.X), GetPixelY(coordinate.Y));
    public Pixel GetPixel(double x, double y) => new(GetPixelX(x), GetPixelY(y));
    public float GetPixelX(double x) => GetPixel(x, false, AxisLimits.XMax, AxisLimits.XMin, PxPerUnitX, DataRect.Left);
    public float GetPixelY(double y) => GetPixel(y, true, AxisLimits.YMax, AxisLimits.YMin, PxPerUnitY, DataRect.Top);


    // TODO: distribute this logic into the functions above
    private static float GetPixel(double value, bool inverted, double max, double min, double pxPerUnit, double pxOffset)
    {
        double unitsFromMin = inverted ? max - value : value - min;
        double pxFromMin = unitsFromMin * pxPerUnit;
        double pixel = pxOffset + pxFromMin;
        return (float)pixel;
    }


    // TODO: distribute this logic into the functions above
    private static double GetCoordinate(float pixel, bool inverted, double dataSizePx, double min, double unitsPerPx, double pxOffset)
    {
        double pxFromMin = inverted ? dataSizePx + pxOffset - pixel : pixel - pxOffset;
        return pxFromMin * unitsPerPx + min;
    }

    public Pixel GetPixel<T>(T x, T y)
    {
        double xVal = System.Convert.ToDouble(x);
        double yVal = System.Convert.ToDouble(y);
        return GetPixel(xVal, yVal);
    }

    public static PlotInfo Default
    {
        get
        {
            PixelSize figureSize = new(400, 300);
            PixelRect dataRect = new PixelRect(figureSize).Contract(40, 20, 30, 20);
            CoordinateRect limits = new(-10, 60, -1.5, 1.5);
            return new(figureSize, dataRect, limits);
        }
    }

    public PlotInfo WithSize(int width, int height)
    {
        float padLeft = DataRect.Left;
        float padRight = FigureRect.Width - DataRect.Right;
        float padTop = DataRect.Top;
        float padBottom = FigureRect.Height - DataRect.Bottom;

        PixelSize newFigureSize = new(width, height);
        PixelSize newDataSize = new(width - padRight - padLeft, height - padTop - padBottom);
        Pixel newDataOffset = new(padLeft, padTop);
        PixelRect newDataRect = new(newDataSize, newDataOffset);

        return new(newFigureSize, newDataRect, AxisLimits);
    }

    public PlotInfo WithPan(Pixel px1, Pixel px2) => WithPan(GetCoordinate(px1) - GetCoordinate(px2));

    public PlotInfo WithPan(Coordinate delta)
    {
        CoordinateRect newLimits = new(
            xMin: AxisLimits.XMin + delta.X,
            xMax: AxisLimits.XMax + delta.X,
            yMin: AxisLimits.YMin + delta.Y,
            yMax: AxisLimits.YMax + delta.Y);

        return new PlotInfo(FigureRect.Size, DataRect, newLimits);
    }

    public PlotInfo WithZoom(Pixel px, double frac)
    {
        var coord = GetCoordinate(px);
        return WithZoom(frac, frac, coord.X, coord.Y);
    }

    public PlotInfo WithZoom(Pixel px1, Pixel px2)
    {
        double deltaX = px2.X - px1.X;
        double deltaFracX = deltaX / (Math.Abs(deltaX) + Width);
        double fracX = Math.Pow(10, deltaFracX);

        double deltaY = px2.Y - px1.Y;
        double deltaFracY = -deltaY / (Math.Abs(deltaY) + Height);
        double fracY = Math.Pow(10, deltaFracY);

        return WithZoom(fracX, fracY, AxisLimits.XCenter, AxisLimits.YCenter);
    }

    public PlotInfo WithZoom(double fracX, double fracY, double zoomToX, double zoomToY)
    {
        double spanLeftX = zoomToX - AxisLimits.XMin;
        double spanRightX = AxisLimits.XMax - zoomToX;
        double newMinX = zoomToX - spanLeftX / fracX;
        double newMaxX = zoomToX + spanRightX / fracX;

        double spanLeftY = zoomToY - AxisLimits.YMin;
        double spanRightY = AxisLimits.YMax - zoomToY;
        double newMinY = zoomToY - spanLeftY / fracY;
        double newMaxY = zoomToY + spanRightY / fracY;

        CoordinateRect newLimits = new(newMinX, newMaxX, newMinY, newMaxY);
        return new PlotInfo(FigureRect.Size, DataRect, newLimits);
    }
}
