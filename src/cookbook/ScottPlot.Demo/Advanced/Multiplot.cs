using System;
using System.Collections.Generic;
using System.Text;

namespace ScottPlot.Demo.Advanced
{
    class Multiplot
    {
        public class Quickstart : PlotDemo, IPlotDemo, IBitmapDemo
        {
            public string name { get; } = "Multiplot Quickstart";
            public string description { get; } = "Multiplots are single images which contain multiple subplots.";

            public void Render(Plot plt)
            {
                throw new InvalidOperationException("use Render(int, int) for IBitmapDemo objects");
            }

            public System.Drawing.Bitmap Render(int width, int height)
            {
                Random rand = new Random(0);

                var mp = new MultiPlot(width: width, height: height, rows: 2, cols: 2);
                mp.GetSubplot(0, 0).PlotSignal(DataGen.Sin(50));
                mp.GetSubplot(0, 1).PlotSignal(DataGen.Cos(50));
                mp.GetSubplot(1, 0).PlotSignal(DataGen.Random(rand, 50));
                mp.GetSubplot(1, 1).PlotSignal(DataGen.RandomWalk(rand, 50));

                return mp.GetBitmap();
            }
        }

        public class MatchAxis : PlotDemo, IPlotDemo, IBitmapDemo
        {
            public string name { get; } = "Match Subplot Axis";
            public string description { get; } = "Axis and layout information from one subplot can be applied to another subplot.";

            public void Render(Plot plt)
            {
                throw new InvalidOperationException("use Render(int, int) for IBitmapDemo objects");
            }

            public System.Drawing.Bitmap Render(int width, int height)
            {
                Random rand = new Random(0);

                var mp = new MultiPlot(width: width, height: height, rows: 2, cols: 2);
                mp.GetSubplot(0, 0).PlotSignal(DataGen.Sin(50));
                mp.GetSubplot(0, 1).PlotSignal(DataGen.Cos(50));
                mp.GetSubplot(1, 0).PlotSignal(DataGen.Random(rand, 50));
                mp.GetSubplot(1, 1).PlotSignal(DataGen.RandomWalk(rand, 50));

                // adjust the bottom left plot to match the bottom right plot
                var plotToAdjust = mp.GetSubplot(1, 0);
                var plotReference = mp.GetSubplot(1, 1);
                plotToAdjust.MatchAxis(plotReference);
                plotToAdjust.MatchLayout(plotReference);

                return mp.GetBitmap();
            }
        }
    }
}
