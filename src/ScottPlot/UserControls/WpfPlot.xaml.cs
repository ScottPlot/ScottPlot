﻿using System;
using System.Collections.Generic;
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
    public partial class WpfPlot : UserControl
    {
        public Plot plt = new Plot();

        private DispatcherTimer timer;

        private bool currentlyRendering = false;

        public WpfPlot()
        {
            InitializeComponent();
            timer = new DispatcherTimer(); // WPF supports only DispatcherTimer
            timer.Tick += (o, arg) =>
            {
                timer.Stop(); // AutoReset = false
                Render(skipIfCurrentlyRendering: false);
            };
            CanvasPlot_SizeChanged(null, null);
        }

        public void Render(bool skipIfCurrentlyRendering = false, bool lowQuality = false)
        {

            if (System.ComponentModel.LicenseManager.UsageMode == System.ComponentModel.LicenseUsageMode.Designtime)
            {
                try
                {
                    System.Drawing.Size controlPixelSize = new System.Drawing.Size(scaledWidth, scaledHeight);
                    System.Drawing.Bitmap bmp = Tools.DesignerModeBitmap(controlPixelSize);
                    imagePlot.Source = Tools.bmpImageFromBmp(bmp);
                }
                catch
                {

                }
                return;
            }

            if (!(skipIfCurrentlyRendering && currentlyRendering))
            {
                if (timer.IsEnabled)
                    timer.Stop();
                currentlyRendering = true;
                imagePlot.Source = Tools.bmpImageFromBmp(plt.GetBitmap(true, lowQuality));
                currentlyRendering = false;
            }
        }

        private int scaledWidth
        {
            get
            {
                double dpiScaleX = plt.GetSettings().gfxFigure.DpiX / 96;
                return (int)(canvasPlot.ActualWidth * dpiScaleX);
            }
        }

        private int scaledHeight
        {
            get
            {
                double dpiScaleY = plt.GetSettings().gfxFigure.DpiY / 96;
                return (int)(canvasPlot.ActualHeight * dpiScaleY);
            }
        }

        private void CanvasPlot_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            plt.Resize(scaledWidth, scaledHeight);
            Render(skipIfCurrentlyRendering: false);
        }

        private void UserControl_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left || e.ChangedButton == MouseButton.Right)
            {
                plt.mouseTracker.MouseDown(e.GetPosition(this));
                CaptureMouse();
            }
        }

        private void UserControl_MouseMove(object sender, MouseEventArgs e)
        {
            plt.mouseTracker.MouseMove(e.GetPosition(this));
            if ((Mouse.LeftButton == MouseButtonState.Pressed) || (Mouse.RightButton == MouseButtonState.Pressed))
                Render(skipIfCurrentlyRendering: true, plt.mouseTracker.lowQualityWhileInteracting);
        }

        private void UserControl_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left || e.ChangedButton == MouseButton.Right)
            {
                plt.mouseTracker.MouseUp(e.GetPosition(this));
                if (plt.mouseTracker.lowQualityWhileInteracting && plt.mouseTracker.mouseUpHQRenderDelay > 0)
                {
                    Render(false, true);
                    timer.Interval = TimeSpan.FromMilliseconds(plt.mouseTracker.mouseUpHQRenderDelay);
                    timer.Start();
                }
                else
                {
                    Render(skipIfCurrentlyRendering: false);
                }
            }
            else if (e.ChangedButton == MouseButton.Middle)
            {
                plt.AxisAuto();
                Render(skipIfCurrentlyRendering: false);
            }
            ReleaseMouseCapture();
        }

        private void UserControl_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            // note: AxisZoom's zoomCenter argument could be used to zoom to the cursor (see code in FormsPlot.cs).
            // However, this requires some work and testing to ensure it works if DPI scaling is used too.
            // Currently, scroll-wheel zooming simply zooms in and out of the center of the plot.

            double zoomAmountY = 0.15;
            double zoomAmountX = 0.15;

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
    }
}
