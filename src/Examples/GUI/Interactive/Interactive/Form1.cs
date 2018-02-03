using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Interactive
{
    public partial class Form1 : Form
    {

        // our figure and data exist at the form level
        ScottPlot.Figure fig;
        double[] Xs, Ys;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // init the figure only after the form loaded (so we know final pictureBox1 dimensions)
            fig = new ScottPlot.Figure(pictureBox1.Width, pictureBox1.Height);

            // create the figure and apply styling
            fig.styleForm();
            fig.title = "Awesome Data";
            fig.yLabel = "Random Walk";
            fig.xLabel = "Sample Number";

            // synthesize data
            int pointCount = 123;
            Xs = fig.gen.Sequence(pointCount);
            Ys = fig.gen.RandomWalk(pointCount);
            fig.ResizeToData(Xs, Ys, .9, .9);

            ResizeAndRedraw();

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            ResizeAndRedraw();
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            ResizeAndRedraw();
        }
        
        public void ResizeAndRedraw()
        {
            if (fig == null) return;
            //fig.BenchmarkThis();
            fig.Resize(pictureBox1.Width, pictureBox1.Height);
            fig.RedrawFrame();
            fig.PlotLines(Xs, Ys, 1, Color.Red);
            fig.PlotScatter(Xs, Ys, 5, Color.Blue);
            pictureBox1.Image = fig.Render();
        }


        /* MOUSE HANDLING */

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) fig.MousePanStart(e.X, e.Y);
            else if (e.Button == MouseButtons.Right) fig.MouseZoomStart(e.X, e.Y);
            else if (e.Button == MouseButtons.Middle) Console.WriteLine("mouse down middle");
        }
        
        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) fig.MousePanEnd();
            else if (e.Button == MouseButtons.Right) fig.MouseZoomEnd();
            else if (e.Button == MouseButtons.Middle) Console.WriteLine("mouse up middle");
        }

        public bool busyDrawingPlot = false;
        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (busyDrawingPlot == false)
            {
                fig.MouseMove(e.X, e.Y);
                busyDrawingPlot = true;
                ResizeAndRedraw();
                Application.DoEvents();
                busyDrawingPlot = false;
            }
        }

    }
}
