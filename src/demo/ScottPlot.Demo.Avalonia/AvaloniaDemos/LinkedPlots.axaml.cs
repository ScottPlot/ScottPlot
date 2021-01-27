using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using ScottPlot.Avalonia;
using System;

namespace ScottPlot.Demo.Avalonia.AvaloniaDemos
{
    public class LinkedPlots : Window
    {
        AvaPlot avaPlot1;
        AvaPlot avaPlot2;

        public LinkedPlots()
        {
            this.InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
            int pointCount = 51;
            double[] dataXs = DataGen.Consecutive(pointCount);
            double[] dataSin = DataGen.Sin(pointCount);
            double[] dataCos = DataGen.Cos(pointCount);

            avaPlot1 = this.Find<AvaPlot>("avaPlot1");
            avaPlot2 = this.Find<AvaPlot>("avaPlot2");

            avaPlot1.Plot.PlotScatter(dataXs, dataSin);
            avaPlot1.Render();

            avaPlot2.Plot.PlotScatter(dataXs, dataCos);
            avaPlot2.Render();

        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private void axisChanged1(object sender, EventArgs e)
        {
            avaPlot2.Plot.SetAxisLimits(avaPlot1.Plot.GetAxisLimits());
            avaPlot2.Render();
        }

        private void axisChanged2(object sender, EventArgs e)
        {
            avaPlot1.Plot.SetAxisLimits(avaPlot2.Plot.GetAxisLimits());
            avaPlot1.Render();
        }
    }
}
