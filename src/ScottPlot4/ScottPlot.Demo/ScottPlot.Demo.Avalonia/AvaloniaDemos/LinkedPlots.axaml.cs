using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using ScottPlot.Avalonia;

namespace ScottPlot.Demo.Avalonia.AvaloniaDemos
{
    public class LinkedPlots : Window
    {
        public LinkedPlots()
        {
            AvaloniaXamlLoader.Load(this);
#if DEBUG
            this.AttachDevTools();
#endif
            AvaPlot avaPlot1 = this.Find<AvaPlot>("avaPlot1");
            AvaPlot avaPlot2 = this.Find<AvaPlot>("avaPlot2");

            avaPlot1.Plot.AddSignal(ScottPlot.DataGen.Sin(51), color: System.Drawing.Color.Blue);
            avaPlot2.Plot.AddSignal(ScottPlot.DataGen.Cos(51), color: System.Drawing.Color.Red);

            avaPlot1.Refresh();
            avaPlot2.Refresh();

            avaPlot1.Configuration.AddLinkedControl(avaPlot2); // update plot 2 when plot 1 changes
            avaPlot2.Configuration.AddLinkedControl(avaPlot1); // update plot 1 when plot 2 changes
        }
    }
}
