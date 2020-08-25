using System;
using System.Drawing;
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

        PointF mouseLeftDown = new PointF(-1, -1);
        PointF mouseRightDown = new PointF(-1, -1);
        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                mouseLeftDown = e.Location;
            else if (e.Button == MouseButtons.Right)
                mouseRightDown = e.Location;
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseLeftDown.X > -1)
            {
                float dX = e.X - mouseLeftDown.X;
                float dY = e.Y - mouseLeftDown.Y;
                fig.MousePan(dX, dY, remember: false);
                Render();
            }

            if (mouseRightDown.X > -1)
            {
                float dX = e.X - mouseRightDown.X;
                float dY = e.Y - mouseRightDown.Y;
                fig.MouseZoom(dX, dY, remember: false);
                Render();
            }
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            if (mouseLeftDown.X > -1)
            {
                float dX = e.X - mouseLeftDown.X;
                float dY = e.Y - mouseLeftDown.Y;
                fig.MousePan(dX, dY);
                Render(false);
            }

            if (mouseRightDown.X > -1)
            {
                float dX = e.X - mouseRightDown.X;
                float dY = e.Y - mouseRightDown.Y;
                fig.MouseZoom(dX, dY);
                Render(false);
            }

            mouseLeftDown = new PointF(-1, -1);
            mouseRightDown = new PointF(-1, -1);
        }
    }
}
