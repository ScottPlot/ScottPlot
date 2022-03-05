using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScottPlotTests.Plot
{
    public class PlotDimensionsTests
    {
        private (double xCoord, double yCoord, float xPixel, float yPixel)[] GetKnownPoints()
        {
            /*
             * This table contains known coordinate/pixel translations for a plot with:
             *   - 600x400 figure
             *   - 500x300 data area
             *   - 50,50 data offset
             *   - X=(-10.0 to 10.0), Y=(-1.0 to 1.0) axis limits
             */

            List<(double, double, float, float)> points = new();

            points.Add((4.52487, 0.63465, 413.12177f, 104.8025f));
            points.Add((5.36045, 0.11632, 434.01126f, 182.552f));
            points.Add((-5.87934, 0.11777, 153.0165f, 182.3345f));
            points.Add((8.12054, -0.11564, 503.0135f, 217.346f));
            points.Add((9.551, -0.45259, 538.775f, 267.8885f));
            points.Add((-4.16187, -0.06537, 195.95325f, 209.8055f));
            points.Add((2.65318, -0.06098, 366.3295f, 209.147f));
            points.Add((9.64303, -0.93927, 541.07574f, 340.8905f));
            points.Add((7.2474, 0.99069, 481.185f, 51.3965f));
            points.Add((3.54362, -0.37082, 388.5905f, 255.623f));
            points.Add((6.33816, 0.6961, 458.454f, 95.585f));
            points.Add((9.83804, -0.93475, 545.951f, 340.2125f));
            points.Add((3.99884, 0.05257, 399.971f, 192.1145f));
            points.Add((8.68037, 0.37524, 517.0093f, 143.714f));
            points.Add((0.93631, -0.83778, 323.40775f, 325.667f));
            points.Add((-6.25751, -0.09335, 143.56226f, 214.0025f));
            points.Add((-4.05656, 0.97709, 198.586f, 53.4365f));
            points.Add((2.85395, 0.52593, 371.34875f, 121.1105f));
            points.Add((-9.39211, -0.23799, 65.19725f, 235.6985f));
            points.Add((-3.13716, 0.91491, 221.571f, 62.7635f));
            points.Add((0.10258, 0.43195, 302.5645f, 135.2075f));
            points.Add((-7.62085, -0.4531, 109.47875f, 267.965f));
            points.Add((8.14196, 0.58953, 503.549f, 111.5705f));
            points.Add((-3.25679, -0.08558, 218.58025f, 212.837f));
            points.Add((-7.0635, -0.55737, 123.4125f, 283.6055f));

            return points.ToArray();
        }

        private ScottPlot.PlotDimensions GetTestDimensions()
        {
            System.Drawing.SizeF figSize = new(600, 400);
            System.Drawing.SizeF dataSize = new(500, 300);
            System.Drawing.PointF dataLocation = new(50, 50);
            ScottPlot.AxisLimits limits = new(-10.0, 10.0, -1.0, 1.0);
            return new(figSize, dataSize, dataLocation, limits, scaleFactor: 1);
        }

        [Test]
        public void Test_PlotDimensions_CreateKnownTranslations()
        {
            ScottPlot.PlotDimensions dims = GetTestDimensions();

            Random rand = new(0);
            for (int i = 0; i < 25; i++)
            {
                double x = Math.Round(dims.XMin + rand.NextDouble() * dims.XSpan, 5);
                double y = Math.Round(dims.YMin + rand.NextDouble() * dims.YSpan, 5);

                ScottPlot.Coordinate coord = new(x, y);
                ScottPlot.Pixel pixel = dims.GetPixel(coord);
                Console.WriteLine($"points.Add(({coord.X}, {coord.Y}, {pixel.X}f, {pixel.Y}f));");

                // ensure forward and backward conversions work as expected
                Assert.AreEqual(coord.X, dims.GetCoordinate(pixel).X, delta: 1e-5);
                Assert.AreEqual(coord.Y, dims.GetCoordinate(pixel).Y, delta: 1e-5);
                Assert.AreEqual(pixel.X, dims.GetPixel(coord).X, delta: 1e-5);
                Assert.AreEqual(pixel.Y, dims.GetPixel(coord).Y, delta: 1e-5);
            }
        }

        [Test]
        public void Test_PlotDimensions_GetPixel()
        {
            ScottPlot.PlotDimensions dims = GetTestDimensions();

            foreach (var pt in GetKnownPoints())
            {
                ScottPlot.Pixel knownPixel = new(pt.xPixel, pt.yPixel);
                ScottPlot.Coordinate knownCoord = new(pt.xCoord, pt.yCoord);

                Assert.AreEqual(knownPixel.X, dims.GetPixel(knownCoord).X, 1e-5);
                Assert.AreEqual(knownPixel.Y, dims.GetPixel(knownCoord).Y, 1e-5);
            }
        }

        [Test]
        public void Test_PlotDimensions_GetCoordinate()
        {
            ScottPlot.PlotDimensions dims = GetTestDimensions();

            foreach (var pt in GetKnownPoints())
            {
                ScottPlot.Pixel knownPixel = new(pt.xPixel, pt.yPixel);
                ScottPlot.Coordinate knownCoord = new(pt.xCoord, pt.yCoord);

                Assert.AreEqual(knownCoord.X, dims.GetCoordinate(knownPixel).X, 1e-5);
                Assert.AreEqual(knownCoord.Y, dims.GetCoordinate(knownPixel).Y, 1e-5);
            }
        }
    }
}
