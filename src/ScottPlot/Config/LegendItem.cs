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
        public System.Drawing.Color backgroundColor;

        public LineStyle lineStyle;
        public double lineWidth;
        public MarkerShape markerShape;
        public double markerSize;
        public HatchStyle brushPattern;

        public LegendItem(
            string label, System.Drawing.Color color,
            LineStyle lineStyle = LineStyle.Solid, double lineWidth = 1,
            MarkerShape markerShape = MarkerShape.filledCircle, double markerSize = 3,
            System.Drawing.Color? backgroundColor = null,
            HatchStyle brushPattern = HatchStyle.None
            )
        {
            this.label = label;
            this.color = color;

            this.lineStyle = lineStyle;
            this.lineWidth = lineWidth;
            this.markerShape = markerShape;
            this.markerSize = markerSize;

            this.backgroundColor = backgroundColor.HasValue ? backgroundColor.Value : System.Drawing.Color.Black;
            this.brushPattern = brushPattern;
        }
    }
}
