using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using ScottPlot.Avalonia;
using System;

namespace ScottPlot.Demo.Avalonia.AvaloniaDemos
{
    public class LinkedPlots : Window
    {
        AvaPlot[] AvaPlots;

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

            AvaPlot avaPlot1 = this.Find<AvaPlot>("avaPlot1");
            AvaPlot avaPlot2 = this.Find<AvaPlot>("avaPlot2");
            AvaPlots = new AvaPlot[] { avaPlot1, avaPlot2 };

            avaPlot1.Plot.AddScatter(dataXs, dataSin);
            avaPlot1.Refresh();

            avaPlot2.Plot.AddScatter(dataXs, dataCos);
            avaPlot2.Refresh();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private void AxesChanged(object sender, EventArgs e)
        {
            AvaPlot changedPlot = (AvaPlot)sender;
            var newAxisLimits = changedPlot.Plot.GetAxisLimits();

            foreach (AvaPlot ap in AvaPlots)
            {
                if (ap == changedPlot)
                    continue;

                // disable this briefly to avoid infinite loop
                ap.Configuration.AxesChangedEventEnabled = false;
                ap.Plot.SetAxisLimits(newAxisLimits);
                ap.Refresh();
                ap.Configuration.AxesChangedEventEnabled = true;
            }
        }
    }
}
