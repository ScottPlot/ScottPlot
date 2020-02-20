using NUnit.Framework;
using ScottPlot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScottPlotTests
{
    public class Legend
    {
        [Test]
        public void Test_Legend_LooksGoodInEveryPosition()
        {

            var mplt = new MultiPlot(1000, 800, 3, 3);

            legendLocation[] locs = Enum.GetValues(typeof(legendLocation)).Cast<legendLocation>().ToArray();
            for (int i = 0; i < locs.Length; i++)
            {
                var plt = mplt.subplots[i];
                plt.PlotScatter(DataGen.Consecutive(20), DataGen.Sin(20), markerShape: MarkerShape.filledSquare, label: "sin");
                plt.PlotScatter(DataGen.Consecutive(20), DataGen.Cos(20), markerShape: MarkerShape.openDiamond, label: "cos");
                plt.Legend(location: locs[i]);
                plt.Title(locs[i].ToString());
            }

            TestTools.SaveFig(mplt);
        }
    }
}
