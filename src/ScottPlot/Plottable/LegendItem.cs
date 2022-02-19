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

        public LineStyle lineStyle;
        public double lineWidth;
        public System.Drawing.Color LineColor => Parent is IHasLine p ? p.LineColor : color;

        public MarkerShape markerShape;
        public double markerSize;

        public float markerLineWidth => Parent is IHasMarker p ? System.Math.Min(p.MarkerLineWidth, 3) : (float)lineWidth;
        public System.Drawing.Color MarkerColor => Parent is IHasMarker p ? p.MarkerColor : color;

        public HatchStyle hatchStyle;
        public bool IsRectangle
        {
            get { return lineWidth >= 10; }
            set { lineWidth = 10; }
        }

        public readonly IPlottable Parent;

        public LegendItem(IPlottable parent)
        {
            Parent = parent;
        }
    }
}
