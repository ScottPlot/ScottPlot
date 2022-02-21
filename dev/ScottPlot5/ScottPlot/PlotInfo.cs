using System;
using System.Collections.Generic;

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

    public readonly List<Axes.IAxis> Axes = new();

    /// <summary>
    /// Defines default styling for the plot such as background color and axis label text colors.
    /// This object is held in <see cref="PlotInfo"/> so it can be accessed by<see cref="IPlottable"/> objects at render time.
    /// </summary>
    public readonly PlotStyle Style = new();

    /// <summary>
    /// Generates ticks at render time based on the size of the figure and axis limits.
    /// You can create your own factory and assign it here to customize tick calculation/placement/styling.
    /// </summary>
    public readonly ITickFactory TickFactory;

    public double PxPerUnitX => DataRect.Width / AxisLimits.Width;
    public double PxPerUnitY => DataRect.Height / AxisLimits.Height;
    public double UnitsPerPxX => AxisLimits.Width / DataRect.Width;
    public double UnitsPerPxY => AxisLimits.Height / DataRect.Height;

    private PlotInfo(
        PixelSize figureSize,
        PixelRect dataRect,
        CoordinateRect axisLimits,
        PlotStyle style,
        ITickFactory tickFactory,
        List<Axes.IAxis> axes)
    {
        FigureRect = new PixelRect(figureSize);
        DataRect = dataRect;
        AxisLimits = axisLimits;
        Style = style;
        TickFactory = tickFactory;
        Axes = axes;
    }

    public Coordinate GetCoordinate(Pixel px) => new(GetCoordinateX(px.X), GetCoordinateY(px.Y));

    public double GetCoordinateX(float xPixel)
    {
        double pxFromMin = xPixel - DataRect.Left;
        double distanceFromMin = pxFromMin * UnitsPerPxX;
        return AxisLimits.XMin + distanceFromMin;
    }

    public double GetCoordinateY(float yPixel)
    {
        double pxFromMin = DataRect.Height + DataRect.Top - yPixel;
        double distanceFromMin = pxFromMin * UnitsPerPxY;
        return AxisLimits.YMin + distanceFromMin;
    }

    public Pixel GetPixel(Coordinate coordinate) => new(GetPixelX(coordinate.X), GetPixelY(coordinate.Y));

    public Pixel GetPixel(double x, double y) => new(GetPixelX(x), GetPixelY(y));

    public float GetPixelX(double x)
    {
        double unitsFromMin = x - AxisLimits.XMin;
        double pxFromMin = unitsFromMin * PxPerUnitX;
        double pixel = DataRect.Left + pxFromMin;
        return (float)pixel;
    }

    public float GetPixelY(double y)
    {
        double unitsFromMin = AxisLimits.YMax - y;
        double pxFromMin = unitsFromMin * PxPerUnitY;
        double pixel = DataRect.Top + pxFromMin;
        return (float)pixel;
    }

    public Pixel GetPixel<T>(T x, T y)
    {
        double xVal = Convert.ToDouble(x);
        double yVal = Convert.ToDouble(y);
        return GetPixel(xVal, yVal);
    }

    public static PlotInfo Default
    {
        get
        {
            PixelSize figureSize = new(400, 300);
            PixelRect dataRect = new PixelRect(figureSize).Contract(40, 20, 30, 20);
            CoordinateRect limits = new(-10, 60, -1.5, 1.5);
            PlotStyle style = new();
            ITickFactory tickFactory = new TickFactories.LegacyNumericTickFactory();

            List<Axes.IAxis> axes = new();
            axes.Add(new Axes.LeftAxis("Vertical Axis"));
            axes.Add(new Axes.BottomAxis("Horizontal Axis"));
            axes.Add(new Axes.RightAxis("Secondary Axis"));
            axes.Add(new Axes.TopAxis("Title"));

            return new PlotInfo(figureSize, dataRect, limits, style, tickFactory, axes);
        }
    }

    public PlotInfo WithDataRect(PixelRect dataRect) =>
        new(FigureRect.Size, dataRect, AxisLimits, Style, TickFactory, Axes);

    public PlotInfo WithPadding(float left, float right, float bottom, float top)
    {
        PixelRect dataRect = new(
            left: left,
            right: FigureRect.Width - right,
            bottom: FigureRect.Height - bottom,
            top: top);

        return this.WithDataRect(dataRect);
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

        return new(newFigureSize, newDataRect, AxisLimits, Style, TickFactory, Axes);
    }

    public PlotInfo WithAxisLimits(CoordinateRect axisLimits) =>
        new(FigureRect.Size, DataRect, axisLimits, Style, TickFactory, Axes);

    public PlotInfo WithPan(Pixel px1, Pixel px2) => WithPan(GetCoordinate(px1) - GetCoordinate(px2));

    public PlotInfo WithPan(Coordinate delta)
    {
        CoordinateRect newLimits = new(
            xMin: AxisLimits.XMin + delta.X,
            xMax: AxisLimits.XMax + delta.X,
            yMin: AxisLimits.YMin + delta.Y,
            yMax: AxisLimits.YMax + delta.Y);

        return new PlotInfo(FigureRect.Size, DataRect, newLimits, Style, TickFactory, Axes);
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
        return WithAxisLimits(newLimits);
    }
}
