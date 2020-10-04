using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using ScottPlot.Avalonia;
using System;

namespace ScottPlot.Demo.Avalonia.AvaloniaDemos
{
    public class ShowValueOnHover : Window
    {
        AvaPlot avaPlot1;
        ScottPlot.PlottableScatterHighlight sph;
        public ShowValueOnHover()
        {
            this.InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
            avaPlot1 = this.Find<AvaPlot>("avaPlot1");

            int pointCount = 100;
            Random rand = new Random(0);
            double[] xs = DataGen.Consecutive(pointCount, 0.1);
            double[] ys = DataGen.NoisySin(rand, pointCount);

            sph = avaPlot1.plt.PlotScatterHighlight(xs, ys);
            avaPlot1.Render();

            avaPlot1.PointerMoved += MouseMove;
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private void MouseMove(object sender, PointerEventArgs e)
        {
            (double mouseX, double mouseY) = avaPlot1.GetMouseCoordinates();

            sph.HighlightClear();
            var (x, y, index) = sph.HighlightPointNearest(mouseX, mouseY);
            avaPlot1.Render();

            this.Find<TextBlock>("label1").Text = $"Closest point to ({mouseX:N2}, {mouseY:N2}) " +
                $"is index {index} ({x:N2}, {y:N2})";
        }
    }
}
