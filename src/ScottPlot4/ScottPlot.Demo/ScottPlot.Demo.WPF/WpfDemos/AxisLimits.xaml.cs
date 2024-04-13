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
    /// Interaction logic for AxisLimits.xaml
    /// </summary>
    public partial class AxisLimits : Window
    {
        public AxisLimits()
        {
            InitializeComponent();
            wpfPlot1.Plot.AddSignal(DataGen.Sin(51));
            wpfPlot1.Plot.AddSignal(DataGen.Cos(51));
            wpfPlot1.Plot.AxisAuto();
            wpfPlot1.Plot.XAxis.SetBoundary(0, 50);
            wpfPlot1.Plot.YAxis.SetBoundary(-1, 1);
            wpfPlot1.Refresh();
        }
    }
}
