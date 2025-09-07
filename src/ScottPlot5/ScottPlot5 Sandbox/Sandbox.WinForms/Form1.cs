using ScottPlot;

namespace Sandbox.WinForms;

public partial class Form1 : Form
{
    public Form1()
    {
        InitializeComponent();

        formsPlot1.Plot.HandlePressed += (s, e) => Text = $"{e} pressed";
        formsPlot1.Plot.HandleMoved += (s, e) => Text = $"{e} moved";
        formsPlot1.Plot.HandleReleased += (s, e) => Text = $"{e} dropped";
        formsPlot1.Plot.HandleHoverChanged += (s, e) => Text = e is null ? "" : $"{e} hovered";

        for (int i = 0; i < 5; i++)
        {
            CoordinateLine line = Generate.RandomCoordinateLine();
            formsPlot1.Plot.Add.InteractiveLine(line);
        }
    }
}
