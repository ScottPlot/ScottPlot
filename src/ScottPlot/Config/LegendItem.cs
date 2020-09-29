using ScottPlot.Drawing;
using System;
using System.Collections.Generic;
using System.Text;

namespace ScottPlot.Config
{
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

        public LegendItem() { }

        // TODO: mark this obsolete
        //[Obsolete("Custimize legend items by modifying their public properties")]
        public LegendItem(
            string label, System.Drawing.Color color,
            LineStyle lineStyle = LineStyle.Solid, double lineWidth = 1,
            MarkerShape markerShape = MarkerShape.filledCircle, double markerSize = 3
            )
        {
            this.label = label;
            this.color = color;

            this.lineStyle = lineStyle;
            this.lineWidth = lineWidth;
            this.markerShape = markerShape;
            this.markerSize = markerSize;
        }
    }
}
