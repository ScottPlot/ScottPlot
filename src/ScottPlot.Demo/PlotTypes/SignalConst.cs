using System;
using System.Collections.Generic;
using System.Text;

namespace ScottPlot.Demo.PlotTypes
{
    class SignalConst
    {
        public class RandomWalk_5millionPoints_SignalConst : PlotDemo, IPlotDemo
        {
            public string name { get; } = "5M points (SignalConst)";
            public string description { get; } = "SignalConst plots pre-processes data to render much faster than Signal plots. Pre-processing takes a little time up-front and requires 4x the memory of Signal.";

            public void Render(Plot plt)
            {
                Random rand = new Random(0);
                int pointCount = 1_000_000;
                int lineCount = 5;

                for (int i = 0; i < lineCount; i++)
                    plt.PlotSignalConst(DataGen.RandomWalk(rand, pointCount));
            }
        }
    }
}
