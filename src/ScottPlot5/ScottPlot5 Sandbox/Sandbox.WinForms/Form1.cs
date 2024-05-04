using ScottPlot;

namespace Sandbox.WinForms;

public partial class Form1 : Form
{
    public Form1()
    {
        InitializeComponent();

        var mrk = formsPlot1.Plot.Add.Marker(5, 5);
        mrk.Size = 100;
        mrk.Color = Colors.Black.WithAlpha(0.5f);
        mrk.MarkerLineWidth = 2;
        mrk.MarkerLineColor = Colors.Black;
        mrk.Shape = MarkerShape.FilledCircle;
    }
}
