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
            wpfPlot1.Plot.AddSignal(DataGen.Sin(51));
            wpfPlot2.Plot.AddSignal(DataGen.Cos(51));
        }

        private void AxesChanged1(object sender, EventArgs e)
        {
            wpfPlot2.Configuration.AxesChangedEventEnabled = false; // disable this to prevent an infinite loop
            wpfPlot2.Plot.SetAxisLimits(wpfPlot1.Plot.GetAxisLimits());
            wpfPlot2.Render();
            wpfPlot2.Configuration.AxesChangedEventEnabled = true;
        }

        private void AxesChanged2(object sender, EventArgs e)
        {
            wpfPlot1.Configuration.AxesChangedEventEnabled = false; // disable this to prevent an infinite loop
            wpfPlot1.Plot.SetAxisLimits(wpfPlot2.Plot.GetAxisLimits());
            wpfPlot1.Render();
            wpfPlot1.Configuration.AxesChangedEventEnabled = true;
        }
    }
}
