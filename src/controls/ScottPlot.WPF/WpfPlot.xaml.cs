using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace ScottPlot
{
    [System.ComponentModel.ToolboxItem(true)]
    [System.ComponentModel.DesignTimeVisible(true)]
    public partial class WpfPlot : UserControl
    {
        public ScottPlot.Plot Plot => Backend.Plot;
        public ScottPlot.Control.Configuration Configuration => Backend.Configuration;
        public event EventHandler AxesChanged;
        public event EventHandler RightClicked;

        private readonly ScottPlot.Control.ControlBackEnd Backend;
        private readonly Dictionary<ScottPlot.Cursor, System.Windows.Input.Cursor> Cursors;
        private readonly System.Windows.Controls.Image PlotImage = new System.Windows.Controls.Image();
        private readonly DispatcherTimer PlottableCountTimer = new DispatcherTimer();

        [Obsolete("Reference Plot instead of plt")]
        public ScottPlot.Plot plt => Plot;

        public WpfPlot()
        {
            Backend = new ScottPlot.Control.ControlBackEnd((float)ActualWidth, (float)ActualHeight);
            Backend.BitmapChanged += new EventHandler(OnBitmapChanged);
            Backend.BitmapUpdated += new EventHandler(OnBitmapUpdated);
            Backend.CursorChanged += new EventHandler(OnCursorChanged);
            Backend.RightClicked += new EventHandler(OnRightClicked);
            Backend.AxesChanged += new EventHandler(OnAxesChanged);

            Cursors = new Dictionary<ScottPlot.Cursor, System.Windows.Input.Cursor>()
            {
                [ScottPlot.Cursor.Arrow] = System.Windows.Input.Cursors.Arrow,
                [ScottPlot.Cursor.WE] = System.Windows.Input.Cursors.SizeWE,
                [ScottPlot.Cursor.NS] = System.Windows.Input.Cursors.SizeNS,
                [ScottPlot.Cursor.All] = System.Windows.Input.Cursors.SizeAll,
                [ScottPlot.Cursor.Crosshair] = System.Windows.Input.Cursors.Cross,
                [ScottPlot.Cursor.Hand] = System.Windows.Input.Cursors.Hand,
                [ScottPlot.Cursor.Question] = System.Windows.Input.Cursors.Help,
            };

            RightClicked += DefaultRightClickEvent;
            PlottableCountTimer.Tick += PlottableCountTimer_Tick;
            PlottableCountTimer.Interval = new TimeSpan(0, 0, 0, 0, milliseconds: 10);
            PlottableCountTimer.Start();

            InitializeComponent();
            InitializeLayout();
        }

        public (double x, double y) GetMouseCoordinates() => Backend.GetMouseCoordinates();
        public (float x, float y) GetMousePixel() => Backend.GetMousePixel();
        public void Reset() => Backend.Reset((float)ActualWidth, (float)ActualHeight);
        public void Reset(Plot newPlot) => Backend.Reset((float)ActualWidth, (float)ActualHeight, newPlot);
        public void Render(bool lowQuality = false) => Backend.Render(lowQuality);
        private void PlottableCountTimer_Tick(object sender, EventArgs e) => Backend.RenderIfPlottableCountChanged();

        private void OnBitmapChanged(object sender, EventArgs e) => PlotImage.Source = BmpImageFromBmp(Backend.GetLatestBitmap());
        private void OnBitmapUpdated(object sender, EventArgs e) => PlotImage.Source = BmpImageFromBmp(Backend.GetLatestBitmap());
        private void OnCursorChanged(object sender, EventArgs e) => Cursor = Cursors[Backend.Cursor];
        private void OnRightClicked(object sender, EventArgs e) => RightClicked?.Invoke(sender, e);
        private void OnAxesChanged(object sender, EventArgs e) => AxesChanged?.Invoke(sender, e);
        private void OnSizeChanged(object sender, EventArgs e) => Backend.Resize((float)ActualWidth, (float)ActualHeight);

        private void OnMouseDown(object sender, MouseButtonEventArgs e) { CaptureMouse(); Backend.MouseDown(GetInputState(e)); }
        private void OnMouseUp(object sender, MouseButtonEventArgs e) { Backend.MouseUp(GetInputState(e)); ReleaseMouseCapture(); }
        private void OnDoubleClick(object sender, MouseButtonEventArgs e) => Backend.DoubleClick();
        private void OnMouseWheel(object sender, MouseWheelEventArgs e) => Backend.MouseWheel(GetInputState(e, e.Delta));
        private void OnMouseMove(object sender, MouseEventArgs e) { Backend.MouseMove(GetInputState(e)); base.OnMouseMove(e); }

        private ScottPlot.Control.IInputState GetInputState(MouseEventArgs e, double? delta = null) =>
            new ScottPlot.Control.DPICorrectedInputState(new ScottPlot.Control.InputState()
            {
                X = (float)e.GetPosition(this).X,
                Y = (float)e.GetPosition(this).Y,
                LeftWasJustPressed = e.LeftButton == MouseButtonState.Pressed,
                RightWasJustPressed = e.RightButton == MouseButtonState.Pressed,
                MiddleWasJustPressed = e.MiddleButton == MouseButtonState.Pressed,
                ShiftDown = Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift),
                CtrlDown = Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl),
                AltDown = Keyboard.IsKeyDown(Key.LeftAlt) || Keyboard.IsKeyDown(Key.RightAlt),
                WheelScrolledUp = delta.HasValue && delta > 0,
                WheelScrolledDown = delta.HasValue && delta < 0,
            }, Backend.DPIFactor);

        private void InitializeLayout()
        {
            bool isDesignerMode = DesignerProperties.GetIsInDesignMode(this);
            if (isDesignerMode)
            {
                MainGrid.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#003366"));
                var sp = new StackPanel() { Orientation = Orientation.Horizontal };
                sp.Children.Add(new Label() { Content = "ScottPlot", Foreground = Brushes.White });
                sp.Children.Add(new Label() { Content = ScottPlot.Plot.Version, Foreground = Brushes.White });
                MainGrid.Children.Add(sp);
            }
            else
            {
                var canvas = new Canvas();
                canvas.SizeChanged += OnSizeChanged;
                MainGrid.Children.Add(canvas);
                canvas.Children.Add(PlotImage);
            }
        }

        private static BitmapImage BmpImageFromBmp(System.Drawing.Bitmap bmp)
        {
            using (var memory = new System.IO.MemoryStream())
            {
                bmp.Save(memory, System.Drawing.Imaging.ImageFormat.Png);
                memory.Position = 0;

                var bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memory;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
                bitmapImage.Freeze();

                return bitmapImage;
            }
        }

        public void DefaultRightClickEvent(object sender, EventArgs e)
        {
            MenuItem SaveImageMenuItem = new MenuItem() { Header = "Save Image" };
            SaveImageMenuItem.Click += RightClickMenu_SaveImage_Click;
            MenuItem CopyImageMenuItem = new MenuItem() { Header = "Copy Image" };
            CopyImageMenuItem.Click += RightClickMenu_Copy_Click;
            MenuItem AutoAxisMenuItem = new MenuItem() { Header = "Zoom to Fit Data" };
            AutoAxisMenuItem.Click += RightClickMenu_AutoAxis_Click;
            MenuItem HelpMenuItem = new MenuItem() { Header = "Help" };
            HelpMenuItem.Click += RightClickMenu_Help_Click;

            var cm = new ContextMenu();
            cm.Items.Add(SaveImageMenuItem);
            cm.Items.Add(CopyImageMenuItem);
            cm.Items.Add(AutoAxisMenuItem);
            cm.Items.Add(HelpMenuItem);
            cm.IsOpen = true;
        }

        private void RightClickMenu_Copy_Click(object sender, EventArgs e) => System.Windows.Clipboard.SetImage(BmpImageFromBmp(Backend.GetLatestBitmap()));
        private void RightClickMenu_Help_Click(object sender, EventArgs e) => new WPF.HelpWindow().Show();
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

            if (sfd.ShowDialog() is true)
                Plot.SaveFig(sfd.FileName);
        }

    }
}
