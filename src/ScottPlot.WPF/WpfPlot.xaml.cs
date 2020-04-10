using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace ScottPlot
{
    /// <summary>
    /// Interaction logic for ScottPlotWPF.xaml
    /// </summary>

    [System.ComponentModel.ToolboxItem(true)]
    [System.ComponentModel.DesignTimeVisible(true)]
    public partial class WpfPlot : UserControl
    {
        public Plot plt { get; private set; }
        private Settings settings;
        private bool isDesignerMode;
        public Cursor cursor = Cursors.Arrow;
        private double dpiScale = 1;

        public WpfPlot(Plot plt)
        {
            InitializeComponent();
            CreateDefaultRightClickMenu();
            Reset(plt);
        }

        public WpfPlot()
        {
            InitializeComponent();
            CreateDefaultRightClickMenu();
            Reset(null);
        }

        private void CreateDefaultRightClickMenu()
        {
            MenuItem SaveImageMenuItem = new MenuItem() { Header = "Save Image" };
            SaveImageMenuItem.Click += SaveImage;
            MenuItem CopyImageMenuItem = new MenuItem() { Header = "Copy Image" };
            CopyImageMenuItem.Click += CopyImage;
            MenuItem NewWindowMenuItem = new MenuItem() { Header = "Open in New Window" };
            NewWindowMenuItem.Click += OpenInNewWindow;
            MenuItem HelpMenuItem = new MenuItem() { Header = "Help" };
            HelpMenuItem.Click += OpenHelp;

            ContextMenu = new ContextMenu();
            ContextMenu.Items.Add(SaveImageMenuItem);
            ContextMenu.Items.Add(CopyImageMenuItem);
            ContextMenu.Items.Add(NewWindowMenuItem);
            ContextMenu.Items.Add(HelpMenuItem);
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
            lblVersion.Content = Tools.GetVersionString();
            isDesignerMode = DesignerProperties.GetIsInDesignMode(this);

            settings = plt.GetSettings(showWarning: false);

            if (isDesignerMode)
            {
                // hide the plot
                mainGrid.RowDefinitions[1].Height = new GridLength(0);
            }
            else
            {
                // hide the version info
                mainGrid.RowDefinitions[0].Height = new GridLength(0);
                CanvasPlot_SizeChanged(null, null);
                dpiScale = settings.gfxFigure.DpiX / 96;
                canvasDesigner.Background = Brushes.Transparent;
                canvasPlot.Background = Brushes.Transparent;
            }
        }

        private static BitmapImage BmpImageFromBmp(System.Drawing.Bitmap bmp)
        {
            System.IO.MemoryStream stream = new System.IO.MemoryStream();
            bmp.Save(stream, System.Drawing.Imaging.ImageFormat.Png); // use PNG to support transparency
            BitmapImage bmpImage = new BitmapImage();
            bmpImage.BeginInit();
            stream.Seek(0, System.IO.SeekOrigin.Begin);
            bmpImage.StreamSource = stream;
            bmpImage.EndInit();
            return bmpImage;
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
                    imagePlot.Source = BmpImageFromBmp(plt.GetBitmap(true, lowQuality));
                    currentlyRendering = false;
                    Rendered?.Invoke(null, null);
                }
            }
        }

        private void CanvasPlot_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            plt.Resize((int)(canvasPlot.ActualWidth * dpiScale), (int)(canvasPlot.ActualHeight * dpiScale));
            Render();
        }

        #region user control configuration

        private bool enablePanning = true;
        private bool enableZooming = true;
        private bool enableScrollWheelZoom = true;
        private bool lowQualityWhileDragging = true;
        private bool doubleClickingTogglesBenchmark = true;
        private bool lockVerticalAxis = false;
        private bool lockHorizontalAxis = false;
        private bool equalAxes = false;
        public void Configure(
            bool? enablePanning = null,
            bool? enableRightClickZoom = null,
            bool? enableScrollWheelZoom = null,
            bool? lowQualityWhileDragging = null,
            bool? enableDoubleClickBenchmark = null,
            bool? lockVerticalAxis = null,
            bool? lockHorizontalAxis = null,
            bool? equalAxes = null
            )
        {
            if (enablePanning != null) this.enablePanning = (bool)enablePanning;
            if (enableRightClickZoom != null) this.enableZooming = (bool)enableRightClickZoom;
            if (enableScrollWheelZoom != null) this.enableScrollWheelZoom = (bool)enableScrollWheelZoom;
            if (lowQualityWhileDragging != null) this.lowQualityWhileDragging = (bool)lowQualityWhileDragging;
            if (enableDoubleClickBenchmark != null) this.doubleClickingTogglesBenchmark = (bool)enableDoubleClickBenchmark;
            if (lockVerticalAxis != null) this.lockVerticalAxis = (bool)lockVerticalAxis;
            if (lockHorizontalAxis != null) this.lockHorizontalAxis = (bool)lockHorizontalAxis;
            if (equalAxes != null) this.equalAxes = (bool)equalAxes;
        }

        private bool isHorizontalLocked { get { return (Keyboard.IsKeyDown(Key.LeftAlt) || Keyboard.IsKeyDown(Key.RightAlt) || (lockHorizontalAxis)); } }
        private bool isVerticalLocked { get { return (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl) || (lockVerticalAxis)); } }

        #endregion

        #region mouse tracking

        private Point? mouseLeftDownLocation, mouseRightDownLocation, mouseMiddleDownLocation;

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

        private Cursor GetCursor(Config.Cursor scottPlotCursor)
        {
            switch (scottPlotCursor)
            {
                case Config.Cursor.Arrow: return Cursors.Arrow;
                case Config.Cursor.WE: return Cursors.SizeWE;
                case Config.Cursor.NS: return Cursors.SizeNS;
                case Config.Cursor.All: return Cursors.SizeAll;
                default: return Cursors.Help;
            }
        }

        private Point GetPixelPosition(MouseButtonEventArgs e)
        {
            Point pos = e.GetPosition(this);
            pos.X *= dpiScale;
            pos.Y *= dpiScale;
            return pos;
        }

        private Point GetPixelPosition(MouseEventArgs e)
        {
            Point pos = e.GetPosition(this);
            pos.X *= dpiScale;
            pos.Y *= dpiScale;
            return pos;
        }

        private System.Drawing.Point SDPoint(Point pt)
        {
            return new System.Drawing.Point((int)pt.X, (int)pt.Y);
        }

        private void UserControl_MouseDown(object sender, MouseButtonEventArgs e)
        {
            CaptureMouse();

            var mousePixel = GetPixelPosition(e);
            plottableBeingDragged = plt.GetDraggableUnderMouse(mousePixel.X, mousePixel.Y);

            if (plottableBeingDragged is null)
            {
                // MouseDown event is to start a pan or zoom
                bool shiftIsPressed = Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift);
                if (e.ChangedButton == MouseButton.Left && shiftIsPressed) mouseMiddleDownLocation = GetPixelPosition(e);
                else if (e.ChangedButton == MouseButton.Left && enablePanning) mouseLeftDownLocation = GetPixelPosition(e);
                else if (e.ChangedButton == MouseButton.Right && enableZooming) mouseRightDownLocation = GetPixelPosition(e);
                else if (e.ChangedButton == MouseButton.Middle) mouseMiddleDownLocation = GetPixelPosition(e);
                axisLimitsOnMouseDown = plt.Axis();
            }
            else
            {
                // mouse is being used to drag a plottable
            }
        }

        [Obsolete("use Plot.CoordinateFromPixelX() and Plot.CoordinateFromPixelY()")]
        public Point mouseCoordinates
        {
            get
            {
                var coord = plt.CoordinateFromPixel(mouseLocation.X, mouseLocation.Y);
                return new Point(coord.X, coord.Y);
            }
        }
        Point mouseLocation;
        private void UserControl_MouseMove(object sender, MouseEventArgs e)
        {
            mouseLocation = GetPixelPosition(e);
            if (isPanningOrZooming)
                MouseMovedToPanOrZoom(e);
            else if (isMovingDraggable)
                MouseMovedToMoveDraggable(e);
            else
                MouseMovedWithoutInteraction(e);
        }

        private void MouseMovedToPanOrZoom(MouseEventArgs e)
        {
            plt.Axis(axisLimitsOnMouseDown);
            var mouseLocation = GetPixelPosition(e);

            if (mouseLeftDownLocation != null)
            {
                // left-click-drag panning
                double deltaX = ((Point)mouseLeftDownLocation).X - mouseLocation.X;
                double deltaY = mouseLocation.Y - ((Point)mouseLeftDownLocation).Y;

                if (isVerticalLocked) deltaY = 0;
                if (isHorizontalLocked) deltaX = 0;

                settings.AxesPanPx((int)deltaX, (int)deltaY);
                AxisChanged?.Invoke(null, null);
            }
            else if (mouseRightDownLocation != null)
            {
                // right-click-drag panning
                double deltaX = ((Point)mouseRightDownLocation).X - mouseLocation.X;
                double deltaY = mouseLocation.Y - ((Point)mouseRightDownLocation).Y;

                if (isVerticalLocked) deltaY = 0;
                if (isHorizontalLocked) deltaX = 0;

                settings.AxesZoomPx(-(int)deltaX, -(int)deltaY);
                AxisChanged?.Invoke(null, null);
            }
            else if (mouseMiddleDownLocation != null)
            {
                // middle-click-drag zooming to rectangle
                double x1 = Math.Min(mouseLocation.X, ((Point)mouseMiddleDownLocation).X);
                double x2 = Math.Max(mouseLocation.X, ((Point)mouseMiddleDownLocation).X);
                double y1 = Math.Min(mouseLocation.Y, ((Point)mouseMiddleDownLocation).Y);
                double y2 = Math.Max(mouseLocation.Y, ((Point)mouseMiddleDownLocation).Y);

                var origin = new System.Drawing.Point((int)x1 - settings.dataOrigin.X, (int)y1 - settings.dataOrigin.Y);
                var size = new System.Drawing.Size((int)(x2 - x1), (int)(y2 - y1));

                settings.mouseMiddleRect = new System.Drawing.Rectangle(origin, size);
            }

            Render(true, lowQuality: lowQualityWhileDragging);
            return;
        }

        private void MouseMovedToMoveDraggable(MouseEventArgs e)
        {
            var coordinate = plt.CoordinateFromPixel(GetPixelPosition(e).X, GetPixelPosition(e).Y);
            plottableBeingDragged.DragTo(coordinate.X, coordinate.Y);
            Render(true);
        }

        private void MouseMovedWithoutInteraction(MouseEventArgs e)
        {
            // set the cursor based on what's beneath it
            var draggableUnderCursor = plt.GetDraggableUnderMouse(GetPixelPosition(e).X, GetPixelPosition(e).Y);
            var spCursor = (draggableUnderCursor is null) ? Config.Cursor.Arrow : draggableUnderCursor.DragCursor;
            imagePlot.Cursor = GetCursor(spCursor);
        }

        private void UserControl_MouseUp(object sender, MouseButtonEventArgs e)
        {
            ReleaseMouseCapture();
            var mouseLocation = GetPixelPosition(e);

            plottableBeingDragged = null;

            if (mouseMiddleDownLocation != null)
            {
                double x1 = Math.Min(mouseLocation.X, ((Point)mouseMiddleDownLocation).X);
                double x2 = Math.Max(mouseLocation.X, ((Point)mouseMiddleDownLocation).X);
                double y1 = Math.Min(mouseLocation.Y, ((Point)mouseMiddleDownLocation).Y);
                double y2 = Math.Max(mouseLocation.Y, ((Point)mouseMiddleDownLocation).Y);

                Point topLeft = new Point(x1, y1);
                Size size = new Size(x2 - x1, y2 - y1);
                Point botRight = new Point(topLeft.X + size.Width, topLeft.Y + size.Height);

                if ((size.Width > 2) && (size.Height > 2))
                {
                    // only change axes if suffeciently large square was drawn
                    plt.Axis(
                            x1: plt.CoordinateFromPixel((int)topLeft.X, (int)topLeft.Y).X,
                            x2: plt.CoordinateFromPixel((int)botRight.X, (int)botRight.Y).X,
                            y1: plt.CoordinateFromPixel((int)botRight.X, (int)botRight.Y).Y,
                            y2: plt.CoordinateFromPixel((int)topLeft.X, (int)topLeft.Y).Y
                        );
                    AxisChanged?.Invoke(null, null);
                }
                else
                {
                    plt.AxisAuto();
                    AxisChanged?.Invoke(null, null);
                }
            }

            if (mouseRightDownLocation != null)
            {
                double deltaX = Math.Abs(mouseLocation.X - mouseRightDownLocation.Value.X);
                double deltaY = Math.Abs(mouseLocation.Y - mouseRightDownLocation.Value.Y);
                bool mouseDraggedFar = (deltaX > 3 || deltaY > 3);
                ContextMenu.Visibility = (mouseDraggedFar) ? Visibility.Hidden : Visibility.Visible;
                ContextMenu.IsOpen = (!mouseDraggedFar);
            }
            else
            {
                ContextMenu.IsOpen = false;
            }

            mouseLeftDownLocation = null;
            mouseRightDownLocation = null;
            mouseMiddleDownLocation = null;
            axisLimitsOnMouseDown = null;
            settings.mouseMiddleRect = null;
            Render(recalculateLayout: true);
        }

        #endregion

        #region mouse clicking

        private void UserControl_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (enableScrollWheelZoom == false)
                return;

            var mousePixel = GetPixelPosition(e); // DPI-scaling aware
            var mouseCoordinate = plt.CoordinateFromPixel(mousePixel.X, mousePixel.Y);

            double xFrac = (e.Delta > 0) ? 1.15 : 0.85;
            double yFrac = (e.Delta > 0) ? 1.15 : 0.85;

            if (isVerticalLocked) yFrac = 1;
            if (isHorizontalLocked) xFrac = 1;

            plt.AxisZoom(xFrac, yFrac, mouseCoordinate.X, mouseCoordinate.Y);
            AxisChanged?.Invoke(null, null);
            Render(recalculateLayout: true);
        }

        private void UserControl_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (doubleClickingTogglesBenchmark)
            {
                plt.Benchmark(toggle: true);
                Render();
            }
        }

        private void SaveImage(object sender, RoutedEventArgs e)
        {
            SaveFileDialog savefile = new SaveFileDialog();
            savefile.FileName = "ScottPlot.png";
            savefile.Filter = "PNG Files (*.png)|*.png;*.png";
            savefile.Filter += "|JPG Files (*.jpg, *.jpeg)|*.jpg;*.jpeg";
            savefile.Filter += "|BMP Files (*.bmp)|*.bmp;*.bmp";
            savefile.Filter += "|TIF files (*.tif, *.tiff)|*.tif;*.tiff";
            savefile.Filter += "|All files (*.*)|*.*";
            if (savefile.ShowDialog() == true)
                plt.SaveFig(savefile.FileName);
        }

        private void CopyImage(object sender, RoutedEventArgs e)
        {
            Clipboard.SetImage((BitmapSource)imagePlot.Source);
        }

        private void OpenInNewWindow(object sender, RoutedEventArgs e)
        {
            new WpfPlotViewer(plt.Copy()).Show();
        }

        private void OpenHelp(object sender, RoutedEventArgs e)
        {
            new WPF.HelpWindow().Show();
        }

        #endregion

        #region event handling

        public event EventHandler Rendered;
        public event EventHandler AxisChanged;

        #endregion
    }
}
