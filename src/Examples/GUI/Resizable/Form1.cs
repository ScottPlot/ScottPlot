using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Resizable
{
    public partial class Form1 : Form
    {

        ScottPlot.Figure fig;
        double[] Xs, Ys;

        public Form1()
        {
            InitializeComponent();

            // create the figure and apply styling
            fig = new ScottPlot.Figure(pictureBox1.Width, pictureBox1.Height);
            fig.styleForm();
            fig.title = "Awesome Data";
            fig.yLabel = "Random Walk";
            fig.xLabel = "Sample Number";
            
            // synthesize data
            int pointCount = 123;
            Xs = fig.gen.Sequence(pointCount);
            Ys = fig.gen.RandomWalk(pointCount);
            fig.ResizeToData(Xs, Ys, .9, .9);
        }
        
        private void Form1_Shown(object sender, EventArgs e) {ResizeAndRedraw();}
        private void Form1_Resize(object sender, EventArgs e) { ResizeAndRedraw(); }

        public void ResizeAndRedraw()
        {
            if (fig == null) return;
            fig.Resize(pictureBox1.Width, pictureBox1.Height);
            fig.RedrawFrame();
            fig.PlotLines(Xs, Ys, 1, Color.Red);
            fig.PlotScatter(Xs, Ys, 5, Color.Blue);
            pictureBox1.Image = fig.Render();
        }

    }
}
