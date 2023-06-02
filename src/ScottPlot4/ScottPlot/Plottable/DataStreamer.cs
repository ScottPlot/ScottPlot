using System;
using System.Collections.Generic;
using System.Drawing;

namespace ScottPlot.Plottable;

#nullable enable

public class DataStreamer : IPlottable
{
    public bool IsVisible { get; set; } = true;
    public int XAxisIndex { get; set; } = 0;
    public int YAxisIndex { get; set; } = 0;
    public string Label { get; set; } = string.Empty;
    public Color Color { get; set; } = Color.Blue;
    public float LineWidth { get; set; } = 1;
    public Plot Plot { get; private set; }
    public double[] Data { get; }
    public int DataIndex { get; private set; } = 0;
    public int Count => Data.Length;
    public int TotalPoints { get; private set; } = 0;
    public int TotalPointsOnLastRender { get; private set; } = -1;
    public bool RenderNeeded => TotalPointsOnLastRender != TotalPoints;
    public bool AutomaticallyExpandAxisLimits { get; set; } = true;

    public double OffsetX { get; set; } = 0;
    public double OffsetY { get; set; } = 0;
    public double SamplePeriod { get; set; } = 1;
    public double SampleRate
    {
        get => 1.0 / SamplePeriod;
        set => SamplePeriod = 1.0 / value;
    }

    public double DataMin { get; private set; } = double.PositiveInfinity;
    public double DataMax { get; private set; } = double.NegativeInfinity;

    public DataStreamer(Plot plot, double[] data)
    {

        Plot = plot;
        Data = data;
    }

    public void Add(double value)
    {
        Data[DataIndex] = value;
        DataIndex += 1;

        if (DataIndex >= Data.Length)
            DataIndex = 0;

        DataMin = Math.Min(value, DataMin);
        DataMax = Math.Max(value, DataMax);

        TotalPoints += 1;
    }

    public void Add(IEnumerable<double> values)
    {
        foreach (double value in values)
        {
            Add(value);
        }
    }

    public void ResetMinMax()
    {
        DataMin = double.PositiveInfinity;
        DataMax = double.NegativeInfinity;
    }

    public void ExpandAxisLimits()
    {
        if (double.IsInfinity(DataMin) || double.IsInfinity(DataMax))
            return;

        AxisLimits limits = Plot.GetAxisLimits(XAxisIndex, YAxisIndex);

        if (DataMin < limits.YMin)
        {
            Plot.SetAxisLimits(yMin: DataMin);
        }

        if (DataMax > limits.YMax)
        {
            Plot.SetAxisLimits(yMax: DataMax);
        }
    }

    public void ValidateData(bool deep = false) => Validate.Pass();

    public AxisLimits GetAxisLimits()
    {
        double min = double.PositiveInfinity;
        double max = double.NegativeInfinity;

        for (int i = 0; i < Data.Length; i++)
        {
            min = Math.Min(min, Data[i]);
            max = Math.Max(max, Data[i]);
        }

        return new AxisLimits(
            xMin: OffsetX,
            xMax: OffsetX + Data.Length * SamplePeriod,
            yMin: min,
            yMax: max);
    }

    public LegendItem[] GetLegendItems() => LegendItem.Single(this, Label, Color);

    private void RenderWipe(PlotDimensions dims, Graphics gfx, Pen pen)
    {
        int newestCount = DataIndex;
        int oldestCount = Data.Length - newestCount;

        PointF[] newest = new PointF[newestCount];
        PointF[] oldest = new PointF[oldestCount];

        for (int i = 0; i < newest.Length; i++)
        {
            float x = dims.GetPixelX(i * SamplePeriod + OffsetX);
            float y = dims.GetPixelY(Data[i] + OffsetY);
            newest[i] = new(x, y);
        }

        for (int i = 0; i < oldest.Length; i++)
        {
            float x = dims.GetPixelX((i + DataIndex) * SamplePeriod + OffsetX);
            float y = dims.GetPixelY(Data[i + DataIndex] + OffsetY);
            oldest[i] = new(x, y);
        }

        if (oldest.Length > 0)
            gfx.DrawLines(pen, oldest);

        if (newest.Length > 0)
            gfx.DrawLines(pen, newest);
    }

    private PointF[] RenderScroll(PlotDimensions dims, Graphics gfx, Pen pen, bool scrollLeft = false)
    {
        PointF[] points = new PointF[Data.Length];

        int oldPointCount = Data.Length - DataIndex;

        for (int i = 0; i < Data.Length; i++)
        {
            bool isNewPoint = i < oldPointCount;
            int sourceIndex = isNewPoint ? DataIndex + i : i - oldPointCount;
            int targetIndex = scrollLeft ? i : Data.Length - 1 - i;
            points[targetIndex] = new(
                x: dims.GetPixelX(targetIndex * SamplePeriod + OffsetX),
                y: dims.GetPixelY(Data[sourceIndex] + OffsetY));
        }

        gfx.DrawLines(pen, points);

        return points;
    }

    public void Render(PlotDimensions dims, Bitmap bmp, bool lowQuality = false)
    {
        if (AutomaticallyExpandAxisLimits)
            ExpandAxisLimits();

        using var gfx = ScottPlot.Drawing.GDI.Graphics(bmp, dims, lowQuality);
        using var pen = ScottPlot.Drawing.GDI.Pen(Color, LineWidth, LineStyle.Solid);
        //RenderWipe(dims, gfx, pen);
        RenderScroll(dims, gfx, pen);
    }
}
