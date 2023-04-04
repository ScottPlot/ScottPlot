using System.Windows;

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

            WpfPlot1.Configuration.DpiStretch = true;
            WpfPlot1.Plot.Title(
                $"System Scaling: {Drawing.GDI.GetScaleRatio() * 100}%\n" +
                $"DPI Stretch Ratio: {WpfPlot1.Configuration.DpiStretchRatio}");
            WpfPlot1.Refresh();
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            if (WpfPlot1 is null)
                return;

            WpfPlot1.Configuration.DpiStretch = false;
            WpfPlot1.Plot.Title(
                $"System Scaling: {Drawing.GDI.GetScaleRatio() * 100}%\n" +
                $"DPI Stretch Ratio: {WpfPlot1.Configuration.DpiStretchRatio}");
            WpfPlot1.Refresh();
        }
    }
}
