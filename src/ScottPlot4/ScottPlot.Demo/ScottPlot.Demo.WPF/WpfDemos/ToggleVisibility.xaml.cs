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
    /// Interaction logic for ToggleVisibility.xaml
    /// </summary>
    public partial class ToggleVisibility : Window
    {
        ScatterPlot sinPlot, cosPlot;
        VLine vline1, vline2;

        public ToggleVisibility()
        {
            InitializeComponent();

            int pointCount = 51;
            double[] dataXs = DataGen.Consecutive(pointCount);
            double[] dataSin = DataGen.Sin(pointCount);
            double[] dataCos = DataGen.Cos(pointCount);

            sinPlot = wpfPlot1.Plot.AddScatter(dataXs, dataSin);
            cosPlot = wpfPlot1.Plot.AddScatter(dataXs, dataCos);
            vline1 = wpfPlot1.Plot.AddVerticalLine(0);
            vline2 = wpfPlot1.Plot.AddVerticalLine(50);

            wpfPlot1.Refresh();
        }

        private void SinHide(object sender, RoutedEventArgs e)
        {
            if (wpfPlot1 is null) return;
            sinPlot.IsVisible = false;
            wpfPlot1.Refresh();
        }

        private void SinShow(object sender, RoutedEventArgs e)
        {
            if (wpfPlot1 is null) return;
            sinPlot.IsVisible = true;
            wpfPlot1.Refresh();
        }

        private void CosShow(object sender, RoutedEventArgs e)
        {
            if (wpfPlot1 is null) return;
            cosPlot.IsVisible = true;
            wpfPlot1.Refresh();
        }

        private void CosHide(object sender, RoutedEventArgs e)
        {
            if (wpfPlot1 is null) return;
            cosPlot.IsVisible = false;
            wpfPlot1.Refresh();
        }

        private void LinesShow(object sender, RoutedEventArgs e)
        {
            if (wpfPlot1 is null) return;
            vline1.IsVisible = true;
            vline2.IsVisible = true;
            wpfPlot1.Refresh();
        }

        private void LinesHide(object sender, RoutedEventArgs e)
        {
            if (wpfPlot1 is null) return;
            vline1.IsVisible = false;
            vline2.IsVisible = false;
            wpfPlot1.Refresh();
        }
    }
}
