using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScottPlotTests.UnitTests
{
    internal class LegendTitleTest
    {
        [Test]
        public void Test_LegendTitle()
        {
            var plt = new Plot();
            double[] dataX = { 1, 2, 3, 4, 5 };
            double[] dataY = { 1, 4, 9, 16, 25 };
            var scatter1 = plt.Add.Scatter(dataX, dataY);
            var scatter2 = plt.Add.Scatter(dataX, dataY);
            var scatter3 = plt.Add.Scatter(dataX, dataY);
            scatter1.LegendText = "Data Series 1 asdfawef aegaesgaergt";
            scatter2.LegendText = "Data Series 2";
            scatter3.LegendText = "Data Series 3";

            plt.Legend.Title = "Legend Title";
            plt.ShowLegend();

            var svg = plt.SaveSvg("legend_title_test.svg", 500, 500);
            svg.LaunchFile();
        }
    }
}
