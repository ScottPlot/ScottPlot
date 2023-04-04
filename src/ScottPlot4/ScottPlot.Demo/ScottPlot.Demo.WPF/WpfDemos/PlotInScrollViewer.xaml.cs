using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ScottPlot.Demo.WPF.WpfDemos
{
    /// <summary>
    /// Interaction logic for PlotInScrollViewer.xaml
    /// </summary>
    public partial class PlotInScrollViewer : Window
    {
        public PlotInScrollViewer()
        {
            InitializeComponent();

            // initialize plots with random data
            Random Rand = new Random(0);
            wpfPlot1.Plot.AddSignal(DataGen.RandomWalk(Rand, 50));
            wpfPlot2.Plot.AddSignal(DataGen.RandomWalk(Rand, 50));
            wpfPlot3.Plot.AddSignal(DataGen.RandomWalk(Rand, 50));

            // perform an initial render for each control
            wpfPlot1.Refresh();
            wpfPlot2.Refresh();
            wpfPlot3.Refresh();
        }

        private void ScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            ScrollViewer myScrollViewer = (ScrollViewer)sender;

            if (ScrollRadio.IsChecked.Value)
            {
                // manually scroll the window then mark the event as handled so it does not zoom
                double scrollOffset = myScrollViewer.VerticalOffset - (e.Delta * .2);
                myScrollViewer.ScrollToVerticalOffset(scrollOffset);
                e.Handled = true;
                return;
            }

            if (ZoomRadio.IsChecked.Value)
            {
                // manually scroll (zero offset) to complete the scroll action, then proceed to zooming
                myScrollViewer.ScrollToVerticalOffset(myScrollViewer.VerticalOffset);
                return;
            }
        }
    }
}
