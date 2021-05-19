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
        VLine vLine;
        HLine hLine;

        public MouseTracker()
        {
            InitializeComponent();
            wpfPlot1.Plot.AddSignal(DataGen.RandomWalk(null, 100));

            vLine = wpfPlot1.Plot.AddVerticalLine(0, color: System.Drawing.Color.Red, style: LineStyle.Dash);
            hLine = wpfPlot1.Plot.AddHorizontalLine(0, color: System.Drawing.Color.Red, style: LineStyle.Dash);

            wpfPlot1.Render();
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

            vLine.X = coordinateX;
            hLine.Y = coordinateY;

            wpfPlot1.Render();
        }

        private void wpfPlot1_MouseEnter(object sender, MouseEventArgs e)
        {
            MouseTrackLabel.Content = "Mouse ENTERED the plot";
            vLine.IsVisible = true;
            hLine.IsVisible = true;
        }

        private void wpfPlot1_MouseLeave(object sender, MouseEventArgs e)
        {
            MouseTrackLabel.Content = "Mouse LEFT the plot";
            XPixelLabel.Content = "--";
            YPixelLabel.Content = "--";
            XCoordinateLabel.Content = "--";
            YCoordinateLabel.Content = "--";

            vLine.IsVisible = false;
            hLine.IsVisible = false;
            wpfPlot1.Render();
        }
    }
}
