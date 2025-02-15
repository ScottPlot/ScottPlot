namespace ScottPlotCookbook.Recipes.PlotTypes;

public class SmithChart : ICategory
{
    public Chapter Chapter => Chapter.PlotTypes;
    public string CategoryName => "Smith Chart";
    public string CategoryDescription => "Create a Smith chart axis and add it to the plot to display " +
        "impedance of RF signals using a horizontal axis indicating resistance " +
        "and vertical axis indicating reactance.";

    public class SmithChartQuickstart : RecipeBase
    {
        public override string Name => "Smith Chart Quickstart";
        public override string Description => "Add a Smith chart to the plot and use its methods " +
            "to translate impedance to Cartesian coordinates that can be used for placing other plot components.";

        [Test]
        public override void Execute()
        {
            var smith = myPlot.Add.SmithChartAxis();

            // translate an impedance location on the Smith chart to a 2D location on the plot
            double resistance = 0.2;
            double reactance = -0.5;
            Coordinates location = smith.GetCoordinates(resistance, reactance);

            // use that location to add traditional plot components
            myPlot.Add.Marker(location, MarkerShape.FilledCircle, size: 15, Colors.Red);
            var txt = myPlot.Add.Text("0.2 - j 0.5", location);
            txt.LabelStyle.FontSize = 24;
            txt.LabelStyle.Bold = true;
            txt.LabelStyle.ForeColor = Colors.Red;
        }
    }
}
