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
    public partial class FormCustomGrid : Form
    {
        public FormCustomGrid()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Random rand = new Random();
            formsPlot1.plt.PlotScatter(ScottPlot.DataGen.Consecutive(100), ScottPlot.DataGen.RandomWalk(rand, 100));
            formsPlot1.plt.PlotScatter(ScottPlot.DataGen.Consecutive(100), ScottPlot.DataGen.RandomWalk(rand, 100));
            formsPlot1.plt.PlotScatter(ScottPlot.DataGen.Consecutive(100), ScottPlot.DataGen.RandomWalk(rand, 100));
            formsPlot1.plt.AxisAuto();
            formsPlot1.Render();
        }

        public void UpdateGridToReflectGui()
        {
            double xSpacing = 0;
            if (nudX.Enabled)
                xSpacing = (double)nudX.Value;
            
            double ySpacing = 0;
            if (nudY.Enabled)
                ySpacing = (double)nudY.Value;

            formsPlot1.plt.Grid(xSpacing: xSpacing, ySpacing: ySpacing);
            formsPlot1.Render();
        }

        private void CbShowGrid_CheckedChanged(object sender, EventArgs e)
        {
            gbSpacing.Enabled = cbShowGrid.Checked;
            formsPlot1.plt.Grid(cbShowGrid.Checked);
            formsPlot1.Render();
        }

        private void NudX_ValueChanged(object sender, EventArgs e)
        {
            UpdateGridToReflectGui();
        }

        private void NudY_ValueChanged(object sender, EventArgs e)
        {
            UpdateGridToReflectGui();
        }

        private void CbAutoX_CheckedChanged(object sender, EventArgs e)
        {
            nudX.Enabled = (!cbAutoX.Checked);
            UpdateGridToReflectGui();
        }

        private void CbAutoY_CheckedChanged(object sender, EventArgs e)
        {
            nudY.Enabled = (!cbAutoY.Checked);
            UpdateGridToReflectGui();
        }
    }
}
