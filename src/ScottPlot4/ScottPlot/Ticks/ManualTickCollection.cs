namespace ScottPlot.Ticks;

public class ManualTickCollection
{
    private readonly List<Tick> Ticks = new();
    public Tick[] GetTicks() => Ticks.ToArray();
    public double[] MajorPositions => Ticks.Where(x => x.IsMajor).Select(x => x.Position).ToArray();
    public double[] MinorPositions => Ticks.Where(x => !x.IsMajor).Select(x => x.Position).ToArray();
    public string[] MajorLabels => Ticks.Where(x => x.IsMajor).Select(x => x.Label).ToArray();

    public ManualTickCollection()
    {

    }

    public void Clear() => Ticks.Clear();


    public void AddMajor(double position, string label)
    {
        Ticks.Add(new Tick(position, label, isMajor: true, isDateTime: false));
    }

    public void AddMajor(DateTime date, string label)
    {
        Ticks.Add(new Tick(date.ToOADate(), label, isMajor: true, isDateTime: true));
    }

    public void AddMinor(double position)
    {
        Ticks.Add(new Tick(position, string.Empty, isMajor: false, isDateTime: false));
    }

    public void AddMinor(DateTime date)
    {
        Ticks.Add(new Tick(date.ToOADate(), string.Empty, isMajor: false, isDateTime: true));
    }
}
