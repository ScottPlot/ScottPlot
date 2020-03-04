using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace ScottPlot.Demo.PlotTypes
{
    class Text
    {
        public class Quickstart : PlotDemo, IPlotDemo
        {
            public string name { get; } = "Text Quickstart";
            public string description { get; } = "Text can be placed at any X/Y location and styled using arguments.";

            public void Render(Plot plt)
            {
                GenericPlots.SinAndCos(plt);

                plt.PlotText("testing one", 25, .8);

                plt.PlotText("testing two", 0, 0, fontName: "comic sans ms", fontSize: 42, color: Color.Magenta, bold: true);
            }
        }
    }
}
