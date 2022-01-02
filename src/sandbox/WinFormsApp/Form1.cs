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
            double[] xs = ScottPlot.DataGen.Consecutive(10_000);
            double[] ys = ScottPlot.DataGen.RandomWalk(rand, 10_000);
            var sig1 = formsPlot1.Plot.AddSignalXY(xs, ys);
            sig1.MarkerSize = 20;

            formsPlot1.Refresh();
        }
    }
}
