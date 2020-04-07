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
    /// Interaction logic for RightClickMenu.xaml
    /// </summary>
    public partial class RightClickMenu : Window
    {
        public RightClickMenu()
        {
            InitializeComponent();

            wpfPlot1.plt.PlotSignal(DataGen.Sin(51));
            wpfPlot1.plt.PlotSignal(DataGen.Cos(51));
            wpfPlot1.Render();

            MenuItem addSinMenuItem = new MenuItem() { Header = "Add Sine Wave" };
            addSinMenuItem.Click += AddSine;
            MenuItem clearPlotMenuItem = new MenuItem() { Header = "Clear Plot" };
            clearPlotMenuItem.Click += ClearPlot;

            ContextMenu rightClickMenu = new ContextMenu();
            rightClickMenu.Items.Add(addSinMenuItem);
            rightClickMenu.Items.Add(clearPlotMenuItem);

            wpfPlot1.ContextMenu = rightClickMenu;
        }

        private void AddSine(object sender, RoutedEventArgs e)
        {
            Random rand = new Random();
            wpfPlot1.plt.PlotSignal(DataGen.Sin(51, phase: rand.NextDouble() * 1000));
            wpfPlot1.plt.AxisAuto();
            wpfPlot1.Render();
        }

        private void ClearPlot(object sender, RoutedEventArgs e)
        {
            wpfPlot1.plt.Clear();
            wpfPlot1.plt.AxisAuto();
            wpfPlot1.Render();
        }
    }
}
