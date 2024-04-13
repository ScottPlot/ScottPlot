using System.Windows;

namespace WpfApp
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            WpfPlot1.Plot.AddSignal(ScottPlot.DataGen.Sin(51));
            WpfPlot1.Plot.AddSignal(ScottPlot.DataGen.Cos(51));

            WpfPlot1.Refresh();
            WpfPlot1.Refresh();

            WpfPlot1.AxesChanged += (s, e) =>
            {
                Title = $"{WpfPlot1.Plot.LastRenderDimensions.FigureWidth}";
            };
        }
    }
}
