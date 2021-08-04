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
        Random Rand = new();
        int WindowNumber = 1;
        List<Window> Windows = new();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Close(object sender, RoutedEventArgs e)
        {
            foreach (Window window in Windows)
            {
                window.Close();
            }
        }

        private void Button_Launch(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < 10; i++)
            {
                ScottPlot.Plot plt = new();
                plt.AddSignal(ScottPlot.DataGen.RandomWalk(Rand, 100));
                ScottPlot.WpfPlotViewer window = new(plt, windowTitle: $"Window {WindowNumber++}");
                Windows.Add(window);
                window.Show();
            }
        }
    }
}
