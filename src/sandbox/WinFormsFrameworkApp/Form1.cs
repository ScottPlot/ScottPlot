using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace WinFormsFrameworkApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Random rand = new Random();
            var plt = new ScottPlot.Plot();
            plt.AddSignal(ScottPlot.DataGen.RandomWalk(rand, 100));

            var v = new ScottPlot.FormsPlotViewer(plt);
            v.Show();
        }
    }
}
