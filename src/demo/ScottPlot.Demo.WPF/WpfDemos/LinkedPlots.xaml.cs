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
        readonly WpfPlot[] WpfPlots;

        public LinkedPlots()
        {
            InitializeComponent();
            wpfPlot1.Plot.AddSignal(DataGen.Sin(51));
            wpfPlot2.Plot.AddSignal(DataGen.Cos(51));

            // create a list of plot controls we can easily iterate through later
            WpfPlots = new WpfPlot[] { wpfPlot1, wpfPlot2 };

            // perform an initial render
            wpfPlot1.Refresh();
            wpfPlot2.Refresh();
        }

        private void AxesChanged(object sender, EventArgs e)
        {
            WpfPlot changedPlot = (WpfPlot)sender;
            var newAxisLimits = changedPlot.Plot.GetAxisLimits();

            foreach (WpfPlot wp in WpfPlots)
            {
                if (wp == changedPlot)
                    continue;

                // disable events briefly to prevent an infinite loop
                wp.Configuration.AxesChangedEventEnabled = false;
                wp.Plot.SetAxisLimits(newAxisLimits);
                wp.Refresh();
                wp.Configuration.AxesChangedEventEnabled = true;
            }
        }
    }
}
