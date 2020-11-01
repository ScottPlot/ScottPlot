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

            wpfPlot1.plt.PlotSignal(DataGen.Sin(51));
            wpfPlot1.plt.PlotSignal(DataGen.Cos(51));
            wpfPlot1.plt.AxisAuto();
            wpfPlot1.plt.AxisBounds(0, 50, -1, 1);
            wpfPlot1.Render();

        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
