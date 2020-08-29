using ScottPlot.Renderer;
using System;
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
            double[] xs = Generate.Consecutive(50);
            plt.PlotScatter(xs, Generate.RandomWalk(rand, 50), Colors.Red);
            plt.PlotScatter(xs, Generate.RandomWalk(rand, 50), Colors.Green);
            plt.PlotScatter(xs, Generate.RandomWalk(rand, 50), Colors.Blue);
            Render();
        }

        private void Render()
        {
            // ensure the picturebox has a properly-sized Bitmap
            if (pictureBox1.Image is null || pictureBox1.Image.Size != pictureBox1.Size)
            {
                if (pictureBox1.Width < 1 | pictureBox1.Height < 1)
                    return;
                pictureBox1.Image?.Dispose();
                pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height, System.Drawing.Imaging.PixelFormat.Format32bppPArgb);
            }

            using(var renderer = new SystemDrawingRenderer((Bitmap)pictureBox1.Image))
            {
                plt.Render(renderer);
            }
            pictureBox1.Invalidate(); // May need to "Application.DoEvents()" in .NET Framework
        }

        private void pictureBox1_SizeChanged(object sender, EventArgs e)
        {
            Render();
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            pc.MouseMove(e.X, e.Y);
            if (e.Button != MouseButtons.None)
                Render();
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
    }
}
