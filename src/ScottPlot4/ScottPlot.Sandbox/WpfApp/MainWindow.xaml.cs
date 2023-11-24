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

            WpfPlot1.SizeChanged += (s, e) =>
            {
                Title = $"Actual={WpfPlot1.ActualWidth}, Reported={WpfPlot1.Plot.XAxis.Dims.FigureSizePx}";
            };
        }
    }
}
