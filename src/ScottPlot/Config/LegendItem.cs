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
    }
}
