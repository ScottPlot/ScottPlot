using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _04_ScottPlotUC
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // create some data to plot
            int nPoints = 1000;
            double[] Xs = ScottPlot.Generate.Sequence(nPoints);
            double[] Ys = ScottPlot.Generate.Sine(nPoints, 5);
            Ys = ScottPlot.Generate.Salt(Ys, .2);

            // load the data into the ScottPlot User Control
            scottPlotUC1.SetData(Xs, Ys);
        }
    }
}
