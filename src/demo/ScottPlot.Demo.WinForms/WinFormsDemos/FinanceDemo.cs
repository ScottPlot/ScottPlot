using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ScottPlot.Demo.WinForms.WinFormsDemos
{
    public partial class FinanceDemo : Form
    {
        public FinanceDemo()
        {
            InitializeComponent();
        }

        private void FinanceDemo_Load(object sender, EventArgs e)
        {
            Random rand = new Random(0);
            OHLC[] prices = ScottPlot.DataGen.RandomStockPrices(rand, 100);
            double[] dates = prices.Select(x => x.DateTime.ToOADate()).ToArray();
            double[] volumes = ScottPlot.DataGen.Random(rand, prices.Length, multiplier: 10, offset: 20);

            formsPlot1.Plot.AddCandlesticks(prices);
            formsPlot1.Plot.YLabel("Price");
            formsPlot1.Plot.XAxis.DateTimeFormat(true);
            formsPlot1.Plot.XAxis.Ticks(false);
            formsPlot1.Plot.XAxis.SetSizeLimit(max: 0);
            formsPlot1.AxesChanged += FormsPlot1_AxesChanged;
            formsPlot1.Refresh();

            formsPlot2.Plot.AddBar(volumes, dates);
            formsPlot2.Plot.YLabel("Volume");
            formsPlot2.Plot.XAxis.DateTimeFormat(true);
            formsPlot2.Plot.XAxis2.SetSizeLimit(max: 0);
            formsPlot2.Plot.AxisAuto();
            formsPlot2.Plot.SetAxisLimits(yMin: 0);
            formsPlot2.Refresh();
        }

        private void FormsPlot1_AxesChanged(object sender, EventArgs e)
        {
            formsPlot2.Plot.MatchAxis(formsPlot1.Plot, horizontal: true, vertical: false);
            formsPlot2.Plot.MatchLayout(formsPlot1.Plot, horizontal: true, vertical: false);
            formsPlot2.Refresh();
        }
    }
}
