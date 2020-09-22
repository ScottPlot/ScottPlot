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
    /// Interaction logic for ToggleVisibility.xaml
    /// </summary>
    public partial class ToggleVisibility : Window
    {
        PlottableScatter sinPlot, cosPlot;
        PlottableVLine vline1, vline2;

        public ToggleVisibility()
        {
            InitializeComponent();

            int pointCount = 51;
            double[] dataXs = DataGen.Consecutive(pointCount);
            double[] dataSin = DataGen.Sin(pointCount);
            double[] dataCos = DataGen.Cos(pointCount);

            sinPlot = wpfPlot1.plt.PlotScatter(dataXs, dataSin);
            cosPlot = wpfPlot1.plt.PlotScatter(dataXs, dataCos);
            vline1 = wpfPlot1.plt.PlotVLine(0);
            vline2 = wpfPlot1.plt.PlotVLine(50);

            wpfPlot1.Render();
        }

        private void SinHide(object sender, RoutedEventArgs e)
        {
            if (wpfPlot1 is null) return;
            sinPlot.visible = false;
            wpfPlot1.Render();
        }

        private void SinShow(object sender, RoutedEventArgs e)
        {
            if (wpfPlot1 is null) return;
            sinPlot.visible = true;
            wpfPlot1.Render();
        }

        private void CosShow(object sender, RoutedEventArgs e)
        {
            if (wpfPlot1 is null) return;
            cosPlot.visible = true;
            wpfPlot1.Render();
        }

        private void CosHide(object sender, RoutedEventArgs e)
        {
            if (wpfPlot1 is null) return;
            cosPlot.visible = false;
            wpfPlot1.Render();
        }

        private void LinesShow(object sender, RoutedEventArgs e)
        {
            if (wpfPlot1 is null) return;
            vline1.visible = true;
            vline2.visible = true;
            wpfPlot1.Render();
        }

        private void LinesHide(object sender, RoutedEventArgs e)
        {
            if (wpfPlot1 is null) return;
            vline1.visible = false;
            vline2.visible = false;
            wpfPlot1.Render();
        }
    }
}
