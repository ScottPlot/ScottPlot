using ScottPlot.Mouse;
using System;
using System.Windows.Forms;

namespace FormsPlotSandbox
{
    public partial class FormRenderTest : Form
    {
        readonly ScottPlot.Space.FigureInfo fig = new ScottPlot.Space.FigureInfo();

        public FormRenderTest()
        {
            InitializeComponent();
            fig.SetLimits(-1, 1, -10, 10);
            pictureBox1_SizeChanged(null, null);
        }

        private bool busyRendering = false;
        private void Render(bool skipIfBusy = true)
        {
            if (busyRendering && skipIfBusy)
                return;

            busyRendering = true;
            pictureBox1.Image?.Dispose();
            pictureBox1.Image = ScottPlot.Space.FakePlot.Triangle(fig);
            Application.DoEvents();
            busyRendering = false;
        }

        private void pictureBox1_SizeChanged(object sender, EventArgs e)
        {
            fig.Resize(
                width: pictureBox1.Width,
                height: pictureBox1.Height,
                dataWidth: pictureBox1.Width - 50,
                dataHeight: pictureBox1.Height - 50,
                dataOffsetX: 25,
                dataOffsetY: 25);
            Render();
        }

        MousePan mousePan = null;
        MouseZoom mouseZoom = null;
        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                mousePan = new MousePan() { X = e.X, Y = e.Y };
            else if (e.Button == MouseButtons.Right)
                mouseZoom = new MouseZoom() { X = e.X, Y = e.Y };
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (mousePan != null)
            {
                mousePan.X2 = e.X;
                mousePan.Y2 = e.Y;
                fig.ApplyMouseAction(mousePan, remember: false);
                Render();
            }

            if (mouseZoom != null)
            {
                mouseZoom.X2 = e.X;
                mouseZoom.Y2 = e.Y;
                fig.ApplyMouseAction(mouseZoom, remember: false);
                Render();
            }
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            if (mousePan != null)
            {
                mousePan.X2 = e.X;
                mousePan.Y2 = e.Y;
                fig.ApplyMouseAction(mousePan);
                mousePan = null;
                Render(false);
            }

            if (mouseZoom != null)
            {
                mouseZoom.X2 = e.X;
                mouseZoom.Y2 = e.Y;
                fig.ApplyMouseAction(mouseZoom);
                mouseZoom = null;
                Render(false);
            }
        }
    }
}
