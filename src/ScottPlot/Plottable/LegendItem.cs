using ScottPlot.Drawing;

namespace ScottPlot.Plottable
{
    /// <summary>
    /// This class describes a single item that appears in the figure legend.
    /// </summary>
    public class LegendItem
    {
        public string label;
        public System.Drawing.Color color;
        public System.Drawing.Color hatchColor;
        public System.Drawing.Color borderColor;
        public float borderWith;
        public LineStyle borderLineStyle;

        public LineStyle lineStyle;
        public double lineWidth
        {
            get { return Parent is IHasLine p ? System.Math.Min(p.LineWidth, 10) : 0; }
            set { }
        }
        public System.Drawing.Color LineColor => Parent is IHasLine p ? p.LineColor : color;

        public MarkerShape markerShape;
        public float markerSize
        {
            get { return Parent is IHasMarker p ? System.Math.Min(p.MarkerSize, 10) : 0; }
            set { }
        }
        public float markerLineWidth => Parent is IHasMarker p ? System.Math.Min(p.MarkerLineWidth, 3) : (float)lineWidth;
        public System.Drawing.Color MarkerColor => Parent is IHasMarker p ? p.MarkerColor : color;

        public HatchStyle hatchStyle;
        public bool ShowAsRectangleInLegend
        {
            get
            {
                bool hasVeryLargeLineWidth = lineWidth >= 10;
                bool hasArea = (Parent is not null) && (Parent is IHasArea);
                return hasVeryLargeLineWidth || hasArea;
            }
            set { lineWidth = 10; }
        }

        public readonly IPlottable Parent;

        public LegendItem(IPlottable parent)
        {
            Parent = parent;
        }
    }
}
