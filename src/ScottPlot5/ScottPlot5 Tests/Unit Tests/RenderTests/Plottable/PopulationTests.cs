namespace ScottPlotTests.RenderTests.Plottable;

public class PopulationTests
{
    [Test]
    public void Test_Population_Render()
    {
        Plot plot = new();

        List<double> tickPositions = [];
        List<string> tickLabels = [];

        for (int i = 0; i < 5; i++)
        {
            Population pop = new(Generate.RandomNormal(20, 5 + i));
            ScottPlot.Plottables.PopulationSymbol popSym = new(pop)
            {
                X = i,
            };
            popSym.FillStyle.Color = plot.Add.Palette.GetColor(i);
            plot.Add.Plottable(popSym);

            tickPositions.Add(i);
            tickLabels.Add($"Group {i + 1}");
        }

        plot.Axes.Margins(bottom: 0);

        plot.Axes.Bottom.SetTicks(tickPositions.ToArray(), tickLabels.ToArray());

        plot.SaveTestImage();
    }
}
