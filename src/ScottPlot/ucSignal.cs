using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ScottPlot
{
    public partial class ucSignal : UserControl
    {

        public Figure fig = new ScottPlot.Figure(123, 123);
        public double[] Ys = null;
        public double RATE = 20_000;

        public ucSignal()
        {
            InitializeComponent();
        }

        public void ResetAxis()
        {
            if (Ys == null) return;
            fig.Axis(0, 1.0 / RATE * Ys.Length, null, null);
            fig.ResizeToData(null, Ys, null, .9);
            Redraw(true);
        }

        public void UpdateSize()
        {
            fig.Resize(pictureBox1.Width, pictureBox1.Height);
            Redraw(true);
        }

        public void Redraw(bool redrawFrameToo=false)
        {
            fig.Benchmark(showBenchmark);
            if (redrawFrameToo) fig.RedrawFrame();
            fig.PlotSignal(Ys, 1.0 / RATE);
            pictureBox1.Image = fig.Render();
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) fig.MousePanStart(e.X, e.Y); // left-click-drag pans
            else if (e.Button == MouseButtons.Right) fig.MouseZoomStart(e.X, e.Y); // right-click-drag zooms
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) fig.MousePanEnd();
            else if (e.Button == MouseButtons.Right) fig.MouseZoomEnd();
        }

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Middle) ResetAxis(); // middle click to reset view
        }

        public bool showBenchmark = false;
        private void pictureBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.showBenchmark = !this.showBenchmark; // double-click graph to display benchmark stats
            Redraw();
        }

        public bool busyDrawingPlot = false;
        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (fig.MouseIsDragging() && busyDrawingPlot == false)
            {
                fig.MouseMove(e.X, e.Y);
                busyDrawingPlot = true;
                UpdateSize();
                Application.DoEvents();
                busyDrawingPlot = false;
            }
        }

        private void pictureBox1_SizeChanged(object sender, EventArgs e)
        {
            UpdateSize();
        }
    }
}
