using ScottPlot;

namespace Sandbox.WinForms;

public partial class Form1 : Form
{
    public Form1()
    {
        InitializeComponent();

        int pointsPerPlot = 1024;
        int numberOfPlots = 1_000;

        for (int n = 0; n < numberOfPlots; n++)
        {
            double[] dataX = Generate.Consecutive(pointsPerPlot);
            double[] dataY = Generate.RandomWalk(pointsPerPlot);

            var scatter = formsPlot1.Plot.Add.Scatter(dataX, dataY);
            scatter.MarkerStyle.IsVisible = false;
            scatter.LineStyle.Width = 1.0F;
            scatter.LineStyle.Pattern = LinePattern.Solid;
            scatter.LineStyle.AntiAlias = true;
        }

        formsPlot1.Plot.RenderManager.RenderFinished += (s, e) =>
        {
            Text = formsPlot1.Plot.LastRender.Elapsed.ToString();
        };
    }
}
