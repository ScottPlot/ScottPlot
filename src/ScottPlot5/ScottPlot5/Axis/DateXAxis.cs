using ScottPlot.Axis.StandardAxes;
using ScottPlot.TickGenerators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScottPlot.Axis
{
    public class DateXAxis : XAxisBase, IXAxis, IDateAxis
    {
        public override Edge Edge { get; } = Edge.Bottom;

        private IDateTickGenerator _tickGenerator = null!;
        public override ITickGenerator TickGenerator
        {
            get => _tickGenerator;
            set
            {
                if (value is IDateTickGenerator dateTickGenerator)
                    _tickGenerator = dateTickGenerator;
                else
                    throw new ArgumentException("Date axis must have a date tick generator");
            }
        }

        public IEnumerable<double> ConvertToCoordinateSpace(IEnumerable<DateTime> dates) =>
            TickGenerator is IDateTickGenerator dateTickGenerator
                ? dateTickGenerator.ConvertToCoordinateSpace(dates)
                : throw new InvalidOperationException("Date axis configured with non-date tick generator");

        public DateXAxis()
        {
            TickGenerator = new TickGenerators.DateAutomatic();
        }
    }
}
