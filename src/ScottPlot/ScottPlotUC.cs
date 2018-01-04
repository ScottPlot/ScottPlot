using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

// this example uses the ScottPlotUC to *STORE* data it interactively displays.

namespace ScottPlot
{
    public partial class ScottPlotUC : UserControl
    {
        public double VERSION = 1.01;
        public double[] Xs;
        public double[] Ys;
        public ScottPlot.Figure SP;

        public ScottPlotUC()
        {
            InitializeComponent();
            // instantiate the SP class we will use forever
            SP = new ScottPlot.Figure();

            // place the debug text box in a good location and set its startup message
            richTextBox1.Location = new System.Drawing.Point(40,10);
            richTextBox1.Text = $"ScottPlot v{VERSION}";

        }

        // put stats about the graph in the debug bar
        public void MessageUpdate()
        {
            double ms = (double)stopwatch.ElapsedTicks / System.Diagnostics.Stopwatch.Frequency * 1000.0;
            string msg = "";
            //msg += $"points: {Xs.Length}\n";
            msg += string.Format("points: {0:n0}\n", Xs.Length);
            msg += string.Format("bitmap bytes: {0:n0}\n", pictureBox1.Width * pictureBox1.Height * 3);
            msg += string.Format("plot time: {0:.02} ms\n", ms);
            msg += string.Format("plot rate: {0:.02} Hz\n", 1000.0/ms);
            richTextBox1.Text = msg;
        }

        // give the ScottPlotUC data to store in memory
        public void SetData(double[] Xs, double[] Ys)
        {
            this.Xs = Xs;
            this.Ys = Ys;
            SP.AxisAuto(Xs, Ys);
            
            UpdateGraph();
            MessageUpdate();
        }
        
        private System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();

        private bool thinking = false;
        public void UpdateGraph()
        {
            thinking = true;
            stopwatch.Restart();
            SP.Resize(pictureBox1.Width, pictureBox1.Height);
            SP.Clear();
            SP.Grid();
            SP.PlotLine(Xs, Ys);
            pictureBox1.BackgroundImage = SP.Render();
            this.Refresh();
            Application.DoEvents();
            stopwatch.Stop();
            MessageUpdate();
            thinking = false;
        }


        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            SP.MouseDown(e.X, e.Y);
            if (e.Button == MouseButtons.Middle)
            {
                SP.AxisAuto(Xs, Ys);
                UpdateGraph();
            }
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
        }

        // as the mouse moves, determine how to handle the change
        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (thinking == true) return;
            if (e.Button == MouseButtons.None) return;

            switch (e.Button.ToString())
            {
                case "Left":
                    SP.MousePan(e.X, e.Y);
                    UpdateGraph();
                    return;
                case "Right":
                    SP.MouseZoom(e.X, e.Y);
                    UpdateGraph();
                    return;
                case "Middle":
                    return;
                default:
                    return;
            }
        }


        // double-click to show/hide debug messages
        private void pictureBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (richTextBox1.Visible == true)
            {
                richTextBox1.Visible = false;
            }
            else
            {
                richTextBox1.Visible = true;
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void ScottPlotUC_SizeChanged(object sender, EventArgs e)
        {

        }

        private void ScottPlotUC_AutoSizeChanged(object sender, EventArgs e)
        {

        }

        private void ScottPlotUC_Load(object sender, EventArgs e)
        {
        }

        private void pictureBox1_VisibleChanged(object sender, EventArgs e)
        {
            // once the user control is visible, size it appropraitely and plot some dummy data
            Xs = new double[] { 1, 1 };
            Ys = new double[] { 1, 1 };
            SP.AxisAuto(Xs, Ys);
            UpdateGraph();
        }
    }
}
