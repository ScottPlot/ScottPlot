using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinFormsApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            Random rand = new(0);
            double[] ys = ScottPlot.DataGen.Random(rand, 10);
            double[] xs = ScottPlot.DataGen.Consecutive(ys.Length);

            var sig = formsPlot1.Plot.AddSignalXY(xs, ys);
            sig.StepDisplay = true;
            sig.FillAbove(Color.Blue);

            formsPlot1.Refresh();
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            formsPlot1.Plot.AxisAuto(horizontalMargin: (double)numericUpDown1.Value);
            formsPlot1.Refresh();
        }
    }
}
