using NUnit.Framework;

namespace ScottPlotTests
{
    public class Styling
    {
        [Test]
        public void Test_Style_CreateThenModify()
        {
            double[] xs = { 1, 2, 3, 4, 5 };
            double[] ys = { 1, 4, 9, 16, 25 };

            var plt = new ScottPlot.Plot(400, 300);

            // initialize with thin green line and diamond markers
            var scatter = plt.PlotScatter(xs: xs, ys: ys,
                color: System.Drawing.Color.Green, lineWidth: 2, markerSize: 10,
                markerShape: ScottPlot.MarkerShape.filledDiamond);


            // modify for thick magenta line and circle markers
            scatter.lineWidth = 10;
            scatter.color = System.Drawing.Color.Magenta;
            scatter.markerSize = 20;
            scatter.markerShape = ScottPlot.MarkerShape.filledCircle;

            // do the same for a scatter plot
            var signal = plt.PlotSignal(ys: ys, yOffset: 5,
                color: System.Drawing.Color.Green, lineWidth: 2, markerSize: 10); // TODO: marker shape not currently supported?
            signal.lineWidth = 10;
            signal.color = System.Drawing.Color.Orange;
            signal.markerSize = 20;

            string name = System.Reflection.MethodBase.GetCurrentMethod().Name;
            plt.SaveFig(System.IO.Path.GetFullPath(name + ".png"));
        }
    }
}
