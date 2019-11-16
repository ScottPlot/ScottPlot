using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        public readonly Plot plt;
        private readonly Settings settings;
        private readonly bool isDesignerMode;

        private int scaledWidth { get { return (int)(canvasPlot.ActualWidth * settings.gfxFigure.DpiX / 96); } }
        private int scaledHeight { get { return (int)(canvasPlot.ActualHeight * settings.gfxFigure.DpiY / 96); } }

        public WpfPlot()
        {
            InitializeComponent();
            isDesignerMode = (LicenseManager.UsageMode == LicenseUsageMode.Designtime);

            plt = new Plot();
            settings = plt.GetSettings(showWarning: false);

            CanvasPlot_SizeChanged(null, null);
        }

        private static BitmapImage BmpImageFromBmp(System.Drawing.Bitmap bmp)
        {
            System.IO.MemoryStream stream = new System.IO.MemoryStream();
            bmp.Save(stream, System.Drawing.Imaging.ImageFormat.Bmp);
            BitmapImage bmpImage = new BitmapImage();
            bmpImage.BeginInit();
            stream.Seek(0, System.IO.SeekOrigin.Begin);
            bmpImage.StreamSource = stream;
            bmpImage.EndInit();
            return bmpImage;
        }

        private bool currentlyRendering = false;
        public void Render(bool skipIfCurrentlyRendering = false, bool lowQuality = false)
        {

            if (isDesignerMode)
            {
                System.Drawing.Size controlPixelSize = new System.Drawing.Size(scaledWidth, scaledHeight);
                System.Drawing.Bitmap bmp = Tools.DesignerModeBitmap(controlPixelSize);
                imagePlot.Source = BmpImageFromBmp(bmp);
                return;
            }
            else if (!(skipIfCurrentlyRendering && currentlyRendering))
            {
                currentlyRendering = true;
                imagePlot.Source = BmpImageFromBmp(plt.GetBitmap(true, lowQuality));
                currentlyRendering = false;
            }
        }

        private void CanvasPlot_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            plt.Resize(scaledWidth, scaledHeight);
            Render();
        }

        #region mouse tracking

        private Point? mouseLeftDownLocation, mouseRightDownLocation, mouseMiddleDownLocation;
        private Point mouseLocation;
        double[] axisLimitsOnMouseDown;
        private bool isMouseDragging
        {
            get
            {
                if (axisLimitsOnMouseDown is null)
                    return false;

                if (mouseLeftDownLocation != null) return true;
                else if (mouseRightDownLocation != null) return true;
                else if (mouseMiddleDownLocation != null) return true;

                return false;
            }
        }

        private void UserControl_MouseDown(object sender, MouseButtonEventArgs e)
        {
            CaptureMouse();

            if (e.ChangedButton == MouseButton.Left) mouseLeftDownLocation = e.GetPosition(this);
            else if (e.ChangedButton == MouseButton.Right) mouseRightDownLocation = e.GetPosition(this);
            else if (e.ChangedButton == MouseButton.Middle) mouseMiddleDownLocation = e.GetPosition(this);
            axisLimitsOnMouseDown = plt.Axis();
        }

        private void UserControl_MouseMove(object sender, MouseEventArgs e)
        {
            mouseLocation = e.GetPosition(this);

            if (isMouseDragging)
            {
                plt.Axis(axisLimitsOnMouseDown);

                if (mouseLeftDownLocation != null)
                {
                    double deltaX = ((Point)mouseLeftDownLocation).X - mouseLocation.X;
                    double deltaY = mouseLocation.Y - ((Point)mouseLeftDownLocation).Y;

                    if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl)) deltaY = 0;
                    if (Keyboard.IsKeyDown(Key.LeftAlt) || Keyboard.IsKeyDown(Key.RightAlt)) deltaX = 0;

                    settings.AxesPanPx((int)deltaX, (int)deltaY);
                }
                else if (mouseRightDownLocation != null)
                {
                    double deltaX = ((Point)mouseRightDownLocation).X - mouseLocation.X;
                    double deltaY = mouseLocation.Y - ((Point)mouseRightDownLocation).Y;

                    if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl)) deltaY = 0;
                    if (Keyboard.IsKeyDown(Key.LeftAlt) || Keyboard.IsKeyDown(Key.RightAlt)) deltaX = 0;

                    settings.AxesZoomPx(-(int)deltaX, -(int)deltaY);
                }
                else if (mouseMiddleDownLocation != null)
                {
                    double x1 = Math.Min(mouseLocation.X, ((Point)mouseMiddleDownLocation).X);
                    double x2 = Math.Max(mouseLocation.X, ((Point)mouseMiddleDownLocation).X);
                    double y1 = Math.Min(mouseLocation.Y, ((Point)mouseMiddleDownLocation).Y);
                    double y2 = Math.Max(mouseLocation.Y, ((Point)mouseMiddleDownLocation).Y);

                    var origin = new System.Drawing.Point((int)x1 - settings.dataOrigin.X, (int)y1 - settings.dataOrigin.Y);
                    var size = new System.Drawing.Size((int)(x2 - x1), (int)(y2 - y1));

                    settings.mouseMiddleRect = new System.Drawing.Rectangle(origin, size);
                }

                Render(true);
                return;
            }
        }

        private void UserControl_MouseUp(object sender, MouseButtonEventArgs e)
        {
            ReleaseMouseCapture();

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
                }
                else
                {
                    plt.AxisAuto();
                }
            }

            mouseLeftDownLocation = null;
            mouseRightDownLocation = null;
            mouseMiddleDownLocation = null;
            axisLimitsOnMouseDown = null;
            settings.mouseMiddleRect = null;
            Render();
        }

        #endregion

        #region mouse clicking

        private void UserControl_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            // note: AxisZoom's zoomCenter argument could be used to zoom to the cursor (see code in FormsPlot.cs).
            // However, this requires some work and testing to ensure it works if DPI scaling is used too.
            // Currently, scroll-wheel zooming simply zooms in and out of the center of the plot.

            double zoomAmountY = 0.15;
            double zoomAmountX = 0.15;

            if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl)) zoomAmountY = 0;
            if (Keyboard.IsKeyDown(Key.LeftAlt) || Keyboard.IsKeyDown(Key.RightAlt)) zoomAmountX = 0;

            if (e.Delta > 1)
            {
                plt.AxisZoom(1 + zoomAmountX, 1 + zoomAmountY);
            }
            else
            {
                plt.AxisZoom(1 - zoomAmountX, 1 - zoomAmountY);
            }

            Render(skipIfCurrentlyRendering: false);
        }

        #endregion
    }
}
