/* Code here extends Plot module with methods to construct plottables.
 *   - Plottables created here are added to the plottables list and returned.
 *   - Long lists of optional arguments (matplotlib style) are permitted.
 *   - Use one line per argument to simplify the tracking of changes.
 */
using System;
using System.Drawing;

namespace ScottPlot
{
    public partial class Plot
    {
        public PlottableSignalXY PlotSignalXY(
            double[] xs,
            double[] ys,
            Color? color = null,
            double lineWidth = 1,
            double markerSize = 5,
            string label = null,
            int? minRenderIndex = null,
            int? maxRenderIndex = null,
            LineStyle lineStyle = LineStyle.Solid,
            bool useParallel = true
            )
        {
            PlottableSignalXY signal = new PlottableSignalXY()
            {
                xs = xs,
                ys = ys,
                color = color ?? settings.GetNextColor(),
                lineWidth = lineWidth,
                markerSize = (float)markerSize,
                label = label,
                minRenderIndex = minRenderIndex ?? 0,
                maxRenderIndex = maxRenderIndex ?? ys.Length - 1,
                lineStyle = lineStyle,
                useParallel = useParallel
            };

            Add(signal);
            return signal;
        }

        public PlottableSignalXYConst<TX, TY> PlotSignalXYConst<TX, TY>(
            TX[] xs,
            TY[] ys,
            Color? color = null,
            double lineWidth = 1,
            double markerSize = 5,
            string label = null,
            int? minRenderIndex = null,
            int? maxRenderIndex = null,
            LineStyle lineStyle = LineStyle.Solid,
            bool useParallel = true
            ) where TX : struct, IComparable where TY : struct, IComparable

        {
            PlottableSignalXYConst<TX, TY> signal = new PlottableSignalXYConst<TX, TY>()
            {
                xs = xs,
                ys = ys,
                color = color ?? settings.GetNextColor(),
                lineWidth = lineWidth,
                markerSize = (float)markerSize,
                label = label,
                minRenderIndex = minRenderIndex ?? 0,
                maxRenderIndex = maxRenderIndex ?? ys.Length - 1,
                lineStyle = lineStyle,
                useParallel = useParallel
            };

            Add(signal);
            return signal;
        }

        public PlottableSignal PlotSignal(
            double[] ys,
            double sampleRate = 1,
            double xOffset = 0,
            double yOffset = 0,
            Color? color = null,
            double lineWidth = 1,
            double markerSize = 5,
            string label = null,
            Color[] colorByDensity = null,
            int? minRenderIndex = null,
            int? maxRenderIndex = null,
            LineStyle lineStyle = LineStyle.Solid,
            bool useParallel = true
            )
        {
            PlottableSignal signal = new PlottableSignal()
            {
                ys = ys,
                sampleRate = sampleRate,
                xOffset = xOffset,
                yOffset = yOffset,
                color = color ?? settings.GetNextColor(),
                lineWidth = lineWidth,
                markerSize = (float)markerSize,
                label = label,
                colorByDensity = colorByDensity,
                minRenderIndex = minRenderIndex ?? 0,
                maxRenderIndex = maxRenderIndex ?? ys.Length - 1,
                lineStyle = lineStyle,
                useParallel = useParallel,
            };

            Add(signal);
            return signal;
        }

        public PlottableSignalConst<T> PlotSignalConst<T>(
            T[] ys,
            double sampleRate = 1,
            double xOffset = 0,
            double yOffset = 0,
            Color? color = null,
            double lineWidth = 1,
            double markerSize = 5,
            string label = null,
            Color[] colorByDensity = null,
            int? minRenderIndex = null,
            int? maxRenderIndex = null,
            LineStyle lineStyle = LineStyle.Solid,
            bool useParallel = true
            ) where T : struct, IComparable
        {
            PlottableSignalConst<T> signal = new PlottableSignalConst<T>()
            {
                ys = ys,
                sampleRate = sampleRate,
                xOffset = xOffset,
                yOffset = yOffset,
                color = color ?? settings.GetNextColor(),
                lineWidth = lineWidth,
                markerSize = (float)markerSize,
                label = label,
                colorByDensity = colorByDensity,
                minRenderIndex = minRenderIndex ?? 0,
                maxRenderIndex = maxRenderIndex ?? ys.Length - 1,
                lineStyle = lineStyle,
                useParallel = useParallel
            };

            Add(signal);
            return signal;
        }
    }
}
