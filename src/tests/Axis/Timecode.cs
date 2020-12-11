using NUnit.Framework;
using ScottPlot;
using System;
using System.Collections.Generic;
using System.Text;

namespace ScottPlotTests.Axis
{
    class Timecode
    {
        [Test]
        public void Test_tickFormat_timecode()
        {
            var plt = new ScottPlot.Plot(800, 300);

            // simulate 10 seconds of audio data
            int pointsPerSecond = 44100;
            Random rand = new Random(0);
            double[] ys = DataGen.RandomWalk(rand, pointsPerSecond * 10);

            // for DateTime compatibility, sample rate must be points/day
            double pointsPerDay = 24.0 * 60 * 60 * pointsPerSecond;
            plt.AddSignal(ys, sampleRate: pointsPerDay);

            plt.XAxis.TickLabelFormat("HH:mm:ss:fff", true);

            TestTools.SaveFig(plt);
        }
    }
}
