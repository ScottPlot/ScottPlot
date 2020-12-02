using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace ScottPlot.Demo.Customize
{
    class Figure
    {
        public class Background : RecipeBase, IRecipe
        {
            public string name { get; } = "Background Colors";
            public string description { get; } = "Figure and data area background colors can be set individually.";

            public void Render(Plot plt)
            {
                int pointCount = 51;
                double[] x = DataGen.Consecutive(pointCount);
                double[] sin = DataGen.Sin(pointCount);
                double[] cos = DataGen.Cos(pointCount);

                plt.PlotScatter(x, sin);
                plt.PlotScatter(x, cos);

                plt.Style(figureBackgroundColor: Color.LightBlue);
                plt.Style(dataBackgroundColor: Color.LightYellow);
            }
        }

        public class Frame : RecipeBase, IRecipe
        {
            public string name { get; } = "Corner Frame";
            public string description { get; } = "The data are is typically surrounded by a frame (a 1px line). This frame can be customized using arguments.";

            public void Render(Plot plt)
            {
                int pointCount = 51;
                double[] x = DataGen.Consecutive(pointCount);
                double[] sin = DataGen.Sin(pointCount);
                double[] cos = DataGen.Cos(pointCount);

                plt.PlotScatter(x, sin);
                plt.PlotScatter(x, cos);

                plt.Frame(left: true, bottom: true, top: false, right: false);
            }
        }

        public class FigurePadding : RecipeBase, IRecipe
        {
            public string name { get; } = "Figure Padding";
            public string description { get; } = "Extra padding can be added around the data area if desired.";

            public void Render(Plot plt)
            {
                int pointCount = 51;
                double[] x = DataGen.Consecutive(pointCount);
                double[] sin = DataGen.Sin(pointCount);
                double[] cos = DataGen.Cos(pointCount);

                plt.PlotScatter(x, sin);
                plt.PlotScatter(x, cos);

                // custom colors are used to make it easier to see the data and figure areas
                plt.Style(figureBackgroundColor: Color.LightBlue);
                plt.Style(dataBackgroundColor: Color.LightYellow);

                plt.Layout(left: 80, top: 50, bottom: 20, right: 20);
            }
        }

        public class NoPad : RecipeBase, IRecipe
        {
            public string name { get; } = "No Padding";
            public string description { get; } = "This example shows how to only plot the data area (no axis labels, ticks, or tick labels)";

            public void Render(Plot plt)
            {
                int pointCount = 51;
                double[] x = DataGen.Consecutive(pointCount);
                double[] sin = DataGen.Sin(pointCount);
                double[] cos = DataGen.Cos(pointCount);

                plt.PlotScatter(x, sin);
                plt.PlotScatter(x, cos);

                // custom colors are used to make it easier to see the data and figure areas
                plt.Style(figureBackgroundColor: Color.LightBlue);
                plt.Style(dataBackgroundColor: Color.LightYellow);

                plt.Ticks(false, false);
                plt.Frame(false);

                plt.LayoutFrameless();
            }
        }
    }
}
