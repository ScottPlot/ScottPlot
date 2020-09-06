using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Text;

namespace ScottPlot.Space
{
    /// <summary>
    /// This class handles state (pixel dimensions and axis limits) of a single dimension
    /// </summary>
    public class LinearAxis : IAxis1D
    {
        public float FigureSizePx { get; private set; }
        public float DataSizePx { get; private set; }
        public float DataOffsetPx { get; private set; }
        public double Min { get; private set; }
        public double Max { get; private set; }
        public bool IsValid { get => (Min != Max) && !double.IsNaN(Min) && !double.IsNaN(Max); }

        public bool IsLocked { get; set; } = true;

        private readonly bool Inverted;
        public LinearAxis(bool inverted = false)
        {
            Inverted = inverted;
        }

        private AxisLimits1D RememberedLimits;
        public void RememberLimits() => RememberedLimits = GetLimits();
        public void RecallLimits() => SetLimits(RememberedLimits);

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

        public AxisLimits1D GetLimits() => new AxisLimits1D(Min, Max);

        public void SetLimits(AxisLimits1D limits)
        {
            Min = limits.Min;
            Max = limits.Max;
            Recalculate();
        }

        public double Span { get; private set; } = double.NaN;
        public double Center { get; private set; } = double.NaN;
        public double UnitsPerPx { get; private set; } = double.NaN;
        public double PxPerUnit { get; private set; } = double.NaN;
        private void Recalculate()
        {
            Span = Max - Min;
            Center = (Max + Min) / 2;
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

        public void PanPx(float deltaPx)
        {
            double panUnits = deltaPx * UnitsPerPx;
            if (Inverted)
            {
                Min += panUnits;
                Max += panUnits;
            }
            else
            {
                Min -= panUnits;
                Max -= panUnits;
            }
            Recalculate();
        }

        public void Zoom(double frac)
        {
            double pad = Span / 2;
            Min = Center - pad / frac;
            Max = Center + pad / frac;
            Recalculate();
        }

        public void ZoomTo(double frac, double target)
        {
            double padLeft = target - Min;
            double padRight = Max - target;
            Min = target - padLeft / frac;
            Max = target + padRight / frac;
            Recalculate();
        }

        public void ZoomPx(float deltaPx)
        {
            if (Inverted)
                deltaPx *= -1;
            double deltaUnits = deltaPx * UnitsPerPx;
            double deltaFrac = deltaUnits / (Math.Abs(deltaUnits) + Span);
            double zoomFrac = Math.Pow(10, deltaFrac);
            Zoom(zoomFrac);
        }

        public void ZoomPx(float deltaPx, float targetPx)
        {
            throw new NotImplementedException();
        }
    }
}
