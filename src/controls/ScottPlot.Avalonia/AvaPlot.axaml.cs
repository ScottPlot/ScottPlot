using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using Avalonia.VisualTree;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ava = Avalonia;

#pragma warning disable IDE1006 // lowercase top-level property

namespace ScottPlot.Avalonia
{
    /// <summary>
    /// Interaction logic for AvaPlot.axaml
    /// </summary>

    [System.ComponentModel.ToolboxItem(true)]
    [System.ComponentModel.DesignTimeVisible(true)]
    public partial class AvaPlot : UserControl
    {
        public Plot Plot => Backend.Plot;
        public ScottPlot.Control.Configuration Configuration => Backend.Configuration;

        /// <summary>
        /// This event is invoked any time the axis limits are modified.
        /// </summary>
        public event EventHandler AxesChanged;

        /// <summary>
        /// This event is invoked any time the plot is right-clicked.
        /// By default it contains DefaultRightClickEvent(), but you can remove this and add your own method.
        /// </summary>
        public event EventHandler RightClicked;

        private readonly Control.ControlBackEnd Backend;
        private readonly Dictionary<ScottPlot.Cursor, Ava.Input.Cursor> Cursors;
        private readonly Ava.Controls.Image PlotImage = new Ava.Controls.Image();
        private readonly DispatcherTimer PlottableCountTimer = new DispatcherTimer();

        [Obsolete("Reference Plot instead of plt")]
        public ScottPlot.Plot plt => Plot;

        public AvaPlot()
        {
            InitializeComponent();

            Cursors = new Dictionary<ScottPlot.Cursor, Ava.Input.Cursor>()
            {
                [ScottPlot.Cursor.Arrow] = new Ava.Input.Cursor(StandardCursorType.Arrow),
                [ScottPlot.Cursor.WE] = new Ava.Input.Cursor(StandardCursorType.SizeWestEast),
                [ScottPlot.Cursor.NS] = new Ava.Input.Cursor(StandardCursorType.SizeNorthSouth),
                [ScottPlot.Cursor.All] = new Ava.Input.Cursor(StandardCursorType.SizeAll),
                [ScottPlot.Cursor.Crosshair] = new Ava.Input.Cursor(StandardCursorType.Cross),
                [ScottPlot.Cursor.Hand] = new Ava.Input.Cursor(StandardCursorType.Hand),
                [ScottPlot.Cursor.Question] = new Ava.Input.Cursor(StandardCursorType.Help),
            };

            Backend = new ScottPlot.Control.ControlBackEnd((float)this.Bounds.Width, (float)this.Bounds.Height);
            Backend.BitmapChanged += new EventHandler(OnBitmapChanged);
            Backend.BitmapUpdated += new EventHandler(OnBitmapUpdated);
            Backend.CursorChanged += new EventHandler(OnCursorChanged);
            Backend.RightClicked += new EventHandler(OnRightClicked);
            Backend.AxesChanged += new EventHandler(OnAxesChanged);

            RightClicked += DefaultRightClickEvent;
            PlottableCountTimer.Tick += PlottableCountTimer_Tick;
            PlottableCountTimer.Interval = new TimeSpan(0, 0, 0, 0, milliseconds: 10);
            PlottableCountTimer.Start();

            InitializeLayout();
            Backend.StartProcessingEvents();
        }

        public (double x, double y) GetMouseCoordinates() => Backend.GetMouseCoordinates();
        public (float x, float y) GetMousePixel() => Backend.GetMousePixel();
        public void Reset() => Backend.Reset((float)this.Bounds.Width, (float)this.Bounds.Height);
        public void Reset(Plot newPlot) => Backend.Reset((float)this.Bounds.Width, (float)this.Bounds.Height, newPlot);
        public void Render(bool lowQuality = false) => Backend.Render(lowQuality);
        private void PlottableCountTimer_Tick(object sender, EventArgs e) => Backend.RenderIfPlottableCountChanged();

        private Task SetImagePlot(Func<Ava.Media.Imaging.Bitmap> getBmp)
        {
            return Task.Run(() =>
            {
                Dispatcher.UIThread.InvokeAsync(() =>
                {
                    PlotImage.Source = getBmp();
                });
            });
        }

        private void OnBitmapChanged(object sender, EventArgs e) => SetImagePlot(() => BmpImageFromBmp(Backend.GetLatestBitmap()));
        private void OnCursorChanged(object sender, EventArgs e) { PlotImage.Cursor = Cursors[Backend.Cursor]; }
        private void OnBitmapUpdated(object sender, EventArgs e) => SetImagePlot(() => BmpImageFromBmp(Backend.GetLatestBitmap()));
        private void OnRightClicked(object sender, EventArgs e) => RightClicked?.Invoke(this, e);
        private void OnAxesChanged(object sender, EventArgs e) => AxesChanged?.Invoke(this, e);
        private void OnSizeChanged(object sender, EventArgs e) => Backend.Resize((float)this.Bounds.Width, (float)this.Bounds.Height);
        private void OnMouseDown(object sender, PointerEventArgs e) { CaptureMouse(e.Pointer); Backend.MouseDown(GetInputState(e)); }
        private void OnMouseUp(object sender, PointerEventArgs e) { Backend.MouseUp(GetInputState(e)); UncaptureMouse(e.Pointer); }
        private void OnDoubleClick(object sender, PointerEventArgs e) => Backend.DoubleClick();
        private void OnMouseWheel(object sender, PointerWheelEventArgs e) => Backend.MouseWheel(GetInputState(e, e.Delta.Y));
        private void OnMouseMove(object sender, PointerEventArgs e) { Backend.MouseMove(GetInputState(e)); base.OnPointerMoved(e); }
        private void OnMouseEnter(object sender, PointerEventArgs e) => base.OnPointerEnter(e);
        private void OnMouseLeave(object sender, PointerEventArgs e) => base.OnPointerLeave(e);
        private void CaptureMouse(IPointer pointer) => pointer.Capture(this);
        private void UncaptureMouse(IPointer pointer) => pointer.Capture(null);

        private ScottPlot.Control.InputState GetInputState(PointerEventArgs e, double? delta = null) =>
            new ScottPlot.Control.InputState()
            {
                X = (float)e.GetPosition(this).X,
                Y = (float)e.GetPosition(this).Y,
                LeftWasJustPressed = e.GetCurrentPoint(null).Properties.PointerUpdateKind == PointerUpdateKind.LeftButtonPressed,
                RightWasJustPressed = e.GetCurrentPoint(null).Properties.PointerUpdateKind == PointerUpdateKind.RightButtonPressed,
                MiddleWasJustPressed = e.GetCurrentPoint(null).Properties.PointerUpdateKind == PointerUpdateKind.MiddleButtonPressed,
                ShiftDown = e.KeyModifiers.HasFlag(KeyModifiers.Shift),
                CtrlDown = e.KeyModifiers.HasFlag(KeyModifiers.Control),
                AltDown = e.KeyModifiers.HasFlag(KeyModifiers.Alt),
                WheelScrolledUp = delta.HasValue && delta > 0,
                WheelScrolledDown = delta.HasValue && delta < 0,
            };

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
            this.Focusable = true;

            PointerPressed += OnMouseDown;
            PointerMoved += OnMouseMove;
            PointerReleased += OnMouseUp;
            PointerWheelChanged += OnMouseWheel;
            PointerEnter += OnMouseEnter;
            PointerLeave += OnMouseLeave;

            PropertyChanged += AvaPlot_PropertyChanged;
        }

        private void InitializeLayout()
        {
            Grid mainGrid = this.Find<Grid>("MainGrid");

            bool isDesignerMode = Design.IsDesignMode;
            if (isDesignerMode)
            {
                mainGrid.Background = new Ava.Media.SolidColorBrush(Ava.Media.Color.FromArgb(0xff, 0, 0x33, 0x66));
                StackPanel sp = new StackPanel() { Orientation = Ava.Layout.Orientation.Horizontal };
                sp.Children.Add(new Label() { Content = "ScottPlot", Foreground = Ava.Media.Brushes.White });
                sp.Children.Add(new Label() { Content = Plot.Version, Foreground = Ava.Media.Brushes.White });

                mainGrid.Children.Add(sp);
            }
            else
            {
                Canvas canvas = new Canvas();
                mainGrid.Children.Add(canvas);
                canvas.Children.Add(PlotImage);
            }
        }

        public static Ava.Media.Imaging.Bitmap BmpImageFromBmp(System.Drawing.Bitmap bmp)
        {
            using var memory = new System.IO.MemoryStream();
            bmp.Save(memory, System.Drawing.Imaging.ImageFormat.Png);
            memory.Position = 0;
            var bitmapImage = new Ava.Media.Imaging.Bitmap(memory);
            return bitmapImage;
        }

        private ContextMenu GetDefaultContextMenu()
        {
            MenuItem SaveImageMenuItem = new MenuItem() { Header = "Save Image" };
            SaveImageMenuItem.Click += RightClickMenu_SaveImage_Click;

            /* Copying binary data to the clipboard remains OS-specific,
             * so it is intentionally not supported at this time.
             * https://github.com/AvaloniaUI/Avalonia/issues/3588
             */
            MenuItem CopyImageMenuItem = new MenuItem() { Header = "Copy Image" };
            CopyImageMenuItem.Click += RightClickMenu_Copy_Click;

            MenuItem AutoAxisMenuItem = new MenuItem() { Header = "Zoom to Fit Data" };
            AutoAxisMenuItem.Click += RightClickMenu_AutoAxis_Click;

            MenuItem HelpMenuItem = new MenuItem() { Header = "Help" };
            HelpMenuItem.Click += RightClickMenu_Help_Click;

            MenuItem OpenInNewWindowMenuItem = new() { Header = "Open in New Window" };
            OpenInNewWindowMenuItem.Click += RightClickMenu_OpenInNewWindow_Click;

            var cm = new ContextMenu();
            List<MenuItem> cmItems = new List<MenuItem>
            {
                SaveImageMenuItem,
                //CopyImageMenuItem,
                AutoAxisMenuItem,
                HelpMenuItem,
                OpenInNewWindowMenuItem
            };
            cm.Items = cmItems;
            return cm;
        }

        public void DefaultRightClickEvent(object sender, EventArgs e) => GetDefaultContextMenu().Open(this);
        private void RightClickMenu_Copy_Click(object sender, EventArgs e) => throw new NotImplementedException();
        private void RightClickMenu_Help_Click(object sender, EventArgs e) => new HelpWindow().Show();
        private void RightClickMenu_AutoAxis_Click(object sender, EventArgs e) { Plot.AxisAuto(); Render(); }
        private async void RightClickMenu_SaveImage_Click(object sender, EventArgs e)
        {
            SaveFileDialog savefile = new SaveFileDialog { InitialFileName = "ScottPlot.png" };

            var filtersPNG = new FileDialogFilter { Name = "PNG Files" };
            filtersPNG.Extensions.Add("png");

            var filtersJPEG = new FileDialogFilter { Name = "JPG Files" };
            filtersJPEG.Extensions.Add("jpg");
            filtersJPEG.Extensions.Add("jpeg");

            var filtersBMP = new FileDialogFilter { Name = "BMP Files" };
            filtersBMP.Extensions.Add("bmp");

            var filtersTIFF = new FileDialogFilter { Name = "TIF Files" };
            filtersTIFF.Extensions.Add("tif");
            filtersTIFF.Extensions.Add("tiff");

            var filtersGeneric = new FileDialogFilter { Name = "All Files" };
            filtersGeneric.Extensions.Add("*");

            savefile.Filters.Add(filtersPNG);
            savefile.Filters.Add(filtersJPEG);
            savefile.Filters.Add(filtersBMP);
            savefile.Filters.Add(filtersTIFF);
            savefile.Filters.Add(filtersGeneric);


            Task<string> filenameTask = savefile.ShowAsync((Window)this.GetVisualRoot());
            await filenameTask;

            if (filenameTask.Exception != null)
            {
                return;
            }

            if ((filenameTask.Result ?? "") != "")
                Plot.SaveFig(filenameTask.Result);
        }
        private void RightClickMenu_OpenInNewWindow_Click(object sender, EventArgs e) { new AvaPlotViewer(Plot).Show(); }

        private void AvaPlot_PropertyChanged(object sender, AvaloniaPropertyChangedEventArgs e)
        {
            if (e.Property.Name == "Bounds")
            {
                Backend.Resize((float)this.Bounds.Width, (float)this.Bounds.Height);
                Render();
            }
        }
    }
}
