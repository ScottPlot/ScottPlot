using System;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace ScottPlot
{
    // Variation of PlottableSignal that uses a segmented tree for faster min/max range queries
    // - frequent min/max lookups are a bottleneck displaying large signals
    // - limited to 60M points (250M in x64 mode) due to memory (tree uses from 2X to 4X memory)
    // - in x64 mode limit can be up to maximum array size (2G points) with special solution and 64 GB RAM (not tested)
    // - if source array is changed UpdateTrees() must be called
    public class PlottableSignalConst : PlottableSignal
    {
        // using 2 x signal memory in best case: ys.Length is Pow2 
        // using 4 x signal memory in worst case: ys.Length is (Pow2 +1);        
        double[] TreeMin;
        double[] TreeMax;
        public bool TreesReady = false;
        public PlottableSignalConst(double[] ys, double sampleRate, double xOffset, double yOffset, Color color, double lineWidth, double markerSize, string label, bool useThreading) : base(ys, sampleRate, xOffset, yOffset, color, lineWidth, markerSize, label)
        {
            if (useThreading)
                UpdateTreesInBackground();
            else
                UpdateTrees();
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
                // Size up to pow2
                int n = (1 << ((int)Math.Log(ys.Length - 1, 2) + 1));
                TreeMin = new double[n];
                TreeMax = new double[n];
                // fill bottom layer of tree                
                for (int i = 0; i < ys.Length / 2; i++) // with source array pairs min/max
                {
                    TreeMin[n / 2 + i] = Math.Min(ys[i * 2], ys[i * 2 + 1]);
                    TreeMax[n / 2 + i] = Math.Max(ys[i * 2], ys[i * 2 + 1]);
                }
                if (ys.Length % 2 == 1) // if array size odd, last element haven't pair to compare
                {
                    TreeMin[n / 2 + ys.Length / 2] = ys[ys.Length - 1];
                    TreeMax[n / 2 + ys.Length / 2] = ys[ys.Length - 1];
                }
                for (int i = n / 2 + (ys.Length + 1) / 2; i < n; i++) // min/max for pairs of nonexistent elements
                {
                    TreeMin[i] = double.MaxValue;
                    TreeMax[i] = double.MinValue;
                }
                // fill other layers
                for (int i = n / 2 - 1; i > 0; i--)
                {
                    TreeMin[i] = Math.Min(TreeMin[2 * i], TreeMin[2 * i + 1]);
                    TreeMax[i] = Math.Max(TreeMax[2 * i], TreeMax[2 * i + 1]);
                }

                TreesReady = true;
            }
            catch (System.OutOfMemoryException ex)
            {
                TreeMin = null;
                TreeMax = null;
                TreesReady = false;
                return;
            }
        }

        //  O(log(n)) for each range min/max query
        protected override void MinMaxRangeQuery(int l, int r, out double lowestValue, out double highestValue)
        {
            // if the tree calculation isn't finished or if it crashed
            if (!TreesReady)
            {
                // use the original (slower) min/max calculated method
                base.MinMaxRangeQuery(l, r, out lowestValue, out highestValue);
                return;
            }

            lowestValue = double.MaxValue;
            highestValue = double.MinValue;
            int n = TreeMin.Length;
            if (l > r)
            {
                int temp = r;
                r = l;
                l = temp;
            }
            if (l == r)
            {
                lowestValue = ys[l];
                highestValue = ys[l];
                return;
            }
            // first iteration on source array that virtualy bottom of tree
            if ((l & 1) != 1) // l is left child
            {
                lowestValue = Math.Min(lowestValue, ys[l]);
                highestValue = Math.Max(highestValue, ys[l]);
            }
            if ((r & 1) == 1) // r is right child
            {
                lowestValue = Math.Min(lowestValue, ys[r]);
                highestValue = Math.Max(highestValue, ys[r]);
            }
            // go up from array to bottom of Tree
            l = (l + n) / 2;
            r = (r + n) / 2;
            // next iterations on tree
            while (l <= r)
            {
                if ((l & 1) == 1) // l is right child
                {
                    lowestValue = Math.Min(lowestValue, TreeMin[l]);
                    highestValue = Math.Max(highestValue, TreeMax[l]);
                }
                if ((r & 1) != 1) // r is left child
                {
                    lowestValue = Math.Min(lowestValue, TreeMin[r]);
                    highestValue = Math.Max(highestValue, TreeMax[r]);
                }
                // go up one level
                l = (l + 1) / 2;
                r = (r - 1) / 2;
            }
        }

        public override double[] GetLimits()
        {
            double[] limits = new double[4];
            limits[0] = 0 + xOffset;
            limits[1] = samplePeriod * ys.Length + xOffset;
            MinMaxRangeQuery(0, ys.Length - 1, out limits[2], out limits[3]);
            limits[2] += yOffset;
            limits[3] += yOffset;
            return limits;
        }

        public override string ToString()
        {
            return $"PlottableSignalConst with {pointCount} points, trees {(TreesReady ? "" : "not")} calculated";
        }
    }
}
