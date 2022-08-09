using ScottPlot.Plottables;
using System.Windows.Forms;

namespace ScottPlot.Sandbox.WinForms;

public partial class Form1 : Form
{
    readonly DebugPoint DebugPoint = new();

    public Form1()
    {
        InitializeComponent();

        const int N = 51;

        formsPlot1.Plot.Add(DebugPoint);
        formsPlot1.Plot.AddScatter(Generate.Consecutive(N), Generate.Sin(N));
        formsPlot1.Plot.AddScatter(Generate.Consecutive(N), Generate.Cos(N));
        formsPlot1.Plot.AddScatter(Generate.Consecutive(N), Generate.Sin(N, 0.5));

        formsPlot1.MouseMove += FormsPlot1_MouseMove;
    }

    private void FormsPlot1_MouseMove(object? sender, MouseEventArgs e)
    {
        Text = e.X.ToString();
        DebugPoint.Position = formsPlot1.Interaction.GetMouseCoordinates();
        formsPlot1.Refresh();
    }
}
