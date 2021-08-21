using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace ScottPlotTests.PlotTypes
{
    class Pie
    {
        [Test]
        public void Test_Pie_Tall()
        {
            double[] values = { 778, 43, 283, 76, 184 };
            string[] labels = { "C#", "JAVA", "Python", "F#", "PHP" };

            var plt = new ScottPlot.Plot(300, 800);

            var pie = plt.AddPie(values);
            pie.SliceLabels = labels;
            pie.ShowLabels = false;
            plt.Legend();

            plt.Grid(false);
            plt.Frameless();
            plt.XAxis.Ticks(false);
            plt.YAxis.Ticks(false);

            TestTools.SaveFig(plt);
        }

        [Test]
        public void Test_Pie_Center()
        {
            double[] values = { 778, 43, 283, 76, 184 };
            string[] labels = { "C#", "JAVA", "Python", "F#", "PHP" };

            var plt = new ScottPlot.Plot(800, 300);

            var pie = plt.AddPie(values);
            pie.SliceLabels = labels;
            pie.ShowLabels = false;
            plt.Legend();

            plt.Grid(false);
            plt.Frameless();
            plt.XAxis.Ticks(false);
            plt.YAxis.Ticks(false);

            TestTools.SaveFig(plt);
        }
    }
}
