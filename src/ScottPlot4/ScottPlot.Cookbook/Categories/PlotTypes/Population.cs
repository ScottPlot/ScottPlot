namespace ScottPlot.Cookbook.Categories.PlotTypes;

public class Population : ICategory
{
    public string Name => "Population";

    public string Folder => "plottable-population";

    public string Description => "The population plot makes it easy to display populations " +
        "as bar graphs, box-and-whisker plots, scattered values, or box plots and data points " +
        "side-by-side. The population plot is different than using a box plot with an error bar " +
        "in that you pass your original data into the population plot and it determines the " +
        "standard deviation, standard error, quartiles, mean, median, outliers, etc., and you get " +
        "to determine how to display these values.";
}
