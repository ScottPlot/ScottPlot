using Avalonia.Controls;
using Avalonia.Input;

namespace ScottPlot.Demo.Avalonia.AvaloniaDemos
{
    public partial class MouseTracker : Window
    {
        private readonly Plottable.Crosshair Crosshair;

        public MouseTracker()
        {
            this.InitializeComponent();

            avaPlot1.Plot.AddSignal(DataGen.RandomWalk(null, 100));
            Crosshair = avaPlot1.Plot.AddCrosshair(0, 0);
            avaPlot1.Refresh();

            avaPlot1.PointerMoved += OnMouseMove;
            avaPlot1.PointerExited += OnMouseLeave;
            avaPlot1.PointerEntered += OnMouseEnter;
        }

        private void OnMouseMove(object sender, PointerEventArgs e)
        {
            int pixelX = (int)e.GetPosition(avaPlot1).X;
            int pixelY = (int)e.GetPosition(avaPlot1).Y;

            (double coordinateX, double coordinateY) = avaPlot1.GetMouseCoordinates();

            this.XPixelLabel.Text = $"{pixelX:0.000}";
            this.YPixelLabel.Text = $"{pixelY:0.000}";

            this.XCoordinateLabel.Text = $"{avaPlot1.Plot.GetCoordinateX(pixelX):0.00000000}";
            this.YCoordinateLabel.Text = $"{avaPlot1.Plot.GetCoordinateY(pixelY):0.00000000}";

            Crosshair.X = coordinateX;
            Crosshair.Y = coordinateY;

            avaPlot1.Refresh();
        }

        private void OnMouseEnter(object sender, PointerEventArgs e)
        {
            this.MouseTrackerMessage.Text = "Mouse ENTERED the plot";

            Crosshair.IsVisible = true;
        }

        private void OnMouseLeave(object sender, PointerEventArgs e)
        {
            this.MouseTrackerMessage.Text = "Mouse LEFT the plot";
            this.XPixelLabel.Text = "--";
            this.YPixelLabel.Text = "--";
            this.XCoordinateLabel.Text = "--";
            this.YCoordinateLabel.Text = "--";

            Crosshair.IsVisible = false;
            avaPlot1.Refresh();
        }
    }
}
