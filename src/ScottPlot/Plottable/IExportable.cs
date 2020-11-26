namespace ScottPlot.Plottable
{
    /// <summary>
    /// For Plottables whose data can be exported as a text file
    /// </summary>
    public interface IExportable
    {
        void SaveCSV(string filePath, string delimiter = ", ", string separator = "\n");
        string GetCSV(string delimiter = ", ", string separator = "\n");
    }
}
