using System.Windows;

namespace WpfApp
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            double[] values = { 778, 283, 184, 76, 43 };
            WpfPlot1.Plot.AddPie(values);
            WpfPlot1.Refresh();
        }
    }
}