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
            VersionLabel.Content = Tools.GetVersionString();

            StringBuilder msg = new StringBuilder();
            msg.AppendLine("Left-click-drag: pan");
            msg.AppendLine("Right-click-drag: zoom");
            msg.AppendLine("Middle-click-drag: zoom region");
            msg.AppendLine("");
            msg.AppendLine("Right-click: show menu");
            msg.AppendLine("Middle-click: auto-axis");
            msg.AppendLine("Double-click: show benchmark");
            msg.AppendLine("");
            msg.AppendLine("Hold CTRL to lock vertical axis");
            msg.AppendLine("Hold ALT to lock horizontal axis");
            InfoTextBlock.Text = msg.ToString();
        }

        private void LaunchScottPlotWebsite(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://swharden.com/scottplot/");
        }

        private void CloseButtonClicked(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
