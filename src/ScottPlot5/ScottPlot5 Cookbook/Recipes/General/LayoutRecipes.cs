namespace ScottPlotCookbook.Recipes.Miscellaneous;

public class Layout : ICategory
{
    public Chapter Chapter => Chapter.General;
    public string CategoryName => "Layout";
    public string CategoryDescription => "How to customize data area size and figure padding";

    public class Frameless : RecipeBase
    {
        public override string Name => "Frameless Plot";
        public override string Description => "How to create a plot containing only the data area and no axes.";

        [Test]
        public override void Execute()
        {
            myPlot.Add.Signal(Generate.Sin(51));
            myPlot.Add.Signal(Generate.Cos(51));

            // make the data area cover the full figure
            myPlot.Layout.Frameless();

            // set the data area background so we can observe its size
            myPlot.DataBackground.Color = Colors.WhiteSmoke;
        }
    }

    public class FixedPadding : RecipeBase
    {
        public override string Name => "Fixed Padding";
        public override string Description => "The plot can be arranged to achieve " +
            "a fixed amount of padding on each side of the data area";

        [Test]
        public override void Execute()
        {
            // add sample data to the plot
            myPlot.Add.Signal(Generate.Sin(51));
            myPlot.Add.Signal(Generate.Cos(51));

            // use a fixed amount of of pixel padding on each side
            PixelPadding padding = new(100, 50, 100, 50);
            myPlot.Layout.Fixed(padding);

            // darken the figure background so we can observe its dimensions
            myPlot.FigureBackground.Color = Colors.LightBlue;
            myPlot.DataBackground.Color = Colors.White;
        }
    }

    public class FixedRectangle : RecipeBase
    {
        public override string Name => "Fixed Rectangle";
        public override string Description => "The plot can be arranged so the data is " +
            "drawn inside a fixed rectangle defined in pixel units";

        [Test]
        public override void Execute()
        {
            // add sample data to the plot
            myPlot.Add.Signal(Generate.Sin(51));
            myPlot.Add.Signal(Generate.Cos(51));

            // set the data area to render inside a fixed rectangle
            PixelSize size = new(300, 200);
            Pixel offset = new(50, 50);
            PixelRect rect = new(size, offset);
            myPlot.Layout.Fixed(rect);

            // darken the figure background so we can observe its dimensions
            myPlot.FigureBackground.Color = Colors.LightBlue;
            myPlot.DataBackground.Color = Colors.White;
        }
    }
}
