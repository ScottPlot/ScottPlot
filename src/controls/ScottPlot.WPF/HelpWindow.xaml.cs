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
