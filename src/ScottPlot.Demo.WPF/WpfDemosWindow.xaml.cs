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

namespace ScottPlot.Demo.WPF
{
    /// <summary>
    /// Interaction logic for WpfDemos.xaml
    /// </summary>
    public partial class WpfDemosWindow : Window
    {
        public WpfDemosWindow()
        {
            InitializeComponent();
        }

        private void LaunchMouseTracker(object sender, RoutedEventArgs e)
        {
            new WpfDemos.MouseTracker().ShowDialog();
        }
    }
}
