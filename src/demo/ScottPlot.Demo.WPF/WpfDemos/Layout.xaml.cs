using System;
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
using System.Windows.Shapes;

namespace ScottPlot.Demo.WPF.WpfDemos
{
    /// <summary>
    /// Interaction logic for Layout.xaml
    /// </summary>
    public partial class Layout : Window
    {
        public Layout()
        {
            InitializeComponent();
        }

        private void PlotRandomData(object sender, RoutedEventArgs e)
        {
            wpfPlot1.Reset();

            int pointCount = 5;
            Random rand = new Random();
            double[] dataX = DataGen.Consecutive(pointCount);
            double[] dataY = DataGen.Random(rand, pointCount);
            string[] labels = { "One", "Two", "Three", "Four", "Five" };

            wpfPlot1.Plot.AddScatter(dataX, dataY, label: "series 1");
            wpfPlot1.Plot.Title("Plot Title");
            wpfPlot1.Plot.XLabel("Horizontal Axis");
            wpfPlot1.Plot.YLabel("Vertical Axis");

            wpfPlot1.Plot.XTicks(dataX, labels);
            wpfPlot1.Plot.XAxis.TickLabelStyle(rotation: 90);
            wpfPlot1.Plot.AxisAuto();
            wpfPlot1.Plot.Layout(left: 20, top: 50, bottom: 100, right: 20);

            wpfPlot1.Render();
        }
    }
}
