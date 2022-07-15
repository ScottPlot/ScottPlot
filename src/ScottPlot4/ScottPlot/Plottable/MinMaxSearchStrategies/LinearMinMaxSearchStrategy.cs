using System;
using System.Linq.Expressions;

namespace ScottPlot.MinMaxSearchStrategies
{
    public class LinearMinMaxSearchStrategy<T> : IMinMaxSearchStrategy<T> where T : struct, IComparable
    {
        private T[] sourceArray;
        public virtual T[] SourceArray
        {
            get => sourceArray;
            set => sourceArray = value;
        }

        // precompiled lambda expressions for fast math on generic
        private static Func<T, T, bool> LessThanExp;
        private static Func<T, T, bool> GreaterThanExp;
        private void InitExp()
        {
            ParameterExpression paramA = Expression.Parameter(typeof(T), "a");
            ParameterExpression paramB = Expression.Parameter(typeof(T), "b");
            // add the parameters together
            BinaryExpression bodyLessThan = Expression.LessThan(paramA, paramB);
            BinaryExpression bodyGreaterThan = Expression.GreaterThan(paramA, paramB);
            // compile it
            LessThanExp = Expression.Lambda<Func<T, T, bool>>(bodyLessThan, paramA, paramB).Compile();
            GreaterThanExp = Expression.Lambda<Func<T, T, bool>>(bodyGreaterThan, paramA, paramB).Compile();
        }

        public LinearMinMaxSearchStrategy()
        {
            InitExp();
        }

        public virtual void MinMaxRangeQuery(int l, int r, out double lowestValue, out double highestValue)
        {
            T lowestValueT = sourceArray[l];
            T highestValueT = sourceArray[l];
            for (int i = l; i <= r; i++)
            {
                if (LessThanExp(sourceArray[i], lowestValueT))
                    lowestValueT = sourceArray[i];
                if (GreaterThanExp(sourceArray[i], highestValueT))
                    highestValueT = sourceArray[i];
            }
            lowestValue = NumericConversion.GenericToDouble(ref lowestValueT);
            highestValue = NumericConversion.GenericToDouble(ref highestValueT);
        }

        public virtual double SourceElement(int index)
        {
            return NumericConversion.GenericToDouble(ref sourceArray[index]);
        }

        public void updateElement(int index, T newValue)
        {
            sourceArray[index] = newValue;
        }

        public void updateRange(int from, int to, T[] newData, int fromData = 0)
        {
            for (int i = from; i < to; i++)
            {
                sourceArray[i] = newData[i - from + fromData];
            }
        }
    }
}
