using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using ScottPlot.Avalonia;

namespace ScottPlot.Demo.Avalonia.AvaloniaDemos
{
    public class MouseTracker : Window
    {
        Plottable.Crosshair Crosshair;
        AvaPlot avaPlot1;

        public MouseTracker()
        {
            this.InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif

            avaPlot1 = this.Find<AvaPlot>("avaPlot1");

            avaPlot1.Plot.AddSignal(DataGen.RandomWalk(null, 100));
            Crosshair = avaPlot1.Plot.AddCrosshair(0, 0);
            avaPlot1.Refresh();

            avaPlot1.PointerMoved += OnMouseMove;
            avaPlot1.PointerLeave += OnMouseLeave;
            avaPlot1.PointerEnter += OnMouseEnter;
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

            this.Find<TextBlock>("XCoordinateLabel").Text = $"{avaPlot1.Plot.GetCoordinateX(pixelX):0.00000000}";
            this.Find<TextBlock>("YCoordinateLabel").Text = $"{avaPlot1.Plot.GetCoordinateY(pixelY):0.00000000}";

            Crosshair.X = coordinateX;
            Crosshair.Y = coordinateY;

            avaPlot1.Refresh();
        }

        private void OnMouseEnter(object sender, PointerEventArgs e)
        {
            this.Find<TextBlock>("MouseTrackerMessage").Text = "Mouse ENTERED the plot";

            Crosshair.IsVisible = true;
        }

        private void OnMouseLeave(object sender, PointerEventArgs e)
        {
            this.Find<TextBlock>("MouseTrackerMessage").Text = "Mouse LEFT the plot";
            this.Find<TextBlock>("XPixelLabel").Text = "--";
            this.Find<TextBlock>("YPixelLabel").Text = "--";
            this.Find<TextBlock>("XCoordinateLabel").Text = "--";
            this.Find<TextBlock>("YCoordinateLabel").Text = "--";

            Crosshair.IsVisible = false;
            avaPlot1.Refresh();
        }
    }
}
