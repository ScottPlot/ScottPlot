using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ScottPlot
{
    public partial class FormsPlotViewer : Form
    {
        public FormsPlotViewer(Plot plt)
        {
            InitializeComponent();
            formsPlot1.ReplacePlot(plt);
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void PlotViewer_Load(object sender, EventArgs e)
        {

        }

        private void formsPlot1_AxesChanged(object sender, EventArgs e)
        {
            statusLabel.Text = formsPlot1.plt.GetSettings(false).benchmark.ToString();
        }
    }
}
