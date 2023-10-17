using ScottPlot;

namespace Sandbox.WinForms;

public partial class Form1 : Form
{
    public Form1()
    {
        InitializeComponent();

        List<double> values = new();

        int pointsPerStep = 1_000;
        int stepCount = 50;

        for (int i = 0; i < stepCount; i++)
        {
            for (int j = 0; j < pointsPerStep; j++)
            {
                values.Add(i % 2);
            }
        }

        formsPlot1.Plot.Add.Signal(values);
    }
}
