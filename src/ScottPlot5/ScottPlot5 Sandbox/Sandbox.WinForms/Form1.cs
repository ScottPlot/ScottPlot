using ScottPlot;

namespace Sandbox.WinForms;

public partial class Form1 : Form
{
    public Form1()
    {
        InitializeComponent();

        //formsPlot1.Plot.InteractionStarted += (s, e) => Text = $"{e} pressed";
        //formsPlot1.Plot.InteractionMoved += (s, e) => Text = $"{e} moved";
        //formsPlot1.Plot.InteractionEnded += (s, e) => Text = $"{e} dropped";

        for (int i = 0; i < 5; i++)
        {
            CoordinateLine line = Generate.RandomCoordinateLine();
            formsPlot1.Plot.Add.InteractiveLine(line);
        }
    }
}
