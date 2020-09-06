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
        public Plot Plot;
        public readonly MouseTracker MouseTracker;

        public delegate void RenderDelegate();
        public RenderDelegate OnRender;

        public InteractivePlot()
        {
            InitializeComponent();
            pictureBox1.MouseWheel += pictureBox1_MouseWheel;

            Plot = new Plot();
            Plot.FigureBackground.Color = Renderer.Color.Convert(SystemColors.Control);
            Plot.DataBackground.Color = Colors.White;
            MouseTracker = new MouseTracker(Plot);
            Render();
        }

        bool currentlyRendering = false;
        public void Render(bool force = true, bool recalculateLayout = true, bool lowQuality = false, bool invokeOnRender = true)
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
                Plot?.Render(renderer, recalculateLayout, lowQuality);
            pictureBox1.Invalidate();
            Application.DoEvents();
            if (invokeOnRender)
                OnRender?.Invoke();
            currentlyRendering = false;
        }

        private void pictureBox1_SizeChanged(object sender, EventArgs e)
        {
            Render();
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            MouseTracker.MouseMove(e.X, e.Y);
            if (e.Button != MouseButtons.None)
                Render(force: false, recalculateLayout: false, lowQuality: true);
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) MouseTracker.MouseDownLeft(e.X, e.Y);
            else if (e.Button == MouseButtons.Middle) MouseTracker.MouseDownCenter(e.X, e.Y);
            else if (e.Button == MouseButtons.Right) MouseTracker.MouseDownRight(e.X, e.Y);
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) MouseTracker.MouseUpLeft(e.X, e.Y);
            else if (e.Button == MouseButtons.Middle) MouseTracker.MouseUpCenter(e.X, e.Y);
            else if (e.Button == MouseButtons.Right) MouseTracker.MouseUpRight(e.X, e.Y);
            Render();
        }

        private void pictureBox1_MouseWheel(object sender, MouseEventArgs e)
        {
            MouseTracker.MouseWheel(e.Delta > 0, e.X, e.Y);
            Render();
        }

        private void pictureBox1_DoubleClick(object sender, EventArgs e)
        {
            MouseTracker.DoubleClick();
            Render();
        }
    }
}
