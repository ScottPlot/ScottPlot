using System.Threading.Tasks;

namespace ScottPlot.DataSources;

#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor.
#pragma warning disable CS8604 // Possible null reference argument.

public class SegmentedTreeDouble
{
    private double[] sourceArray;
    private double[] TreeMin;
    private double[] TreeMax;
    private int n = 0; // size of each Tree
    public bool TreesReady = false;

    public double[] SourceArray
    {
        get => sourceArray;
        set
        {
            sourceArray = value ?? throw new Exception("Source Array cannot be null");
            UpdateTrees();
        }
    }

    public async Task SetSourceAsync(double[] data)
    {
        sourceArray = data ?? throw new ArgumentNullException("Data cannot be null");
        await Task.Run(() => UpdateTrees());
    }

    public void updateElement(int index, double newValue)
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
            TreeMin[n / 2 + index / 2] = Math.Min(sourceArray[index], sourceArray[index + 1]);
            TreeMax[n / 2 + index / 2] = Math.Max(sourceArray[index], sourceArray[index + 1]);
        }
        else // odd elem have left pair
        {
            TreeMin[n / 2 + index / 2] = Math.Min(sourceArray[index], sourceArray[index - 1]);
            TreeMax[n / 2 + index / 2] = Math.Max(sourceArray[index], sourceArray[index - 1]);
        }

        double candidate;
        for (int i = (n / 2 + index / 2) / 2; i > 0; i /= 2)
        {
            candidate = Math.Min(TreeMin[i * 2], TreeMin[i * 2 + 1]);
            if (TreeMin[i] == candidate) // if node same then new value don't need to recalc all upper
                break;
            TreeMin[i] = candidate;
        }
        for (int i = (n / 2 + index / 2) / 2; i > 0; i /= 2)
        {
            candidate = Math.Max(TreeMax[i * 2], TreeMax[i * 2 + 1]);
            if (TreeMax[i] == candidate) // if node same then new value don't need to recalc all upper
                break;
            TreeMax[i] = candidate;
        }
    }

    public void updateRange(int from, int to, double[] newData, int fromData = 0) // RangeUpdate
    {
        //update source signal
        for (int i = from; i < to; i++)
        {
            sourceArray[i] = newData[i - from + fromData];
        }

        for (int i = n / 2 + from / 2; i < n / 2 + to / 2; i++)
        {
            TreeMin[i] = Math.Min(sourceArray[i * 2 - n], sourceArray[i * 2 + 1 - n]);
            TreeMax[i] = Math.Max(sourceArray[i * 2 - n], sourceArray[i * 2 + 1 - n]);
        }
        if (to == sourceArray.Length) // last elem haven't pair
        {
            TreeMin[n / 2 + to / 2] = sourceArray[to - 1];
            TreeMax[n / 2 + to / 2] = sourceArray[to - 1];
        }
        else if (to % 2 == 1) //last elem even(to-1) and not last
        {
            TreeMin[n / 2 + to / 2] = Math.Min(sourceArray[to - 1], sourceArray[to]);
            TreeMax[n / 2 + to / 2] = Math.Max(sourceArray[to - 1], sourceArray[to]);
        }

        from = (n / 2 + from / 2) / 2;
        to = (n / 2 + to / 2) / 2;

        double candidate;
        while (from != 0) // up to root elem, that is [1], [0] - is free elem
        {
            if (from != to)
            {
                for (int i = from; i <= to; i++) // Recalc all level nodes in range 
                {
                    TreeMin[i] = Math.Min(TreeMin[i * 2], TreeMin[i * 2 + 1]);
                    TreeMax[i] = Math.Max(TreeMax[i * 2], TreeMax[i * 2 + 1]);
                }
            }
            else
            {
                // left == rigth, so no need more from to loop
                for (int i = from; i > 0; i /= 2) // up to root node
                {
                    candidate = Math.Min(TreeMin[i * 2], TreeMin[i * 2 + 1]);
                    if (TreeMin[i] == candidate) // if node same then new value don't need to recalc all upper
                        break;
                    TreeMin[i] = candidate;
                }

                for (int i = from; i > 0; i /= 2) // up to root node
                {
                    candidate = Math.Max(TreeMax[i * 2], TreeMax[i * 2 + 1]);
                    if (TreeMax[i] == candidate) // if node same then new value don't need to recalc all upper
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

    public void updateData(int from, double[] newData)
    {
        updateRange(from, newData.Length, newData);
    }

    public void updateData(double[] newData)
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
            TreeMin = new double[n];
            TreeMax = new double[n];
            double maxValue = double.MaxValue;
            double minValue = double.MinValue;

            // fill bottom layer of tree
            for (int i = 0; i < sourceArray.Length / 2; i++) // with source array pairs min/max
            {
                TreeMin[n / 2 + i] = Math.Min(sourceArray[i * 2], sourceArray[i * 2 + 1]);
                TreeMax[n / 2 + i] = Math.Max(sourceArray[i * 2], sourceArray[i * 2 + 1]);
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
                TreeMin[i] = Math.Min(TreeMin[2 * i], TreeMin[2 * i + 1]);
                TreeMax[i] = Math.Max(TreeMax[2 * i], TreeMax[2 * i + 1]);
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
        double lowestValueT;
        double highestValueT;
        // if the tree calculation isn't finished or if it crashed
        if (!TreesReady)
        {
            // use the original (slower) min/max calculated method
            lowestValueT = sourceArray[l];
            highestValueT = sourceArray[l];
            for (int i = l; i < r; i++)
            {
                if (sourceArray[i] < lowestValueT)
                    lowestValueT = sourceArray[i];
                if (sourceArray[i] > highestValueT)
                    highestValueT = sourceArray[i];
            }
            lowestValue = NumericConversion.GenericToDouble(ref lowestValueT);
            highestValue = NumericConversion.GenericToDouble(ref highestValueT);
            return;
        }

        lowestValueT = double.MaxValue;
        highestValueT = double.MinValue;
        if (l == r)
        {
            lowestValue = highestValue = NumericConversion.GenericToDouble(ref sourceArray[l]);
            return;
        }
        // first iteration on source array that virtualy bottom of tree
        if ((l & 1) == 1) // l is right child
        {
            lowestValueT = Math.Min(lowestValueT, sourceArray[l]);
            highestValueT = Math.Max(highestValueT, sourceArray[l]);
        }
        if ((r & 1) != 1) // r is left child
        {
            lowestValueT = Math.Min(lowestValueT, sourceArray[r]);
            highestValueT = Math.Max(highestValueT, sourceArray[r]);
        }
        // go up from array to bottom of Tree
        l = (l + n + 1) / 2;
        r = (r + n - 1) / 2;
        // next iterations on tree
        while (l <= r)
        {
            if ((l & 1) == 1) // l is right child
            {
                lowestValueT = Math.Min(lowestValueT, TreeMin[l]);
                highestValueT = Math.Max(highestValueT, TreeMax[l]);
            }
            if ((r & 1) != 1) // r is left child
            {
                lowestValueT = Math.Min(lowestValueT, TreeMin[r]);
                highestValueT = Math.Max(highestValueT, TreeMax[r]);
            }
            // go up one level
            l = (l + 1) / 2;
            r = (r - 1) / 2;
        }
        lowestValue = NumericConversion.GenericToDouble(ref lowestValueT);
        highestValue = NumericConversion.GenericToDouble(ref highestValueT);
    }
}
