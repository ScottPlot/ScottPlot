using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace ScottPlot.WinForms
{
    public partial class FormsPlotExperimental : UserControl
    {
        public readonly Plot MainPlot = new Plot();
        private readonly Settings Settings;

        [Obsolete("Access 'MainPlot' instead of 'plt'")]
        public Plot plt => MainPlot;

        private readonly Timer RenderTimer = new Timer();
        private readonly bool IsDesignerMode;

        private bool IsMouseDown => MouseButtons != MouseButtons.None;
        private bool IsMouseDownLeft => MouseButtons == MouseButtons.Left;
        private bool IsMouseDownRight => MouseButtons == MouseButtons.Right;
        private bool IsMouseDownMiddle => MouseButtons == MouseButtons.Middle;

        public FormsPlotExperimental()
        {
            InitializeComponent();
            IsDesignerMode = Process.GetCurrentProcess().ProcessName == "devenv";
            Settings = MainPlot.GetSettings(false);

            if (IsDesignerMode)
            {
                MainPlot.Title($"ScottPlot {Plot.Version}");
                MainPlot.PlotSignal(DataGen.Sin(51));
                MainPlot.PlotSignal(DataGen.Cos(51));
                MainPlot.Legend();
                MainPlot.XLabel("Horizontal Axis Label");
                MainPlot.YLabel("Vertical Axis Label");
            }

            // set up a timer to check when renders are needed
            RenderTimer.Interval = 1;
            RenderTimer.Tick += new EventHandler(RenderTimer_Tick);
            RenderTimer.Start();

            // connect mouse events to plot interactions
            pictureBox1.SizeChanged += PictureBox1_SizeChanged;
            pictureBox1.DoubleClick += PictureBox1_DoubleClick;
            pictureBox1.MouseDown += PictureBox1_MouseDown;
            pictureBox1.MouseMove += PictureBox1_MouseMove;
            pictureBox1.MouseUp += PictureBox1_MouseUp;
        }

        #region rendering

        /* A timer continuously checks if renders are needed and initiates a render if required. 
         * This allows the Render() call to be non-blocking.
         * However, the timer tick method that calls RenderNow() does block the UI thread.
         */

        private Bitmap Bmp;
        private bool NeedsRender;

        private void RenderTimer_Tick(object sender, EventArgs e)
        {
            if (NeedsRender)
            {
                NeedsRender = false;
                RenderNow();
            }
        }

        private void RenderNow()
        {
            if (pictureBox1.Width < 1 || pictureBox1.Height < 1)
                return;

            if (Bmp is null || Bmp.Size != pictureBox1.Size)
            {
                Bmp?.Dispose();
                Bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height, System.Drawing.Imaging.PixelFormat.Format32bppPArgb);
                pictureBox1.Image = Bmp;
                pictureBox1.BackColor = Settings.DataBackground.Color;
            }

            lock (MainPlot)
            {
                MainPlot.Render(Bmp);
            }
            pictureBox1.Invalidate();
        }

        public void Render()
        {
            NeedsRender = true;
        }

        #endregion

        #region Interactions

        private void PictureBox1_SizeChanged(object sender, EventArgs e)
        {
            Render();
        }

        private void PictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            Settings.MouseDown(e.Location.X, e.Location.Y);
        }

        private void PictureBox1_DoubleClick(object sender, EventArgs e)
        {
            MainPlot.BenchmarkToggle();
            Render();
        }

        private void PictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (IsMouseDown == false)
                return;

            if (IsMouseDownLeft)
                Settings.MousePan(e.Location.X, e.Location.Y);
            else if (IsMouseDownRight)
                Settings.MouseZoom(e.Location.X, e.Location.Y);
            else if (IsMouseDownMiddle)
                Settings.MouseZoomRect(e.Location.X, e.Location.Y);

            Render();
        }

        private void PictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Middle)
            {
                Settings.RecallAxisLimits();
                if (Settings.MouseHasMoved(e.Location.X, e.Location.Y) == false)
                {
                    Settings.ZoomRectangle.Clear();
                    MainPlot.AxisAuto();
                }
                else
                {
                    Settings.MouseZoomRect(e.Location.X, e.Location.Y, finalize: true);
                }
                Render();
            }
        }

        #endregion
    }
}
