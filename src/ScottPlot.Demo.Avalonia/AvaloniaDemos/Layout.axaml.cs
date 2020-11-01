using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using ScottPlot.Avalonia;
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

            avaPlot1.plt.PlotScatter(dataX, dataY, label: "series 1");
            avaPlot1.plt.Title("Plot Title");
            avaPlot1.plt.XLabel("Horizontal Axis");
            avaPlot1.plt.YLabel("Vertical Axis");

            avaPlot1.plt.XTicks(dataX, labels);
            avaPlot1.plt.Ticks(xTickRotation: 90);
            avaPlot1.plt.AxisAuto();
            avaPlot1.plt.Layout(yLabelWidth: 20, titleHeight: 50, xLabelHeight: 50, y2LabelWidth: 20, xScaleHeight: 50);
            avaPlot1.Configure(recalculateLayoutOnMouseUp: false);

            avaPlot1.Render();
        }
    }
}
