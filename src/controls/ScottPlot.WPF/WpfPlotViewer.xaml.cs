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

namespace ScottPlot
{
    /// <summary>
    /// Interaction logic for WpfPlotViewer.xaml
    /// </summary>
    public partial class WpfPlotViewer : Window
    {
        public WpfPlotViewer(ScottPlot.Plot plot, int windowWidth = 600, int windowHeight = 400, string windowTitle = "ScottPlot Viewer")
        {
            InitializeComponent();

            Width = windowWidth;
            Height = windowHeight;
            Title = windowTitle;

            wpfPlot1.Reset(plot);
            wpfPlot1.Refresh();
        }
    }
}
