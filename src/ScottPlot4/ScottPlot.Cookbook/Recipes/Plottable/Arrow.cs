namespace ScottPlot.Cookbook.Recipes.Plottable
{
    public class Arrow : IRecipe
    {
        public ICategory Category => new Categories.PlotTypes.Arrow();
        public string ID => "plottable_arrow_quickstart";
        public string Title => "Arrows";
        public string Description =>
            "Arrows point to specific locations on the plot. ";

        public void ExecuteRecipe(Plot plt)
        {
            // plot some sample data
            plt.AddSignal(DataGen.Sin(51));

            // add arrows using coordinates
            plt.AddArrow(25, 0, 27, .2);

            // you can define a minimum length so the line persists even when zooming out
            var arrow2 = plt.AddArrow(27, -.25, 23, -.5);
            arrow2.Color = System.Drawing.Color.Red;
            arrow2.MinimumLengthPixels = 100;

            // the shape of the arrowhead can be adjusted
            var skinny = plt.AddArrow(12, 1, 12, .5);
            skinny.Color = System.Drawing.Color.Green;
            skinny.ArrowheadLength = 5;
            skinny.ArrowheadWidth = 2;

            var fat = plt.AddArrow(20, .6, 20, 1);
            fat.Color = System.Drawing.Color.Blue;
            fat.ArrowheadLength = 2;
            fat.ArrowheadWidth = 5;

            // a marker can be drawn at the base of the arrow
            var arrow3 = plt.AddArrow(30, -.58, 35, -.4);
            arrow3.MarkerSize = 15;
        }
    }
}
