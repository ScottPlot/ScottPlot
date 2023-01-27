namespace ScottPlot.Axis.TimeUnits;

public interface ITimeUnit
{
    public IReadOnlyList<int> NiceIncrements { get; }
    public string GetFormat();
    public DateTime Next(DateTime dateTime, int increment = 1);
    public TimeSpan MinSize { get; }
}
