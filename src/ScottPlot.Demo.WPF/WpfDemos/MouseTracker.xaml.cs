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
        PlottableVLine vLine;
        PlottableHLine hLine;

        public MouseTracker()
        {
            InitializeComponent();
            wpfPlot1.plt.PlotSignal(DataGen.RandomWalk(null, 100));

            vLine = wpfPlot1.plt.PlotVLine(0, color: System.Drawing.Color.Red, lineStyle: LineStyle.Dash);
            hLine = wpfPlot1.plt.PlotHLine(0, color: System.Drawing.Color.Red, lineStyle: LineStyle.Dash);

            wpfPlot1.Render();
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            int pixelX = (int)e.MouseDevice.GetPosition(wpfPlot1).X;
            int pixelY = (int)e.MouseDevice.GetPosition(wpfPlot1).Y;

            (double coordinateX, double coordinateY) = wpfPlot1.GetMouseCoordinates();

            XPixelLabel.Content = $"{pixelX:0.000}";
            YPixelLabel.Content = $"{pixelY:0.000}";

            XCoordinateLabel.Content = $"{wpfPlot1.plt.CoordinateFromPixelX(pixelX):0.00000000}";
            YCoordinateLabel.Content = $"{wpfPlot1.plt.CoordinateFromPixelY(pixelY):0.00000000}";

            vLine.position = coordinateX;
            hLine.position = coordinateY;

            wpfPlot1.Render(skipIfCurrentlyRendering: true);
        }
    }
}
