using ScottPlot;

namespace Sandbox.WinForms;

public partial class Form1 : Form
{
    public Form1()
    {
        InitializeComponent();

        var crosshair = formsPlot1.Plot.Add.Crosshair(0, 0);

        formsPlot1.MouseMove += (s, e) =>
        {
            Pixel mousePixel = new(e.X, e.Y);
            Coordinates mouseCoordinates = formsPlot1.GetCoordinates(mousePixel);
            crosshair.Position = mouseCoordinates;
            formsPlot1.Refresh();
        };
    }
}
