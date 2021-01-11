using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

namespace ScottPlot.MinMaxSearchStrategies
{
    public class LinearMinMaxSearchStrategy<T> : IMinMaxSearchStrategy<T> where T : struct, IComparable
    {
        public virtual T[] SourceArray { get; set; }

        // precompiled lambda expressions for fast math on generic
        private static Func<T, T, bool> LessThanExp = null!; // they will be assigned on constructor call
        private static Func<T, T, bool> GreaterThanExp = null!;

        public LinearMinMaxSearchStrategy()
        {
            if (LessThanExp is not null && GreaterThanExp is not null)
            {
                return;
            }

            ParameterExpression paramA = Expression.Parameter(typeof(T), "a");
            ParameterExpression paramB = Expression.Parameter(typeof(T), "b");
            // add the parameters together
            BinaryExpression bodyLessThan = Expression.LessThan(paramA, paramB);
            BinaryExpression bodyGreaterThan = Expression.GreaterThan(paramA, paramB);
            // compile it
            LessThanExp = Expression.Lambda<Func<T, T, bool>>(bodyLessThan, paramA, paramB).Compile();
            GreaterThanExp = Expression.Lambda<Func<T, T, bool>>(bodyGreaterThan, paramA, paramB).Compile();
        }

        public virtual void MinMaxRangeQuery(int l, int r, out double lowestValue, out double highestValue)
        {
            T lowestValueT = SourceArray[l];
            T highestValueT = SourceArray[l];
            for (int i = l; i <= r; i++)
            {
                if (LessThanExp(SourceArray[i], lowestValueT))
                    lowestValueT = SourceArray[i];
                if (GreaterThanExp(SourceArray[i], highestValueT))
                    highestValueT = SourceArray[i];
            }
            lowestValue = Convert.ToDouble(lowestValueT);
            highestValue = Convert.ToDouble(highestValueT);
        }

        public virtual double SourceElement(int index)
        {
            return Convert.ToDouble(SourceArray[index]);
        }

        public void updateElement(int index, T newValue)
        {
            SourceArray[index] = newValue;
        }

        public void updateRange(int from, int to, T[] newData, int fromData = 0)
        {
            Array.Copy(newData, fromData, SourceArray, from, to - from);
        }
    }
}
