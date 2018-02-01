/* minimal-case example drawing a line and showing it in a picturebox */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using ScottPlot;

namespace Simple
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Figure fig = new Figure(pictureBox1.Width, pictureBox1.Height);
            fig.styleForm(); // optimizes colors for forms
            fig.title = "Plotting Point Arrays";
            fig.yLabel = "Random Walk";
            fig.xLabel = "Sample Number";

            // generate data
            int pointCount = 123;
            double[] Xs = fig.gen.Sequence(pointCount);
            double[] Ys = fig.gen.RandomWalk(pointCount);
            fig.ResizeToData(Xs, Ys, .9, .9);

            // make the plot
            fig.BenchmarkThis();
            fig.PlotLines(Xs, Ys, 1, Color.Red);
            fig.PlotScatter(Xs, Ys, 5, Color.Blue);

            pictureBox1.Image = fig.Render();
        }
    }
}
