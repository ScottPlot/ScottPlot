using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace ScottPlotTests
{
    class Grid
    {
        [Test]
        public void Test_Grid_VerticalOnly()
        {
            int pointCount = 50;
            double[] dataXs = ScottPlot.DataGen.Consecutive(pointCount);
            double[] dataSin = ScottPlot.DataGen.Sin(pointCount);
            double[] dataCos = ScottPlot.DataGen.Cos(pointCount);

            var plt = new ScottPlot.Plot();
            plt.AddScatter(dataXs, dataSin);
            plt.AddScatter(dataXs, dataCos);
            plt.YAxis.Grid(false);

            string name = System.Reflection.MethodBase.GetCurrentMethod().Name;
            string filePath = System.IO.Path.GetFullPath(name + ".png");
            plt.SaveFig(filePath);
            Console.WriteLine($"Saved {filePath}");
        }

        [Test]
        public void Test_Grid_HorizontalOnly()
        {
            int pointCount = 50;
            double[] dataXs = ScottPlot.DataGen.Consecutive(pointCount);
            double[] dataSin = ScottPlot.DataGen.Sin(pointCount);
            double[] dataCos = ScottPlot.DataGen.Cos(pointCount);

            var plt = new ScottPlot.Plot();
            plt.AddScatter(dataXs, dataSin);
            plt.AddScatter(dataXs, dataCos);
            plt.XAxis.Grid(false);

            string name = System.Reflection.MethodBase.GetCurrentMethod().Name;
            string filePath = System.IO.Path.GetFullPath(name + ".png");
            plt.SaveFig(filePath);
            Console.WriteLine($"Saved {filePath}");
        }
    }
}
