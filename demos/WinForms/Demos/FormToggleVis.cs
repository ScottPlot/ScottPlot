using ScottPlot.Plottables;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ScottPlotDemos
{
    public partial class FormToggleVis : Form
    {
        IPlottable plottedSin, plottedCos, plottedRandom;

        public FormToggleVis()
        {
            InitializeComponent();
            plottedSin = formsPlot1.plt.PlotSignal(ScottPlot.DataGen.Sin(50), label: "sin");
            plottedCos = formsPlot1.plt.PlotSignal(ScottPlot.DataGen.Cos(50), label: "cos");
            plottedRandom = formsPlot1.plt.PlotSignal(ScottPlot.DataGen.RandomWalk(null, 50), label: "random");
            formsPlot1.plt.Legend();
            formsPlot1.Render();
        }

        private void cbSin_CheckedChanged(object sender, EventArgs e)
        {
            plottedSin.visible = cbSin.Checked;
            formsPlot1.Render();
        }

        private void cbCos_CheckedChanged(object sender, EventArgs e)
        {
            plottedCos.visible = cbCos.Checked;
            formsPlot1.Render();
        }

        private void cbRandom_CheckedChanged(object sender, EventArgs e)
        {
            plottedRandom.visible = cbRandom.Checked;
            formsPlot1.Render();
        }
    }
}
