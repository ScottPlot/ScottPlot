using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ScottPlot_Sandbox_Forms
{
    public partial class Form1 : Form
    {
        private ScottPlot.Plot plt;
        Random rand = new Random();

        public Form1()
        {
            InitializeComponent();
            plt = new ScottPlot.Plot();
            plt.settings.figureBackgroundColor = SystemColors.Control;

            UpdateSize();
            PlotSomeStuff();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void UpdateSize()
        {
            plt.Resize(pictureBox1.Width, pictureBox1.Height);
        }

        private void Render()
        {
            pictureBox1.Image = plt.GetBitmap();
            Application.DoEvents();
        }

        public void PlotSomeStuff()
        {

            double xFrac = rand.NextDouble();
            double yFrac = rand.NextDouble();

            // text
            plt.PlotText("Scott", xFrac, yFrac);

            // marker
            plt.PlotMarker(xFrac, yFrac);

            // scatter
            int scatterPointCount = 100;
            double[] xs = new double[scatterPointCount];
            double[] ys = new double[scatterPointCount];
            for (int i = 0; i < xs.Length; i++)
            {
                xs[i] = rand.NextDouble() * 10;
                ys[i] = rand.NextDouble() * 10;
            }
            plt.PlotScatter(xs, ys);
            plt.Axis(-1, 11, -1, 11);
            Render();
        }

        private void BtnBenchmark_Click(object sender, EventArgs e)
        {
            Debug.WriteLine("\n\nSTARTING BENCHMARK");
            int benchmarkCount = 50;
            plt.Clear();
            double totalTimeMs = 0;
            for (int i = 0; i < benchmarkCount; i++)
            {
                PlotSomeStuff();
                totalTimeMs += plt.settings.benchmarkMsec;
                lblStatus.Text = $"benchmarking {(i + 1) * 100 / benchmarkCount}%";
                Application.DoEvents(); // force display update
            }
            lblStatus.Text = String.Format("BENCHMARK: {0} frames in {1:00.000} ms = avg {2:0.000} ms/frame ({3:0.000} Hz)",
                benchmarkCount, totalTimeMs, totalTimeMs / benchmarkCount, benchmarkCount / totalTimeMs * 1000.0);
        }

        private void PictureBox1_SizeChanged(object sender, EventArgs e)
        {
            UpdateSize();
            Render();
        }

        private void CbContinuous_CheckedChanged(object sender, EventArgs e)
        {
            timer1.Enabled = cbContinuous.Checked;
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            PlotSomeStuff();
        }

        private void CbDark_CheckedChanged(object sender, EventArgs e)
        {
            if (cbDark.Checked)
                this.BackColor = Color.Maroon;
            else
                this.BackColor = Color.White;
        }

        #region mouse pan and zoom


        private void PictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (Control.MouseButtons == MouseButtons.Left)
                plt.settings.MouseDown(Cursor.Position.X, Cursor.Position.Y, panning: true);
            else if (Control.MouseButtons == MouseButtons.Right)
                plt.settings.MouseDown(Cursor.Position.X, Cursor.Position.Y, zooming: true);
        }

        private void PictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            plt.settings.MouseMove(Cursor.Position.X, Cursor.Position.Y);
            plt.settings.MouseUp();
            Render();
        }

        private bool mouseMoveRedrawInProgress = false;
        private void PictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseMoveRedrawInProgress)
                return;

            if (Control.MouseButtons != MouseButtons.None)
            {
                mouseMoveRedrawInProgress = true;
                plt.settings.MouseMove(Cursor.Position.X, Cursor.Position.Y);
                Render();
                mouseMoveRedrawInProgress = false;
            }
        }

        #endregion
    }
}