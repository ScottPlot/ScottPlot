/* Code here extends Plot module with methods to construct plottables.
 *   - Plottables created here are added to the plottables list and returned.
 *   - Long lists of optional arguments (matplotlib style) are permitted.
 *   - Use one line per argument to simplify the tracking of changes.
 */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace ScottPlot
{
    public partial class Plot
    {


        public PlottablePolygon PlotFill(
            double[] xs,
            double[] ys,
            string label = null,
            double lineWidth = 0,
            Color? lineColor = null,
            bool fill = true,
            Color? fillColor = null,
            double fillAlpha = 1,
            double baseline = 0
            )
        {
            if (xs.Length != ys.Length)
                throw new ArgumentException("xs and ys must all have the same length");

            double[] xs2 = Tools.Pad(xs, cloneEdges: true);
            double[] ys2 = Tools.Pad(ys, padWithLeft: baseline, padWithRight: baseline);

            return PlotPolygon(xs2, ys2, label, lineWidth, lineColor, fill, fillColor, fillAlpha);
        }

        public PlottablePolygon PlotFill(
            double[] xs1,
            double[] ys1,
            double[] xs2,
            double[] ys2,
            string label = null,
            double lineWidth = 0,
            Color? lineColor = null,
            bool fill = true,
            Color? fillColor = null,
            double fillAlpha = 1,
            double baseline = 0
            )
        {
            if ((xs1.Length != ys1.Length) || (xs2.Length != ys2.Length))
                throw new ArgumentException("xs and ys for each dataset must have the same length");

            int pointCount = xs1.Length + xs2.Length;
            double[] bothX = new double[pointCount];
            double[] bothY = new double[pointCount];

            // copy the first dataset as-is
            Array.Copy(xs1, 0, bothX, 0, xs1.Length);
            Array.Copy(ys1, 0, bothY, 0, ys1.Length);

            // copy the second dataset in reverse order
            for (int i = 0; i < xs2.Length; i++)
            {
                bothX[xs1.Length + i] = xs2[xs2.Length - 1 - i];
                bothY[ys1.Length + i] = ys2[ys2.Length - 1 - i];
            }

            return PlotPolygon(bothX, bothY, label, lineWidth, lineColor, fill, fillColor, fillAlpha);
        }

        public (PlottablePolygon, PlottablePolygon) PlotFillAboveBelow(
            double[] xs,
            double[] ys,
            string labelAbove = null,
            string labelBelow = null,
            double lineWidth = 1,
            Color? lineColor = null,
            bool fill = true,
            Color? fillColorAbove = null,
            Color? fillColorBelow = null,
            double fillAlpha = 1,
            double baseline = 0
            )
        {
            if (xs.Length != ys.Length)
                throw new ArgumentException("xs and ys must all have the same length");

            var (xs2, ysAbove, ysBelow) = Drawing.Tools.PolyAboveAndBelow(xs, ys, baseline);
            var polyAbove = PlotPolygon(xs2, ysAbove, labelAbove, lineWidth, lineColor, fill, fillColorAbove ?? Color.Green, fillAlpha);
            var polyBelow = PlotPolygon(xs2, ysBelow, labelBelow, lineWidth, lineColor, fill, fillColorBelow ?? Color.Red, fillAlpha);

            return (polyBelow, polyAbove);
        }

        public PlottablePolygon PlotPolygon(
            double[] xs,
            double[] ys,
            string label = null,
            double lineWidth = 0,
            Color? lineColor = null,
            bool fill = true,
            Color? fillColor = null,
            double fillAlpha = 1
            )
        {
            var plottable = new PlottablePolygon(xs, ys)
            {
                label = label,
                lineWidth = lineWidth,
                lineColor = lineColor ?? Color.Black,
                fill = fill,
                fillColor = fillColor ?? settings.GetNextColor(),
                fillAlpha = fillAlpha
            };

            Add(plottable);
            return plottable;
        }

        public PlottablePolygons PlotPolygons(
            List<List<(double x, double y)>> polys,
            string label = null,
            double lineWidth = 0,
            Color? lineColor = null,
            bool fill = true,
            Color? fillColor = null,
            double fillAlpha = 1
            )
        {
            var plottable = new PlottablePolygons(polys)
            {
                label = label,
                lineWidth = lineWidth,
                lineColor = lineColor ?? Color.Black,
                fill = fill,
                fillColor = fillColor ?? settings.GetNextColor(),
                fillAlpha = fillAlpha
            };

            Add(plottable);
            return plottable;
        }
    }
}
