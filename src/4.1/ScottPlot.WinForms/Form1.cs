using ScottPlot.Renderer;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace ScottPlot.WinForms
{
    public partial class Form1 : Form
    {
        public Plot plt;
        private readonly PlotControl pc;

        public Form1()
        {
            InitializeComponent();
            plt = new Plot();
            pc = new PlotControl(plt);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Random rand = new Random();
            double[] xs = Generate.Consecutive(51);
            var s1 = plt.PlotScatter(xs, Generate.RandomWalk(rand, 51, .01), Colors.Red);
            var s2 = plt.PlotScatter(xs, Generate.RandomWalk(rand, 51), Colors.Green);
            var s3 = plt.PlotScatter(xs, Generate.RandomWalk(rand, 51, 100), Colors.Blue);

            plt.AddAxes(1, 3);
            s1.YAxisIndex = 0;
            s2.YAxisIndex = 1;
            s3.YAxisIndex = 2;

            Render();
        }

        bool currentlyRendering = false;
        Stopwatch stopwatch = new Stopwatch();
        private void Render(bool skipIfCurrentlyRendering = false)
        {
            if (skipIfCurrentlyRendering && currentlyRendering)
                return;

            stopwatch.Restart();

            // ensure the picturebox has a properly-sized Bitmap
            if (pictureBox1.Image is null || pictureBox1.Image.Size != pictureBox1.Size)
            {
                if (pictureBox1.Width < 1 | pictureBox1.Height < 1)
                    return;
                pictureBox1.Image?.Dispose();
                pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height, System.Drawing.Imaging.PixelFormat.Format32bppPArgb);
            }

            currentlyRendering = true;
            using (var renderer = new SystemDrawingRenderer((Bitmap)pictureBox1.Image))
            {
                plt.Render(renderer);
            }
            stopwatch.Stop();
            pictureBox1.Invalidate();
            Application.DoEvents();
            currentlyRendering = false;

            double elapsedSec = (double)stopwatch.ElapsedTicks / Stopwatch.Frequency;
            Text = $"Rendered in {elapsedSec * 1000:0.00} ms ({1 / elapsedSec:0.00} Hz)";
        }

        private void pictureBox1_SizeChanged(object sender, EventArgs e)
        {
            Render();
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            pc.MouseMove(e.X, e.Y);
            if (e.Button != MouseButtons.None)
            {
                Render(skipIfCurrentlyRendering: true);
            }
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                pc.MouseDownLeft(e.X, e.Y);
            else if (e.Button == MouseButtons.Middle)
                pc.MouseDownCenter(e.X, e.Y);
            else if (e.Button == MouseButtons.Right)
                pc.MouseDownRight(e.X, e.Y);
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                pc.MouseUpLeft(e.X, e.Y);
            else if (e.Button == MouseButtons.Middle)
                pc.MouseUpCenter(e.X, e.Y);
            else if (e.Button == MouseButtons.Right)
                pc.MouseUpRight(e.X, e.Y);

            Render();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e) => UpdateActiveAxes();
        private void checkBox2_CheckedChanged(object sender, EventArgs e) => UpdateActiveAxes();
        private void checkBox3_CheckedChanged(object sender, EventArgs e) => UpdateActiveAxes();
        private void UpdateActiveAxes()
        {
            // customize indexes in active Ys array
            List<int> activeYsList = new List<int>();
            if (checkBox1.Checked) activeYsList.Add(0);
            if (checkBox2.Checked) activeYsList.Add(1);
            if (checkBox3.Checked) activeYsList.Add(2);
            int[] activeYs = activeYsList.ToArray();

            // there is only one X axis and it is active
            int[] activeXs = { 0 };

            pc.SetActiveAxes(activeXs, activeYs);
        }
    }
}
