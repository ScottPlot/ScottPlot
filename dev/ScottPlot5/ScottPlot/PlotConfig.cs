using Microsoft.Maui.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ScottPlot;

public class PlotConfig
{
    public readonly PixelRect FigureRect;
    public float Width => FigureRect.Width;
    public float Height => FigureRect.Height;

    public readonly PixelRect DataRect;

    public readonly CoordinateRect AxisLimits;

    public readonly float DisplayScale = 1.0f;

    public readonly List<Axes.IAxis> Axes = new();

    public readonly PlotStyle Style = new();

    public double PxPerUnitX => DataRect.Width / AxisLimits.Width;
    public double PxPerUnitY => DataRect.Height / AxisLimits.Height;
    public double UnitsPerPxX => AxisLimits.Width / DataRect.Width;
    public double UnitsPerPxY => AxisLimits.Height / DataRect.Height;

    private PlotConfig(
        PixelSize figureSize,
        PixelRect dataRect,
        CoordinateRect axisLimits,
        PlotStyle style,
        List<Axes.IAxis> axes)
    {
        FigureRect = new PixelRect(figureSize);
        DataRect = dataRect;
        AxisLimits = axisLimits;
        Style = style;
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

    public static PlotConfig Default
    {
        get
        {
            PixelSize figureSize = new(400, 300);
            PixelRect dataRect = new PixelRect(figureSize).Contract(40, 20, 30, 20);
            CoordinateRect limits = new(-10, 60, -1.5, 1.5);
            PlotStyle style = new();

            List<Axes.IAxis> axes = new()
            {
                new Axes.LeftAxis("Vertical Axis", true),
                new Axes.BottomAxis("Horizontal Axis", true),
                new Axes.RightAxis("Secondary Axis", true),
                new Axes.TopAxis("Title", true),

                new Axes.LeftAxis("Vertical Axis #3", true),
                new Axes.RightAxis("Vertical Axis #4", true),
                new Axes.LeftAxis("Vertical Axis #5", true),
                new Axes.RightAxis("Vertical Axis #6", true),

                new Axes.BottomAxis("Horizontal Axis #7", true),
                new Axes.TopAxis("Horizontal Axis #8", true),
            };

            return new PlotConfig(figureSize, dataRect, limits, style, axes);
        }
    }

    public PlotConfig WithTightLayout(ICanvas canvas)
    {
        /* Layout adjustment is a circular problem:
         *   - Tick label size determines figure edge padding
         *   - Figure edge padding determines data area size
         *   - Data area size determines tick labels
         *   
         * The solution is:
         *   - Estimate figure edge padding using generic ticks with fixed size
         *   - Calculate preliminary ticks using the estimated padding
         *   - Measure tick labels to determine the final figure edge padding
         *   - Regenerate ticks using the final layout
         */

        List<(Axes.IAxis, Tick[])>? genericTicks = null;
        var genericInfo = this.WithTightDataRect(canvas, genericTicks);
        List<(Axes.IAxis, Tick[])> preliminaryTicks = genericInfo.GenerateAllTicks();
        var preliminaryInfo = genericInfo.WithTightDataRect(canvas, preliminaryTicks);
        List<(Axes.IAxis, Tick[])> realTicks = preliminaryInfo.GenerateAllTicks();
        return preliminaryInfo.WithTightDataRect(canvas, realTicks);
    }

    public List<(Axes.IAxis, Tick[])> GenerateAllTicks()
    {
        // TODO: is there a better datatype for this?

        List<(Axes.IAxis, Tick[])> ticksByAxis = new();

        foreach (Axes.IAxis axis in Axes)
        {
            Tick[] ticks = axis.TickFactory.GenerateTicks(this);
            ticksByAxis.Add((axis, ticks));
        }

        return ticksByAxis;
    }

    private PlotConfig WithTightDataRect(ICanvas canvas, List<(Axes.IAxis, Tick[])>? ticksByAxis)
    {
        List<Tick> ticks = new();
        if (ticksByAxis is not null)
            foreach (var item in ticksByAxis)
                ticks.AddRange(item.Item2);

        float padL = Axes.Where(x => x.Edge is Edge.Left).Select(x => x.Measure(canvas, ticks.ToArray())).Sum();
        float padR = Axes.Where(x => x.Edge is Edge.Right).Select(x => x.Measure(canvas, ticks.ToArray())).Sum();
        float padT = Axes.Where(x => x.Edge is Edge.Top).Select(x => x.Measure(canvas, ticks.ToArray())).Sum();
        float padB = Axes.Where(x => x.Edge is Edge.Bottom).Select(x => x.Measure(canvas, ticks.ToArray())).Sum();
        return WithPadding(padL, padR, padB, padT);
    }

    public PlotConfig WithDataRect(PixelRect dataRect) =>
        new(FigureRect.Size, dataRect, AxisLimits, Style, Axes);

    public PlotConfig WithPadding(float left, float right, float bottom, float top)
    {
        PixelRect dataRect = new(
            left: left,
            right: FigureRect.Width - right,
            bottom: FigureRect.Height - bottom,
            top: top);

        return this.WithDataRect(dataRect);
    }

    public PlotConfig WithSize(int width, int height)
    {
        float padLeft = DataRect.Left;
        float padRight = FigureRect.Width - DataRect.Right;
        float padTop = DataRect.Top;
        float padBottom = FigureRect.Height - DataRect.Bottom;

        PixelSize newFigureSize = new(width, height);
        PixelSize newDataSize = new(width - padRight - padLeft, height - padTop - padBottom);
        Pixel newDataOffset = new(padLeft, padTop);
        PixelRect newDataRect = new(newDataSize, newDataOffset);

        return new(newFigureSize, newDataRect, AxisLimits, Style, Axes);
    }

    public PlotConfig WithAxisLimits(CoordinateRect axisLimits) =>
        new(FigureRect.Size, DataRect, axisLimits, Style, Axes);

    public PlotConfig WithPan(Pixel px1, Pixel px2) => WithPan(GetCoordinate(px1) - GetCoordinate(px2));

    public PlotConfig WithPan(Coordinate delta)
    {
        CoordinateRect newLimits = new(
            xMin: AxisLimits.XMin + delta.X,
            xMax: AxisLimits.XMax + delta.X,
            yMin: AxisLimits.YMin + delta.Y,
            yMax: AxisLimits.YMax + delta.Y);

        return new PlotConfig(FigureRect.Size, DataRect, newLimits, Style, Axes);
    }

    public PlotConfig WithZoom(Pixel px, double frac)
    {
        var coord = GetCoordinate(px);
        return WithZoom(frac, frac, coord.X, coord.Y);
    }

    public PlotConfig WithZoom(Pixel px1, Pixel px2)
    {
        double deltaX = px2.X - px1.X;
        double deltaFracX = deltaX / (Math.Abs(deltaX) + Width);
        double fracX = Math.Pow(10, deltaFracX);

        double deltaY = px2.Y - px1.Y;
        double deltaFracY = -deltaY / (Math.Abs(deltaY) + Height);
        double fracY = Math.Pow(10, deltaFracY);

        return WithZoom(fracX, fracY, AxisLimits.XCenter, AxisLimits.YCenter);
    }

    public PlotConfig WithZoom(double fracX, double fracY, double zoomToX, double zoomToY)
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
