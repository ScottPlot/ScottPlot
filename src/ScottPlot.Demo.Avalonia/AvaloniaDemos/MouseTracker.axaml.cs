using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using ScottPlot.Avalonia;

namespace ScottPlot.Demo.Avalonia.AvaloniaDemos
{
    public class MouseTracker : Window
    {
        PlottableVLine vLine;
        PlottableHLine hLine;
        AvaPlot avaPlot1;

        public MouseTracker()
        {
            this.InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif

            avaPlot1 = this.Find<AvaPlot>("avaPlot1");

            avaPlot1.plt.PlotSignal(DataGen.RandomWalk(null, 100));
            vLine = avaPlot1.plt.PlotVLine(0, color: System.Drawing.Color.Red, lineStyle: LineStyle.Dash);
            hLine = avaPlot1.plt.PlotHLine(0, color: System.Drawing.Color.Red, lineStyle: LineStyle.Dash);
            avaPlot1.Render();

            avaPlot1.PointerMoved += OnMouseMove;
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private void OnMouseMove(object sender, PointerEventArgs e)
        {
            int pixelX = (int)e.GetPosition(avaPlot1).X;
            int pixelY = (int)e.GetPosition(avaPlot1).Y;

            (double coordinateX, double coordinateY) = avaPlot1.GetMouseCoordinates();

            this.Find<TextBlock>("XPixelLabel").Text = $"{pixelX:0.000}";
            this.Find<TextBlock>("YPixelLabel").Text = $"{pixelY:0.000}";

            this.Find<TextBlock>("XCoordinateLabel").Text = $"{avaPlot1.plt.CoordinateFromPixelX(pixelX):0.00000000}";
            this.Find<TextBlock>("YCoordinateLabel").Text = $"{avaPlot1.plt.CoordinateFromPixelY(pixelY):0.00000000}";

            vLine.position = coordinateX;
            hLine.position = coordinateY;

            avaPlot1.Render();
        }
    }
}
