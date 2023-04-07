using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using ScottPlot.Avalonia;

namespace ScottPlot.Demo.Avalonia.AvaloniaDemos
{
    public class AxisLimits : Window
    {
        public AxisLimits()
        {
            this.InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
            AvaPlot wpfPlot1 = this.Find<AvaPlot>("avaPlot1");

            wpfPlot1.Plot.AddSignal(DataGen.Sin(51));
            wpfPlot1.Plot.AddSignal(DataGen.Cos(51));
            wpfPlot1.Plot.AxisAuto();
            wpfPlot1.Plot.XAxis.SetBoundary(0, 50);
            wpfPlot1.Plot.YAxis.SetBoundary(-1, 1);
            wpfPlot1.Refresh();

        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
