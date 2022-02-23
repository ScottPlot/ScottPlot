using ScottPlot.Drawing;
using System;
using System.Drawing;

namespace ScottPlot.Plottable
{
    /// <summary>
    /// This class describes a single item that appears in the figure legend.
    /// </summary>
    public class LegendItem
    {
        public string label;
        public Color color;
        public Color hatchColor;
        public Color borderColor;
        public float borderWith;
        public LineStyle borderLineStyle;

        public LineStyle lineStyle;
        public double lineWidth
        {
            get
            {
                if (Parent is not IHasLine)
                    return 0;
                double lineWidth = ((IHasLine)Parent).LineWidth;
                return Math.Min(lineWidth, 10);
            }
            set
            {
                // TODO: !!!!!
            }
        }
        public Color LineColor => Parent is IHasLine p ? p.LineColor : color;

        public MarkerShape markerShape;
        public float markerSize
        {
            get
            {
                if (Parent is not IHasMarker)
                    return 0;
                float markerSize = ((IHasMarker)Parent).MarkerSize;
                return Math.Min(markerSize, 10);
            }
            set
            {
                // TODO: !!!!!
            }
        }

        public float markerLineWidth =>
            Parent is IHasMarker parent ? Math.Min(parent.MarkerLineWidth, 3) : (float)lineWidth;

        public Color MarkerColor =>
            Parent is IHasMarker parent ? parent.MarkerColor : color;

        public HatchStyle hatchStyle;
        public bool ShowAsRectangleInLegend
        {
            get
            {
                bool hasVeryLargeLineWidth = lineWidth >= 10;
                bool hasArea = (Parent is not null) && (Parent is IHasArea);
                return hasVeryLargeLineWidth || hasArea;
            }
        }

        public readonly IPlottable Parent;

        public LegendItem(IPlottable parent)
        {
            Parent = parent;
        }
    }
}
