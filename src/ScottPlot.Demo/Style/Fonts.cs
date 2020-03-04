using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace ScottPlot.Demo.Style
{
    class Fonts
    {
        public class StyledLabels : PlotDemo, IPlotDemo
        {
            public string name { get; } = "Styled Labels";
            public string description { get; }

            public void Render(Plot plt)
            {
                GenericPlots.SinAndCos(plt);

                plt.Title("Impressive Graph", fontName: "courier new", fontSize: 24, color: Color.Purple, bold: true);
                plt.YLabel("vertical units", fontName: "impact", fontSize: 24, color: Color.Red, bold: true);
                plt.XLabel("horizontal units", fontName: "georgia", fontSize: 24, color: Color.Blue, bold: true);
                plt.PlotText("very graph", 25, .8, fontName: "comic sans ms", fontSize: 24, color: Color.Blue, bold: true);
                plt.PlotText("so data", 0, 0, fontName: "comic sans ms", fontSize: 42, color: Color.Magenta, bold: true);
                plt.PlotText("many documentation", 3, -.6, fontName: "comic sans ms", fontSize: 18, color: Color.DarkCyan, bold: true);
                plt.PlotText("wow.", 10, .6, fontName: "comic sans ms", fontSize: 36, color: Color.Green, bold: true);
                plt.PlotText("NuGet", 32, 0, fontName: "comic sans ms", fontSize: 24, color: Color.Gold, bold: true);
                plt.Legend(fontName: "comic sans ms", fontSize: 16, bold: true, fontColor: Color.DarkBlue);
                plt.Ticks(fontName: "comic sans ms", fontSize: 12, color: Color.DarkBlue);
            }
        }
    }
}
