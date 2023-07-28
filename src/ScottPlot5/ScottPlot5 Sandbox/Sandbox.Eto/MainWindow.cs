using Eto.Forms;
using ScottPlot;

namespace Sandbox.Eto;

partial class MainWindow : Form
{
    public MainWindow()
    {
        InitializeComponent();

        var crosshair = EtoPlot1.Plot.Add.Crosshair(0, 0);

        EtoPlot1.MouseMove += (s, e) =>
        {
            Pixel mousePixel = new(e.Location.X, e.Location.Y);
            Coordinates mouseCoordinates = EtoPlot1.GetCoordinates(mousePixel);
            crosshair.Position = mouseCoordinates;
            EtoPlot1.Refresh();
        };
    }
}
