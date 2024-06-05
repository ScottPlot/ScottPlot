using ScottPlot.WinForms;

namespace WinForms_Demo.Demos;

public partial class PlotViewer : Form, IDemoWindow
{
    public string Title => "Plot Viewer";

    public string Description => "A Plot can be created programmatically and " +
        "displayed in a pop-up window. This strategy can be used to launch mouse-interactive " +
        "plots from console applications if the ScottPlot.WinForms package is included.";

    public PlotViewer()
    {
        InitializeComponent();

        button1.Click += (s, e) =>
        {
            for (int i = 0; i < numericUpDown1.Value; i++)
            {
                // create a random data
                int count = ScottPlot.Generate.RandomInteger(5, 20);
                double[] ys = ScottPlot.Generate.RandomWalk(count);
                double[] xs = ScottPlot.Generate.Consecutive(count);

                // create plot programmatically
                ScottPlot.Plot myPlot = new();
                var mp = myPlot.Add.Scatter(xs, ys);
                mp.MarkerShape = ScottPlot.Generate.RandomMarkerShape();
                mp.MarkerSize = (float)ScottPlot.Generate.RandomNumber(25, 50);
                mp.MarkerStyle.LineWidth = 3;
                mp.Color = new ScottPlot.Colormaps.Turbo().GetColor(ScottPlot.Generate.RandomNumber());

                // Create a plot viewer form and display it to the user
                Form viewerForm = FormsPlotViewer.CreateForm(myPlot);
                viewerForm.Text = $"Plot with {numericUpDown1.Value} lines";
                viewerForm.StartPosition = FormStartPosition.WindowsDefaultLocation;
                viewerForm.Show();
            }
        };
    }
}
