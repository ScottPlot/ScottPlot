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
    /// Interaction logic for ShowValueOnHover.xaml
    /// </summary>
    public partial class ShowValueOnHover : Window
    {
        ScottPlot.PlottableScatterHighlight sph;

        public ShowValueOnHover()
        {
            InitializeComponent();

            int pointCount = 100;
            Random rand = new Random(0);
            double[] xs = DataGen.Consecutive(pointCount, 0.1);
            double[] ys = DataGen.NoisySin(rand, pointCount);

            sph = wpfPlot1.plt.PlotScatterHighlight(xs, ys);
            wpfPlot1.Render();
        }

        private void wpfPlot1_MouseMove(object sender, MouseEventArgs e)
        {
            var mousePos = e.MouseDevice.GetPosition(wpfPlot1);
            double mouseX = wpfPlot1.plt.CoordinateFromPixelX(mousePos.X);
            double mouseY = wpfPlot1.plt.CoordinateFromPixelY(mousePos.Y);

            sph.HighlightClear();
            var (x, y, index) = sph.HighlightPointNearest(mouseX, mouseY);
            wpfPlot1.Render();

            label1.Content = $"Closest point to ({mouseX:N2}, {mouseY:N2}) " +
                $"is index {index} ({x:N2}, {y:N2})";
        }
    }
}
