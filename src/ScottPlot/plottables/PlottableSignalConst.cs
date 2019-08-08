using System;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace ScottPlot
{
    // Variation of PlottableSignal that uses a segmented tree for faster min/max range queries
    // - frequent min/max lookups are a bottleneck displaying large signals
    // - limited to 30M points (60M in x64 mode) due to memory (tree uses 4X memory)
    // - if source array is changed UpdateTrees() must be called
    public class PlottableSignalConst : PlottableSignal
    {
        // using 4 x signal memory to story trees, can be optimized in cost of code readability
        double[] TreeMin;
        double[] TreeMax;
        public bool TreesReady = false;
        public PlottableSignalConst(double[] ys, double sampleRate, double xOffset, double yOffset, Color color, double lineWidth, double markerSize, string label, bool useParallel) : base(ys, sampleRate, xOffset, yOffset, color, lineWidth, markerSize, label, useParallel)
        {
            if (useParallel)
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

                TreeMin = Enumerable.Repeat(double.MaxValue, 2 * n).ToArray();
                for (int i = n; i < n + ys.Length; i++)
                    TreeMin[i] = ys[i - n];
                for (int i = n - 1; i > 0; i--)
                    TreeMin[i] = Math.Min(TreeMin[2 * i], TreeMin[2 * i + 1]);

                TreeMax = Enumerable.Repeat(double.MinValue, 2 * n).ToArray();
                for (int i = n; i < n + ys.Length; i++)
                    TreeMax[i] = ys[i - n];
                for (int i = n - 1; i > 0; i--)
                    TreeMax[i] = Math.Max(TreeMax[2 * i], TreeMax[2 * i + 1]);

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
            int n = TreeMin.Length / 2;
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
            l += n - 1;
            r += n - 1;
            while (l <= r)
            {
                // l is right child
                if ((l & 1) == 1)
                {
                    lowestValue = Math.Min(lowestValue, TreeMin[l]);
                    highestValue = Math.Max(highestValue, TreeMax[l]);
                }
                // r is left child
                if ((r & 1) != 1)
                {
                    lowestValue = Math.Min(lowestValue, TreeMin[r]);
                    highestValue = Math.Max(highestValue, TreeMax[r]);
                }
                // go up one level
                l = (l + 1) / 2;
                r = (r - 1) / 2;
            }
        }
    }
}
