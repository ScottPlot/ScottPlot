using System.Threading.Tasks;

namespace ScottPlot.DataSources;

#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor.
#pragma warning disable CS8604 // Possible null reference argument.

public class SegmentedTree<T> where T : struct, IComparable
{
    private T[] sourceArray;

    private T[] TreeMin;
    private T[] TreeMax;
    private int n = 0; // size of each Tree
    public bool TreesReady = false;

    // precompiled lambda expressions for fast math on generic
    private static Func<T, T, T> MinExp;
    private static Func<T, T, T> MaxExp;
    private static Func<T, T, bool> EqualExp;
    private static Func<T> MaxValue;
    private static Func<T> MinValue;
    private static Func<T, T, bool> LessThanExp;
    private static Func<T, T, bool> GreaterThanExp;

    public T[] SourceArray
    {
        get => sourceArray;
        set
        {
            sourceArray = value ?? throw new Exception("Source Array cannot be null");
            UpdateTrees();
        }
    }

    public SegmentedTree()
    {
        try // runtime check
        {
            var v = new T();
            NumericConversion.GenericToDouble(ref v);
        }
        catch
        {
            throw new ArgumentOutOfRangeException("Unsupported data type, provide convertable to double data types");
        }
        InitExp();
    }

    public async Task SetSourceAsync(T[] data)
    {
        sourceArray = data ?? throw new ArgumentNullException("Data cannot be null");
        await Task.Run(() => UpdateTrees());
    }

    private void InitExp()
    {
        ParameterExpression paramA = Expression.Parameter(typeof(T), "a");
        ParameterExpression paramB = Expression.Parameter(typeof(T), "b");
        // add the parameters together
        ConditionalExpression bodyMin = Expression.Condition(Expression.LessThanOrEqual(paramA, paramB), paramA, paramB);
        ConditionalExpression bodyMax = Expression.Condition(Expression.GreaterThanOrEqual(paramA, paramB), paramA, paramB);
        BinaryExpression bodyEqual = Expression.Equal(paramA, paramB);
        MemberExpression bodyMaxValue = Expression.MakeMemberAccess(null, typeof(T).GetField("MaxValue"));
        MemberExpression bodyMinValue = Expression.MakeMemberAccess(null, typeof(T).GetField("MinValue"));
        BinaryExpression bodyLessThan = Expression.LessThan(paramA, paramB);
        BinaryExpression bodyGreaterThan = Expression.GreaterThan(paramA, paramB);
        // compile it
        MinExp = Expression.Lambda<Func<T, T, T>>(bodyMin, paramA, paramB).Compile();
        MaxExp = Expression.Lambda<Func<T, T, T>>(bodyMax, paramA, paramB).Compile();
        EqualExp = Expression.Lambda<Func<T, T, bool>>(bodyEqual, paramA, paramB).Compile();
        MaxValue = Expression.Lambda<Func<T>>(bodyMaxValue).Compile();
        MinValue = Expression.Lambda<Func<T>>(bodyMinValue).Compile();
        LessThanExp = Expression.Lambda<Func<T, T, bool>>(bodyLessThan, paramA, paramB).Compile();
        GreaterThanExp = Expression.Lambda<Func<T, T, bool>>(bodyGreaterThan, paramA, paramB).Compile();
    }

    public void updateElement(int index, T newValue)
    {
        sourceArray[index] = newValue;
        // Update Tree, can be optimized            
        if (index == sourceArray.Length - 1) // last elem haven't pair
        {
            TreeMin[n / 2 + index / 2] = sourceArray[index];
            TreeMax[n / 2 + index / 2] = sourceArray[index];
        }
        else if (index % 2 == 0) // even elem have right pair
        {
            TreeMin[n / 2 + index / 2] = MinExp(sourceArray[index], sourceArray[index + 1]);
            TreeMax[n / 2 + index / 2] = MaxExp(sourceArray[index], sourceArray[index + 1]);
        }
        else // odd elem have left pair
        {
            TreeMin[n / 2 + index / 2] = MinExp(sourceArray[index], sourceArray[index - 1]);
            TreeMax[n / 2 + index / 2] = MaxExp(sourceArray[index], sourceArray[index - 1]);
        }

        T candidate;
        for (int i = (n / 2 + index / 2) / 2; i > 0; i /= 2)
        {
            candidate = MinExp(TreeMin[i * 2], TreeMin[i * 2 + 1]);
            if (EqualExp(TreeMin[i], candidate)) // if node same then new value don't need to recalc all upper
                break;
            TreeMin[i] = candidate;
        }
        for (int i = (n / 2 + index / 2) / 2; i > 0; i /= 2)
        {
            candidate = MaxExp(TreeMax[i * 2], TreeMax[i * 2 + 1]);
            if (EqualExp(TreeMax[i], candidate)) // if node same then new value don't need to recalc all upper
                break;
            TreeMax[i] = candidate;
        }
    }

    public void updateRange(int from, int to, T[] newData, int fromData = 0) // RangeUpdate
    {
        //update source signal
        for (int i = from; i < to; i++)
        {
            sourceArray[i] = newData[i - from + fromData];
        }

        for (int i = n / 2 + from / 2; i < n / 2 + to / 2; i++)
        {
            TreeMin[i] = MinExp(sourceArray[i * 2 - n], sourceArray[i * 2 + 1 - n]);
            TreeMax[i] = MaxExp(sourceArray[i * 2 - n], sourceArray[i * 2 + 1 - n]);
        }
        if (to == sourceArray.Length) // last elem haven't pair
        {
            TreeMin[n / 2 + to / 2] = sourceArray[to - 1];
            TreeMax[n / 2 + to / 2] = sourceArray[to - 1];
        }
        else if (to % 2 == 1) //last elem even(to-1) and not last
        {
            TreeMin[n / 2 + to / 2] = MinExp(sourceArray[to - 1], sourceArray[to]);
            TreeMax[n / 2 + to / 2] = MaxExp(sourceArray[to - 1], sourceArray[to]);
        }

        from = (n / 2 + from / 2) / 2;
        to = (n / 2 + to / 2) / 2;

        T candidate;
        while (from != 0) // up to root elem, that is [1], [0] - is free elem
        {
            if (from != to)
            {
                for (int i = from; i <= to; i++) // Recalc all level nodes in range 
                {
                    TreeMin[i] = MinExp(TreeMin[i * 2], TreeMin[i * 2 + 1]);
                    TreeMax[i] = MaxExp(TreeMax[i * 2], TreeMax[i * 2 + 1]);
                }
            }
            else
            {
                // left == rigth, so no need more from to loop
                for (int i = from; i > 0; i /= 2) // up to root node
                {
                    candidate = MinExp(TreeMin[i * 2], TreeMin[i * 2 + 1]);
                    if (EqualExp(TreeMin[i], candidate)) // if node same then new value don't need to recalc all upper
                        break;
                    TreeMin[i] = candidate;
                }

                for (int i = from; i > 0; i /= 2) // up to root node
                {
                    candidate = MaxExp(TreeMax[i * 2], TreeMax[i * 2 + 1]);
                    if (EqualExp(TreeMax[i], candidate)) // if node same then new value don't need to recalc all upper
                        break;
                    TreeMax[i] = candidate;
                }
                // all work done exit while loop
                break;
            }
            // level up
            from = from / 2;
            to = to / 2;
        }
    }

    public void updateData(int from, T[] newData)
    {
        updateRange(from, newData.Length, newData);
    }

    public void updateData(T[] newData)
    {
        updateRange(0, newData.Length, newData);
    }

    public void UpdateTreesInBackground()
    {
        Task.Run(() => { UpdateTrees(); });
    }

    public void UpdateTrees()
    {
        // O(n) to build trees
        TreesReady = false;
        try
        {
            if (sourceArray.Length == 0)
                throw new ArgumentOutOfRangeException($"Array cant't be empty");
            // Size up to pow2
            if (sourceArray.Length > 0x40_00_00_00) // pow 2 must be more then int.MaxValue
                throw new ArgumentOutOfRangeException($"Array higher than {0x40_00_00_00} not supported by SignalConst");
            int pow2 = 1;
            while (pow2 < 0x40_00_00_00 && pow2 < sourceArray.Length)
                pow2 <<= 1;
            n = pow2;
            TreeMin = new T[n];
            TreeMax = new T[n];
            T maxValue = MaxValue();
            T minValue = MinValue();

            // fill bottom layer of tree
            for (int i = 0; i < sourceArray.Length / 2; i++) // with source array pairs min/max
            {
                TreeMin[n / 2 + i] = MinExp(sourceArray[i * 2], sourceArray[i * 2 + 1]);
                TreeMax[n / 2 + i] = MaxExp(sourceArray[i * 2], sourceArray[i * 2 + 1]);
            }
            if (sourceArray.Length % 2 == 1) // if array size odd, last element haven't pair to compare
            {
                TreeMin[n / 2 + sourceArray.Length / 2] = sourceArray[sourceArray.Length - 1];
                TreeMax[n / 2 + sourceArray.Length / 2] = sourceArray[sourceArray.Length - 1];
            }
            for (int i = n / 2 + (sourceArray.Length + 1) / 2; i < n; i++) // min/max for pairs of nonexistent elements
            {
                TreeMin[i] = minValue;
                TreeMax[i] = maxValue;
            }
            // fill other layers
            for (int i = n / 2 - 1; i > 0; i--)
            {
                TreeMin[i] = MinExp(TreeMin[2 * i], TreeMin[2 * i + 1]);
                TreeMax[i] = MaxExp(TreeMax[2 * i], TreeMax[2 * i + 1]);
            }
            TreesReady = true;
        }
        catch (OutOfMemoryException)
        {
            TreeMin = null;
            TreeMax = null;
            TreesReady = false;
            return;
        }
    }

    //  O(log(n)) for each range min/max query
    public void MinMaxRangeQuery(int l, int r, out double lowestValue, out double highestValue)
    {
        T lowestValueT;
        T highestValueT;
        // if the tree calculation isn't finished or if it crashed
        if (!TreesReady)
        {
            // use the original (slower) min/max calculated method
            lowestValueT = sourceArray[l];
            highestValueT = sourceArray[l];
            for (int i = l; i < r; i++)
            {
                if (LessThanExp(sourceArray[i], lowestValueT))
                    lowestValueT = sourceArray[i];
                if (GreaterThanExp(sourceArray[i], highestValueT))
                    highestValueT = sourceArray[i];
            }
            lowestValue = NumericConversion.GenericToDouble(ref lowestValueT);
            highestValue = NumericConversion.GenericToDouble(ref highestValueT);
            return;
        }

        lowestValueT = MaxValue();
        highestValueT = MinValue();
        if (l == r)
        {
            lowestValue = highestValue = NumericConversion.GenericToDouble(ref sourceArray[l]);
            return;
        }
        // first iteration on source array that virtualy bottom of tree
        if ((l & 1) == 1) // l is right child
        {
            lowestValueT = MinExp(lowestValueT, sourceArray[l]);
            highestValueT = MaxExp(highestValueT, sourceArray[l]);
        }
        if ((r & 1) != 1) // r is left child
        {
            lowestValueT = MinExp(lowestValueT, sourceArray[r]);
            highestValueT = MaxExp(highestValueT, sourceArray[r]);
        }
        // go up from array to bottom of Tree
        l = (l + n + 1) / 2;
        r = (r + n - 1) / 2;
        // next iterations on tree
        while (l <= r)
        {
            if ((l & 1) == 1) // l is right child
            {
                lowestValueT = MinExp(lowestValueT, TreeMin[l]);
                highestValueT = MaxExp(highestValueT, TreeMax[l]);
            }
            if ((r & 1) != 1) // r is left child
            {
                lowestValueT = MinExp(lowestValueT, TreeMin[r]);
                highestValueT = MaxExp(highestValueT, TreeMax[r]);
            }
            // go up one level
            l = (l + 1) / 2;
            r = (r - 1) / 2;
        }
        lowestValue = NumericConversion.GenericToDouble(ref lowestValueT);
        highestValue = NumericConversion.GenericToDouble(ref highestValueT);
    }
}
