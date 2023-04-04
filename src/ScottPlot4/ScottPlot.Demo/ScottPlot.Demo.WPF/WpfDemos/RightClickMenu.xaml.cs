using System;
using System.Windows;
using System.Windows.Controls;

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
            wpfPlot1.Plot.AddSignal(DataGen.Sin(51));
            wpfPlot1.Plot.AddSignal(DataGen.Cos(51));

            // unsubscribe from the default right-click menu event
            wpfPlot1.RightClicked -= wpfPlot1.DefaultRightClickEvent;

            // add your own custom event
            wpfPlot1.RightClicked += DeployCustomMenu;

            // perform an initial render
            wpfPlot1.Refresh();
        }

        private void DeployCustomMenu(object sender, EventArgs e)
        {
            MenuItem addSinMenuItem = new MenuItem() { Header = "Add Sine Wave" };
            addSinMenuItem.Click += AddSine;
            MenuItem clearPlotMenuItem = new MenuItem() { Header = "Clear Plot" };
            clearPlotMenuItem.Click += ClearPlot;

            ContextMenu rightClickMenu = new ContextMenu();
            rightClickMenu.Items.Add(addSinMenuItem);
            rightClickMenu.Items.Add(clearPlotMenuItem);

            rightClickMenu.IsOpen = true;
        }

        private void AddSine(object sender, RoutedEventArgs e)
        {
            Random rand = new Random();
            double[] data = DataGen.Sin(51, phase: rand.NextDouble() * 1000);
            wpfPlot1.Plot.AddSignal(data);
            wpfPlot1.Plot.AxisAuto();
            wpfPlot1.Refresh();
        }

        private void ClearPlot(object sender, RoutedEventArgs e)
        {
            wpfPlot1.Plot.Clear();
            wpfPlot1.Plot.AxisAuto();
            wpfPlot1.Refresh();
        }
    }
}
