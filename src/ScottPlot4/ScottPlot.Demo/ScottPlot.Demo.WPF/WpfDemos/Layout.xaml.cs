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

            wpfPlot1.plt.PlotScatter(dataX, dataY, label: "series 1");
            wpfPlot1.plt.Title("Plot Title");
            wpfPlot1.plt.XLabel("Horizontal Axis");
            wpfPlot1.plt.YLabel("Vertical Axis");

            wpfPlot1.plt.XTicks(dataX, labels);
            wpfPlot1.plt.Ticks(xTickRotation: 90);
            wpfPlot1.plt.AxisAuto();
            wpfPlot1.plt.Layout(yLabelWidth: 20, titleHeight: 50, xLabelHeight: 50, y2LabelWidth: 20, xScaleHeight: 50);
            wpfPlot1.Configure(recalculateLayoutOnMouseUp: false);

            wpfPlot1.Render();
        }
    }
}
