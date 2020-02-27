using System;
using System.Collections.Generic;
using System.Text;

namespace ScottPlot.Demo.PlotTypes
{
    public class BoxAndWhisker
    {
        public class Quickstart : IPlotDemo
        {

            public string name { get; }
            public string description { get; }

            public void Render(Plot plt)
            {
                Random rand = new Random(0);
                var baw1 = new Statistics.BoxAndWhisker(0, DataGen.Random(rand, 20));
                var baw2 = new Statistics.BoxAndWhisker(1, DataGen.Random(rand, 20));
                var baw3 = new Statistics.BoxAndWhisker(2, DataGen.Random(rand, 20));
                var baws = new Statistics.BoxAndWhisker[] { baw1 , baw2, baw3 };
                plt.PlotBoxAndWhisker(baws);
            }
        }

        public class Advanced : IPlotDemo
        {

            public string name { get; }
            public string description { get; }

            public void Render(Plot plt)
            {
                var boxAndWiskers = new Statistics.BoxAndWhisker[3];
                boxAndWiskers[0] = Statistics.BoxAndWhiskerFromData.OutlierQuartileMedian(Data.LineLengths.plot, 1);
                boxAndWiskers[1] = Statistics.BoxAndWhiskerFromData.OutlierQuartileMedian(Data.LineLengths.formsPlot, 2);
                boxAndWiskers[2] = Statistics.BoxAndWhiskerFromData.OutlierQuartileMedian(Data.LineLengths.wpfPlot, 3);

                plt.Title("Source Code Line Length");
                plt.PlotBoxAndWhisker(boxAndWiskers);
                plt.YLabel("Number of Characters");

                double[] xPositions = { 1, 2, 3 };
                string[] labels = { "Plot.cs", "FormsPlot.cs", "WpfPlot.cs" };
                plt.XTicks(xPositions, labels);

                plt.AxisAuto(.3, .2);
            }
        }
    }
}
