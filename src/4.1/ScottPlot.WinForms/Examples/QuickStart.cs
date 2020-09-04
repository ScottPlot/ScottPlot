using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ScottPlot.WinForms.Examples
{
    public partial class QuickStart : Form
    {
        public QuickStart()
        {
            InitializeComponent();
        }

        private void QuickStart_Load(object sender, EventArgs e)
        {
            double[] xs = Generate.Consecutive(51);
            double[] sin = Generate.Sin(51);
            double[] cos = Generate.Cos(51);

            interactivePlot1.Plot.PlotScatter(xs, sin);
            interactivePlot1.Plot.PlotScatter(xs, cos);
        }
    }
}
