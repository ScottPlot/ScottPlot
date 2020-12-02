using System;
using System.Collections.Generic;
using System.Text;

namespace ScottPlot.Demo.Customize
{
    public class PlotStyles
    {
        public class Default : RecipeBase, IRecipe
        {
            public string name { get; } = "Plot Style (Default)";
            public string description { get; }

            public void Render(Plot plt)
            {
                int pointCount = 51;
                double[] x = DataGen.Consecutive(pointCount);
                double[] sin = DataGen.Sin(pointCount);
                double[] cos = DataGen.Cos(pointCount);

                plt.PlotScatter(x, sin);
                plt.PlotScatter(x, cos);
                plt.XLabel("Horizontal Axis");
                plt.YLabel("Vertical Axis");
                plt.Title("Plot Style: Default");

                plt.Style(ScottPlot.Style.Default);
            }
        }

        public class Seaborn : RecipeBase, IRecipe
        {
            public string name { get; } = "Plot Style (Seaborn)";
            public string description { get; }

            public void Render(Plot plt)
            {
                int pointCount = 51;
                double[] x = DataGen.Consecutive(pointCount);
                double[] sin = DataGen.Sin(pointCount);
                double[] cos = DataGen.Cos(pointCount);

                plt.PlotScatter(x, sin);
                plt.PlotScatter(x, cos);
                plt.XLabel("Horizontal Axis");
                plt.YLabel("Vertical Axis");
                plt.Title("Plot Style: Seaborn");

                plt.Style(ScottPlot.Style.Seaborn);
            }
        }

        public class Control : RecipeBase, IRecipe
        {
            public string name { get; } = "Plot Style (Control)";
            public string description { get; }

            public void Render(Plot plt)
            {
                int pointCount = 51;
                double[] x = DataGen.Consecutive(pointCount);
                double[] sin = DataGen.Sin(pointCount);
                double[] cos = DataGen.Cos(pointCount);

                plt.PlotScatter(x, sin);
                plt.PlotScatter(x, cos);
                plt.XLabel("Horizontal Axis");
                plt.YLabel("Vertical Axis");
                plt.Title("Plot Style: Control");

                plt.Style(ScottPlot.Style.Control);
            }
        }

        public class Blue1 : RecipeBase, IRecipe
        {
            public string name { get; } = "Plot Style (Blue1)";
            public string description { get; }

            public void Render(Plot plt)
            {
                int pointCount = 51;
                double[] x = DataGen.Consecutive(pointCount);
                double[] sin = DataGen.Sin(pointCount);
                double[] cos = DataGen.Cos(pointCount);

                plt.PlotScatter(x, sin);
                plt.PlotScatter(x, cos);
                plt.XLabel("Horizontal Axis");
                plt.YLabel("Vertical Axis");
                plt.Title("Plot Style: Blue1");

                plt.Style(ScottPlot.Style.Blue1);
            }
        }

        public class Blue2 : RecipeBase, IRecipe
        {
            public string name { get; } = "Plot Style (Blue2)";
            public string description { get; }

            public void Render(Plot plt)
            {
                int pointCount = 51;
                double[] x = DataGen.Consecutive(pointCount);
                double[] sin = DataGen.Sin(pointCount);
                double[] cos = DataGen.Cos(pointCount);

                plt.PlotScatter(x, sin);
                plt.PlotScatter(x, cos);
                plt.XLabel("Horizontal Axis");
                plt.YLabel("Vertical Axis");
                plt.Title("Plot Style: Blue2");

                plt.Style(ScottPlot.Style.Blue2);
            }
        }

        public class Blue3 : RecipeBase, IRecipe
        {
            public string name { get; } = "Plot Style (Blue3)";
            public string description { get; }

            public void Render(Plot plt)
            {
                int pointCount = 51;
                double[] x = DataGen.Consecutive(pointCount);
                double[] sin = DataGen.Sin(pointCount);
                double[] cos = DataGen.Cos(pointCount);

                plt.PlotScatter(x, sin);
                plt.PlotScatter(x, cos);
                plt.XLabel("Horizontal Axis");
                plt.YLabel("Vertical Axis");
                plt.Title("Plot Style: Blue3");

                plt.Style(ScottPlot.Style.Blue3);
            }
        }

        public class Light1 : RecipeBase, IRecipe
        {
            public string name { get; } = "Plot Style (Light1)";
            public string description { get; }

            public void Render(Plot plt)
            {
                int pointCount = 51;
                double[] x = DataGen.Consecutive(pointCount);
                double[] sin = DataGen.Sin(pointCount);
                double[] cos = DataGen.Cos(pointCount);

                plt.PlotScatter(x, sin);
                plt.PlotScatter(x, cos);
                plt.XLabel("Horizontal Axis");
                plt.YLabel("Vertical Axis");
                plt.Title("Plot Style: Light1");

                plt.Style(ScottPlot.Style.Light1);
            }
        }

        public class Light2 : RecipeBase, IRecipe
        {
            public string name { get; } = "Plot Style (Light2)";
            public string description { get; }

            public void Render(Plot plt)
            {
                int pointCount = 51;
                double[] x = DataGen.Consecutive(pointCount);
                double[] sin = DataGen.Sin(pointCount);
                double[] cos = DataGen.Cos(pointCount);

                plt.PlotScatter(x, sin);
                plt.PlotScatter(x, cos);
                plt.XLabel("Horizontal Axis");
                plt.YLabel("Vertical Axis");
                plt.Title("Plot Style: Light2");

                plt.Style(ScottPlot.Style.Light2);
            }
        }

        public class Gray1 : RecipeBase, IRecipe
        {
            public string name { get; } = "Plot Style (Gray1)";
            public string description { get; }

            public void Render(Plot plt)
            {
                int pointCount = 51;
                double[] x = DataGen.Consecutive(pointCount);
                double[] sin = DataGen.Sin(pointCount);
                double[] cos = DataGen.Cos(pointCount);

                plt.PlotScatter(x, sin);
                plt.PlotScatter(x, cos);
                plt.XLabel("Horizontal Axis");
                plt.YLabel("Vertical Axis");
                plt.Title("Plot Style: Gray1");

                plt.Style(ScottPlot.Style.Gray1);
            }
        }

        public class Gray2 : RecipeBase, IRecipe
        {
            public string name { get; } = "Plot Style (Gray2)";
            public string description { get; }

            public void Render(Plot plt)
            {
                int pointCount = 51;
                double[] x = DataGen.Consecutive(pointCount);
                double[] sin = DataGen.Sin(pointCount);
                double[] cos = DataGen.Cos(pointCount);

                plt.PlotScatter(x, sin);
                plt.PlotScatter(x, cos);
                plt.XLabel("Horizontal Axis");
                plt.YLabel("Vertical Axis");
                plt.Title("Plot Style: Gray2");

                plt.Style(ScottPlot.Style.Gray2);
            }
        }

        public class Black : RecipeBase, IRecipe
        {
            public string name { get; } = "Plot Style (Black)";
            public string description { get; }

            public void Render(Plot plt)
            {
                int pointCount = 51;
                double[] x = DataGen.Consecutive(pointCount);
                double[] sin = DataGen.Sin(pointCount);
                double[] cos = DataGen.Cos(pointCount);

                plt.PlotScatter(x, sin);
                plt.PlotScatter(x, cos);
                plt.XLabel("Horizontal Axis");
                plt.YLabel("Vertical Axis");
                plt.Title("Plot Style: Black");

                plt.Style(ScottPlot.Style.Black);
            }
        }
    }
}
