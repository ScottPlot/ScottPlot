using System.Windows;

namespace ScottPlot.WPF
{
    /// <summary>
    /// Interaction logic for HelpWindow.xaml
    /// </summary>
    public partial class HelpWindow : Window
    {
        public HelpWindow()
        {
            InitializeComponent();
            VersionLabel.Content = $"ScottPlot {Plot.Version}";
            InfoTextBlock.Text = Control.ControlBackEnd.GetHelpMessage();
        }

        private void LaunchScottPlotWebsite(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://ScottPlot.NET");
        }

        private void CloseButtonClicked(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
