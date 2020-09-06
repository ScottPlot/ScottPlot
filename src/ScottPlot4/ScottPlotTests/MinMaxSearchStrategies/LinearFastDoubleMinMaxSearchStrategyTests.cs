using NUnit.Framework;
using ScottPlot.MinMaxSearchStrategies;

namespace ScottPlotTests.MinMaxSearchStrategies
{
    [TestFixture]
    public class LinearFastDoubleMinMaxSearchStrategyTests : SegmentedTreeMinMaxSearchStrategyTests
    {
        public override IMinMaxSearchStrategy<T> CreateStrategy<T>()
        {
            return new LinearFastDoubleMinMaxSearchStrategy<T>();
        }
    }
}
