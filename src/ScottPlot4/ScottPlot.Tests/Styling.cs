using System;
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
            var scatter = plt.AddScatter(xs: xs, ys: ys,
                color: System.Drawing.Color.Green, lineWidth: 2, markerSize: 10,
                markerShape: ScottPlot.MarkerShape.filledDiamond);


            // modify for thick magenta line and circle markers
            scatter.LineWidth = 10;
            scatter.Color = System.Drawing.Color.Magenta;
            scatter.MarkerSize = 20;
            scatter.MarkerShape = ScottPlot.MarkerShape.filledCircle;

            // do the same for a scatter plot
            var signal = plt.AddSignal(ys: ys, color: System.Drawing.Color.Green);
            signal.OffsetY = 5;
            signal.LineWidth = 10;
            signal.Color = System.Drawing.Color.Orange;
            signal.MarkerSize = 20;

            string name = System.Reflection.MethodBase.GetCurrentMethod().Name;
            plt.SaveFig(System.IO.Path.GetFullPath(name + ".png"));
        }

        [Test]
        public void Test_List_Styles()
        {
            var styles = ScottPlot.Style.GetStyles();
            Assert.IsNotNull(styles);
            Assert.IsNotEmpty(styles);
            foreach (var style in styles)
                Console.WriteLine(style);
        }
    }
}
