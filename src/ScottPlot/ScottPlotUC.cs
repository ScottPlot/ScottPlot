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
    public partial class ScottPlotUC : UserControl
    {

        public Figure fig = new ScottPlot.Figure(123, 123);
        public double[] Ys = null;
        public double[] Xs = null;

        public class signal
        {
            public double[] values;
            public double sampleRate;
            public double xSpacing;
            public double offsetX;
            public double offsetY;
            public Color color;
            public float lineWidth;
            public string label;
            
            public signal(double[] values, double sampleRate, double offsetX = 0, double offsetY = 0, Color? color = null, float lineWidth=1, string label = null)
            {
                this.values = values;
                this.sampleRate = sampleRate;
                this.xSpacing = 1.0 / sampleRate;
                this.offsetX = offsetX;
                this.offsetY = offsetY;
                if (color == null) color = Color.Red;
                this.color = (Color)color;
                this.lineWidth = lineWidth;
                this.label = label;
            }
        }

        public signal[] signals = null;

        public double signalRate = 20_000;
        
        public ScottPlotUC()
        {
            InitializeComponent();

            // add a mousewheel scroll handler
            pictureBox1.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseWheel);

            // style the plot area
            fig.styleForm();
            fig.Zoom(.8, .8);
            fig.title = "ScottPlot User Control";
        }

        public void ResetAxis()
        {
            if (signals != null)
            {
                // signal mode

                // resize to the first sweep
                double y1 = signals[0].values.Min() + signals[0].offsetY;
                double y2 = signals[0].values.Max() + signals[0].offsetY;
                for (int i=1; i<signals.Length; i++)
                {
                    // check later sweeps to see if they're more expansive
                    y1 = Math.Min(y1, signals[i].values.Min() + signals[i].offsetY);
                    y2 = Math.Max(y2, signals[i].values.Max() + signals[i].offsetY);
                }

                // x limits can be calculated
                double x1 = signals[0].offsetX;
                double x2 = x1 + (double)signals[0].values.Length / (double)signals[0].sampleRate;

                // apply these limits
                fig.Axis(x1, x2, y1, y2);
                fig.Zoom(null, .9);
            }
            else if (Xs != null && Ys != null)
            {
                // X/Y pairs
                fig.ResizeToData(Xs, Ys, .9, .9);
            }
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

            // plot signals (if there are any)
            if (signals != null)
            {
                for (int i = 0; i < signals.Length; i++)
                {
                    fig.PlotSignal(signals[i].values, 1.0 / signals[i].sampleRate, signals[i].offsetX, 
                                   signals[i].offsetY, signals[i].lineWidth, signals[i].color);
                }
            }

            // plot XY points (if they're not null)
            if (Xs != null && Ys != null)
            {
                fig.PlotLines(Xs, Ys);
                fig.PlotScatter(Xs, Ys);
            }

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

        private void pictureBox1_MouseWheel(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            double mag = 1.2;
            if (e.Delta>0) fig.Zoom(mag, mag);
            else fig.Zoom(1.0 / mag, 1.0 / mag);
            Redraw();
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

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}
