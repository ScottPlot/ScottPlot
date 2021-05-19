using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;

#pragma warning disable IDE1006 // ignore warning about lowercase 'plt' property

namespace ScottPlot
{
    public partial class FormsPlot : UserControl
    {
        /// <summary>
        /// This is the plot displayed by the user control.
        /// After modifying it you may need to call Render() to request the plot be redrawn on the screen.
        /// </summary>
        public Plot Plot => Backend.Plot;

        /// <summary>
        /// This object can be used to modify advanced behaior and customization of this user control.
        /// </summary>
        public Control.Configuration Configuration => Backend.Configuration;

        /// <summary>
        /// This event is invoked any time the axis limits are modified.
        /// </summary>
        public event EventHandler AxesChanged;

        /// <summary>
        /// This event is invoked any time the plot is right-clicked.
        /// By default it contains DefaultRightClickEvent(), but you can remove this and add your own method.
        /// </summary>
        public event EventHandler RightClicked;

        private readonly Control.ControlBackEnd Backend = new(1, 1);
        private readonly Dictionary<Cursor, System.Windows.Forms.Cursor> Cursors;
        private readonly bool IsDesignerMode = Process.GetCurrentProcess().ProcessName == "devenv";

        [Obsolete("Reference 'Plot' instead of 'plt'")]
        public Plot plt => Plot;

        public FormsPlot()
        {
            Backend.Resize(Width, Height);
            Backend.BitmapChanged += new EventHandler(OnBitmapChanged);
            Backend.BitmapUpdated += new EventHandler(OnBitmapUpdated);
            Backend.CursorChanged += new EventHandler(OnCursorChanged);
            Backend.RightClicked += new EventHandler(OnRightClicked);
            Backend.AxesChanged += new EventHandler(OnAxesChanged);

            Cursors = new Dictionary<Cursor, System.Windows.Forms.Cursor>()
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
            Plot.Style(figureBackground: BackColor);
            pictureBox1.MouseWheel += PictureBox1_MouseWheel;
            RightClicked += DefaultRightClickEvent;
            if (IsDesignerMode)
                Plot.Title($"ScottPlot {Plot.Version}");

            Backend.StartProcessingEvents();
        }

        /// <summary>
        /// Return the mouse position on the plot (in coordinate space) for the latest X and Y coordinates
        /// </summary>
        public (double x, double y) GetMouseCoordinates() => Backend.GetMouseCoordinates();

        /// <summary>
        /// Return the mouse position (in pixel space) for the last observed mouse position
        /// </summary>
        public (float x, float y) GetMousePixel() => Backend.GetMousePixel();

        /// <summary>
        /// Reset this control by replacing the current plot with a new empty plot
        /// </summary>
        public void Reset() => Backend.Reset(Width, Height);

        /// <summary>
        /// Reset this control by replacing the current plot with an existing plot
        /// </summary>
        public void Reset(Plot newPlot) => Backend.Reset(Width, Height, newPlot);

        /// <summary>
        /// Re-render the plot and update the image displayed by this control.
        /// </summary>
        /// <param name="lowQuality">disable anti-aliasing to produce faster (but lower quality) plots</param>
        /// <param name="skipIfCurrentlyRendering"></param>
        public void Render(bool lowQuality = false, bool skipIfCurrentlyRendering = false)
        {
            Application.DoEvents();
            Backend.Render(lowQuality, skipIfCurrentlyRendering);
        }

        /// <summary>
        /// Render the plot using low quality (fast) then immediate re-render using high quality (slower)
        /// </summary>
        public void RenderLowThenImmediateHighQuality() => Backend.RenderLowThenImmediateHighQuality();

        private void PlottableCountTimer_Tick(object sender, EventArgs e) => Backend.RenderIfPlottableCountChanged();
        private void FormsPlot_Load(object sender, EventArgs e) { OnSizeChanged(null, null); }
        private void OnBitmapUpdated(object sender, EventArgs e) { Application.DoEvents(); pictureBox1.Invalidate(); }
        private void OnBitmapChanged(object sender, EventArgs e) { pictureBox1.Image = Backend.GetLatestBitmap(); }
        private void OnCursorChanged(object sender, EventArgs e) => Cursor = Cursors[Backend.Cursor];
        private void OnSizeChanged(object sender, EventArgs e) => Backend.Resize(Width, Height);
        private void OnAxesChanged(object sender, EventArgs e) => AxesChanged?.Invoke(this, e);
        private void OnRightClicked(object sender, EventArgs e) => RightClicked?.Invoke(this, e);
        private void PictureBox1_MouseDown(object sender, MouseEventArgs e) { Backend.MouseDown(GetInputState(e)); base.OnMouseDown(e); }
        private void PictureBox1_MouseUp(object sender, MouseEventArgs e) { Backend.MouseUp(GetInputState(e)); base.OnMouseUp(e); }
        private void PictureBox1_DoubleClick(object sender, EventArgs e) { Backend.DoubleClick(); base.OnDoubleClick(e); }
        private void PictureBox1_MouseWheel(object sender, MouseEventArgs e) { Backend.MouseWheel(GetInputState(e)); base.OnMouseWheel(e); }
        private void PictureBox1_MouseMove(object sender, MouseEventArgs e) { Backend.MouseMove(GetInputState(e)); base.OnMouseMove(e); }
        private void PictureBox1_MouseEnter(object sender, EventArgs e) => base.OnMouseEnter(e);
        private void PictureBox1_MouseLeave(object sender, EventArgs e) => base.OnMouseLeave(e);

        private Control.InputState GetInputState(MouseEventArgs e) =>
            new()
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

        /// <summary>
        /// Launch the default right-click menu.
        /// </summary>
        public void DefaultRightClickEvent(object sender, EventArgs e) => DefaultRightClickMenu.Show(System.Windows.Forms.Cursor.Position);
        private void RightClickMenu_Copy_Click(object sender, EventArgs e) => Clipboard.SetImage(Plot.Render());
        private void RightClickMenu_Help_Click(object sender, EventArgs e) => new FormHelp().Show();
        private void RightClickMenu_AutoAxis_Click(object sender, EventArgs e) { Plot.AxisAuto(); Render(); }
        private void RightClickMenu_OpenInNewWindow_Click(object sender, EventArgs e) => new FormsPlotViewer(Plot).Show();
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
