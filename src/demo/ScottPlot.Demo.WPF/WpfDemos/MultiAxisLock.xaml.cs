using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ScottPlot.Demo.WPF.WpfDemos
{
    /// <summary>
    /// Interaction logic for MultiAxisLock.xaml
    /// </summary>
    public partial class MultiAxisLock : Window
    {
        private readonly ScottPlot.Renderable.Axis YAxis3;
        public bool Primary { get; set; } = true;
        public bool Secondary { get; set; }
        public bool Tertiary { get; set; }

        public MultiAxisLock()
        {
            InitializeComponent();
            PrimaryCheckbox.DataContext = this;
            SecondaryCheckbox.DataContext = this;
            TertiaryCheckbox.DataContext = this;

            Random rand = new Random();

            // Add 3 signals each with a different vertical axis index.
            // Each signal defaults to X axis index 0 so their horizontal axis will be shared.

            var plt1 = WpfPlot1.Plot.AddSignal(DataGen.RandomWalk(rand, 100, mult: 1));
            plt1.YAxisIndex = 0;
            plt1.LineWidth = 3;
            plt1.Color = System.Drawing.Color.Magenta;

            var plt2 = WpfPlot1.Plot.AddSignal(DataGen.RandomWalk(rand, 100, mult: 10));
            plt2.YAxisIndex = 1;
            plt2.LineWidth = 3;
            plt2.Color = System.Drawing.Color.Green;

            var plt3 = WpfPlot1.Plot.AddSignal(DataGen.RandomWalk(rand, 100, mult: 100));
            plt3.YAxisIndex = 2;
            plt3.LineWidth = 3;
            plt3.Color = System.Drawing.Color.Navy;

            // The horizontal axis is shared by these signal plots (XAxisIndex defaults to 0)
            WpfPlot1.Plot.XAxis.Label("Horizontal Axis");

            // Customize the primary (left) and secondary (right) axes
            WpfPlot1.Plot.YAxis.Color(System.Drawing.Color.Magenta);
            WpfPlot1.Plot.YAxis.Label("Primary Axis");
            WpfPlot1.Plot.YAxis2.Color(System.Drawing.Color.Green);
            WpfPlot1.Plot.YAxis2.Label("Secondary Axis");

            // the secondary (right) axis ticks are hidden by default so enable them
            WpfPlot1.Plot.YAxis2.Ticks(true);

            // Create an additional vertical axis and customize it
            YAxis3 = WpfPlot1.Plot.AddAxis(Renderable.Edge.Left, 2);
            YAxis3.Color(System.Drawing.Color.Navy);
            YAxis3.Label("Tertiary Axis");

            // adjust axis limits to fit the data once before locking them
            WpfPlot1.Plot.AxisAuto();
            WpfPlot1.Refresh();
            CheckChanged(null, null);
        }

        private void CheckChanged(object sender, RoutedEventArgs e)
        {
            WpfPlot1.Plot.YAxis.LockLimits(!Primary);
            WpfPlot1.Plot.YAxis2.LockLimits(!Secondary);
            YAxis3.LockLimits(!Tertiary);
        }
    }
}
