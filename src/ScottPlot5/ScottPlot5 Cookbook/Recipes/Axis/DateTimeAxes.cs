namespace ScottPlotCookbook.Recipes.Axis;

public class DateTimeAxes : ICategory
{
    public string Chapter => "Axis";
    public string CategoryName => "DateTime Axes";
    public string CategoryDescription => "Plot data values on a DataTime axes";

    public class DateTimeAxisQuickstart : RecipeBase
    {
        public override string Name => "DateTime Axis Quickstart";
        public override string Description => "Axis tick labels can be displayed using a time format.";

        [Test]
        public override void Execute()
        {
            // plot data using DateTime units
            DateTime[] dates = Generate.DateTime.Days(100);
            double[] ys = Generate.RandomWalk(100);
            myPlot.Add.Scatter(dates, ys);

            // tell the plot to display dates on the bottom axis
            myPlot.Axes.DateTimeTicksBottom();
        }
    }

    public class DateTimeAxisMixed : RecipeBase
    {
        public override string Name => "DateTime Axis Values";
        public override string Description => "DateTime axes are achieved using Microsoft's " +
            "DateTime.ToOADate() and DateTime.FromOADate() methods to convert between " +
            "dates and numeric values. Advanced users who wish to display data on DateTime axes " +
            "may prefer to work with collections of doubles rather than collections of DateTimes.";

        [Test]
        public override void Execute()
        {
            // create an array of DateTimes one hour apart
            int numberOfHours = 24;
            DateTime[] dateTimes = new DateTime[numberOfHours];
            DateTime startDateTime = new(2024, 1, 1);
            TimeSpan deltaTimeSpan = TimeSpan.FromHours(1);
            for (int i = 0; i < numberOfHours; i++)
            {
                dateTimes[i] = startDateTime + i * deltaTimeSpan;
            }

            // create an array of doubles representing the same DateTimes one hour apart
            double[] dateDoubles = new double[numberOfHours];
            double startDouble = startDateTime.ToOADate(); // days since 1900
            double deltaDouble = 1.0 / 24.0; // an hour is 1/24 of a day
            for (int i = 0; i < numberOfHours; i++)
            {
                dateDoubles[i] = startDouble + i * deltaDouble;
            }

            // now both arrays represent the same dates
            myPlot.Add.Scatter(dateTimes, Generate.Sin(numberOfHours));
            myPlot.Add.Scatter(dateDoubles, Generate.Cos(numberOfHours));
            myPlot.Axes.DateTimeTicksBottom();
        }
    }

    public class DateTimeAxisCustomFormatter : RecipeBase
    {
        public override string Name => "Custom DateTime Label Format";
        public override string Description => "Users can provide their own logic for customizing DateTime tick labels";

        [Test]
        public override void Execute()
        {
            // plot sample DateTime data
            DateTime[] dates = Generate.DateTime.Days(100);
            double[] ys = Generate.RandomWalk(100);
            myPlot.Add.Scatter(dates, ys);
            myPlot.Axes.DateTimeTicksBottom();

            // add logic into the RenderStarting event to customize tick labels
            myPlot.RenderManager.RenderStarting += (s, e) =>
            {
                Tick[] ticks = myPlot.Axes.Bottom.TickGenerator.Ticks;
                for (int i = 0; i < ticks.Length; i++)
                {
                    DateTime dt = DateTime.FromOADate(ticks[i].Position);
                    string label = $"{dt:MMM} '{dt:yy}";
                    ticks[i] = new Tick(ticks[i].Position, label);
                }
            };
        }
    }
}
