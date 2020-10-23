using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ScottPlot.Demo.DataDiagnostic
{
    public class DataDiagnostic
    {
        public class SignalNANDemo : PlotDemo, IPlotDemo
        {
            // TODO make good name
            public string name => "Signal with NaN";

            // TODO make good description
            public string description => "Diagnostic mode identifies bad values in plotted data";

            public void Render(Plot plt)
            {
                plt.DiagnosticMode = true;

                // plot some valid data
                Random rand = new Random(0);
                double[] data = DataGen.RandomWalk(rand, 100_000);
                plt.PlotSignal(data);

                // change the value of a single data point to be invalid
                data[1234] = double.NaN;
            }
        }

        public class SignalXYAccendingCheck : PlotDemo, IPlotDemo
        {
            public string name { get; } = "Signal with bad X data";
            public string description { get; } = "Diagnostic mode identifies bad X values in plotted data";

            public void Render(Plot plt)
            {
                plt.DiagnosticMode = true;

                // plot some valid data
                Random rand = new Random(0);
                double[] ys = DataGen.RandomWalk(rand, 1000);
                double[] xs = DataGen.Consecutive(ys.Length);
                plt.PlotSignalXY(xs, ys);

                // modify X values so they are not all ascending
                xs[245] = xs[244] - 9;
            }
        }

        public class ScatterEqualLengthCheck : PlotDemo, IPlotDemo
        {
            public string name { get; } = "Scatter with mismatched X/Y pairs";
            public string description { get; } = "Diagnostic mode identifies when xs and ys do not match length";

            public void Render(Plot plt)
            {
                plt.DiagnosticMode = true;

                double[] xs = { 1, 2, 3 };
                double[] ys = { 1, 2, 3, 4 };
                plt.PlotScatter(xs, ys);
            }
        }
    }
}
