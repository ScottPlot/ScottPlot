using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ScottPlot.Demo.WinForms.WinFormsDemos;
public partial class HistogramDemo : Form
{
    readonly Random Rand = new();
    readonly ScottPlot.Statistics.Histogram Hist = new(min: 0, max: 1, binCount: 40);

    public HistogramDemo()
    {
        InitializeComponent();
        var bar = formsPlot1.Plot.AddBar(values: Hist.Counts, positions: Hist.BinCenters);
        bar.BarWidth = Hist.BinSize;
        formsPlot1.Plot.XLabel("Value");
        formsPlot1.Plot.YLabel("Count");
        formsPlot1.Refresh();
    }

    private void timer1_Tick(object sender, EventArgs e)
    {
        if (!cbRun.Checked)
            return;

        int newValueCount = Rand.Next(123, 1234);
        double[] newData = ScottPlot.DataGen.RandomNormal(Rand, newValueCount, stdDev: .15);
        Hist.AddRange(newData);

        formsPlot1.Plot.AxisAuto();
        formsPlot1.Plot.SetAxisLimits(xMin: -.1, xMax: 1.1, yMin: 0);
        formsPlot1.Plot.Title($"Histogram with {Hist.ValuesCounted:N0} Values");
        formsPlot1.Refresh();
    }

    private void btnReset_Click(object sender, EventArgs e)
    {
        Hist.Clear();
        formsPlot1.Plot.Title($"Histogram with 0 Values");
        formsPlot1.Refresh();
    }
}
