using System;
using System.Collections.Generic;
using System.Windows;

namespace WpfApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<Window> Windows = new();
        Random Rand = new();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Open(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < 50; i++)
            {
                ScottPlot.Plot plt = new();
                plt.AddSignal(ScottPlot.DataGen.RandomWalk(Rand, 100));
                ScottPlot.WpfPlotViewer window = new(plt, windowTitle: $"Window {i}");
                window.Show();
                Windows.Add(window);
            }
        }

        private void Button_Close(object sender, RoutedEventArgs e)
        {
            foreach (Window window in Windows)
                window.Close();
            Windows.Clear();
        }
    }
}
