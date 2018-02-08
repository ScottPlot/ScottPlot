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
        int sampleRate = 20000;

        private void button1_Click(object sender, EventArgs e)
        {
            fig = new ScottPlot.Figure(pictureBox1.Width, pictureBox1.Height);
            fig.styleForm();
            fig.labelY = "value";
            fig.labelX = "time (seconds)";

            
            int pointCount = (int)(nud_sec.Value * sampleRate);
            double[] Ys = fig.gen.RandomWalk(pointCount);
            fig.AxisSet(0, pointCount / sampleRate, null, null);
            fig.AxisAuto(null, Ys, .9, .9);

            fig.BenchmarkThis();
            fig.PlotSignal(Ys, 1.0 / sampleRate);
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
