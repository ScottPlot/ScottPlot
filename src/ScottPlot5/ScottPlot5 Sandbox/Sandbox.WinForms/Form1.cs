using ScottPlot;

namespace Sandbox.WinForms;

public partial class Form1 : Form
{
    public Form1()
    {
        InitializeComponent();

        double[] clean = Generate.SquareWave(low: 0, high: 5);
        formsPlot1.Plot.Add.Signal(clean);

        double[] noisy = Generate.SquareWave(low: 10, high: 15);
        Generate.AddNoiseInPlace(noisy, magnitude: .001);
        formsPlot1.Plot.Add.Signal(noisy);

        formsPlot1.Plot.Grids.Clear();
    }
}
