using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using ScottPlot.Avalonia;
using ScottPlot.Renderable;
using System;

namespace ScottPlot.Demo.Avalonia.AvaloniaDemos
{
    public class Layout : Window
    {
        AvaPlot avaPlot1;

        public Layout()
        {
            this.InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
            avaPlot1 = this.Find<AvaPlot>("avaPlot1");
            this.Find<Button>("PlotButton").Click += PlotRandomData;
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private void PlotRandomData(object sender, RoutedEventArgs e)
        {
            avaPlot1.Reset();

            int pointCount = 5;
            Random rand = new Random();
            double[] dataX = DataGen.Consecutive(pointCount);
            double[] dataY = DataGen.Random(rand, pointCount);
            string[] labels = { "One", "Two", "Three", "Four", "Five" };

            avaPlot1.Plot.AddScatter(dataX, dataY, label: "series 1");
            avaPlot1.Plot.Title("Plot Title");
            avaPlot1.Plot.XLabel("Horizontal Axis");
            avaPlot1.Plot.YLabel("Vertical Axis");

            avaPlot1.Plot.XTicks(dataX, labels);
            avaPlot1.Plot.XAxis.TickLabelStyle(rotation: 90);
            avaPlot1.Plot.AxisAuto();
            avaPlot1.Plot.Layout(left: 20, top: 50, bottom: 100, right: 20);
            //avaPlot1.Configure(recalculateLayoutOnMouseUp: false);

            avaPlot1.Render();
        }
    }
}
