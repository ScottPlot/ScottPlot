using System.Windows;

namespace Sandbox.WPFFramework
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            WpfPlot1.Plot.Add.Signal(ScottPlot.Generate.Sin());
            WpfPlot1.Plot.Add.Signal(ScottPlot.Generate.Cos());
        }
    }
}
