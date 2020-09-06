﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ScottPlot.Config
{
    public class LegendItem
    {
        public string label;
        public System.Drawing.Color color;

        public LineStyle lineStyle;
        public double lineWidth;
        public MarkerShape markerShape;
        public double markerSize;

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
