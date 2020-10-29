using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace ScottPlot.Renderable
{
    /// <summary>
    /// This class holds styling information about tick marks, tick labels, and grid lines
    /// </summary>
    public class Ticks
    {
        public double[] Positions;
        public float MarkLength = 10;
        public Color MarkColor = Color.Black;

        public string[] Labels;
        public Drawing.Font LabelFont = new Drawing.Font();
        public float LabelRotation = 0;

        public bool IsGridVisible { get => (GridEnable) && (GridLineWidth > 0) && (GridLineStyle != LineStyle.None); }
        public bool GridEnable = false;
        public Color GridLineColor = ColorTranslator.FromHtml("#efefef");
        public LineStyle GridLineStyle = LineStyle.Solid;
        public float GridLineWidth = 1;

        public bool PixelSnap = true;
    }
}
