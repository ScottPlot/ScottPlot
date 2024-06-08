namespace ScottPlotCookbook.Recipes.PlotTypes;

public class Images : ICategory
{
    public string Chapter => "Plot Types";
    public string CategoryName => "Images";
    public string CategoryDescription => "Images can be placed on plots in a variety of ways";

    public class ImageRectQuickstart : RecipeBase
    {
        public override string Name => "Image Rectangle";
        public override string Description => "An image can be drawn inside a rectangle defined in coordinate units.";

        [Test]
        public override void Execute()
        {
            // Images may be loaded from files or created dynamically
            ScottPlot.Image img = ScottPlot.SampleImages.MonaLisa();

            CoordinateRect rect = new(left: 0, right: img.Width, bottom: 0, top: img.Height);

            myPlot.Add.ImageRect(img, rect);
        }
    }
}
