using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/* code in this file relates to adding and storing data to be plotted */

namespace ScottPlot
{
    public class Data
    {

        public List<Plottables.PlottableThing> plotObjects = new List<Plottables.PlottableThing>();

        public Data()
        {
        }

        public double[] GetAxisLimits()
        {
            if (plotObjects.Count() == 0)
                return null;

            double xMin = 0;
            double xMax = 0;
            double yMin = 0;
            double yMax = 0;

            // then expand as needed
            for (int i = 0; i < plotObjects.Count(); i++)
            {
                Plottables.PlottableThing thing = plotObjects[i];
                double[] axisLimits = thing.AxisLimits();

                // skip things without clear axes
                if (axisLimits is null)
                    continue;

                if (xMin == xMax)
                {
                    // if this is the first, use all limits
                    xMin = axisLimits[0];
                    xMax = axisLimits[1];
                    yMin = axisLimits[2];
                    yMax = axisLimits[3];
                }
                else
                {
                    // otherwise use if expanding
                    xMin = Math.Min(xMin, axisLimits[0]);
                    xMax = Math.Max(xMax, axisLimits[1]);
                    yMin = Math.Min(yMin, axisLimits[2]);
                    yMax = Math.Max(yMax, axisLimits[3]);
                }
            }

            return new double[] { xMin, xMax, yMin, yMax };
        }

        public int Count()
        {
            return plotObjects.Count();
        }

        public void Clear()
        {
            plotObjects.Clear();
        }

        public void ClearAxisLines()
        {

            for (int i = 0; i < plotObjects.Count; i++)
            {
                Plottables.PlottableThing plotObject = plotObjects[i];
                if (plotObject is Plottables.AxLine)
                    plotObjects[i] = null;
            }
            plotObjects.RemoveAll(item => item == null);
        }

        ///////////////////////////////////////////////////////////////////////////////////////
        // SCATTER PLOTS

        private double[] AscendingNumbers(int pointCount, double multiplier = 1, double offset = 0)
        {
            double[] xs = new double[pointCount];
            for (int i = 0; i < xs.Length; i++)
                xs[i] = i * multiplier + offset;
            return xs;
        }

        // primary function (accepts a style)
        public void AddScatter(double[] xs, double[] ys, Style style)
        {
            xs = xs ?? AscendingNumbers(ys.Length);
            var scatter = new Plottables.Scatter(xs, ys, style);
            plotObjects.Add(scatter);
        }

        // minimal function (minimal arguments)
        public void AddScatter(double[] xs, double[] ys)
        {
            xs = xs ?? AscendingNumbers(ys.Length);
            AddScatter(xs, ys, new Style(plotObjects.Count()));
        }

        // detailed function (named arguments)
        public void AddScatter(double[] xs, double[] ys,
            int markerSize = 3, Color? markerColor = null, Style.MarkerShape markerShape = Style.MarkerShape.circleFilled,
            int lineWidth = 1, Color? lineColor = null, Style.LineStyle lineStyle = Style.LineStyle.solid)
        {
            xs = xs ?? AscendingNumbers(ys.Length);
            var style = new Style(plotObjects.Count());
            style.markerSize = markerSize;
            style.markerColor = markerColor.GetValueOrDefault(style.markerColor);
            style.markerShape = markerShape;
            style.lineWidth = lineWidth;
            style.lineColor = lineColor.GetValueOrDefault(style.lineColor);
            style.lineStyle = Style.LineStyle.solid;
            AddScatter(xs, ys, style);
        }

        ///////////////////////////////////////////////////////////////////////////////////////
        // POINTS (SINGLE POINT SCATTER PLOTS)

        // primary function (accepts a style)
        private void AddPoint(double x, double y, Style style)
        {
            AddScatter(new double[] { x }, new double[] { y }, style);
        }

        // minimal function (minimal arguments)
        public void AddPoint(double x, double y)
        {
            AddPoint(x, y, new Style(plotObjects.Count()));
        }

        // detailed function (named arguments)
        public void AddPoint(double x, double y,
            int markerSize = 5, Color? markerColor = null, Style.MarkerShape markerShape = Style.MarkerShape.circleFilled)
        {
            var style = new Style(plotObjects.Count());
            style.markerSize = markerSize;
            style.markerShape = markerShape;
            style.markerColor = markerColor.GetValueOrDefault(style.markerColor);
            AddPoint(x, y, style);
        }


        ///////////////////////////////////////////////////////////////////////////////////////
        // SIGNALS

        // primary function (accepts a style)
        private void AddSignal(double[] y, double sampleRateHz, Style style)
        {
            plotObjects.Add(new Plottables.Signal(y, sampleRateHz, style));
        }

        // detailed function (named arguments)
        public void AddSignal(double[] y, double sampleRateHz,
            int lineWidth = 1, Color? lineColor = null, Style.LineStyle lineStyle = Style.LineStyle.solid)
        {
            var style = new Style(plotObjects.Count());
            style.lineWidth = lineWidth;
            style.lineColor = lineColor.GetValueOrDefault(style.lineColor);
            style.lineStyle = lineStyle;
            plotObjects.Add(new Plottables.Signal(y, sampleRateHz, style));
        }


        ///////////////////////////////////////////////////////////////////////////////////////
        // AXIS LINES

        // primary function (accepts a style)
        private void AddAxisLine(double y, bool vertical, Style style)
        {
            plotObjects.Add(new Plottables.AxLine(y, vertical, style));
        }

        // detailed function (named arguments)
        public void AddHorizLine(double y, int lineWidth = 1, Color? lineColor = null, Style.LineStyle lineStyle = Style.LineStyle.solid)
        {
            var style = new Style(plotObjects.Count());
            style.lineWidth = lineWidth;
            style.lineColor = lineColor.GetValueOrDefault(style.lineColor);
            style.lineStyle = lineStyle;
            AddAxisLine(y, false, style);
        }

        // detailed function (named arguments)
        public void AddVertLine(double y, int lineWidth = 1, Color? lineColor = null, Style.LineStyle lineStyle = Style.LineStyle.solid)
        {
            var style = new Style(plotObjects.Count());
            style.lineWidth = lineWidth;
            style.lineColor = lineColor.GetValueOrDefault(style.lineColor);
            style.lineStyle = lineStyle;
            AddAxisLine(y, true, style);
        }

        public string PlotObjectInfo()
        {
            string txt = "";
            for (int i = 0; i < plotObjects.Count; i++)
                txt += $"plot object #{i + 1}: {plotObjects[i].ToString()}\n";
            return txt.Trim();
        }
    }
}
