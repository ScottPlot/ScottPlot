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
    /// Interaction logic for DisplayScaling.xaml
    /// </summary>
    public partial class DisplayScaling : Window
    {
        public DisplayScaling()
        {
            InitializeComponent();
            WpfPlot1.Plot.AddSignal(DataGen.Sin(51));
            WpfPlot1.Plot.AddSignal(DataGen.Cos(51));
            CheckBox_Checked(null, null);
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (WpfPlot1 is null)
                return;

            WpfPlot1.DisplayScale.Enable();
            WpfPlot1.Plot.Title("Display Scaling: Enabled");
            WpfPlot1.Refresh();
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            if (WpfPlot1 is null)
                return;

            WpfPlot1.DisplayScale.Disable();
            WpfPlot1.Width = WpfPlot1.Width;
            WpfPlot1.Plot.Title("Display Scaling: Disabled");
            WpfPlot1.Refresh();
        }
    }
}
