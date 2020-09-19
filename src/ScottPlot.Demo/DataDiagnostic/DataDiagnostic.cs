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
            public string name => "SignalContainNAN";

            // TODO make good description
            public string description => "Data values containing NAN values can be catched using DiagnosticMode";

            public void Render(Plot plt)
            {
                plt.DiagnosticMode = true;

                Random rand = new Random(0);
                int pointCount = (int)1e6;
                int lineCount = 5;
                var y = Enumerable.Range(0, lineCount).Select(i => DataGen.RandomWalk(rand, pointCount)).ToArray();
                y[3][18] = Double.NaN;
                for (int i = 0; i < lineCount; i++)
                    plt.PlotSignal(y[i]);

                plt.DiagnosticMode = false; // disable to not affect on another demos

                //TODO make good Title
                plt.Title("Signal Plot Quickstart with NAN value throw");
                plt.YLabel("Vertical Units");
                plt.XLabel("Horizontal Units");
                plt.DiagnosticMode = false;

            }
        }

        public class SignalXYAccendingCheck : PlotDemo, IPlotDemo
        {
            // TODO make good name/description
            public string name { get; } = "Signal with X and Y data";
            public string description { get; } = "After 5 seconds wrong data writed to xs array, this can be catch on Render with DiagnosticMode";

            public void Render(Plot plt)
            {
                plt.DiagnosticMode = true;

                // generate random, unevenly-spaced data
                Random rand = new Random(0);
                int pointCount = 100_000;
                double[] ys = new double[pointCount];
                double[] xs = new double[pointCount];
                for (int i = 1; i < ys.Length; i++)
                {
                    ys[i] = ys[i - 1] + rand.NextDouble() - .5;
                    xs[i] = xs[i - 1] + rand.NextDouble();
                }

                plt.Title($"SignalXY Plot ({pointCount:N0} points)");
                plt.PlotSignalXY(xs, ys);

                plt.DiagnosticMode = false; // disable to not affect another demos

                // All plotables created with DiagnosticMode on, will be checked also after diagnostic mode = false
                
                // Check goes only on Render, so user must interract with plot after 5 seconds
                Task.Run(() =>
                  {
                      Thread.Sleep(5000);
                      xs[245] = xs[244] - 9;
                  });
            }
        }
    }
}
