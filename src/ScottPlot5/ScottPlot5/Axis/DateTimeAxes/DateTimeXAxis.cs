using ScottPlot.Axis.StandardAxes;
using ScottPlot.TickGenerators;

namespace ScottPlot.Axis.DateTimeAxes;

public class DateTimeXAxis : XAxisBase, IXAxis, IDateAxis
{
    public override Edge Edge { get; } = Edge.Bottom;

    private IDateTickGenerator _tickGenerator = new DateAutomatic();
    public override ITickGenerator TickGenerator
    {
        get => _tickGenerator;
        set
        {
            if (value is not IDateTickGenerator)
                throw new ArgumentException($"Date axis must have a {nameof(ITickGenerator)} generator");

            _tickGenerator = (IDateTickGenerator)value;
        }
    }

    public IEnumerable<double> ConvertToCoordinateSpace(IEnumerable<DateTime> dates) =>
        TickGenerator is IDateTickGenerator dateTickGenerator
            ? dateTickGenerator.ConvertToCoordinateSpace(dates)
            : throw new InvalidOperationException("Date axis configured with non-date tick generator");
}
