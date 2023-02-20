using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using Avalonia.VisualTree;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Ava = Avalonia;

#pragma warning disable IDE1006 // lowercase public properties
#pragma warning disable CS0067 // unused events

namespace ScottPlot.Avalonia
{
    /// <summary>
    /// Interaction logic for AvaPlot.axaml
    /// </summary>

    [System.ComponentModel.ToolboxItem(true)]
    [System.ComponentModel.DesignTimeVisible(true)]
    public partial class AvaPlot : UserControl, ScottPlot.Control.IPlotControl
    {
        public Plot Plot => Backend.Plot;
        public ScottPlot.Control.Configuration Configuration { get; }

        /// <summary>
        /// This event is invoked any time the axis limits are modified.
        /// </summary>
        public event EventHandler AxesChanged;

        /// <summary>
        /// This event is invoked any time the plot is right-clicked.
        /// </summary>
        public event EventHandler RightClicked;

        /// <summary>
        /// This event is invoked any time the plot is left-clicked.
        /// It is typically used to interact with custom plot types.
        /// </summary>
        public event EventHandler LeftClicked;

        /// <summary>
        /// This event is invoked when a <seealso cref="Plottable.IHittable"/> plottable is left-clicked.
        /// </summary>
        public event EventHandler LeftClickedPlottable;

        /// <summary>
        /// This event is invoked after the mouse moves while dragging a draggable plottable.
        /// The object passed is the plottable being dragged.
        /// </summary>
        public event EventHandler PlottableDragged;

        [Obsolete("use 'PlottableDragged' instead", error: true)]
        public event EventHandler MouseDragPlottable;

        /// <summary>
        /// This event is invoked right after a draggable plottable was dropped.
        /// The object passed is the plottable that was just dropped.
        /// </summary>
        public event EventHandler PlottableDropped;

        [Obsolete("use 'PlottableDropped' instead", error: true)]
        public event EventHandler MouseDropPlottable;

        private readonly Control.ControlBackEnd Backend;
        private readonly Dictionary<ScottPlot.Cursor, Ava.Input.Cursor> Cursors;
        private readonly Ava.Controls.Image PlotImage = new Ava.Controls.Image();
        private float ScaledWidth => (float)(Bounds.Width * Configuration.DpiStretchRatio);
        private float ScaledHeight => (float)(Bounds.Height * Configuration.DpiStretchRatio);

        [Obsolete("Reference Plot instead of plt")]
        public ScottPlot.Plot plt => Plot;

        static AvaPlot()
        {
            ContextMenuProperty.Changed.AddClassHandler<AvaPlot>((plot, args) => plot.ContextMenuChanged(args));
        }

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

            Backend = new ScottPlot.Control.ControlBackEnd((float)Bounds.Width, (float)Bounds.Height, GetType().Name);
            Backend.BitmapChanged += new EventHandler(OnBitmapChanged);
            Backend.BitmapUpdated += new EventHandler(OnBitmapUpdated);
            Backend.CursorChanged += new EventHandler(OnCursorChanged);
            Backend.RightClicked += new EventHandler(OnRightClicked);
            Backend.LeftClicked += new EventHandler(OnLeftClicked);
            Backend.LeftClickedPlottable += new EventHandler(OnLeftClickedPlottable);
            Backend.AxesChanged += new EventHandler(OnAxesChanged);
            Backend.PlottableDragged += new EventHandler(OnPlottableDragged);
            Backend.PlottableDropped += new EventHandler(OnPlottableDropped);
            Backend.Configuration.ScaleChanged += new EventHandler(OnScaleChanged);
            Configuration = Backend.Configuration;

            ContextMenu = GetDefaultContextMenu();

            InitializeLayout();
            Backend.StartProcessingEvents();
        }

        public (double x, double y) GetMouseCoordinates(int xAxisIndex = 0, int yAxisIndex = 0) => Backend.GetMouseCoordinates(xAxisIndex, yAxisIndex);

        public (float x, float y) GetMousePixel() => Backend.GetMousePixel();
        public void Reset() => Backend.Reset(ScaledWidth, ScaledHeight);
        public void Reset(Plot newPlot) => Backend.Reset(ScaledWidth, ScaledHeight, newPlot);
        public void Refresh() => Refresh(false);
        public void Refresh(bool lowQuality = false)
        {
            Backend.WasManuallyRendered = true;
            Backend.Render(lowQuality);
        }
        public void RefreshRequest(RenderType renderType = RenderType.LowQualityThenHighQualityDelayed)
        {
            Backend.WasManuallyRendered = true;
            Backend.RenderRequest(renderType);
        }

        // TODO: mark this obsolete in ScottPlot 5.0 (favor Refresh)
        public void Render(bool lowQuality = false) => Refresh(lowQuality);

        // TODO: mark this obsolete in ScottPlot 5.0 (favor Refresh)
        public void RenderRequest(RenderType renderType = RenderType.LowQualityThenHighQualityDelayed) => RefreshRequest(renderType);

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
        private void OnLeftClicked(object sender, EventArgs e) => LeftClicked?.Invoke(this, e);
        private void OnLeftClickedPlottable(object sender, EventArgs e) => LeftClickedPlottable?.Invoke(sender, e);
        private void OnPlottableDragged(object sender, EventArgs e) => PlottableDragged?.Invoke(sender, e);
        private void OnPlottableDropped(object sender, EventArgs e) => PlottableDropped?.Invoke(sender, e);
        private void OnAxesChanged(object sender, EventArgs e) => AxesChanged?.Invoke(this, e);
        private void OnSizeChanged(object sender, EventArgs e) => Backend.Resize(ScaledWidth, ScaledHeight, useDelayedRendering: true);
        private void OnScaleChanged(object sender, EventArgs e) { System.Diagnostics.Debug.WriteLine("SCALECHANGED"); OnSizeChanged(null, null); }
        private void OnMouseDown(object sender, PointerEventArgs e) { CaptureMouse(e.Pointer); Backend.MouseDown(GetInputState(e)); }
        private void OnMouseUp(object sender, PointerEventArgs e) { Backend.MouseUp(GetInputState(e)); UncaptureMouse(e.Pointer); }
        private void OnDoubleClick(object sender, RoutedEventArgs e) => Backend.DoubleClick();
        private void OnMouseWheel(object sender, PointerWheelEventArgs e) => Backend.MouseWheel(GetInputState(e, e.Delta.Y));
        private void OnMouseMove(object sender, PointerEventArgs e) { Backend.MouseMove(GetInputState(e)); base.OnPointerMoved(e); }
        private void OnMouseEnter(object sender, PointerEventArgs e) => base.OnPointerEnter(e);
        private void OnMouseLeave(object sender, PointerEventArgs e) => base.OnPointerLeave(e);
        private void CaptureMouse(IPointer pointer) => pointer.Capture(this);
        private void UncaptureMouse(IPointer pointer) => pointer.Capture(null);

        private ScottPlot.Control.InputState GetInputState(PointerEventArgs e, double? delta = null) =>
            new ScottPlot.Control.InputState()
            {
                X = (float)e.GetPosition(this).X * Configuration.DpiStretchRatio,
                Y = (float)e.GetPosition(this).Y * Configuration.DpiStretchRatio,
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
            // Note: PointerReleased is handled in OnPointerReleased override instead
            PointerWheelChanged += OnMouseWheel;
            PointerEnter += OnMouseEnter;
            PointerLeave += OnMouseLeave;
            DoubleTapped += OnDoubleClick;
            PropertyChanged += AvaPlot_PropertyChanged;
        }

        private void InitializeLayout()
        {
            Grid mainGrid = this.Find<Grid>("MainGrid");

            bool isDesignerMode = Design.IsDesignMode;
            if (isDesignerMode)
            {
                try
                {
                    Plot.Title($"ScottPlot {Plot.Version}");
                    Plot.Render();
                }
                catch (Exception e)
                {
                    InitializeComponent();
                    this.Find<TextBlock>("ErrorLabel").Text = "ERROR: ScottPlot failed to render in design mode.\n\n" +
                        "This may be due to incompatible System.Drawing.Common versions or a 32-bit/64-bit mismatch.\n\n" +
                        "Although rendering failed at design time, it may still function normally at runtime.\n\n" +
                        $"Exception details:\n{e}";
                    return;
                }
            }

            this.Find<TextBlock>("ErrorLabel").IsVisible = false;

            Canvas canvas = new Canvas();
            mainGrid.Children.Add(canvas);
            canvas.Children.Add(PlotImage);
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

        private void ContextMenuChanged(AvaloniaPropertyChangedEventArgs e)
        {
            // Make sure that any context menus that get assigned do not 
            // display when the right mouse button is released if the mouse
            // was dragged while the button was down

            if (e.OldValue is ContextMenu oldMenu)
            {
                oldMenu.ContextMenuOpening -= InhibitContextMenuIfMouseDragged;
            }
            if (e.NewValue is ContextMenu newMenu)
            {
                newMenu.ContextMenuOpening += InhibitContextMenuIfMouseDragged;
            }
        }

        private void InhibitContextMenuIfMouseDragged(object sender, CancelEventArgs e)
        {
            e.Cancel = Backend.MouseDownDragged;
        }

        protected override void OnPointerReleased(PointerReleasedEventArgs e)
        {
            // First, make sure backend sees that we are no longer pressing mouse button.
            // Otherwise, after selecting an item from the context menu, the control
            // will still think we are right-click-dragging even though the button
            // is no longer down.
            OnMouseUp(this, e);

            // Then allow Avalonia's own click handling to allow the context menu
            // to be displayed if needed.
            base.OnPointerReleased(e);
        }

        /// <summary>
        /// This default handler is no longer used to display a context menu, but is kept 
        /// around for backwards compatibility with existing code that expects to remove 
        /// this handler before adding custom right-click handling.
        /// </summary>
        [Obsolete("use 'ContextMenu' property instead to assign custom context menus")]
        public void DefaultRightClickEvent(object sender, EventArgs e) { }
        private void RightClickMenu_Copy_Click(object sender, EventArgs e) => throw new NotImplementedException();
        private void RightClickMenu_Help_Click(object sender, EventArgs e) => new HelpWindow().Show();
        private void RightClickMenu_AutoAxis_Click(object sender, EventArgs e) { Plot.AxisAuto(); Refresh(); }
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
                Backend.Resize(ScaledWidth, ScaledHeight, useDelayedRendering: true);
                PlotImage.Width = ScaledWidth;
                PlotImage.Height = ScaledHeight;
                Refresh();
            }
        }
    }
}
