using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Text;

namespace ScottPlot.Space
{
    /// <summary>
    /// This class handles state (pixel dimensions and axis limits) of a single dimension
    /// </summary>
    public class LinearAxis : IAxis
    {
        public float FigureSizePx { get; private set; }
        public float DataSizePx { get; private set; }
        public float DataOffsetPx { get; private set; }
        public double Min { get; private set; }
        public double Max { get; private set; }

        private double OldMin, OldMax;
        public void Remember() => (OldMin, OldMax) = (Min, Max);
        public void Recall() => SetLimits(OldMin, OldMax);

        private readonly bool Inverted;
        public LinearAxis(bool inverted = false)
        {
            Inverted = inverted;
        }

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
                UnitsPerPx = Span / (DataSizePx - 1);
                PxPerUnit = (DataSizePx - 1) / Span;
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

        public float GetPixel(double position)
        {
            if (Inverted)
            {
                double unitsFromMax = Max - position;
                double pxFromMax = unitsFromMax * PxPerUnit;
                double pixel = DataOffsetPx + pxFromMax;
                return (float)pixel;
            }
            else
            {
                double unitsFromMin = position - Min;
                double pxFromMin = unitsFromMin * PxPerUnit;
                double pixel = DataOffsetPx + pxFromMin;
                return (float)pixel;
            }
        }

        public double GetPosition(float pixel)
        {
            if (Inverted)
            {
                float pxFromMax = pixel - DataOffsetPx;
                double unitsFromMax = pxFromMax * UnitsPerPx;
                return Max - unitsFromMax;
            }
            else
            {
                float pxFromMin = pixel - DataOffsetPx;
                double unitsFromMin = pxFromMin * UnitsPerPx;
                return Min + unitsFromMin;
            }
        }
    }
}
