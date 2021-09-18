using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using ScottPlot.Avalonia;

namespace ScottPlot.Demo.Avalonia.AvaloniaDemos
{
    public class ToggleVisibility : Window
    {
        Plottable.ScatterPlot sinPlot, cosPlot;
        Plottable.VLine vline1, vline2;

        AvaPlot avaPlot1;
        public ToggleVisibility()
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

            sinPlot = avaPlot1.Plot.AddScatter(dataXs, dataSin);
            cosPlot = avaPlot1.Plot.AddScatter(dataXs, dataCos);
            vline1 = avaPlot1.Plot.AddVerticalLine(0);
            vline2 = avaPlot1.Plot.AddVerticalLine(50);

            avaPlot1.Refresh();

            this.Find<CheckBox>("sineCheckbox").Checked += SinShow;
            this.Find<CheckBox>("sineCheckbox").Unchecked += SinHide;

            this.Find<CheckBox>("cosineCheckbox").Checked += CosShow;
            this.Find<CheckBox>("cosineCheckbox").Unchecked += CosHide;

            this.Find<CheckBox>("linesCheckbox").Checked += LinesShow;
            this.Find<CheckBox>("linesCheckbox").Unchecked += LinesHide;
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private void SinHide(object sender, RoutedEventArgs e)
        {
            if (avaPlot1 is null) return;
            sinPlot.IsVisible = false;
            avaPlot1.Refresh();
        }

        private void SinShow(object sender, RoutedEventArgs e)
        {
            if (avaPlot1 is null) return;
            sinPlot.IsVisible = true;
            avaPlot1.Refresh();
        }

        private void CosShow(object sender, RoutedEventArgs e)
        {
            if (avaPlot1 is null) return;
            cosPlot.IsVisible = true;
            avaPlot1.Refresh();
        }

        private void CosHide(object sender, RoutedEventArgs e)
        {
            if (avaPlot1 is null) return;
            cosPlot.IsVisible = false;
            avaPlot1.Refresh();
        }

        private void LinesShow(object sender, RoutedEventArgs e)
        {
            if (avaPlot1 is null) return;
            vline1.IsVisible = true;
            vline2.IsVisible = true;
            avaPlot1.Refresh();
        }

        private void LinesHide(object sender, RoutedEventArgs e)
        {
            if (avaPlot1 is null) return;
            vline1.IsVisible = false;
            vline2.IsVisible = false;
            avaPlot1.Refresh();
        }
    }
}
