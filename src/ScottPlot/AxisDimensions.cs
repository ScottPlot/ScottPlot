using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace ScottPlot
{
    /* This class stores dimensions for a single axis.
     * Unlike PlotDimensions (which are created just before rendering),
     * values in this class are intended for long term storage.
     */
    public class AxisDimensions
    {
        public float FigureSizePx { get; private set; }
        public float DataSizePx { get; private set; }
        public float DataOffsetPx { get; private set; }
        public readonly bool IsInverted;

        public double Min { get; private set; } = double.NaN;
        public double Max { get; private set; } = double.NaN;
        public bool HasBeenSet { get; private set; } = false;

        public double Span => Max - Min;
        public double Center => (Max + Min) / 2;
        public double PxPerUnit => DataSizePx / Span;
        public double UnitsPerPx => Span / DataSizePx;

        public (double min, double max) RationalLimits()
        {
            double min = double.IsNaN(Min) ? -10 : Min;
            double max = double.IsNaN(Max) ? 10 : Max;
            return (min == max) ? (min - .5, max + .5) : (min, max);
        }

        public void ResetLimits()
        {
            Min = double.NaN;
            Max = double.NaN;
        }

        public void Resize(float figureSizePx)
        {
            FigureSizePx = figureSizePx;
        }

        public void SetPadding(float padBelow, float padAfter)
        {
            DataOffsetPx = padBelow;
            DataSizePx = FigureSizePx - padBelow - padAfter;
        }

        public void SetAxis(double? min, double? max)
        {
            Min = min ?? Min;
            Max = max ?? Max;
        }

        public void Pan(double units)
        {
            Min += units;
            Max += units;
        }

        public void PanPx(float pixels)
        {
            Pan(pixels * UnitsPerPx);
        }

        public void Zoom(double frac = 1, double? zoomTo = null)
        {
            zoomTo = zoomTo ?? Center;
            double spanLeft = zoomTo.Value - Min;
            double spanRight = Max - zoomTo.Value;
            Min = zoomTo.Value - spanLeft / frac;
            Max = zoomTo.Value + spanRight / frac;
        }

        public float GetPixel(double unit)
        {
            if (IsInverted)
            {
                double unitsFromMin = unit - Min;
                double pxFromMin = unitsFromMin * PxPerUnit;
                double pixel = DataOffsetPx + pxFromMin;
                return (float)pixel;
            }
            else
            {
                double unitsFromMax = Max - unit;
                double pxFromMax = unitsFromMax * PxPerUnit;
                double pixel = DataOffsetPx + pxFromMax;
                return (float)pixel;
            }
        }

        public double GetUnit(float pixel)
        {
            if (IsInverted)
            {
                return DataSizePx - ((pixel - Min) * PxPerUnit);
            }
            else
            {
                return (pixel - DataOffsetPx) / PxPerUnit + Min;
            }
        }
    }
}
