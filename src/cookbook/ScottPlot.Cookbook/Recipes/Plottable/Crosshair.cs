using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScottPlot.Cookbook.Recipes.Plottable
{
    public class CrosshairQuickstart : IRecipe
    {
        public string Category => "Plottable: Crosshair";
        public string ID => "crosshair_quickstart";
        public string Title => "Crosshair";
        public string Description =>
            "The Crosshair plot type draws vertical and horizontal lines that intersect at " +
            "a point on the plot and the coordinates of those lines are displayed on top of the " +
            "axis ticks. This plot type is typically updated after MouseMove events to track the mouse";

        public void ExecuteRecipe(Plot plt)
        {
            plt.AddSignal(ScottPlot.DataGen.Sin(51));
            plt.AddSignal(ScottPlot.DataGen.Cos(51));
            plt.AddCrosshair(42, 0.48);

            plt.Title("Crosshair Demo");
            plt.XLabel("Horizontal Axis");
            plt.YLabel("Vertical Axis");
        }
    }

    public class CrosshairCustomize : IRecipe
    {
        public string Category => "Plottable: Crosshair";
        public string ID => "crosshair_customize";
        public string Title => "Crosshair Customization";
        public string Description =>
            "Crosshair styling and label formatting can be customized by accessing public fields.";

        public void ExecuteRecipe(Plot plt)
        {
            plt.Title("Customized Crosshair");
            plt.XLabel("Horizontal Axis");
            plt.YLabel("Vertical Axis");

            plt.AddSignal(ScottPlot.DataGen.Sin(51));
            plt.AddSignal(ScottPlot.DataGen.Cos(51));

            var ch = plt.AddCrosshair(42, 0.48);
            ch.LabelBackgroundColor = System.Drawing.Color.LightBlue;
            ch.LabelFont.Color = System.Drawing.Color.Black;
            ch.LabelFont.Name = ScottPlot.Drawing.InstalledFont.Monospace();
            ch.LabelFont.Size = 16;
            ch.LineStyle = LineStyle.Dot;
            ch.LineColor = System.Drawing.Color.Blue;
        }
    }

    public class CrosshairDateTime : IRecipe
    {
        public string Category => "Plottable: Crosshair";
        public string ID => "crosshair_formatting";
        public string Title => "DateTime Axis Label";
        public string Description =>
            "Crosshair labels display numeric labels by default, but a public field makes it possible to " +
            "convert positions to DateTime (FromOATime) when generating their axis labels.";

        public void ExecuteRecipe(Plot plt)
        {
            plt.Title("Crosshair with DateTime Axis");
            plt.XLabel("Horizontal Axis");
            plt.YLabel("Vertical Axis");

            // plot DateTime data
            int pointCount = 100;
            Random rand = new Random(0);
            double[] values = ScottPlot.DataGen.RandomWalk(rand, pointCount);
            DateTime[] dates = Enumerable.Range(0, pointCount)
                                          .Select(x => new DateTime(2016, 06, 27).AddDays(x))
                                          .ToArray();
            double[] xs = dates.Select(x => x.ToOADate()).ToArray();
            plt.AddScatter(xs, values);

            // add a crosshair
            var ch = plt.AddCrosshair(xs[50], values[50]);

            // indicaite horizontal axis is DateTime and give a proper DateTime format string
            // https://docs.microsoft.com/en-us/dotnet/standard/base-types/custom-date-and-time-format-strings
            ch.IsDateTimeX = true;
            ch.StringFormatX = "d";

            // use a numeric vertical axis but customize the format string
            // https://docs.microsoft.com/en-us/dotnet/standard/base-types/standard-numeric-format-strings
            ch.IsDateTimeY = false;
            ch.StringFormatY = "F4";
        }
    }
}
