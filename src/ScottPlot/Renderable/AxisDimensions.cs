using System;

namespace ScottPlot.Renderable
{
    /* This class stores mutable limits for a single axis.
     * Unlike PlotDimensions (immutable objects created just before rendering),
     * values in this class are intended for long term storage.
     */
    public class AxisDimensions
    {
        public float FigureSizePx { get; private set; }
        public float DataSizePx { get; private set; }
        public float DataOffsetPx { get; private set; }
        public bool IsInverted;

        public double Min { get; private set; } = double.NaN;
        public double Max { get; private set; } = double.NaN;
        public double LowerBound { get; private set; } = double.NegativeInfinity;
        public double UpperBound { get; private set; } = double.PositiveInfinity;
        public bool HasBeenSet { get; private set; } = false;
        public bool IsNan => double.IsNaN(Min) || double.IsNaN(Max);

        public double Span => Max - Min;
        public double Center => (Max + Min) / 2;
        public double PxPerUnit => DataSizePx / Span;
        public double UnitsPerPx => Span / DataSizePx;

        public override string ToString() =>
             $"Axis ({Min} to {Max}), figure size {FigureSizePx}, data size {DataSizePx}";

        private double MinRemembered = double.NaN;
        private double MaxRemembered = double.NaN;
        public void Remember() => (MinRemembered, MaxRemembered) = (Min, Max);
        public void Recall() => (Min, Max) = (MinRemembered, MaxRemembered);

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
            HasBeenSet = false;
        }

        public void Resize(float figureSizePx, float? dataSizePx = null, float? dataOffsetPx = null)
        {
            FigureSizePx = figureSizePx;
            DataSizePx = dataSizePx ?? DataSizePx;
            DataOffsetPx = dataOffsetPx ?? DataOffsetPx;
        }

        public void SetPadding(float padBelow, float padAfter)
        {
            DataOffsetPx = padBelow;
            DataSizePx = FigureSizePx - padBelow - padAfter;
        }

        public void SetBounds(double lower = double.NegativeInfinity, double upper = double.PositiveInfinity)
        {
            LowerBound = lower;
            UpperBound = upper;
        }

        private void ApplyBounds()
        {
            Min = Math.Max(Min, LowerBound);
            Max = Math.Min(Max, UpperBound);
        }

        public void SetAxis(double? min, double? max)
        {
            Min = min ?? Min;
            Max = max ?? Max;
            ApplyBounds();
            HasBeenSet = true;
        }

        public void Pan(double units)
        {
            Min += units;
            Max += units;
            ApplyBounds();
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
            ApplyBounds();
        }

        public float GetPixel(double unit)
        {
            double unitsFromMin = IsInverted ? unit - Min : unit - Min;
            double pxFromMin = unitsFromMin * PxPerUnit;
            double pixel = DataOffsetPx + pxFromMin;
            return (float)pixel;
        }

        public double GetUnit(float pixel)
        {
            double pxFromMin = IsInverted ? DataSizePx + DataOffsetPx - pixel : pixel - DataOffsetPx;
            return pxFromMin * UnitsPerPx + Min;
        }
    }
}
