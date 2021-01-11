using System;

namespace ScottPlot.MinMaxSearchStrategies
{
    public class LinearDoubleOnlyMinMaxStrategy : IMinMaxSearchStrategy<double>
    {
        public double[] SourceArray { get; set; }

        public void MinMaxRangeQuery(int l, int r, out double lowestValue, out double highestValue)
        {
            lowestValue = SourceArray[l];
            highestValue = SourceArray[l];
            for (int i = l; i <= r; i++)
            {
                if (SourceArray[i] < lowestValue)
                    lowestValue = SourceArray[i];
                if (SourceArray[i] > highestValue)
                    highestValue = SourceArray[i];
            }
        }

        public double SourceElement(int index)
        {
            return SourceArray[index];
        }

        public void updateElement(int index, double newValue)
        {
            SourceArray[index] = newValue;
        }

        public void updateRange(int from, int to, double[] newData, int fromData = 0)
        {
            Array.Copy(newData, fromData, SourceArray, from, to - from);
        }
    }
}
