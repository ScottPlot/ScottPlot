using System;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace ScottPlot
{
    // Uses Segmented tree to process MinMaxQueries(bottleneck for large signals)
    // allow to draw responsitive plots up to 30M points in Any CPU mode, 
    // 60M points in x64 mode, more is array type limitations
    // Loosing ability to change source array with automatic redraw (Const),  
    // call UpdateTrees() manualy  take a time (Update not implemented in plot)
    // using additional space 4X signal memory, and preprocess time O(n)
    public class PlottableSignalConst:PlottableSignal
    {
        // using 4 x signal memory to story trees, can be optimized in cost of code readability
        double[] TreeMin;
        double[] TreeMax;
        bool TreesReady = false;
        public PlottableSignalConst(double[] ys, double sampleRate, double xOffset, double yOffset, Color color, double lineWidth, double markerSize, string label) : base(ys, sampleRate, xOffset, yOffset, color, lineWidth, markerSize, label)
        {
           UpdateTrees(); 
        }

        public void  UpdateTrees()
        {
            TreesReady = false;
            // O(n) to build trees

            
            Task.Run(() =>
            {
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
                }
                catch (System.OutOfMemoryException ex)
                {
                    // free references to allow  GC clear them
                    TreeMin = null;
                    TreeMax = null;
                    // Trees not computed, using standart alg
                    return;
                }
                TreesReady = true;
            });
        }

        //  O(log(n)) for each range min/max query
        protected override void MinMaxRangeQuery(int l, int r, out double lowestValue, out double highestValue)
        {
            if (!TreesReady)
            {
                base.MinMaxRangeQuery(l, r, out lowestValue, out highestValue);
                return;
            }            
            lowestValue = double.MaxValue;
            highestValue = double.MinValue;
            int n = TreeMin.Length / 2;
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
