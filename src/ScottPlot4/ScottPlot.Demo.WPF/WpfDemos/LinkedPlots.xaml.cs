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
    /// Interaction logic for LinkedPlots.xaml
    /// </summary>
    public partial class LinkedPlots : Window
    {
        public LinkedPlots()
        {
            InitializeComponent();

            int pointCount = 51;
            double[] dataXs = DataGen.Consecutive(pointCount);
            double[] dataSin = DataGen.Sin(pointCount);
            double[] dataCos = DataGen.Cos(pointCount);

            wpfPlot1.plt.PlotScatter(dataXs, dataSin);
            wpfPlot1.Render();

            wpfPlot2.plt.PlotScatter(dataXs, dataCos);
            wpfPlot2.Render();
        }

        private void axisChanged1(object sender, EventArgs e)
        {
            wpfPlot2.plt.MatchAxis(wpfPlot1.plt);
            wpfPlot2.Render();
        }

        private void axisChanged2(object sender, EventArgs e)
        {
            wpfPlot1.plt.MatchAxis(wpfPlot2.plt);
            wpfPlot1.Render();
        }
    }
}
