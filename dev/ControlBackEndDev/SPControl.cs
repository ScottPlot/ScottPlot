using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;

namespace ControlBackEndDev
{
    public partial class SPControl : UserControl
    {
        private readonly ScottPlot.Control.ControlBackEnd Backend;
        private readonly Dictionary<ScottPlot.Cursor, Cursor> Cursors;

        public ScottPlot.Plot Plot => Backend.Plot;
        public ScottPlot.Control.Configuration Configuration => Backend.Configuration;
        public ContextMenuStrip RightClickMenu;

        public SPControl()
        {
            InitializeComponent();
            RightClickMenu = DefaultRightClickMenu;
            Cursors = new Dictionary<ScottPlot.Cursor, Cursor>()
            {
                [ScottPlot.Cursor.Arrow] = System.Windows.Forms.Cursors.Arrow,
                [ScottPlot.Cursor.WE] = System.Windows.Forms.Cursors.SizeWE,
                [ScottPlot.Cursor.NS] = System.Windows.Forms.Cursors.SizeNS,
                [ScottPlot.Cursor.All] = System.Windows.Forms.Cursors.SizeAll,
                [ScottPlot.Cursor.Crosshair] = System.Windows.Forms.Cursors.Cross,
                [ScottPlot.Cursor.Hand] = System.Windows.Forms.Cursors.Hand,
                [ScottPlot.Cursor.Question] = System.Windows.Forms.Cursors.Help,
            };

            Backend = new ScottPlot.Control.ControlBackEnd(Width, Height);
            Backend.BitmapChanged += new EventHandler(OnBitmapChanged);
            Backend.BitmapUpdated += new EventHandler(OnBitmapUpdated);
            Backend.CursorChanged += new EventHandler(OnCursorChanged);
            Backend.RightClicked += new EventHandler(OnRightClicked);
            pictureBox1.MouseWheel += PictureBox1_MouseWheel;
        }

        public void Render(bool lowQuality = false) => Backend.Render(lowQuality);
        private void PlottableCountTimer_Tick(object sender, EventArgs e) => Backend.RenderIfPlottableCountChanged();

        private void OnBitmapUpdated(object sender, EventArgs e) => pictureBox1.Invalidate();
        private void OnBitmapChanged(object sender, EventArgs e) => pictureBox1.Image = Backend.GetLatestBitmap();
        private void OnCursorChanged(object sender, EventArgs e) => Cursor = Cursors[Backend.Cursor];
        private void OnRightClicked(object sender, EventArgs e) => RightClickMenu.Show(Cursor.Position);
        private void OnSizeChanged(object sender, EventArgs e) => Backend.Resize(Width, Height);

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e) => Backend.MouseDown(GetInputState(e));
        private void pictureBox1_MouseUp(object sender, MouseEventArgs e) => Backend.MouseUp(GetInputState(e));
        private void pictureBox1_DoubleClick(object sender, EventArgs e) => Backend.DoubleClick();
        private void PictureBox1_MouseWheel(object sender, MouseEventArgs e) => Backend.MouseWheel(GetInputState(e), e.Delta > 0);
        private void pictureBox1_MouseMove(object sender, MouseEventArgs e) => Backend.MouseMove(GetInputState(e));

        private ScottPlot.Control.InputState GetInputState(MouseEventArgs e) =>
            new ScottPlot.Control.InputState()
            {
                X = e.X,
                Y = e.Y,
                LeftDown = e.Button == MouseButtons.Left,
                RightDown = e.Button == MouseButtons.Right,
                MiddleDown = e.Button == MouseButtons.Middle,
                ShiftDown = ModifierKeys.HasFlag(Keys.Shift),
                CtrlDown = ModifierKeys.HasFlag(Keys.Control),
                AltDown = ModifierKeys.HasFlag(Keys.Alt),
            };

        private void RightClickMenu_Copy_Click(object sender, EventArgs e) => Clipboard.SetImage(Plot.Render());
        private void RightClickMenu_Help_Click(object sender, EventArgs e) => Process.Start("https://swharden.com/scottplot");
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
