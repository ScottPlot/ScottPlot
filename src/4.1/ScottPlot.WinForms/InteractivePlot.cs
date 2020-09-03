using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using ScottPlot.Renderer;

namespace ScottPlot.WinForms
{
    public partial class InteractivePlot : UserControl
    {
        public Plot plt;
        private readonly MouseTracker pc;

        public InteractivePlot()
        {
            InitializeComponent();
            pictureBox1.MouseWheel += pictureBox1_MouseWheel;

            plt = new Plot { OnRender = () => Render() };
            plt.FigureBackground.Color = Renderer.Color.Convert(SystemColors.Control);
            plt.DataBackground.Color = Colors.White;
            pc = new MouseTracker(plt);
            Render();
        }

        bool currentlyRendering = false;
        private void Render(bool force = true, bool recalculateLayout = true, bool antiAlias = true)
        {
            if (force == false && currentlyRendering)
                return;

            if (pictureBox1.Image is null || pictureBox1.Image.Size != pictureBox1.Size)
            {
                if (pictureBox1.Width < 1 | pictureBox1.Height < 1)
                    return;
                pictureBox1.Image?.Dispose();
                pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height, System.Drawing.Imaging.PixelFormat.Format32bppPArgb);
            }

            currentlyRendering = true;
            using (var renderer = new SystemDrawingRenderer((Bitmap)pictureBox1.Image))
                plt.Render(renderer, recalculateLayout);
            pictureBox1.Invalidate();
            Application.DoEvents();
            currentlyRendering = false;
        }

        private void pictureBox1_SizeChanged(object sender, EventArgs e)
        {
            Render();
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            pc.MouseMove(e.X, e.Y);
            if (e.Button != MouseButtons.None)
                Render(force: false, recalculateLayout: false, antiAlias: false);
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) pc.MouseDownLeft(e.X, e.Y);
            else if (e.Button == MouseButtons.Middle) pc.MouseDownCenter(e.X, e.Y);
            else if (e.Button == MouseButtons.Right) pc.MouseDownRight(e.X, e.Y);
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) pc.MouseUpLeft(e.X, e.Y);
            else if (e.Button == MouseButtons.Middle) pc.MouseUpCenter(e.X, e.Y);
            else if (e.Button == MouseButtons.Right) pc.MouseUpRight(e.X, e.Y);
            Render();
        }

        private void pictureBox1_MouseWheel(object sender, MouseEventArgs e)
        {
            pc.MouseWheel(e.Delta > 0, e.X, e.Y);
            Render();
        }
    }
}
