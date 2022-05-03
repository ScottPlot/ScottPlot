using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace ScottPlot.Cookbook.Recipes.Plottable
{
    public class SignalQuickstart : IRecipe
    {
        public ICategory Category => new Categories.PlotTypes.Signal();
        public string ID => "signal_quickstart";
        public string Title => "Signal Plot Quickstart";
        public string Description => "Signal plots are ideal for evenly-spaced data with thousands or millions of points.";

        public void ExecuteRecipe(Plot plt)
        {
            var rand = new Random(0);
            double[] values = DataGen.RandomWalk(rand, 100_000);
            int sampleRate = 20_000;

            // Signal plots require a data array and a sample rate (points per unit)
            plt.AddSignal(values, sampleRate);

            plt.Benchmark(enable: true);
            plt.Title($"Signal Plot: One Million Points");
        }
    }

    public class SignalOffset : IRecipe
    {
        public ICategory Category => new Categories.PlotTypes.Signal();
        public string ID => "signal_offset";
        public string Title => "Signal Offset";
        public string Description =>
            "Signal plots can have X and Y offsets that shift all data by a defined amount.";

        public void ExecuteRecipe(Plot plt)
        {
            var rand = new Random(0);
            double[] values = DataGen.RandomWalk(rand, 100_000);
            var sig = plt.AddSignal(values);
            sig.OffsetX = 10_000;
            sig.OffsetY = 100;
        }
    }

    public class SignalScatterComparison : IRecipe
    {
        public ICategory Category => new Categories.PlotTypes.Signal();
        public string ID => "signal_advantage";
        public string Title => "Speed Test";
        public string Description => "Compare the speed to the same data plotted as a scatter plot.";

        public void ExecuteRecipe(Plot plt)
        {
            double[] xs = DataGen.Consecutive(100_000, 1.0 / 20_000);
            double[] values = DataGen.RandomWalk(null, 100_000);

            plt.AddScatter(xs, values, Color.Red, markerSize: 0);

            plt.Benchmark(enable: true);
            plt.Title($"Scatter Plot: One Million Points");
        }
    }

    public class CustomLineAndMarkers : IRecipe
    {
        public ICategory Category => new Categories.PlotTypes.Signal();
        public string ID => "signal_styled";
        public string Title => "Styled Signal Plot";
        public string Description =>
            "Signal plots can be styled using public fields. " +
            "Signal plots can also be offset by a defined X or Y amount.";

        public void ExecuteRecipe(Plot plt)
        {
            double[] ys = DataGen.RandomWalk(null, 500);
            int sampleRate = 10;

            var sp2 = plt.AddSignal(ys, sampleRate, Color.Magenta);
            sp2.OffsetY = 1000;
            sp2.OffsetX = 300;
            sp2.LineStyle = LineStyle.Dash;
            sp2.LineWidth = 2;
        }
    }

    public class CustomLineStep : IRecipe
    {
        public ICategory Category => new Categories.PlotTypes.Signal();
        public string ID => "signal_step";
        public string Title => "Step Display";
        public string Description =>
            "Signal plots can be styled as step plots where points " +
            "are connected by right angles instead of straight lines.";

        public void ExecuteRecipe(Plot plt)
        {
            double[] ys = DataGen.Sin(51);

            var sig = plt.AddSignal(ys);
            sig.StepDisplay = true;
            sig.MarkerSize = 0;
        }
    }

    public class RandomWalk_5millionPoints_Signal : IRecipe
    {
        public ICategory Category => new Categories.PlotTypes.Signal();
        public string ID => "signal_5millionPoints";
        public string Title => "5 Million Points";
        public string Description => "Signal plots with millions of points can be interacted with in real time.";

        public void ExecuteRecipe(Plot plt)
        {
            Random rand = new Random(0);
            for (int i = 0; i < 5; i++)
            {
                // add a new signal plot with one million points
                double[] values = DataGen.RandomWalk(rand, 1_000_000);
                plt.AddSignal(values);
            }
            plt.Benchmark(enable: true);
        }
    }

    public class Density : IRecipe
    {
        public ICategory Category => new Categories.PlotTypes.Signal();
        public string ID => "signal_density";
        public string Title => "Display data density";
        public string Description =>
            "When plotting extremely high density data, you can't always see the trends " +
            "underneath all those overlapping data points. If you send an array of colors " +
            "to PlotSignal(), it will use those colors to display density.";

        public void ExecuteRecipe(Plot plt)
        {
            // create an extremely noisy signal with a subtle sine wave beneath it
            Random rand = new Random(0);
            int pointCount = 100_000;
            double[] signal1 = DataGen.Sin(pointCount, 3);
            double[] noise = DataGen.RandomNormal(rand, pointCount, 0, 5);
            double[] data = new double[pointCount];
            for (int i = 0; i < data.Length; i++)
                data[i] = signal1[i] + noise[i];

            // plot the noisy signal using the traditional method
            var sp1 = plt.AddSignal(data);
            sp1.OffsetY = -40;
            sp1.Color = Color.Red;

            // use a custom colors to display data of different densities
            string[] colorCodes = { "#440154", "#39568C", "#1F968B", "#73D055" };
            Color[] colors = colorCodes.Select(x => ColorTranslator.FromHtml(x)).ToArray();

            var sp2 = plt.AddSignal(data);
            sp2.DensityColors = colors;
            sp2.Color = colors[0];

            plt.Title("Color by Density vs. Solid Color");
            plt.AxisAuto(0, .1);
        }
    }

    public class FirstNPoints : IRecipe
    {
        public ICategory Category => new Categories.PlotTypes.Signal();
        public string ID => "signal_firstNpoints";
        public string Title => "Display first N points";
        public string Description =>
            "When plotting live data it is useful to allocate a large array in memory then fill " +
            "it with values as they come in. By setting the maxRenderIndex property of a scatter " +
            "plot to can prevent rendering the end of the array (which is probably filled with zeros).";

        public void ExecuteRecipe(Plot plt)
        {
            // create an array larger than we intend to display
            double[] values = DataGen.RandomWalk(1000);

            // only render the first N points of the signal
            var sig = plt.AddSignal(values);
            sig.MaxRenderIndex = 500;
        }
    }

    public class PlotRange : IRecipe
    {
        public ICategory Category => new Categories.PlotTypes.Signal();
        public string ID => "signal_range";
        public string Title => "Plot a Range of Points";
        public string Description =>
            "It is sometimes useful to only display values within a range of the source data array.";

        public void ExecuteRecipe(Plot plt)
        {
            // create an array larger than we intend to display
            double[] values = DataGen.RandomWalk(1000);

            // only render values between the two defined indexes
            var sig = plt.AddSignal(values);
            sig.MinRenderIndex = 400;
            sig.MaxRenderIndex = 500;
        }
    }

    public class PlotFillRange : IRecipe
    {
        public ICategory Category => new Categories.PlotTypes.Signal();
        public string ID => "signal_fillBelow";
        public string Title => "Fill Below";
        public string Description =>
            "Signal plots can be filled below with a solid color.";

        public void ExecuteRecipe(Plot plt)
        {
            double[] data = DataGen.RandomWalk(1000);

            var sig = plt.AddSignal(data);
            sig.FillBelow();

            plt.Margins(x: 0);
        }
    }

    public class PlotGradientFillRange : IRecipe
    {
        public ICategory Category => new Categories.PlotTypes.Signal();
        public string ID => "signal_fillBelowGradient";
        public string Title => "Gradient Fill Below";
        public string Description =>
            "Signal plots can be filled below using a color gradient.";

        public void ExecuteRecipe(Plot plt)
        {
            double[] data = DataGen.RandomWalk(1000);

            var sig = plt.AddSignal(data);
            sig.FillBelow(Color.Blue, Color.Transparent);

            plt.Margins(x: 0);
        }
    }

    public class PlotGradientFillAboveRange : IRecipe
    {
        public ICategory Category => new Categories.PlotTypes.Signal();
        public string ID => "signal_fillAbove";
        public string Title => "Gradient Fill Above";
        public string Description =>
            "Signal plots can be filled above using a color gradient.";

        public void ExecuteRecipe(Plot plt)
        {
            double[] data = DataGen.RandomWalk(1000);

            var sig = plt.AddSignal(data);
            sig.FillAbove(Color.Blue, Color.Transparent);

            plt.Margins(x: 0);
        }
    }

    public class PlotFillAboveAndBelowRange : IRecipe
    {
        public ICategory Category => new Categories.PlotTypes.Signal();
        public string ID => "signal_fillAboveAndBelow";
        public string Title => "Fill Above and Below";
        public string Description =>
            "Signal plots can be filled above and below";

        public void ExecuteRecipe(Plot plt)
        {
            double[] data = DataGen.RandomWalk(1000);

            var sig = plt.AddSignal(data);
            sig.FillAboveAndBelow(Color.Green, Color.Red);
            sig.Color = Color.Black;
            sig.BaselineY = 7;

            plt.Margins(x: 0);
        }
    }

    public class PlotGradientFillAboveAndBelowRange : IRecipe
    {
        public ICategory Category => new Categories.PlotTypes.Signal();
        public string ID => "signal_gradientAboveAndBelowGradient";
        public string Title => "Gradient Fill Above and Below";
        public string Description =>
            "Gradients can be used to fill above and below.";

        public void ExecuteRecipe(Plot plt)
        {
            double[] data = DataGen.RandomWalk(1000);

            plt.Style(Style.Gray1);

            var sig = plt.AddSignal(data);
            sig.MarkerSize = 0;
            sig.Color = Color.Black;
            sig.FillAboveAndBelow(Color.Green, Color.Transparent, Color.Transparent, Color.Red, 1);
            sig.BaselineY = 5;

            plt.Margins(x: 0);
        }
    }
}
