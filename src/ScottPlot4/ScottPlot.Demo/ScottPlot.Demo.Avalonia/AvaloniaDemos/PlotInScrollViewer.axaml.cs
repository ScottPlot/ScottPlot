using System;

using Avalonia.Controls;

using ScottPlot.Avalonia;

namespace ScottPlot.Demo.Avalonia.AvaloniaDemos
{
    public partial class PlotInScrollViewer : Window
    {
        private readonly Random rand = new Random();
        public PlotInScrollViewer()
        {
            this.InitializeComponent();

            AvaPlot[] avaPlots = { this.avaPlot1, this.avaPlot2, this.avaPlot3 };

            foreach (AvaPlot avaPlot in avaPlots)
            {
                for (int i = 0; i < 3; i++)
                    avaPlot.Plot.AddSignal(DataGen.RandomWalk(rand, 100));

                //avaPlot.Configure(enableScrollWheelZoom: false, enableRightClickMenu: false);

                avaPlot.Refresh();
            }
        }
    }
}
