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
        public MarkerShape markerShape;
        public double markerSize;
        public HatchStyle hatchStyle;
        public bool IsRectangle
        {
            get { return lineWidth >= 10; }
            set { lineWidth = 10; }
        }
    }
}
