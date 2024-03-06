namespace ScottPlotCookbook.Recipes.Miscellaneous;

public class AdvancedStyling : ICategory
{
    public string Chapter => "Miscellaneous";
    public string CategoryName => "Advanced Styling";
    public string CategoryDescription => "Features for users seeking extensive customization options.";

    public class AdvancedStylingFigureBackground : RecipeBase
    {
        public override string Name => "Figure Background Image";
        public override string Description => "The background of the figure can be set to an image.";

        [Test]
        public override void Execute()
        {
            myPlot.Add.Signal(Generate.Sin());
            myPlot.Add.Signal(Generate.Cos());

            Image img = SampleImages.ScottPlotLogo(600, 400);
            myPlot.Style.DataBackgroundImage(img);
        }
    }
}
