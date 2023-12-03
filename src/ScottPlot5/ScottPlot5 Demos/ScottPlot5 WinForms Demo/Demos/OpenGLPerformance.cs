using ScottPlot;

namespace WinForms_Demo.Demos;

public partial class OpenGLPerformance : Form, IDemoWindow
{
    public string Title => "OpenGL Performance";

    public string Description => "Compare CPU vs. GPU rendered plot controls";

    public OpenGLPerformance()
    {
        InitializeComponent();

        cbDataType.Items.Add("Random Walk");
        cbDataType.Items.Add("Sine Wave");
        cbDataType.Items.Add("Random Values");
        cbDataType.SelectedIndex = 0;
        cbDataType.SelectedIndexChanged += (s, e) => UpdatePlots();

        cbPointCount.Items.Add("1k");
        cbPointCount.Items.Add("10k");
        cbPointCount.Items.Add("100k");
        cbPointCount.Items.Add("1M");
        cbPointCount.SelectedIndex = 1;
        cbPointCount.SelectedIndexChanged += (s, e) => UpdatePlots();

        cbPlotType.Items.Add("Signal Plot");
        cbPlotType.Items.Add("Scatter Plot");
        cbPlotType.SelectedIndex = 0;
        cbPlotType.SelectedIndexChanged += (s, e) => UpdatePlots();

        formsPlot1.Plot.Title("FormsPlot");
        formsPlot1.Plot.Benchmark.IsVisible = true;

        formsPlotgl1.Plot.Title("FormsPlotGL");
        formsPlot1.Plot.Benchmark.IsVisible = true;

        UpdatePlots();
    }

    private void UpdatePlots()
    {
        string countString = cbPointCount.Text.Replace("k", "000").Replace("M", "000000");
        int count = int.Parse(countString);

        double[] data = cbDataType.Text switch
        {
            "Random Walk" => Generate.RandomWalk(count),
            "Sine Wave" => Generate.Sin(count),
            "Random Values" => Generate.Random(count),
            _ => throw new NotImplementedException(),
        };

        formsPlot1.Plot.Clear();
        formsPlotgl1.Plot.Clear();

        if (cbPlotType.Text == "Signal Plot")
        {
            formsPlot1.Plot.Add.Signal(data);
            formsPlotgl1.Plot.Add.Signal(data);
        }
        else
        {
            double[] xs = Generate.Consecutive(count);

            var sp1 = formsPlot1.Plot.Add.Scatter(xs, data);
            sp1.MarkerStyle.IsVisible = false;

            var sp2 = formsPlotgl1.Plot.Add.Scatter(xs, data);
            sp2.MarkerStyle.IsVisible = false;
        }

        formsPlot1.Plot.AutoScale();
        formsPlotgl1.Plot.AutoScale();

        formsPlot1.Refresh();
        formsPlotgl1.Refresh();
    }
}
