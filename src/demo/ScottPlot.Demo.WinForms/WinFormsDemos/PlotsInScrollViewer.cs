using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ScottPlot.Demo.WinForms.WinFormsDemos
{
    public partial class PlotsInScrollViewer : Form
    {
        public PlotsInScrollViewer()
        {
            InitializeComponent();

            Random rand = new Random();
            formsPlot1.Plot.AddSignal(DataGen.RandomWalk(rand, 100));
            formsPlot2.Plot.AddSignal(DataGen.RandomWalk(rand, 100));
            formsPlot3.Plot.AddSignal(DataGen.RandomWalk(rand, 100));

            formsPlot1.MouseWheel += FormsPlot_MouseWheel;
            formsPlot2.MouseWheel += FormsPlot_MouseWheel;
            formsPlot3.MouseWheel += FormsPlot_MouseWheel;

            formsPlot1.Refresh();
            formsPlot2.Refresh();
            formsPlot3.Refresh();
        }

        private void FormsPlot_MouseWheel(object sender, MouseEventArgs e)
        {
            ((HandledMouseEventArgs)e).Handled = rbZoom.Checked;
        }

        private void rbScroll_CheckedChanged(object sender, EventArgs e)
        {
            formsPlot1.Configuration.ScrollWheelZoom = false;
            formsPlot2.Configuration.ScrollWheelZoom = false;
            formsPlot3.Configuration.ScrollWheelZoom = false;
        }

        private void rbZoom_CheckedChanged(object sender, EventArgs e)
        {
            formsPlot1.Configuration.ScrollWheelZoom = true;
            formsPlot2.Configuration.ScrollWheelZoom = true;
            formsPlot3.Configuration.ScrollWheelZoom = true;
        }
    }
}
