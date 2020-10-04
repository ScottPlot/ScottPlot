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

        public PlottableOHLC PlotOHLC(
            OHLC[] ohlcs,
            Color? colorUp = null,
            Color? colorDown = null,
            bool autoWidth = true,
            bool sequential = false
            )
        {
            if (colorUp is null)
                colorUp = ColorTranslator.FromHtml("#26a69a");
            if (colorDown is null)
                colorDown = ColorTranslator.FromHtml("#ef5350");

            PlottableOHLC ohlc = new PlottableOHLC(ohlcs, false, autoWidth, colorUp.Value, colorDown.Value, sequential);
            Add(ohlc);
            return ohlc;
        }

        public PlottableOHLC PlotCandlestick(
            OHLC[] ohlcs,
            Color? colorUp = null,
            Color? colorDown = null,
            bool autoWidth = true,
            bool sequential = false
            )
        {
            if (colorUp is null)
                colorUp = ColorTranslator.FromHtml("#26a69a");
            if (colorDown is null)
                colorDown = ColorTranslator.FromHtml("#ef5350");

            PlottableOHLC ohlc = new PlottableOHLC(ohlcs, true, autoWidth, colorUp.Value, colorDown.Value, sequential);
            Add(ohlc);
            return ohlc;
        }
    }
}
