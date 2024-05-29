using ScottPlot.TickGenerators;
using ScottPlot.TickGenerators.TimeUnits;

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
            DateTime[] dates = Generate.ConsecutiveDays(100);
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

            // add padding on the right to make room for wide tick labels
            myPlot.Axes.Right.MinimumSize = 50;
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
            DateTime[] dates = Generate.ConsecutiveDays(100);
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

    public class DateTimeAxisFixedIntervalTicks : RecipeBase
    {
        public override string Name => "DateTime Axis Fixed Interval Ticks";
        public override string Description => "Make ticks render at fixed intervals. Optionally make the ticks render " +
            "from a custom start date, rather than using the start date of the plot (e.g. to draw ticks on the hour " +
            "every hour, or on the first of every month, etc).";

        [Test]
        public override void Execute()
        {
            // Plot 24 hours sample DateTime data (1 point every minute)
            DateTime[] dates = Generate.ConsecutiveMinutes(24 * 60, new DateTime(2000, 1, 1, 2, 12, 0));
            double[] ys = Generate.RandomWalk(24 * 60);
            myPlot.Add.Scatter(dates, ys);
            var dtAx = myPlot.Axes.DateTimeTicksBottom();

            // Create fixed-intervals ticks, major ticks every 6 hours, minor ticks every hour
            dtAx.TickGenerator = new DateTimeFixedInterval(
                new Hour(), 6,
                new Hour(), 1,
                // Here we provide a delegate to override when the ticks start. In this case, we want the majors to be
                // 00:00, 06:00, 12:00, etc. and the minors to be on the hour, every hour, so we start at midnight.
                // If you do not provide this delegate, the ticks will start at whatever the Min on the x-axis is.
                // The major ticks might end up as 1:30am, 7:30am, etc, and the tick positions will be fixed on the plot
                // when it is panned around.
                dt => new DateTime(dt.Year, dt.Month, dt.Day));

            // Customise gridlines to make the ticks easier to see
            myPlot.Grid.XAxisStyle.MajorLineStyle.Color = Colors.Black.WithOpacity();
            myPlot.Grid.XAxisStyle.MajorLineStyle.Width = 2;

            myPlot.Grid.XAxisStyle.MinorLineStyle.Color = Colors.Gray.WithOpacity(0.25);
            myPlot.Grid.XAxisStyle.MinorLineStyle.Width = 1;
            myPlot.Grid.XAxisStyle.MinorLineStyle.Pattern = LinePattern.DenselyDashed;

            // Remove labels on minor ticks, otherwise there is a lot of tick label overlap
            myPlot.RenderManager.RenderStarting += (s, e) =>
            {
                Tick[] ticks = myPlot.Axes.Bottom.TickGenerator.Ticks;
                for (int i = 0; i < ticks.Length; i++)
                {
                    if (ticks[i].IsMajor)
                    {
                        continue;
                    }

                    ticks[i] = new Tick(ticks[i].Position, "", ticks[i].IsMajor);
                }
            };
        }
    }
}
