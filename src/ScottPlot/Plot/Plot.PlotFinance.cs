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
            PlottableOHLC ohlc = new PlottableOHLC()
            {
                ohlcs = ohlcs,
                Candle = false,
                AutoWidth = autoWidth,
                Sqeuential = sequential,
                ColorUp = colorUp ?? ColorTranslator.FromHtml("#26a69a"),
                ColorDown = colorDown ?? ColorTranslator.FromHtml("#ef5350")
            };
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
            PlottableOHLC ohlc = new PlottableOHLC()
            {
                ohlcs = ohlcs,
                Candle = true,
                AutoWidth = autoWidth,
                Sqeuential = sequential,
                ColorUp = colorUp ?? ColorTranslator.FromHtml("#26a69a"),
                ColorDown = colorDown ?? ColorTranslator.FromHtml("#ef5350")
            };
            Add(ohlc);
            return ohlc;
        }
    }
}
