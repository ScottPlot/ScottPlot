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
    /// Interaction logic for RightClickMenu.xaml
    /// </summary>
    public partial class RightClickMenu : Window
    {
        public RightClickMenu()
        {
            InitializeComponent();
            wpfPlot2.plt.PlotSignal(DataGen.Sin(51));
            wpfPlot2.plt.PlotSignal(DataGen.Cos(51));
        }
    }
}
