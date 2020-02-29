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

namespace ScottPlot.Demo.WPF.WpfDemos
{
    /// <summary>
    /// Interaction logic for MouseTracker.xaml
    /// </summary>
    public partial class MouseTracker : Window
    {
        public MouseTracker()
        {
            InitializeComponent();
            wpfPlot1.plt.PlotSignal(DataGen.RandomWalk(null, 100));
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            double pixelX = e.MouseDevice.GetPosition(wpfPlot1).X;
            double pixelY = e.MouseDevice.GetPosition(wpfPlot1).Y;

            XPixelLabel.Content = $"{pixelX:0.000}";
            YPixelLabel.Content = $"{pixelY:0.000}";

            XCoordinateLabel.Content = $"{wpfPlot1.plt.CoordinateFromPixelX(pixelX):0.00000000}";
            YCoordinateLabel.Content = $"{wpfPlot1.plt.CoordinateFromPixelY(pixelY):0.00000000}";
        }
    }
}
