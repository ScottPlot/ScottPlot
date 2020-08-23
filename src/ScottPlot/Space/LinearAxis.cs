using System;
using System.Collections.Generic;
using System.Text;

namespace ScottPlot.Space
{
    /// <summary>
    /// This class handles state (pixel dimensions and axis limits) of a single dimension
    /// </summary>
    public class LinearAxis
    {
        public float FigureSizePx { get; private set; }
        public float DataSizePx { get; private set; }
        public float DataOffsetPx { get; private set; }
        public double Min { get; private set; }
        public double Max { get; private set; }

        public void Resize(float figureSize, float dataSize, float dataOffset)
        {
            FigureSizePx = figureSize;
            DataSizePx = dataSize;
            DataOffsetPx = dataOffset;
            Recalculate();
        }

        public void SetLimits(double min, double max)
        {
            Min = min;
            Max = max;
            Recalculate();
        }

        public double Span { get; private set; } = double.NaN;
        public double UnitsPerPx { get; private set; } = double.NaN;
        public double PxPerUnit { get; private set; } = double.NaN;
        private void Recalculate()
        {
            Span = Max - Min;
            if (FigureSizePx > 0 && Span > 0)
            {
                UnitsPerPx = Span / DataSizePx;
                PxPerUnit = DataSizePx / Span;
            }
            else
            {
                UnitsPerPx = double.NaN;
                PxPerUnit = double.NaN;
            }
        }

        public void Pan(float deltaPx)
        {
            double panUnits = deltaPx * UnitsPerPx;
            Min += panUnits;
            Max += panUnits;
        }

        public float GetPixel(double unit, bool inverted = false)
        {
            double unitsFromMin = unit - Min;
            double pxFromMin = unitsFromMin * PxPerUnit;
            double pixel = inverted ? DataOffsetPx + DataSizePx - pxFromMin : DataOffsetPx + pxFromMin;
            return (float)pixel;
        }
    }
}
