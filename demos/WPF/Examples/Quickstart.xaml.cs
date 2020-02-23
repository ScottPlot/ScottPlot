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

namespace WPF.Examples
{
    /// <summary>
    /// Interaction logic for Quickstart.xaml
    /// </summary>
    public partial class Quickstart : Window
    {
        public Quickstart()
        {
            InitializeComponent();

            wpfPlot1.plt.PlotSignal(ScottPlot.DataGen.Sin(50));
            wpfPlot1.plt.PlotSignal(ScottPlot.DataGen.Cos(50));
            wpfPlot1.plt.Title("ScottPlot WPF Quickstart");
            wpfPlot1.Render();
        }
    }
}
