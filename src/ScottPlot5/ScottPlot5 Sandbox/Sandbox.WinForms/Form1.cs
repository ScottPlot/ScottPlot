using ScottPlot;

namespace Sandbox.WinForms;

public partial class Form1 : Form
{
    public Form1()
    {
        InitializeComponent();

        formsPlot1.Plot.NodePressed += (s, e) => Text = $"{e} pressed";
        formsPlot1.Plot.NodeMoved += (s, e) => Text = $"{e} moved";
        formsPlot1.Plot.NodeReleased += (s, e) => Text = $"{e} dropped";
        formsPlot1.Plot.NodeHoverChanged += (s, e) => Cursor = e is null ? Cursors.Default : Cursors.Hand;

        for (int i = 0; i < 5; i++)
        {
            CoordinateLine line = Generate.RandomCoordinateLine();
            formsPlot1.Plot.Add.InteractiveLine(line);
        }
    }
}
