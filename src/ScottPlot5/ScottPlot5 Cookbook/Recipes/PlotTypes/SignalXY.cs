namespace ScottPlotCookbook.Recipes.PlotTypes;

public class SignalXY : ICategory
{
    public string Chapter => "Plot Types";
    public string CategoryName => "SignalXY Plot";
    public string CategoryDescription => "SignalXY are a high performance plot type " +
        "optimized for X/Y pairs where the X values are always ascending. " +
        "For large datasets SignalXY plots are much more performant than " +
        "Scatter plots (which allow unordered X points) but not as performant " +
        "as Signal plots (which require fixed spacing between X points).";

    public class SignalXYQuickstart : RecipeBase
    {
        public override string Name => "SignalXY Quickstart";
        public override string Description => "SignalXY plots are a high performance plot type " +
            "for X/Y data where the X values are always ascending.";

        [Test]
        public override void Execute()
        {
            // generate sample data with gaps
            List<double> xList = new();
            List<double> yList = new();
            for (int i = 0; i < 5; i++)
            {
                xList.AddRange(Generate.Consecutive(1000, first: 2000 * i));
                yList.AddRange(Generate.RandomSample(1000));
            }
            double[] xs = xList.ToArray();
            double[] ys = yList.ToArray();

            // add a SignalXY plot
            myPlot.Add.SignalXY(xs, ys);
        }
    }

    public class SignalXYGeneric : RecipeBase
    {
        public override string Name => "SignalXY Generic";
        public override string Description => "SignalXY plots support generic data types, " +
            "although double is typically the most performant.";

        [Test]
        public override void Execute()
        {
            // generate sample data with gaps
            List<int> xList = new();
            List<float> yList = new();
            for (int i = 0; i < 5; i++)
            {
                xList.AddRange(Generate.Consecutive(1000, first: 2000 * i).Select(x => (int)x));
                yList.AddRange(Generate.RandomSample(1000).Select(x => (float)x));
            }
            int[] xs = xList.ToArray();
            float[] ys = yList.ToArray();

            // add a SignalXY plot
            myPlot.Add.SignalXY(xs, ys);
        }
    }
}
