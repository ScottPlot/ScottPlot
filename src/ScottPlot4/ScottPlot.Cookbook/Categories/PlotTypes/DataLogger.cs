namespace ScottPlot.Cookbook.Categories.PlotTypes;

public class DataLogger : ICategory
{
    public string Name => "Data Logger";

    public string Description => "The DataLogger plot type facilitates displaying live data by giving the developer " +
        "a simple way to Add() new data points by either shifting them in or appending them to a growing list. " +
        "This plot type also has special options to manage axis limits as new data arrives. " +
        "See code in the WinForms Demo app for advanced usage information.";

    public string Folder => "plottable-datalogger";
}
