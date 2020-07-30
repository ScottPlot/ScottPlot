using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.VisualTree;
using ScottPlot.Avalonia;
using ScottPlot.Config;
using Avalonia.Platform;

using Ava = Avalonia;

namespace ScottPlot.Avalonia
{
    /// <summary>
    /// Interaction logic for AvaPlot.axaml
    /// </summary>

    [System.ComponentModel.ToolboxItem(true)]
    [System.ComponentModel.DesignTimeVisible(true)]
    public partial class AvaPlot : UserControl
    {
        public Plot plt { get; private set; }
        private Settings settings;
        private bool isDesignerMode;
        private double dpiScaleInput = 1;
        private double dpiScaleOutput = 1;
        private readonly SolidColorBrush transparentBrush = new SolidColorBrush(Ava.Media.Color.FromUInt32(0), 0);

        public AvaPlot(Plot plt)
        {
            InitializeComponent();
            ContextMenu = DefaultRightClickMenu();
            Reset(plt);
        }

        public AvaPlot()
        {
            InitializeComponent();
            ContextMenu = DefaultRightClickMenu();
            Reset(null);
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
            this.Focusable = true;

            PointerPressed += UserControl_MouseDown;
            PointerMoved += UserControl_MouseMove;
            PointerReleased += UserControl_MouseUp;
            KeyDown += OnKeyDown;
            KeyUp += OnKeyUp;
            PointerWheelChanged += UserControl_MouseWheel;
        }

        private ContextMenu DefaultRightClickMenu()
        {
            MenuItem SaveImageMenuItem = new MenuItem() { Header = "Save Image" };
            SaveImageMenuItem.Click += SaveImage;
            MenuItem NewWindowMenuItem = new MenuItem() { Header = "Open in New Window" };
            NewWindowMenuItem.Click += OpenInNewWindow;
            MenuItem HelpMenuItem = new MenuItem() { Header = "Help" };
            HelpMenuItem.Click += OpenHelp;

            var cm = new ContextMenu();
            var backingList = new List<MenuItem>();
            backingList.Add(SaveImageMenuItem);
            backingList.Add(NewWindowMenuItem);
            backingList.Add(HelpMenuItem);

            cm.Items = backingList;

            return cm;
        }

        public void Reset()
        {
            Reset(null);
        }

        public void Reset(Plot plt)
        {
            this.plt = (plt is null) ? new Plot() : plt;
            InitializeScottPlot();
            Render();
        }

        private void InitializeScottPlot()
        {
            this.Find<TextBlock>("lblVersion").Text = Tools.GetVersionString();
            //isDesignerMode = DesignerProperties.GetIsInDesignMode(this);
            isDesignerMode = false;

            settings = plt.GetSettings(showWarning: false);

            var mainGrid = this.Find<Ava.Controls.Grid>("mainGrid");

            if (isDesignerMode)
            {
                // hide the plot
                mainGrid.RowDefinitions[1].Height = new GridLength(0);
            }
            else
            {
                // hide the version info
                mainGrid.RowDefinitions[0].Height = new GridLength(0);
                //CanvasPlot_SizeChanged(null, null);
                //dpiScaleInput = settings.gfxFigure.DpiX / 96; THIS IS ONLY NECESSARY ON WPF
                dpiScaleOutput = settings.gfxFigure.DpiX / 96;
                this.Find<StackPanel>("canvasDesigner").Background = transparentBrush;
                this.Find<Canvas>("canvasPlot").Background = transparentBrush;
            }
        }

        private static Ava.Media.Imaging.Bitmap BmpImageFromBmp(System.Drawing.Bitmap bmp)
        {
            using (var memory = new System.IO.MemoryStream())
            {
                bmp.Save(memory, System.Drawing.Imaging.ImageFormat.Png);
                memory.Position = 0;

                var bitmapImage = new Ava.Media.Imaging.Bitmap(memory);

                return bitmapImage;
            }
        }

        private bool currentlyRendering = false;
        public void Render(bool skipIfCurrentlyRendering = false, bool lowQuality = false, bool recalculateLayout = false)
        {
            if (!isDesignerMode)
            {
                if (recalculateLayout)
                    plt.TightenLayout();

                if (equalAxes)
                    plt.AxisEqual();

                if (!(skipIfCurrentlyRendering && currentlyRendering))
                {
                    currentlyRendering = true;
                    this.Find<Ava.Controls.Image>("imagePlot").Source = BmpImageFromBmp(plt.GetBitmap(true, lowQuality));
                    currentlyRendering = false;
                    Rendered?.Invoke(null, null);
                }
            }
        }

        //private void CanvasPlot_SizeChanged(object sender, SizeChangedEventArgs e)
        //{
        //    plt?.Resize((int)(canvasPlot.ActualWidth * dpiScale), (int)(canvasPlot.ActualHeight * dpiScale));
        //    Render();
        //}

        #region user control configuration

        private bool enablePanning = true;
        private bool enableZooming = true;
        private bool enableScrollWheelZoom = true;
        private bool lowQualityWhileDragging = true;
        private bool doubleClickingTogglesBenchmark = true;
        private bool lockVerticalAxis = false;
        private bool lockHorizontalAxis = false;
        private bool equalAxes = false;
        private double middleClickMarginX = .1;
        private double middleClickMarginY = .1;
        private bool? recalculateLayoutOnMouseUp = null;
        public void Configure(
            bool? enablePanning = null,
            bool? enableRightClickZoom = null,
            bool? enableRightClickMenu = null,
            bool? enableScrollWheelZoom = null,
            bool? lowQualityWhileDragging = null,
            bool? enableDoubleClickBenchmark = null,
            bool? lockVerticalAxis = null,
            bool? lockHorizontalAxis = null,
            bool? equalAxes = null,
            double? middleClickMarginX = null,
            double? middleClickMarginY = null,
            bool? recalculateLayoutOnMouseUp = null
            )
        {
            if (enablePanning != null) this.enablePanning = (bool)enablePanning;
            if (enableRightClickZoom != null) this.enableZooming = (bool)enableRightClickZoom;
            if (enableRightClickMenu != null) ContextMenu = (enableRightClickMenu.Value) ? DefaultRightClickMenu() : null;
            if (enableScrollWheelZoom != null) this.enableScrollWheelZoom = (bool)enableScrollWheelZoom;
            if (lowQualityWhileDragging != null) this.lowQualityWhileDragging = (bool)lowQualityWhileDragging;
            if (enableDoubleClickBenchmark != null) this.doubleClickingTogglesBenchmark = (bool)enableDoubleClickBenchmark;
            if (lockVerticalAxis != null) this.lockVerticalAxis = (bool)lockVerticalAxis;
            if (lockHorizontalAxis != null) this.lockHorizontalAxis = (bool)lockHorizontalAxis;
            if (equalAxes != null) this.equalAxes = (bool)equalAxes;
            this.middleClickMarginX = middleClickMarginX ?? this.middleClickMarginX;
            this.middleClickMarginY = middleClickMarginY ?? this.middleClickMarginY;
            this.recalculateLayoutOnMouseUp = recalculateLayoutOnMouseUp;
        }

        private bool isAltPressed = false;
        private bool isShiftPressed = false;
        private bool isCtrlPressed = false;

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.LeftAlt:
                case Key.RightAlt:
                    isAltPressed = true;
                    break;
                case Key.LeftShift:
                case Key.RightShift:
                    isShiftPressed = true;
                    break;
                case Key.LeftCtrl:
                case Key.RightCtrl:
                    isCtrlPressed = true;
                    break;
            }
        }

        private void OnKeyUp(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.LeftAlt:
                case Key.RightAlt:
                    isAltPressed = false;
                    break;
                case Key.LeftShift:
                case Key.RightShift:
                    isShiftPressed = false;
                    break;
                case Key.LeftCtrl:
                case Key.RightCtrl:
                    isCtrlPressed = false;
                    break;
            }
        }
        #endregion

        #region mouse tracking

        private Ava.Point? mouseLeftDownLocation, mouseRightDownLocation, mouseMiddleDownLocation;

        double[] axisLimitsOnMouseDown;
        private bool isPanningOrZooming
        {
            get
            {
                if (axisLimitsOnMouseDown is null) return false;
                if (mouseLeftDownLocation != null) return true;
                else if (mouseRightDownLocation != null) return true;
                else if (mouseMiddleDownLocation != null) return true;
                return false;
            }
        }

        IDraggable plottableBeingDragged = null;
        private bool isMovingDraggable { get { return (plottableBeingDragged != null); } }

        private Ava.Point GetPixelPosition(PointerEventArgs e)
        {
            Ava.Point pos = e.GetPosition(this);
            Ava.Point dpiCorrectedPos = new Ava.Point(pos.X * dpiScaleInput, pos.Y * dpiScaleInput);
            return dpiCorrectedPos;
        }

        private System.Drawing.Point SDPoint(Ava.Point pt)
        {
            return new System.Drawing.Point((int)pt.X, (int)pt.Y);
        }

        void UserControl_MouseDown(object sender, PointerPressedEventArgs e)
        {
            e.Pointer.Capture(this);

            var mousePixel = GetPixelPosition(e);
            plottableBeingDragged = plt.GetDraggableUnderMouse(mousePixel.X, mousePixel.Y);

            if (plottableBeingDragged is null)
            {
                // MouseDown event is to start a pan or zoom
                if (e.MouseButton == MouseButton.Left && isAltPressed) mouseMiddleDownLocation = GetPixelPosition(e);
                else if (e.MouseButton == MouseButton.Left && enablePanning) mouseLeftDownLocation = GetPixelPosition(e);
                else if (e.MouseButton == MouseButton.Right && enableZooming) mouseRightDownLocation = GetPixelPosition(e);
                else if (e.MouseButton == MouseButton.Middle && enableScrollWheelZoom) mouseMiddleDownLocation = GetPixelPosition(e);
                axisLimitsOnMouseDown = plt.Axis();
            }
            else
            {
                // mouse is being used to drag a plottable
            }
        }

        [Obsolete("use Plot.CoordinateFromPixelX() and Plot.CoordinateFromPixelY()")]
        public Ava.Point mouseCoordinates
        {
            get
            {
                var coord = plt.CoordinateFromPixel(mouseLocation.X, mouseLocation.Y);
                return new Ava.Point((int)coord.X, (int)coord.Y);
            }
        }
        Ava.Point mouseLocation;
        private void UserControl_MouseMove(object sender, PointerEventArgs e)
        {
            mouseLocation = GetPixelPosition(e);
            if (isPanningOrZooming)
                MouseMovedToPanOrZoom(e);
            else if (isMovingDraggable)
                MouseMovedToMoveDraggable(e);
            else
                MouseMovedWithoutInteraction(e);
        }

        private void MouseMovedToPanOrZoom(PointerEventArgs e)
        {
            plt.Axis(axisLimitsOnMouseDown);
            var mouseLocation = GetPixelPosition(e);

            if (mouseLeftDownLocation != null)
            {
                // left-click-drag panning
                double deltaX = ((Ava.Point)mouseLeftDownLocation).X - mouseLocation.X;
                double deltaY = mouseLocation.Y - ((Ava.Point)mouseLeftDownLocation).Y;

                if (isCtrlPressed) deltaY = 0;
                if (isShiftPressed) deltaX = 0;

                settings.AxesPanPx((int)deltaX, (int)deltaY);
                AxisChanged?.Invoke(null, null);
            }
            else if (mouseRightDownLocation != null)
            {
                // right-click-drag zooming
                double deltaX = ((Ava.Point)mouseRightDownLocation).X - mouseLocation.X;
                double deltaY = mouseLocation.Y - ((Ava.Point)mouseRightDownLocation).Y;

                if (isCtrlPressed == true && isShiftPressed == false) deltaY = 0;
                if (isShiftPressed == true && isCtrlPressed == false) deltaX = 0;

                settings.AxesZoomPx(-(int)deltaX, -(int)deltaY, lockRatio: isCtrlPressed && isShiftPressed);
                AxisChanged?.Invoke(null, null);
            }
            else if (mouseMiddleDownLocation != null)
            {
                // middle-click-drag zooming to rectangle
                double x1 = Math.Min(mouseLocation.X, ((Ava.Point)mouseMiddleDownLocation).X);
                double x2 = Math.Max(mouseLocation.X, ((Ava.Point)mouseMiddleDownLocation).X);
                double y1 = Math.Min(mouseLocation.Y, ((Ava.Point)mouseMiddleDownLocation).Y);
                double y2 = Math.Max(mouseLocation.Y, ((Ava.Point)mouseMiddleDownLocation).Y);

                var origin = new System.Drawing.Point((int)x1 - settings.dataOrigin.X, (int)y1 - settings.dataOrigin.Y);
                var size = new System.Drawing.Size((int)(x2 - x1), (int)(y2 - y1));

                if (lockVerticalAxis)
                {
                    origin.Y = 0;
                    size.Height = settings.dataSize.Height - 1;
                }
                if (lockHorizontalAxis)
                {
                    origin.X = 0;
                    size.Width = settings.dataSize.Width - 1;
                }

                settings.mouseMiddleRect = new System.Drawing.Rectangle(origin, size);
            }

            Render(true, lowQuality: lowQualityWhileDragging);
            return;
        }

        public (double x, double y) GetMouseCoordinates()
        {
            double x = plt.CoordinateFromPixelX(mouseLocation.X / dpiScaleInput);
            double y = plt.CoordinateFromPixelY(mouseLocation.Y / dpiScaleInput);
            return (x, y);
        }

        private void MouseMovedToMoveDraggable(PointerEventArgs e)
        {
            plottableBeingDragged.DragTo(plt.CoordinateFromPixelX(GetPixelPosition(e).X), plt.CoordinateFromPixelY(GetPixelPosition(e).Y));
            Render(true);
        }

        private void MouseMovedWithoutInteraction(PointerEventArgs e)
        {
            // set the cursor based on what's beneath it
            //var draggableUnderCursor = plt.GetDraggableUnderMouse(GetPixelPosition(e).X, GetPixelPosition(e).Y);
            //var spCursor = (draggableUnderCursor is null) ? Config.Cursor.Arrow : draggableUnderCursor.DragCursor;
            //imagePlot.Cursor = GetCursor(spCursor);
        }

        private void UserControl_MouseUp(object sender, PointerEventArgs e)
        {
            e.Pointer.Capture(null);
            var mouseLocation = GetPixelPosition(e);

            plottableBeingDragged = null;

            if (mouseMiddleDownLocation != null)
            {
                double x1 = Math.Min(mouseLocation.X, ((Ava.Point)mouseMiddleDownLocation).X) / dpiScaleOutput;
                double x2 = Math.Max(mouseLocation.X, ((Ava.Point)mouseMiddleDownLocation).X) / dpiScaleOutput;
                double y1 = Math.Min(mouseLocation.Y, ((Ava.Point)mouseMiddleDownLocation).Y) / dpiScaleOutput;
                double y2 = Math.Max(mouseLocation.Y, ((Ava.Point)mouseMiddleDownLocation).Y) / dpiScaleOutput;

                Ava.Point topLeft = new Ava.Point(x1, y1);
                Ava.Size size = new Ava.Size(x2 - x1, y2 - y1);
                Ava.Point botRight = new Ava.Point(topLeft.X + size.Width, topLeft.Y + size.Height);

                if ((size.Width > 2) && (size.Height > 2))
                {
                    // only change axes if suffeciently large square was drawn
                    if (!lockHorizontalAxis)
                        plt.Axis(
                            x1: plt.CoordinateFromPixelX(topLeft.X),
                            x2: plt.CoordinateFromPixelX(botRight.X));
                    if (!lockVerticalAxis)
                        plt.Axis(
                            y1: plt.CoordinateFromPixelY(botRight.Y),
                            y2: plt.CoordinateFromPixelY(topLeft.Y));
                    AxisChanged?.Invoke(null, null);
                }
                else
                {
                    bool shouldTighten = recalculateLayoutOnMouseUp ?? !plt.containsHeatmap;
                    plt.AxisAuto(middleClickMarginX, middleClickMarginY, tightenLayout: shouldTighten);
                    AxisChanged?.Invoke(null, null);
                }
            }

            if (mouseRightDownLocation != null)
            {
                double deltaX = Math.Abs(mouseLocation.X - mouseRightDownLocation.Value.X);
                double deltaY = Math.Abs(mouseLocation.Y - mouseRightDownLocation.Value.Y);
                bool mouseDraggedFar = (deltaX > 3 || deltaY > 3);
                if (mouseDraggedFar)
                {
                    e.Handled = true; //I wish I was bullshitting you but this is the only way to prevent opening the context menu that works in Avalonia right now
                }
            }
            else
            {
                if (ContextMenu != null)
                {
                    ContextMenu.Close();
                }
            }

            mouseLeftDownLocation = null;
            mouseRightDownLocation = null;
            mouseMiddleDownLocation = null;
            axisLimitsOnMouseDown = null;
            settings.mouseMiddleRect = null;

            bool shouldRecalculate = recalculateLayoutOnMouseUp ?? !plt.containsHeatmap;
            Render(recalculateLayout: shouldRecalculate);
        }

        #endregion

        #region mouse clicking

        private void UserControl_MouseWheel(object sender, PointerWheelEventArgs e)
        {
            if (enableScrollWheelZoom == false)
                return;

            var mousePixel = GetPixelPosition(e); // DPI-scaling aware

            double xFrac = (e.Delta.Y > 0) ? 1.15 : 0.85;
            double yFrac = (e.Delta.Y > 0) ? 1.15 : 0.85;

            if (isCtrlPressed) yFrac = 1;
            if (isShiftPressed) xFrac = 1;

            plt.AxisZoom(xFrac, yFrac, plt.CoordinateFromPixelX(mousePixel.X), plt.CoordinateFromPixelY(mousePixel.Y));
            AxisChanged?.Invoke(null, null);
            bool shouldRecalculate = recalculateLayoutOnMouseUp ?? !plt.containsHeatmap;
            Render(recalculateLayout: shouldRecalculate);
        }

        //private void UserControl_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        //{
        //    if (doubleClickingTogglesBenchmark)
        //    {
        //        plt.Benchmark(toggle: true);
        //        Render();
        //    }
        //}

        private async void SaveImage(object sender, RoutedEventArgs e)
        {
            SaveFileDialog savefile = new SaveFileDialog();
            savefile.InitialFileName = "ScottPlot.png";

            var filtersPNG = new FileDialogFilter();
            filtersPNG.Name = "PNG Files";
            filtersPNG.Extensions.Add("png");

            var filtersJPEG = new FileDialogFilter();
            filtersJPEG.Name = "JPG Files";
            filtersJPEG.Extensions.Add("jpg");
            filtersJPEG.Extensions.Add("jpeg");

            var filtersBMP = new FileDialogFilter();
            filtersBMP.Name = "BMP Files";
            filtersBMP.Extensions.Add("bmp");

            var filtersTIFF = new FileDialogFilter();
            filtersTIFF.Name = "TIF Files";
            filtersTIFF.Extensions.Add("tif");
            filtersTIFF.Extensions.Add("tiff");

            var filtersGeneric = new FileDialogFilter();
            filtersGeneric.Name = "All Files";
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
                plt.SaveFig(filenameTask.Result);
        }

        //private void CopyImage(object sender, RoutedEventArgs e)
        //{
        //    Clipboard.SetImage((BitmapSource)imagePlot.Source);
        //}

        private void OpenInNewWindow(object sender, RoutedEventArgs e)
        {
            new AvaPlotViewer(plt.Copy()).Show();
        }

        private void OpenHelp(object sender, RoutedEventArgs e)
        {
            new HelpWindow().Show();
        }

        #endregion

        #region event handling

        public event EventHandler Rendered;
        public event EventHandler AxisChanged;

        #endregion
    }
}
