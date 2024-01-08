using ScottPlot;

namespace Sandbox.WinForms;

public partial class Form1 : Form
{
    public Form1()
    {
        InitializeComponent();

        double[,] data = SampleData.MonaLisa();
        var hm = formsPlot1.Plot.Add.Heatmap(data);

        formsPlot1.Refresh();

        formsPlot1.MouseMove += (s, e) =>
        {
            Coordinates coordinates = formsPlot1.Plot.GetCoordinates(e.X, e.Y);
            (int x, int y) = hm.GetIndexes(coordinates);
            double value = hm.GetValue(coordinates);

            Text = double.IsNaN(value)
                ? "None"
                : $"data[{y}, {x}] = {value}";
        };
    }
}
