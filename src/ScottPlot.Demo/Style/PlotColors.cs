using System;
using System.Collections.Generic;
using System.Text;

namespace ScottPlot.Demo.Style
{
    public class PlotColors
    {
        public class Default : PlotDemo, IPlotDemo
        {
            public string name { get; } = "Plot Style (Default)";
            public string description { get; }

            public void Render(Plot plt)
            {
                GenericPlots.SinAndCos(plt);
                plt.Style(ScottPlot.Style.Default);
            }
        }

        public class Control : PlotDemo, IPlotDemo
        {
            public string name { get; } = "Plot Style (Control)";
            public string description { get; }

            public void Render(Plot plt)
            {
                GenericPlots.SinAndCos(plt);
                plt.Style(ScottPlot.Style.Control);
            }
        }

        public class Blue1 : PlotDemo, IPlotDemo
        {
            public string name { get; } = "Plot Style (Blue1)";
            public string description { get; }

            public void Render(Plot plt)
            {
                GenericPlots.SinAndCos(plt);
                plt.Style(ScottPlot.Style.Blue1);
            }
        }

        public class Blue2 : PlotDemo, IPlotDemo
        {
            public string name { get; } = "Plot Style (Blue2)";
            public string description { get; }

            public void Render(Plot plt)
            {
                GenericPlots.SinAndCos(plt);
                plt.Style(ScottPlot.Style.Blue2);
            }
        }

        public class Blue3 : PlotDemo, IPlotDemo
        {
            public string name { get; } = "Plot Style (Blue3)";
            public string description { get; }

            public void Render(Plot plt)
            {
                GenericPlots.SinAndCos(plt);
                plt.Style(ScottPlot.Style.Blue3);
            }
        }

        public class Light1 : PlotDemo, IPlotDemo
        {
            public string name { get; } = "Plot Style (Light1)";
            public string description { get; }

            public void Render(Plot plt)
            {
                GenericPlots.SinAndCos(plt);
                plt.Style(ScottPlot.Style.Light1);
            }
        }

        public class Light2 : PlotDemo, IPlotDemo
        {
            public string name { get; } = "Plot Style (Light2)";
            public string description { get; }

            public void Render(Plot plt)
            {
                GenericPlots.SinAndCos(plt);
                plt.Style(ScottPlot.Style.Light2);
            }
        }

        public class Gray1 : PlotDemo, IPlotDemo
        {
            public string name { get; } = "Plot Style (Gray1)";
            public string description { get; }

            public void Render(Plot plt)
            {
                GenericPlots.SinAndCos(plt);
                plt.Style(ScottPlot.Style.Gray1);
            }
        }

        public class Gray2 : PlotDemo, IPlotDemo
        {
            public string name { get; } = "Plot Style (Gray2)";
            public string description { get; }

            public void Render(Plot plt)
            {
                GenericPlots.SinAndCos(plt);
                plt.Style(ScottPlot.Style.Gray2);
            }
        }

        public class Black : PlotDemo, IPlotDemo
        {
            public string name { get; } = "Plot Style (Black)";
            public string description { get; }

            public void Render(Plot plt)
            {
                GenericPlots.SinAndCos(plt);
                plt.Style(ScottPlot.Style.Black);
            }
        }
    }
}
