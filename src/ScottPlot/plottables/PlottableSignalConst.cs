using System;
using System.Collections.Generic;
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
        float[] TreeMinF;
        float[] TreeMaxF;
        private int n = 0; // size of each Tree
        public bool TreesReady = false;
        private bool singlePrecision = false; // float type for trees, which uses half memory
        public PlottableSignalConst(double[] ys, double sampleRate, double xOffset, double yOffset, Color color, double lineWidth, double markerSize, string label, bool useParallel) : base(ys, sampleRate, xOffset, yOffset, color, lineWidth, markerSize, label, useParallel)
        {
            if (useParallel)
                UpdateTreesInBackground();
            else
                UpdateTrees();
        }

        public void updateData(int index, double newValue)
        {
            ys[index] = newValue;
            // Update Tree, can be optimized            
            if (index == ys.Length - 1) // last elem haven't pair
            {
                TreeMin[n / 2 + index / 2] = ys[index];
                TreeMax[n / 2 + index / 2] = ys[index];
            }
            else if (index % 2 == 0) // even elem have right pair
            {
                TreeMin[n / 2 + index / 2] = Math.Min(ys[index], ys[index + 1]);
                TreeMax[n / 2 + index / 2] = Math.Max(ys[index], ys[index + 1]);
            }
            else // odd elem have left pair
            {
                TreeMin[n / 2 + index / 2] = Math.Min(ys[index], ys[index - 1]);
                TreeMax[n / 2 + index / 2] = Math.Max(ys[index], ys[index - 1]);
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

        public void updateData(int from, int to, double[] newData, int fromData = 0)
        {            
            //update source signal
            for (int i = from; i < to; i++)
            {
                ys[i] = newData[i - from + fromData];
            }
            
            for (int i = n / 2 + from / 2; i < n / 2 + to / 2; i++)
            {
                TreeMin[i] = Math.Min(ys[i * 2 - n], ys[i * 2 + 1 - n]);
                TreeMax[i] = Math.Max(ys[i * 2 - n], ys[i * 2 + 1 - n]);
            }
            if (to == ys.Length) // last elem haven't pair
            {
                TreeMin[n / 2 + to / 2] = ys[to - 1];
                TreeMax[n / 2 + to / 2] = ys[to - 1];
            }
            else if (to % 2 == 1) //last elem even(to-1) and not last
            {
                TreeMin[n / 2 + to / 2] = Math.Min(ys[to - 1], ys[to]);
                TreeMax[n / 2 + to / 2] = Math.Max(ys[to - 1], ys[to]);
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
            updateData(from, newData.Length, newData);
        }

        public void updateData(double[] newData)
        {
            updateData(0, newData.Length, newData);
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
                n = (1 << ((int)Math.Log(ys.Length - 1, 2) + 1));
                if (singlePrecision == false)
                {
                    TreeMin = new double[n];
                    TreeMax = new double[n];
                }
                else
                {
                    TreeMinF = new float[n];
                    TreeMaxF = new float[n];
                }
                // fill bottom layer of tree

                if (singlePrecision == false)
                {
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
                }
                else
                {
                    for (int i = 0; i < ys.Length / 2; i++) // with source array pairs min/max
                    {
                        TreeMinF[n / 2 + i] = (float)Math.Min(ys[i * 2], ys[i * 2 + 1]);
                        TreeMaxF[n / 2 + i] = (float)Math.Max(ys[i * 2], ys[i * 2 + 1]);
                    }
                    if (ys.Length % 2 == 1) // if array size odd, last element haven't pair to compare
                    {
                        TreeMinF[n / 2 + ys.Length / 2] = (float)ys[ys.Length - 1];
                        TreeMaxF[n / 2 + ys.Length / 2] = (float)ys[ys.Length - 1];
                    }
                    for (int i = n / 2 + (ys.Length + 1) / 2; i < n; i++) // min/max for pairs of nonexistent elements
                    {
                        TreeMinF[i] = float.MaxValue;
                        TreeMaxF[i] = float.MinValue;
                    }
                    // fill other layers
                    for (int i = n / 2 - 1; i > 0; i--)
                    {
                        TreeMinF[i] = (float)Math.Min(TreeMinF[2 * i], TreeMinF[2 * i + 1]);
                        TreeMaxF[i] = (float)Math.Max(TreeMaxF[2 * i], TreeMaxF[2 * i + 1]);
                    }
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
                if (singlePrecision == false)
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
                }
                else
                {
                    if ((l & 1) == 1) // l is right child
                    {
                        lowestValue = Math.Min(lowestValue, TreeMinF[l]);
                        highestValue = Math.Max(highestValue, TreeMaxF[l]);
                    }
                    if ((r & 1) != 1) // r is left child
                    {
                        lowestValue = Math.Min(lowestValue, TreeMinF[r]);
                        highestValue = Math.Max(highestValue, TreeMaxF[r]);
                    }
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
