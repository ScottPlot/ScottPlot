using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ScottPlot.Plottable;

namespace ScottPlot.Demo.WPF.WpfDemos
{
    /// <summary>
    /// Interaction logic for MouseTracker.xaml
    /// </summary>
    public partial class MouseTracker : Window
    {
        Crosshair Crosshair;

        public MouseTracker()
        {
            InitializeComponent();
            wpfPlot1.Plot.AddSignal(DataGen.RandomWalk(null, 100));
            Crosshair = wpfPlot1.Plot.AddCrosshair(0, 0);
            wpfPlot1.Refresh();
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            int pixelX = (int)e.MouseDevice.GetPosition(wpfPlot1).X;
            int pixelY = (int)e.MouseDevice.GetPosition(wpfPlot1).Y;

            (double coordinateX, double coordinateY) = wpfPlot1.GetMouseCoordinates();

            XPixelLabel.Content = $"{pixelX:0.000}";
            YPixelLabel.Content = $"{pixelY:0.000}";

            XCoordinateLabel.Content = $"{wpfPlot1.Plot.GetCoordinateX(pixelX):0.00000000}";
            YCoordinateLabel.Content = $"{wpfPlot1.Plot.GetCoordinateY(pixelY):0.00000000}";

            Crosshair.X = coordinateX;
            Crosshair.Y = coordinateY;

            wpfPlot1.Refresh();
        }

        private void wpfPlot1_MouseEnter(object sender, MouseEventArgs e)
        {
            MouseTrackLabel.Content = "Mouse ENTERED the plot";
            Crosshair.IsVisible = true;
        }

        private void wpfPlot1_MouseLeave(object sender, MouseEventArgs e)
        {
            MouseTrackLabel.Content = "Mouse LEFT the plot";
            XPixelLabel.Content = "--";
            YPixelLabel.Content = "--";
            XCoordinateLabel.Content = "--";
            YCoordinateLabel.Content = "--";

            Crosshair.IsVisible = false;
            wpfPlot1.Refresh();
        }
    }
}
