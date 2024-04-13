using System;

namespace ScottPlot.Cookbook.Recipes.Plottable
{
    public class DataLoggerQuickstart : IRecipe
    {
        public ICategory Category => new Categories.PlotTypes.DataLogger();
        public string ID => "datalogger_quickstart";
        public string Title => "DataLogger";
        public string Description =>
            "A DataLogger is a plot type designed for growing datasets. " +
            "Unlike most other plot types, the DataLogger can automatically expand the axis limits " +
            "to accommodate new data as it is added.";

        public void ExecuteRecipe(Plot plt)
        {
            var logger = plt.AddDataLogger();

            for (int i = 0; i < 100; i++)
            {
                double x = i * .2;
                double y = Math.Sin(x);
                logger.Add(x, y); // data grows as new data is added
            }
        }
    }

    public class DataStreamerQuickstart : IRecipe
    {
        public ICategory Category => new Categories.PlotTypes.DataLogger();
        public string ID => "datastreamer_quickstart";
        public string Title => "DataStreamer";
        public string Description =>
            "A DataStreamer is a plot type designed for streaming datasets with a fixed length display " +
            "and even X spacing between Y data points. " +
            "As new data is shifted in, old data is shifted out, and the displayed trace is always the same size.";

        public void ExecuteRecipe(Plot plt)
        {
            var streamer = plt.AddDataStreamer(length: 20);
            streamer.SamplePeriod = 0.2;

            for (int i = 0; i < 100; i++)
            {
                double y = Math.Sin(i * .2);
                streamer.Add(y); // data remains same length as new data is shifted in
            }

            // Call different view methods to change the shift behavior
            streamer.ViewWipeRight(); // new data overwrites old data left to right
            streamer.ViewWipeLeft(); // new data overwrites old data right to left
            streamer.ViewScrollRight(); // new data is inserted on the left
            streamer.ViewScrollLeft(); // new data is inserted on the right
        }
    }
}
