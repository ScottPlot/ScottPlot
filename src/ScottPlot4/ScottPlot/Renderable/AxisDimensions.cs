using System;

namespace ScottPlot.Renderable
{
    /// <summary>
    /// This class stores MUTABLE axis limits and pixel size information for a SINGLE AXIS. 
    /// Unlike PlotDimensions(immutable objects created just before rendering), 
    /// values in this class are intended for long term storage.
    /// </summary>
    public class AxisDimensions
    {
        /// <summary>
        /// Size of the entire figure (in pixels) if it were to be exported as an image
        /// </summary>
        public float FigureSizePx { get; private set; }

        /// <summary>
        /// Side of just the data area (in pixels).
        /// The data area is the inner rectangle that displays plots.
        /// </summary>
        public float DataSizePx { get; private set; }

        /// <summary>
        /// Offset of the data area (in pixels) relative to the left or top edge of the figure.
        /// </summary>
        public float DataOffsetPx { get; private set; }

        /// <summary>
        /// Indicates whether pixel values ascend in the same direciton as axis unit values.
        /// Horizontal axes are not inverted (both ascend from left to right).
        /// Vertical axes are inverted (units ascend from bottom to top but pixel positions ascend from top to bottom).
        /// </summary>
        public bool IsInverted;

        /// <summary>
        /// Lower edge of the data area (axis units).
        /// </summary>
        public double Min { get; private set; } = double.NaN;

        /// <summary>
        /// Upper edge of the data area (axis units).
        /// </summary>
        public double Max { get; private set; } = double.NaN;

        /// <summary>
        /// Limit beyond which the plot cannot be zoomed in
        /// </summary>
        public double OuterBoundaryMin { get; private set; } = double.NegativeInfinity;

        /// <summary>
        /// Limit beyond which the plot cannot be zoomed in
        /// </summary>
        public double OuterBoundaryMax { get; private set; } = double.PositiveInfinity;

        /// <summary>
        /// Limit which will always be visible on the data area
        /// </summary>
        public double InnerBoundaryMin { get; private set; } = double.PositiveInfinity;

        /// <summary>
        /// Limit which will always be visible on the data area
        /// </summary>
        public double InnerBoundaryMax { get; private set; } = double.NegativeInfinity;

        /// <summary>
        /// Size of the view boundaries.
        /// This should always be greater or equal to the Span.
        /// </summary>
        public double SpanBound => OuterBoundaryMax - OuterBoundaryMin;

        /// <summary>
        /// False until axes are intentionally set.
        /// Unset axes default to NaN min/max limits.
        /// </summary>
        public bool HasBeenSet { get; private set; } = false;

        /// <summary>
        /// True if min or max is NaN.
        /// </summary>
        public bool IsNan => double.IsNaN(Min) || double.IsNaN(Max);

        /// <summary>
        /// Size of the data area (axis units)
        /// </summary>
        public double Span => Max - Min;

        /// <summary>
        /// Center of the data area (axis units)
        /// </summary>
        public double Center => (Max + Min) / 2;

        /// <summary>
        /// Number of pixels for each axis unit
        /// </summary>
        public double PxPerUnit => DataSizePx / Span;

        /// <summary>
        /// Size of 1 screen pixel in axis units
        /// </summary>
        public double UnitsPerPx => Span / DataSizePx;

        public override string ToString() =>
             $"Axis ({Min} to {Max}), figure size {FigureSizePx}, data size {DataSizePx}";

        /// <summary>
        /// Limit remember/recall is used while mouse dragging
        /// </summary>
        private double MinRemembered = double.NaN;

        /// <summary>
        /// Limit remember/recall is used while mouse dragging
        /// </summary>
        private double MaxRemembered = double.NaN;

        /// <summary>
        /// If true, min/max cannot bet set.
        /// </summary>
        private bool LockedLimits = false;

        /// <summary>
        /// Limit remember/recall is used while mouse dragging
        /// </summary>
        public void Remember() => (MinRemembered, MaxRemembered) = (Min, Max);

        /// <summary>
        /// Limit remember/recall is used while mouse dragging
        /// </summary>
        public void Recall() => (Min, Max) = (MinRemembered, MaxRemembered);

        /// <summary>
        /// Return limits that contain no NaNs.
        /// NaNs will be replaced with +/- 10.
        /// </summary>
        /// <returns></returns>
        public (double min, double max) RationalLimits()
        {
            double min = double.IsNaN(Min) ? -10 : Min;
            double max = double.IsNaN(Max) ? 10 : Max;
            return (min == max) ? (min - .5, max + .5) : (min, max);
        }

        /// <summary>
        /// Reset the axis as if it were never set.
        /// </summary>
        public void ResetLimits()
        {
            Min = double.NaN;
            Max = double.NaN;
            HasBeenSet = false;
            SetBoundsOuter();
            SetBoundsInner();
        }

        /// <summary>
        /// Resize/reposition this axis according to the given pixel units
        /// </summary>
        public void Resize(float figureSizePx, float? dataSizePx = null, float? dataOffsetPx = null)
        {
            FigureSizePx = figureSizePx;
            DataSizePx = dataSizePx ?? DataSizePx;
            DataOffsetPx = dataOffsetPx ?? DataOffsetPx;
        }

        /// <summary>
        /// Set data size and offset based on desired padding between the edge of the figure and data area
        /// </summary>
        public void SetPadding(float padBefore, float padAfter)
        {
            DataOffsetPx = padBefore;
            DataSizePx = FigureSizePx - padBefore - padAfter;
        }

        /// <summary>
        /// Set boundaries beyond which this axis cannot be panned or zoomed
        /// </summary>
        public void SetBoundsOuter(double lower = double.NegativeInfinity, double upper = double.PositiveInfinity)
        {
            OuterBoundaryMin = lower;
            OuterBoundaryMax = upper;
        }

        /// <summary>
        /// Set boundaries beyond which this axis cannot be panned or zoomed
        /// </summary>
        public void SetBoundsInner(double lower = double.PositiveInfinity, double upper = double.NegativeInfinity)
        {
            InnerBoundaryMin = lower;
            InnerBoundaryMax = upper;
        }

        /// <summary>
        /// Modify axis limits such that none extend beyond the boundaries.
        /// </summary>
        private void ApplyBounds()
        {
            if (Span > SpanBound)
            {
                Min = OuterBoundaryMin;
                Max = OuterBoundaryMax;
                return;
            }

            if (Min < OuterBoundaryMin)
            {
                double span = Span;
                Min = OuterBoundaryMin;
                Max = OuterBoundaryMin + span;
            }

            if (Max > OuterBoundaryMax)
            {
                double span = Span;
                Max = OuterBoundaryMax;
                Min = OuterBoundaryMax - span;
            }

            if (Min > InnerBoundaryMin)
                Min = InnerBoundaryMin;

            if (Max < InnerBoundaryMax)
                Max = InnerBoundaryMax;
        }

        /// <summary>
        /// Set axis limits
        /// </summary>
        public void SetAxis(double? min, double? max)
        {
            if (LockedLimits)
                return;

            HasBeenSet = true;
            Min = min ?? Min;
            Max = max ?? Max;
            ApplyBounds();
        }

        /// <summary>
        /// Shift min and max by the given number of units
        /// </summary>
        public void Pan(double units)
        {
            if (LockedLimits)
                return;

            Min += units;
            Max += units;
            ApplyBounds();
        }

        /// <summary>
        /// Shift min and max by the given number of pixels
        /// </summary>
        public void PanPx(float pixels)
        {
            if (LockedLimits)
                return;

            Pan(pixels * UnitsPerPx);
        }

        /// <summary>
        /// Zoom by simultaneously adjusting Min and Max
        /// </summary>
        /// <param name="frac">1 for no change, 2 zooms in, .5 zooms out.</param>
        /// <param name="zoomTo">If given, zoom toward/from this alternative center point.</param>
        public void Zoom(double frac = 1, double? zoomTo = null)
        {
            if (LockedLimits)
                return;

            zoomTo ??= Center;
            (Min, Max) = RationalLimits();
            double spanLeft = zoomTo.Value - Min;
            double spanRight = Max - zoomTo.Value;
            Min = zoomTo.Value - spanLeft / frac;
            Max = zoomTo.Value + spanRight / frac;
            ApplyBounds();
        }

        /// <summary>
        /// Get the pixel location on the figure for a given position in axis units
        /// </summary>
        public float GetPixel(double unit)
        {
            double unitsFromMin = IsInverted ? Max - unit : unit - Min;
            double pxFromMin = unitsFromMin * PxPerUnit;
            double pixel = DataOffsetPx + pxFromMin;
            return (float)pixel;
        }

        /// <summary>
        /// Get the axis unit position for the given pixel location on the figure
        /// </summary>
        public double GetUnit(float pixel)
        {
            double pxFromMin = IsInverted ? DataSizePx + DataOffsetPx - pixel : pixel - DataOffsetPx;
            return pxFromMin * UnitsPerPx + Min;
        }

        /// <summary>
        /// Sets a flag indicating whether axis limits are mutable.
        /// </summary>
        public void LockLimits(bool locked = true)
        {
            LockedLimits = locked;
        }
    }
}
