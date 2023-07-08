using Avalonia.Controls;
using Avalonia.Interactivity;

namespace ScottPlot.Demo.Avalonia.AvaloniaDemos
{
    public partial class ToggleVisibility : Window
    {
        private readonly Plottable.ScatterPlot sinPlot, cosPlot;
        private readonly Plottable.VLine vline1, vline2;

        public ToggleVisibility()
        {
            this.InitializeComponent();

            const int pointCount = 51;
            double[] dataXs = DataGen.Consecutive(pointCount);
            double[] dataSin = DataGen.Sin(pointCount);
            double[] dataCos = DataGen.Cos(pointCount);

            sinPlot = avaPlot1.Plot.AddScatter(dataXs, dataSin);
            cosPlot = avaPlot1.Plot.AddScatter(dataXs, dataCos);
            vline1 = avaPlot1.Plot.AddVerticalLine(0);
            vline2 = avaPlot1.Plot.AddVerticalLine(50);

            avaPlot1.Refresh();

            this.sineCheckbox.IsCheckedChanged += UpdateSinVisibility;
            this.cosineCheckbox.IsCheckedChanged += UpdateCosVisibility;
            this.linesCheckbox.IsCheckedChanged += UpdateLinesVisibility;
        }

        private void UpdateSinVisibility(object sender, RoutedEventArgs e)
        {
            if (avaPlot1 is null) return;
            sinPlot.IsVisible = sineCheckbox.IsChecked == true;
            avaPlot1.Refresh();
        }

        private void UpdateCosVisibility(object sender, RoutedEventArgs e)
        {
            if (avaPlot1 is null) return;
            cosPlot.IsVisible = cosineCheckbox.IsChecked == true;
            avaPlot1.Refresh();
        }

        private void UpdateLinesVisibility(object sender, RoutedEventArgs e)
        {
            if (avaPlot1 is null) return;
            vline1.IsVisible = linesCheckbox.IsChecked == true;
            vline2.IsVisible = linesCheckbox.IsChecked == true;
            avaPlot1.Refresh();
        }
    }
}
