using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace ScottPlot
{
    // style of data objects things to be plotted
    public class Style
    {
        public enum LineStyle { solid, dashed, dotted };
        public enum MarkerShape { circleFilled, circleOpen, square, point, x, cross, triangle };

        // defaults are defined here

        public int lineWidth = 1;
        public LineStyle lineStyle = LineStyle.dashed;
        public Color lineColor = Color.Red;

        public int markerSize = 2;
        public MarkerShape markerShape = MarkerShape.circleFilled;
        public Color markerColor = Color.Red;

        public Style(int colorNumber = 0)
        {
            lineColor = ColorByNumber(colorNumber);
            markerColor = ColorByNumber(colorNumber);
        }

        Random rand;
        Color RandomColor()
        {
            if (rand == null)
                rand = new Random();
            return Color.FromArgb(rand.Next(255), rand.Next(255), rand.Next(255));
        }

        Color ColorByNumber(int i)
        {
            // https://github.com/vega/vega/wiki/Scales#scale-range-literals
            /*
            string[] colors20 = new string[] { "#1f77b4", "#aec7e8", "#ff7f0e", "#ffbb78",
                "#2ca02c", "#98df8a", "#d62728", "#ff9896", "#9467bd", "#c5b0d5",
                "#8c564b", "#c49c94", "#e377c2", "#f7b6d2", "#7f7f7f", "#c7c7c7",
                "#bcbd22", "#dbdb8d", "#17becf", "#9edae5", };
            */
            string[] colors = new string[] { "#1f77b4", "#ff7f0e", "#2ca02c", "#d62728",
                    "#9467bd", "#8c564b", "#e377c2", "#7f7f7f", "#bcbd22", "#17becf" };
            return System.Drawing.ColorTranslator.FromHtml(colors[i % colors.Length]);
        }
}
}
