using ScottPlot.Config;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Text;

namespace ScottPlot
{
    public class PlottableFunction : Plottable
    {
        private readonly Func<double, double?> function;
        private readonly double minX;
        private readonly double maxX;
        private readonly double minY;
        private readonly double maxY;
        private readonly double lineWidth;
        private readonly double markerSize;

        public PlottableFunction(Func<double, double?> function, double minX, double maxX, double minY, double maxY, Color color, double lineWidth, double markerSize, string label, MarkerShape markerShape, LineStyle lineStyle)
        {
            this.function = function;
            this.minX = minX;
            this.maxX = maxX;
            this.minY = minY;
            this.maxY = maxY;
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
            double step = settings.xAxisUnitsPerPixel;
            double minRenderedX = minX > settings.axes.limits[0] ? minX : settings.axes.limits[0];
            double maxRenderedX = maxX < settings.axes.limits[1] ? maxX : settings.axes.limits[1];

            int maxSeriesLength = (int)Math.Ceiling((maxRenderedX - minRenderedX) / step);

            List<double> xList = new List<double>();
            List<double> yList = new List<double>();

            for (int i = 0; i < maxSeriesLength; i++)
            {
                double x = i * step + minRenderedX;
                double? y;
                try
                {
                    y = function(x);
                }
                catch (Exception e) //Domain error, such log(-1) or 1/0
                {
                    Debug.WriteLine(e);
                    continue;
                }

                if (y.HasValue)
                {
                    xList.Add(x);
                    yList.Add(y.Value);
                }


                //Console.WriteLine($"({xs[i]},{ys[i]})");
            }


            PlottableScatter scatter = new PlottableScatter(xList.ToArray(), yList.ToArray(), color, lineWidth, markerSize, label, null, null, 0, 0, false, markerShape, lineStyle);
            scatter.Render(settings);
        }

        public override string ToString()
        {
            throw new NotImplementedException();
        }
    }
}
