using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using ScottPlot.Avalonia;
using System;

namespace ScottPlot.Demo.Avalonia.AvaloniaDemos
{
    public class PlotInScrollViewer : Window
    {
        Random rand = new Random();
        public PlotInScrollViewer()
        {
            this.InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
            AvaPlot[] avaPlots = { this.Find<AvaPlot>("avaPlot1"), this.Find<AvaPlot>("avaPlot2"), this.Find<AvaPlot>("avaPlot3") };

            foreach (AvaPlot avaPlot in avaPlots)
            {
                for (int i = 0; i < 3; i++)
                    avaPlot.Plot.AddSignal(DataGen.RandomWalk(rand, 100));

                //avaPlot.Configure(enableScrollWheelZoom: false, enableRightClickMenu: false);

                avaPlot.Refresh();
            }

        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

    }
}
