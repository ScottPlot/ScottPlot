using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;

namespace ScottPlot
{
    public partial class FormsPlot : UserControl
    {
        public ScottPlot.Plot Plot => Backend.Plot;
        public ScottPlot.Control.Configuration Configuration => Backend.Configuration;
        public event EventHandler AxesChanged;
        public event EventHandler RightClicked;
        private readonly ScottPlot.Control.ControlBackEnd Backend;
        private readonly Dictionary<ScottPlot.Cursor, System.Windows.Forms.Cursor> Cursors;

        [Obsolete("Reference Plot instead of plt")]
        public ScottPlot.Plot plt => Plot;

        public FormsPlot()
        {
            // TODO: something different in designer mode
            Backend = new ScottPlot.Control.ControlBackEnd(Width, Height);
            Backend.BitmapChanged += new EventHandler(OnBitmapChanged);
            Backend.BitmapUpdated += new EventHandler(OnBitmapUpdated);
            Backend.CursorChanged += new EventHandler(OnCursorChanged);
            Backend.RightClicked += new EventHandler(OnRightClicked);
            Backend.AxesChanged += new EventHandler(OnAxesChanged);

            Cursors = new Dictionary<ScottPlot.Cursor, System.Windows.Forms.Cursor>()
            {
                [ScottPlot.Cursor.Arrow] = System.Windows.Forms.Cursors.Arrow,
                [ScottPlot.Cursor.WE] = System.Windows.Forms.Cursors.SizeWE,
                [ScottPlot.Cursor.NS] = System.Windows.Forms.Cursors.SizeNS,
                [ScottPlot.Cursor.All] = System.Windows.Forms.Cursors.SizeAll,
                [ScottPlot.Cursor.Crosshair] = System.Windows.Forms.Cursors.Cross,
                [ScottPlot.Cursor.Hand] = System.Windows.Forms.Cursors.Hand,
                [ScottPlot.Cursor.Question] = System.Windows.Forms.Cursors.Help,
            };

            InitializeComponent();
            pictureBox1.BackColor = System.Drawing.Color.Transparent;
            BackColor = System.Drawing.Color.Transparent;
            pictureBox1.MouseWheel += PictureBox1_MouseWheel;
            RightClicked += DefaultRightClickEvent;
        }

        public (double x, double y) GetMouseCoordinates() => Backend.GetMouseCoordinates();
        public (float x, float y) GetMousePixel() => Backend.GetMousePixel();
        public void Reset() => Backend.Reset(Width, Height);
        public void Reset(Plot newPlot) => Backend.Reset(Width, Height, newPlot);
        private void PlottableCountTimer_Tick(object sender, EventArgs e) => Backend.RenderIfPlottableCountChanged();
        public void Render(bool lowQuality = false, bool skipIfCurrentlyRendering = false)
        {
            // TODO: if "skipIfCurrentlyRendering", setup a timer to render later
            Application.DoEvents();
            Backend.Render(lowQuality, skipIfCurrentlyRendering);
        }

        private void OnBitmapUpdated(object sender, EventArgs e) { Application.DoEvents(); pictureBox1.Invalidate(); }
        private void OnBitmapChanged(object sender, EventArgs e) { pictureBox1.Image = Backend.GetLatestBitmap(); }
        private void OnCursorChanged(object sender, EventArgs e) => Cursor = Cursors[Backend.Cursor];
        private void OnSizeChanged(object sender, EventArgs e) => Backend.Resize(Width, Height);
        private void OnAxesChanged(object sender, EventArgs e) => AxesChanged?.Invoke(sender, e);
        private void OnRightClicked(object sender, EventArgs e) => RightClicked?.Invoke(sender, e);

        private void PictureBox1_MouseDown(object sender, MouseEventArgs e) => Backend.MouseDown(GetInputState(e));
        private void PictureBox1_MouseUp(object sender, MouseEventArgs e) => Backend.MouseUp(GetInputState(e));
        private void PictureBox1_DoubleClick(object sender, EventArgs e) => Backend.DoubleClick();
        private void PictureBox1_MouseWheel(object sender, MouseEventArgs e) => Backend.MouseWheel(GetInputState(e));
        private void PictureBox1_MouseMove(object sender, MouseEventArgs e) { Backend.MouseMove(GetInputState(e)); base.OnMouseMove(e); }

        private ScottPlot.Control.InputState GetInputState(MouseEventArgs e) =>
            new ScottPlot.Control.InputState()
            {
                X = e.X,
                Y = e.Y,
                LeftWasJustPressed = e.Button == MouseButtons.Left,
                RightWasJustPressed = e.Button == MouseButtons.Right,
                MiddleWasJustPressed = e.Button == MouseButtons.Middle,
                ShiftDown = ModifierKeys.HasFlag(Keys.Shift),
                CtrlDown = ModifierKeys.HasFlag(Keys.Control),
                AltDown = ModifierKeys.HasFlag(Keys.Alt),
                WheelScrolledUp = e.Delta > 0,
                WheelScrolledDown = e.Delta < 0,
            };

        public void DefaultRightClickEvent(object sender, EventArgs e) => DefaultRightClickMenu.Show(System.Windows.Forms.Cursor.Position);
        private void RightClickMenu_Copy_Click(object sender, EventArgs e) => Clipboard.SetImage(Plot.Render());
        private void RightClickMenu_Help_Click(object sender, EventArgs e) => new FormHelp().Show();
        private void RightClickMenu_AutoAxis_Click(object sender, EventArgs e) { Plot.AxisAuto(); Render(); }
        private void RightClickMenu_SaveImage_Click(object sender, EventArgs e)
        {
            var sfd = new SaveFileDialog
            {
                FileName = "ScottPlot.png",
                Filter = "PNG Files (*.png)|*.png;*.png" +
                         "|JPG Files (*.jpg, *.jpeg)|*.jpg;*.jpeg" +
                         "|BMP Files (*.bmp)|*.bmp;*.bmp" +
                         "|All files (*.*)|*.*"
            };

            if (sfd.ShowDialog() == DialogResult.OK)
                Plot.SaveFig(sfd.FileName);
        }
    }
}
