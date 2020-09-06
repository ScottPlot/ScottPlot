﻿using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace ScottPlotTests.PlotTypes
{
    public class Polygon
    {
        [Test]
        public void Test_ManualPolygon_Render()
        {
            Random rand = new Random(0);

            var plt = new ScottPlot.Plot();

            for (int i = 0; i < 5; i++)
            {
                var color = plt.GetSettings(false).GetNextColor();

                var plottable = new ScottPlot.PlottablePolygon(
                        xs: ScottPlot.DataGen.Random(rand, 3, 100),
                        ys: ScottPlot.DataGen.Random(rand, 3, 100),
                        label: $"polygon {i + 1}",
                        lineWidth: 2,
                        lineColor: color,
                        fill: true,
                        fillColor: color,
                        fillAlpha: .5
                    );

                plt.Add(plottable);
            }

            plt.Title("Polygon Example");
            plt.Legend(location: ScottPlot.legendLocation.lowerLeft);
            TestTools.SaveFig(plt);
        }

        [Test]
        public void Test_PlotPolygon_Render()
        {
            Random rand = new Random(0);

            var plt = new ScottPlot.Plot();

            for (int i = 0; i < 5; i++)
            {
                double[] xs = ScottPlot.DataGen.Random(rand, 3, 100);
                double[] ys = ScottPlot.DataGen.Random(rand, 3, 100);
                plt.PlotPolygon(xs, ys, $"polygon {i + 1}",
                    lineWidth: 2, lineColor: Color.Black, fillAlpha: .5);
            }

            plt.Title("Polygon Example");
            plt.Legend(location: ScottPlot.legendLocation.lowerLeft);
            TestTools.SaveFig(plt);
        }

        [Test]
        public void Test_PlotPolygon_SimulateShadedCurve()
        {
            // generate sample data
            Random rand = new Random(0);
            var dataY = ScottPlot.DataGen.RandomWalk(rand, 1000, offset: -10);
            var dataX = ScottPlot.DataGen.Consecutive(dataY.Length, spacing: 0.025);

            // create an array with an extra point on each side of the data
            double baseline = 0;
            var arrX = new double[dataX.Length + 2];
            var arrY = new double[dataY.Length + 2];
            Array.Copy(dataX, 0, arrX, 1, dataX.Length);
            Array.Copy(dataY, 0, arrY, 1, dataY.Length);
            arrX[0] = dataX[0];
            arrX[arrX.Length - 1] = dataX[dataX.Length - 1];
            arrY[0] = baseline;
            arrY[arrY.Length - 1] = baseline;

            var plt = new ScottPlot.Plot();
            plt.PlotPolygon(arrX, arrY, lineWidth: 1, lineColor: Color.Black, fillAlpha: .5);

            plt.Title("Shaded Line Plot");
            plt.Legend(location: ScottPlot.legendLocation.lowerLeft);
            TestTools.SaveFig(plt);
        }

        [Test]
        public void Test_PlotPolygon_PosAndNeg()
        {
            // generate sample data
            Random rand = new Random(0);
            var dataY = ScottPlot.DataGen.RandomWalk(rand, 1000, offset: -10);
            var dataX = ScottPlot.DataGen.Consecutive(dataY.Length, spacing: 0.025);

            // create an array with an extra point on each side of the data
            double baseline = 0;
            var xs = new double[dataX.Length + 2];
            var ys = new double[dataY.Length + 2];
            Array.Copy(dataX, 0, xs, 1, dataX.Length);
            Array.Copy(dataY, 0, ys, 1, dataY.Length);
            xs[0] = dataX[0];
            xs[xs.Length - 1] = dataX[dataX.Length - 1];
            ys[0] = baseline;
            ys[ys.Length - 1] = baseline;

            // separate the data into two arrays (for positive and negative)
            double[] neg = new double[ys.Length];
            double[] pos = new double[ys.Length];
            for (int i = 0; i < ys.Length; i++)
            {
                if (ys[i] < 0)
                    neg[i] = ys[i];
                else
                    pos[i] = ys[i];
            }

            // now plot the arrays as polygons
            var plt = new ScottPlot.Plot();
            plt.PlotPolygon(xs, neg, "negative", lineWidth: 1,
                lineColor: Color.Black, fillColor: Color.Red, fillAlpha: .5);
            plt.PlotPolygon(xs, pos, "positive", lineWidth: 1,
                lineColor: Color.Black, fillColor: Color.Green, fillAlpha: .5);
            plt.Title("Shaded Line Plot (negative vs. positive)");
            plt.AxisAuto(0);
            TestTools.SaveFig(plt);
        }
    }
}
