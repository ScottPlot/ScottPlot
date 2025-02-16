namespace ScottPlotCookbook.Recipes.Introduction;

public class AxisAndTicks : ICategory
{
    public Chapter Chapter => Chapter.General;
    public string CategoryName => "Axis and Ticks";
    public string CategoryDescription => "Examples of common customizations for axis labels and ticks";

    public class SetAxisLimits : RecipeBase
    {
        public override string Name => "Set Axis Limits";
        public override string Description => "Axis Limits can be set by the user.";

        [Test]
        public override void Execute()
        {
            myPlot.Add.Signal(Generate.Sin(51));
            myPlot.Add.Signal(Generate.Cos(51));

            myPlot.Axes.SetLimits(-100, 150, -5, 5);
        }
    }

    public class GetAxisLimits : RecipeBase
    {
        public override string Name => "Read Axis Limits";
        public override string Description => "Use GetLimits() to obtain the current axis limits.";

        [Test]
        public override void Execute()
        {
            myPlot.Add.Signal(Generate.Sin(51));
            myPlot.Add.Signal(Generate.Cos(51));

            AxisLimits limits = myPlot.Axes.GetLimits();
            double xMin = limits.Left;
            double xMax = limits.Right;
            double yMin = limits.Bottom;
            double yMax = limits.Top;
        }
    }

    public class AutoScale : RecipeBase
    {
        public override string Name => "AutoScale Axis Limits to Fit Data";
        public override string Description => "The axis limits can be automatically adjusted to fit the data. " +
            "Optional arguments allow users to define the amount of whitespace around the edges of the data." +
            "In older versions of ScottPlot this functionality was achieved by a method named AxisAuto().";

        [Test]
        public override void Execute()
        {
            myPlot.Add.Signal(Generate.Sin(51));
            myPlot.Add.Signal(Generate.Cos(51));

            // set limits that do not fit the data
            myPlot.Axes.SetLimits(-100, 150, -5, 5);

            // reset limits to fit the data
            myPlot.Axes.AutoScale();
        }
    }

    public class InvertedAxis : RecipeBase
    {
        public override string Name => "Inverted Axis";
        public override string Description => "Users can display data on an inverted axis " +
            "by setting axis limits setting the lower edge to a value more positive than the upper edge.";

        [Test]
        public override void Execute()
        {
            myPlot.Add.Signal(Generate.Sin());
            myPlot.Add.Signal(Generate.Cos());

            myPlot.Axes.SetLimitsY(bottom: 1.5, top: -1.5);
        }
    }

    public class InvertedAutoAxis : RecipeBase
    {
        public override string Name => "Inverted Auto-Axis";
        public override string Description => "Customize the logic for the " +
            "automatic axis scaler to ensure that axis limits " +
            "for a particular axis are always inverted when autoscaled.";

        [Test]
        public override void Execute()
        {
            myPlot.Add.Signal(Generate.Sin());
            myPlot.Add.Signal(Generate.Cos());

            myPlot.Axes.AutoScaler.InvertedY = true;
        }
    }

    public class SquareAxisUnits : RecipeBase
    {
        public override string Name => "Square Axis Units";
        public override string Description => "Axis rules can be put in place which " +
            "force the vertical scale (units per pixel) to match the horizontal scale " +
            "so circles always appear as circles and not stretched ellipses.";

        [Test]
        public override void Execute()
        {
            myPlot.Add.Circle(0, 0, 10);

            // force pixels to have a 1:1 scale ratio
            myPlot.Axes.SquareUnits();

            // even if you try to "stretch" the axis, it will adjust the axis limits automatically
            myPlot.Axes.SetLimits(-10, 10, -20, 20);
        }
    }

    public class ExperimentalAxisWithSubtitle : RecipeBase
    {
        public override string Name => "Axis with Subtitle";

        public override string Description => "Users can create their own fully custom " +
            "axes to replace the default ones (as demonstrated in the demo app). " +
            "Some experimental axes are available for users who may be interested in " +
            "alternative axis display styles.";

        [Test]
        public override void Execute()
        {
            // Plot some sample data
            myPlot.Add.Signal(Generate.Sin());
            myPlot.Add.Signal(Generate.Cos());

            // Instantiate a custom axis and customize it as desired
            ScottPlot.AxisPanels.Experimental.LeftAxisWithSubtitle customAxisY = new()
            {
                LabelText = "My Custom Y Axis",
                SubLabelText = "It comes with a subtitle for the axis"
            };

            // Remove the default Y axis and add the custom one to the plot
            myPlot.Axes.Remove(myPlot.Axes.Left);
            myPlot.Axes.AddLeftAxis(customAxisY);
        }
    }

    public class AxisAntiAliasing : RecipeBase
    {
        public override string Name => "Axis AntiAliasing";
        public override string Description => "To improve crispness of straight vertical and horizontal lines, " +
            "Anti-aliasing is disabled by default for axis frames, tick marks, and grid lines. Anti-aliasing " +
            "can be enabled for all these objects by calling the AntiAlias helper method.";

        [Test]
        public override void Execute()
        {
            double[] dataX = { 1, 2, 3, 4, 5 };
            double[] dataY = { 1, 4, 9, 16, 25 };
            myPlot.Add.Scatter(dataX, dataY);

            myPlot.Axes.AntiAlias(true);
        }
    }

    public class HideAxis : RecipeBase
    {
        public override string Name => "Hide Axis and Turn frame lines on/off";
        public override string Description =>
            "Demonstrates how to hide axis ticks and turn frame lines on and off.";

        [Test]
        public override void Execute()
        {
            myPlot.Add.Signal(Generate.Sin());
            myPlot.Add.Signal(Generate.Cos());

            // Hide axis label and tick
            myPlot.Axes.Bottom.TickLabelStyle.IsVisible = false;
            myPlot.Axes.Bottom.MajorTickStyle.Length = 0;
            myPlot.Axes.Bottom.MinorTickStyle.Length = 0;

            // Hide axis edge line
            myPlot.Axes.Bottom.FrameLineStyle.Width = 0;
            myPlot.Axes.Right.FrameLineStyle.Width = 0;
            myPlot.Axes.Top.FrameLineStyle.Width = 0;
        }
    }

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
            dtAx.TickGenerator = new ScottPlot.TickGenerators.DateTimeFixedInterval(
                new ScottPlot.TickGenerators.TimeUnits.Hour(), 6,
                new ScottPlot.TickGenerators.TimeUnits.Hour(), 1,
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

    public class FloatingAxis : RecipeBase
    {
        public override string Name => "Floating Axis";
        public override string Description => "A floating or centered axis may be realized by " +
            "hiding the default axes which appear at the edges of the plot and creating a new " +
            "floating axis and adding it to the plot.";

        [Test]
        public override void Execute()
        {
            // create floating X and Y axes using one of the existing axes for reference
            ScottPlot.Plottables.FloatingAxis floatingX = new(myPlot.Axes.Bottom);
            ScottPlot.Plottables.FloatingAxis floatingY = new(myPlot.Axes.Left);

            // hide the default axes and add the custom ones to the plot
            myPlot.Axes.Frameless();
            myPlot.HideGrid();
            myPlot.Add.Plottable(floatingX);
            myPlot.Add.Plottable(floatingY);

            // add sample data last so it appears on top
            myPlot.Add.Signal(Generate.Sin(51));
            myPlot.Add.Signal(Generate.Cos(51));
        }
    }

    public class CustomGridLineStyle : RecipeBase
    {
        public override string Name => "Grid Line Style";
        public override string Description => "Grid lines have many options " +
            "to allow extensive customization.";

        [Test]
        public override void Execute()
        {
            myPlot.Add.Signal(Generate.Sin(51));
            myPlot.Add.Signal(Generate.Cos(51));

            myPlot.Grid.LineColor = Colors.Blue.WithAlpha(.2);
            myPlot.Grid.LinePattern = LinePattern.Dotted;
        }
    }

    public class ImageAxisLabel : RecipeBase
    {
        public override string Name => "Image Axis Label";
        public override string Description => "For cases where axis label font styling " +
            "does not provide the desired level of customization, a bitmap image may be " +
            "displayed as an axis label. This strategy allows rich text to be realized " +
            "using any third party tool that can render that text as a bitmap. It also " +
            "enables users to place icons or images in their axis labels.";

        [Test]
        public override void Execute()
        {
            myPlot.Add.Signal(Generate.Sin(51));
            myPlot.Add.Signal(Generate.Cos(51));

            // This array holds the bytes of a bitmap. Here it's generated,
            // but it could be a byte array read from a bitmap file on disk.
            byte[] bytes1 = SampleImages.NoisyText("Horiz", 150, 50).GetImageBytes();
            byte[] bytes2 = SampleImages.NoisyText("Vert", 150, 50).GetImageBytes();

            // Create a ScottPlot.Image from the bitmap bytes
            ScottPlot.Image img1 = new(bytes1);
            ScottPlot.Image img2 = new(bytes2);

            // Display the image for the bottom axis label
            myPlot.Axes.Bottom.Label.Image = img1;
            myPlot.Axes.Left.Label.Image = img2;
        }
    }

    public class MultiplierNotation : RecipeBase
    {
        public override string Name => "Multiplier Notation";
        public override string Description => "Numeric tick labels may be displayed using multiplier notation " +
            "(where tick labels are displayed using scientific notation with the eponent displayed in the corner of the plot). " +
            "A helper method is available to set-up multiplier notation with a single statement, but users can " +
            "interact with the object this method returns (not shown here) or inspect the code inside of that method " +
            "to learn how to achieve enhanced customization abilities.";

        [Test]
        public override void Execute()
        {
            // plot sample data with extremely large values
            double[] xs = Generate.RandomSample(50, -1e10, 1e10);
            double[] ys = Generate.RandomSample(50, -1e20, 1e20);
            myPlot.Add.Scatter(xs, ys);

            // enable multiplier notation on both primary axes
            myPlot.Axes.SetupMultiplierNotation(myPlot.Axes.Left);
            myPlot.Axes.SetupMultiplierNotation(myPlot.Axes.Bottom);
        }
    }
}
