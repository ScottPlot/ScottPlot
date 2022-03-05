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

        private double _lineWidth = 0;
        public double lineWidth
        {
            get => (Parent is IHasLine parent) ? Math.Min(parent.LineWidth, 10) : _lineWidth;
            set { _lineWidth = value; }
        }
        public Color LineColor => Parent is IHasLine p ? p.LineColor : color;

        public MarkerShape markerShape;
        private float _markerSize = 0;
        public float markerSize
        {
            get => (Parent is IHasMarker parent) ? parent.MarkerSize : _markerSize;
            set { _markerSize = value; }
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
