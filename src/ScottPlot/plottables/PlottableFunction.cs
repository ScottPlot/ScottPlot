using ScottPlot.Config;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace ScottPlot
{
    public class PlottableFunction : Plottable
    {
        private readonly Func<double, double> function;
        private readonly double minX;
        private readonly double maxX;
        private readonly double minY;
        private readonly double maxY;
        private readonly Color color;
        private readonly double lineWidth;
        private readonly double markerSize;
        private readonly string label;
        private readonly MarkerShape markerShape;
        private readonly LineStyle lineStyle;


        public PlottableFunction(Func<double, double> function, double minX, double maxX, double minY, double maxY, Color color, double lineWidth, double markerSize, string label, MarkerShape markerShape, LineStyle lineStyle)
        {
            this.function = function;
            this.minX = minX;
            this.maxX = maxX;
            this.minY = minY;
            this.maxY = maxX;
            this.color = color;
            this.lineWidth = lineWidth;
            this.markerSize = markerSize;
            this.label = label;
            this.markerShape = markerShape;
            this.lineStyle = lineStyle;
        }

        public override AxisLimits2D GetLimits()
        {
            double[] limits = { minX, maxX, minY, maxY };

            return new Config.AxisLimits2D(limits);
        }


        public override void Render(Settings settings)
        {
            double step = 1 / (settings.xAxisUnitsPerPixel * 250); // 250 may not be ideal, bigger number is more smooth, but less performant
            int seriesLength = (int)Math.Ceiling((maxX - minX) / step);

            double[] xs = new double[seriesLength];
            double[] ys = new double[seriesLength];

            for (int i = 0; i < seriesLength; i++)
            {
                xs[i] = i * step + minX;
                ys[i] = function(xs[i]);

                //Console.WriteLine($"({xs[i]},{ys[i]})");
            }


            PlottableScatter scatter = new PlottableScatter(xs, ys, color, lineWidth, markerSize, label, null, null, 0, 0, false, markerShape, lineStyle);
            scatter.Render(settings);
        }

        public override string ToString()
        {
            throw new NotImplementedException();
        }
    }
}
