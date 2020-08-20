using NUnit.Framework;
using ScottPlot.MinMaxSearchStrategies;

namespace ScottPlotTests.MinMaxSearchStrategies
{
    [TestFixture]
    public class LinearMinMaxSearchStrategyTests : SegmentedTreeMinMaxSearchStrategyTests
    {
        public override IMinMaxSearchStrategy<T> CreateStrategy<T>()
        {
            return new LinearMinMaxSearchStrategy<T>();
        }
    }
}
