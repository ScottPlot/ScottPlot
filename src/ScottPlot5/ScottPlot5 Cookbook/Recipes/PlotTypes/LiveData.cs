namespace ScottPlotCookbook.Recipes.PlotTypes;

public class LiveData : ICategory
{
    public Chapter Chapter => Chapter.PlotTypes;
    public string CategoryName => "Live Data";
    public string CategoryDescription => "Plottables like DataLogger and DataStreamer " +
        "are designed for displaying datasets that change in real time. " +
        "They have the ability to control axis limits to ensure the latest data is always in view. " +
        "See the ScottPlot Demo for live example of these plot types.";

    public class DataLoggerQuickstart : RecipeBase
    {
        public override string Name => "DataLogger Quickstart";
        public override string Description => "Use a DataLogger to display growing datasets " +
            "(such as sensor data). This plot type assumes that new data will always be added " +
            "to the end of the existing data, so like SignalXY new data points must " +
            "have an X value that is greater to or than or equal than the previous one. " +
            "See the ScottPlot Demo for a live example of this plot type.";

        [Test]
        public override void Execute()
        {
            // setup a logger that will grow as data is added
            var logger = myPlot.Add.DataLogger();

            // simulate live data streaming in
            for (int x = 0; x < 10; x++)
            {
                double y = Generate.RandomWalker.Next();
                logger.Add(x, y);
            }
        }
    }

    public class DataStreamerQuickstart : RecipeBase
    {
        public override string Name => "DataStreamer Quickstart";
        public override string Description => "Use a DataStreamer to display streaming data " +
            "using a fixed-length display with a fixed horizontal distance between points. " +
            "This type of plot is ideal for signals like ECG (heart monitor) waveforms. " +
            "This plot type has advanced customizations for controlling how new data replaces " +
            "old data (e.g., slide the old data to the left as new data appears on the right or " +
            "place new data values from left to right, then wrap around to the start and " +
            "wipe away the oldest data values by replacing them from left to right again). " +
            "See the ScottPlot Demo for a live example of this plot type.";

        [Test]
        public override void Execute()
        {
            // setup a streamer that shows the latest 100 points
            var streamer = myPlot.Add.DataStreamer(100);

            // simulate live data streaming in.
            for (int x = 0; x < 123; x++)
            {
                double y = Generate.RandomWalker.Next();
                streamer.Add(y);
            }

            // tell new data to overwrite old data from left to right
            streamer.ViewWipeRight();
        }
    }

    public class DataLoggerEditing : RecipeBase
    {
        public override string Name => "DataLogger Editing";
        public override string Description => "Values accumulated by a data logger " +
            "may be edited after they are acquired.";

        [Test]
        public override void Execute()
        {
            // setup a logger that will grow as data is added
            var logger = myPlot.Add.DataLogger();

            // add ten values
            logger.Add(Generate.RandomSample(10));

            // remove the oldest five values
            logger.Data.Coordinates.RemoveRange(0, 5);
        }
    }
}
