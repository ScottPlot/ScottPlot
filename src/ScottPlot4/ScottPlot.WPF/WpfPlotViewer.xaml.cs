using System.Windows;

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
