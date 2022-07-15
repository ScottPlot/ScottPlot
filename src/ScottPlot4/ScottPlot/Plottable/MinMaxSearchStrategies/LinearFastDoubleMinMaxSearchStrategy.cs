using System;

namespace ScottPlot.MinMaxSearchStrategies
{
    public class LinearFastDoubleMinMaxSearchStrategy<T> : LinearMinMaxSearchStrategy<T> where T : struct, IComparable
    {
        private double[] sourceArrayDouble;

        public override T[] SourceArray
        {
            get => base.SourceArray;
            set
            {
                sourceArrayDouble = value as double[];
                base.SourceArray = value;
            }
        }

        public override void MinMaxRangeQuery(int l, int r, out double lowestValue, out double highestValue)
        {
            if (sourceArrayDouble != null)
            {
                lowestValue = sourceArrayDouble[l];
                highestValue = sourceArrayDouble[l];
                for (int i = l; i <= r; i++)
                {
                    if (sourceArrayDouble[i] < lowestValue)
                        lowestValue = sourceArrayDouble[i];
                    if (sourceArrayDouble[i] > highestValue)
                        highestValue = sourceArrayDouble[i];
                }
                return;
            }
            else
            {
                base.MinMaxRangeQuery(l, r, out lowestValue, out highestValue);
            }
        }

        public override double SourceElement(int index)
        {
            if (sourceArrayDouble != null)
                return sourceArrayDouble[index];
            return NumericConversion.GenericToDouble(ref SourceArray[index]);
        }
    }
}
