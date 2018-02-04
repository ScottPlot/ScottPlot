using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Signal
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            nud_sec_ValueChanged(null, null);
        }

        ScottPlot.Figure fig;

        private void button1_Click(object sender, EventArgs e)
        {
            fig = new ScottPlot.Figure(pictureBox1.Width, pictureBox1.Height);
            fig.styleForm();
            fig.yLabel = "value";
            fig.xLabel = "time (seconds)";

            int pointCount = (int)(nud_sec.Value * 20000);
            double[] Xs = fig.gen.Sequence(pointCount, 1.0 / 20000.0);
            double[] Ys = fig.gen.RandomWalk(pointCount);

            fig.ResizeToData(Xs, Ys, .9, .9);

            fig.PlotLines(Xs, Ys, 1, Color.Red);
            pictureBox1.Image = fig.Render();
        }

        private void Redraw()
        {
            pictureBox1.Image = fig.Render();
        }

        private void nud_sec_ValueChanged(object sender, EventArgs e)
        {
            int pointCount = (int)(nud_sec.Value*20000);
            label4.Text = string.Format("{0:0.00} million data points", pointCount / 1000000.0);
            label5.Text = string.Format("{0:0.00} minutes of data", pointCount / 20000.0 / 60.0);
        }
    }
}
