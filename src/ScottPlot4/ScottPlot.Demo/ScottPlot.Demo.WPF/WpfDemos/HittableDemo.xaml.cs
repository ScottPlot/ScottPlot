using System;
using System.Drawing;
using System.Windows;

namespace ScottPlot.Demo.WPF.WpfDemos
{
    /// <summary>
    /// Interaction logic for AxisLimits.xaml
    /// </summary>
    public partial class HittableDemo : Window
    {
        int Clicks = 0;

        public HittableDemo()
        {
            InitializeComponent();
            wpfPlot1.LeftClickedPlottable += WpfPlot1_LeftClickedPlottable; ;
            wpfPlot1.Plot.AddSignal(DataGen.Sin(51));
            wpfPlot1.Plot.AddSignal(DataGen.Cos(51));
            var vl = wpfPlot1.Plot.AddVerticalLine(25);
            vl.DragEnabled = true;

            wpfPlot1.Plot.AddTooltip("clickable", 30, .6);
            wpfPlot1.Plot.AxisAuto();
            wpfPlot1.Refresh();
        }

        private void WpfPlot1_LeftClickedPlottable(object sender, RoutedEventArgs e)
        {
            if (e.OriginalSource is ScottPlot.Plottable.Tooltip tt)
            {
                Clicks += 1;
                Random rand = new Random();
                Color randomColor = Color.FromArgb(255, rand.Next(256), rand.Next(256), rand.Next(256));
                tt.Color = randomColor;
                wpfPlot1.Refresh();
            }

            Title = $"Clicked the thing {Clicks} times";
        }
    }
}
