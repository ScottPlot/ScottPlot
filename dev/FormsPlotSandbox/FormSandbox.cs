using ScottPlot;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FormsPlotSandbox
{
    public partial class FormSandbox : Form
    {
        public FormSandbox()
        {
            InitializeComponent();
        }

        OHLC[] ohlcs = DataGen.RandomStockPrices(new Random(0), 200);
        private void FormSandbox_Load(object sender, EventArgs e)
        {
            Random rand = new Random();
            formsPlot1.plt.PlotCandlestick(ohlcs);
            formsPlot1.Render();
        }

        private void formsPlot1_MouseUp(object sender, MouseEventArgs e)
        {
            // determine the axis where we are now
            double[] axes = formsPlot1.plt.Axis();
            double xLow = axes[0];
            double xHigh = axes[1];

            // determine what the high and low of the visible axis range is
            double yLow = double.PositiveInfinity;
            double yHigh = double.NegativeInfinity;
            for (int i = 0; i < ohlcs.Length; i++)
            {
                var ohlc = ohlcs[i];
                if (ohlc.time < xLow || ohlc.time > xHigh)
                    continue;
                yLow = Math.Min(yLow, ohlc.low);
                yHigh = Math.Max(yHigh, ohlc.high);
            }

            // set the Y axis limits to the high and low of the range
            formsPlot1.plt.Axis(xLow, xHigh, yLow, yHigh);
        }
    }
}
