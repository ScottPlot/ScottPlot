using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using ScottPlot;
using FluentAssertions;

namespace ScottPlotTests.PlottableRenderTests
{
    class Heatmap
    {
        [Test]
        public void Test_Heatmap_Interpolation()
        {
            // see discussion in https://github.com/ScottPlot/ScottPlot/issues/1003

            // Interpolation doesn't seem to work on Linux or MacOS in Azure Pipelines
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows) == false)
                return;

            Random rand = new(0);
            double[,] data = ScottPlot.DataGen.Random2D(rand, 4, 5);

            var plt = new ScottPlot.Plot(300, 300);
            var hm = plt.AddHeatmap(data, lockScales: false);
            plt.SetAxisLimits(0, data.GetLength(1), 0, data.GetLength(0));

            TestTools.SaveFig(plt, "default");
            string hash1 = ScottPlot.Tools.BitmapHash(plt.Render());

            hm.Smooth = true;

            TestTools.SaveFig(plt, "smooth");
            string hash2 = ScottPlot.Tools.BitmapHash(plt.Render());

            Assert.AreNotEqual(hash1, hash2);
        }

        [Test]
        public void Test_Heatmap_AutoScaling()
        {
            // https://github.com/ScottPlot/ScottPlot/issues/1485

            double[,] intensities = new double[100, 100];
            for (int i = 0; i < 100; i++)
                for (int j = 0; j < 100; j++)
                    intensities[i, j] = (Math.Sin(i * .2) + Math.Cos(j * .2)) * 100;

            var plt = new ScottPlot.Plot(500, 400);

            var hmap = plt.AddHeatmap(intensities, ScottPlot.Drawing.Colormap.Viridis, lockScales: false);
            hmap.Interpolation = InterpolationMode.Bicubic;

            var cbar = plt.AddColorbar(hmap);
            double[] tickPositions = ScottPlot.DataGen.Range(-150, 150, 50, true);
            string[] tickLabels = tickPositions.Select(x => x.ToString()).ToArray();
            cbar.SetTicks(tickPositions, tickLabels, -200, 200);

            plt.Margins(0, 0);
            TestTools.SaveFig(plt);
        }

        [Test]
        public void Test_Heatmap_ManualScaling()
        {
            // The goal is to span the whole colormap only over values 0-200
            // even though the original data has many values outside this range.
            // https://github.com/ScottPlot/ScottPlot/issues/1485

            double[,] intensities = new double[100, 100];
            for (int i = 0; i < 100; i++)
                for (int j = 0; j < 100; j++)
                    intensities[i, j] = (Math.Sin(i * .2) + Math.Cos(j * .2)) * 100;

            var plt = new ScottPlot.Plot(500, 400);

            var cmap = ScottPlot.Drawing.Colormap.Viridis;

            var hmap = plt.AddHeatmap(intensities, cmap, lockScales: false);
            hmap.Interpolation = InterpolationMode.Bicubic;
            hmap.ScaleMin = 0;
            hmap.Update(intensities, cmap, min: 0, max: 200); // intentionally cut-off data
            Console.WriteLine(hmap.ScaleMin);

            double[] tickPositions = ScottPlot.DataGen.Range(0, 200, 25, true);
            string[] tickLabels = tickPositions.Select(x => x.ToString()).ToArray();

            var cbar = plt.AddColorbar(hmap);
            cbar.SetTicks(tickPositions, tickLabels, 0, 200);

            plt.Margins(0, 0);
            TestTools.SaveFig(plt);
        }

        [Test]
        public void Test_Heatmap_GetBitmap()
        {
            double[,] data = DataGen.SampleImageData();

            ScottPlot.Plot plt = new(500, 400);
            var hm = plt.AddHeatmap(data);

            System.Drawing.Bitmap bmp = hm.GetBitmap();
            bmp.Should().NotBeNull();
            bmp.Width.Should().Be(65);
            bmp.Height.Should().Be(100);

            TestTools.SaveFig(bmp);
        }

        [Test]
        public void Test_Heatmap_PaletteColormap()
        {
            double[,] data = DataGen.SampleImageData();

            ScottPlot.Plot plt = new(500, 400);

            // create a colormap from a defined set of colors
            System.Drawing.Color[] colors = {
                System.Drawing.Color.Indigo,
                System.Drawing.Color.Blue,
                System.Drawing.Color.Green,
                System.Drawing.Color.Yellow,
                System.Drawing.Color.Orange,
                System.Drawing.Color.Red,
            };

            // display the colormap on the plot as a colorbar
            ScottPlot.Drawing.Colormap cmap = new(colors);
            var cbar = plt.AddColorbar(cmap);
            cbar.MaxValue = 255;

            // use custom tick positions
            double[] tickPositions = Enumerable.Range(0, colors.Length + 1)
                .Select(x => (double)x / colors.Length)
                .ToArray();
            string[] tickLabels = tickPositions.Select(x => $"{x * 255:N2}").ToArray();
            cbar.SetTicks(tickPositions, tickLabels);

            // add a heatmap using the custom colormap
            plt.AddHeatmap(data, cmap);

            TestTools.SaveFig(plt);
        }
    }
}
