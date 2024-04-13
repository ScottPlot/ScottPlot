using ScottPlot.TickGenerators;

namespace ScottPlot.AxisPanels;

public class DateTimeXAxis : XAxisBase, IXAxis
{
    public override Edge Edge { get; } = Edge.Bottom;

    private IDateTimeTickGenerator _tickGenerator = new DateTimeAutomatic();

    public override ITickGenerator TickGenerator
    {
        get => _tickGenerator;
        set
        {
            if (value is not IDateTimeTickGenerator)
                throw new ArgumentException($"Date axis must have a {nameof(ITickGenerator)} generator");

            _tickGenerator = (IDateTimeTickGenerator)value;
        }
    }

    public IEnumerable<double> ConvertToCoordinateSpace(IEnumerable<DateTime> dates) =>
        TickGenerator is IDateTimeTickGenerator dateTickGenerator
            ? dateTickGenerator.ConvertToCoordinateSpace(dates)
            : throw new InvalidOperationException("Date axis configured with non-date tick generator");
}
