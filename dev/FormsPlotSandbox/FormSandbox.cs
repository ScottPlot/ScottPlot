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
        private System.Threading.Thread dataUpdateThread;
        private PlottableScatter scatterPlot;

        public FormSandbox()
        {
            InitializeComponent();
            double[] ys = { 200, 150, 1100, 100, 125, 175, 125, 450, 250, 1000, 150, 450, 50, 50, 200, 400, 150, 100 };
            double[] xs = DataGen.Consecutive(ys.Length);
            formsPlot1.plt.PlotFillAboveBelow(xs, ys, baseline: 200);
            formsPlot1.Render();
        }
    }
}
