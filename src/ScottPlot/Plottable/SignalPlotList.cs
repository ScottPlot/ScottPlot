using ScottPlot.MinMaxSearchStrategies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScottPlot.Plottable
{
    /// <summary>
    /// A SignalPlotList is a SignalPlot that is designed to grow using Add() and AddRange() methods
    /// </summary>
    public class SignalPlotList : SignalPlotBase<double>
    {
        /// <summary>
        /// The total number of values the list can hold without resizing.
        /// </summary>
        public int Capacity => Ys.Length;

        /// <summary>
        /// Number of values in the list.
        /// This number will never exceed the Capacity.
        /// </summary>
        public int Count => NextPointIndex;

        /// <summary>
        /// Percentage of the capacity currently filled with data
        /// </summary>
        public double FillPercentage => 100.0 * Count / Capacity;

        /// <summary>
        /// Return the last value in the list (or null if empty)
        /// </summary>
        public double? LastY => (Count > 0) ? Ys[NextPointIndex - 1] : null;

        /// <summary>
        /// Return the time point fot the last value in the list (or null if empty)
        /// </summary>
        public double? LastX => (Count > 0) ? (NextPointIndex - 1) / SampleRate + OffsetX : null;

        /// <summary>
        /// Index in the Ys array where the next data point will be placed.
        /// </summary>
        private int NextPointIndex = 0;

        /// <summary>
        /// When the Ys array must be expanded, increase its length by this number of values.
        /// </summary>
        private readonly int CapacityIncriment;

        /// <summary>
        /// Create a SignalPlotList
        /// </summary>
        /// <param name="capacity">Initial capacity of the list. Also the size by which to expand it as needed.</param>
        public SignalPlotList(int capacity) : base()
        {
            CapacityIncriment = capacity;
            Strategy = new LinearDoubleOnlyMinMaxStrategy();
            Clear(resetCapacity: true);
        }

        public override string ToString() =>
            $"SignalPlotList (label={Label}) with {Count}/{Capacity} points filled ({FillPercentage:N2}%)";

        /// <summary>
        /// Add a single point to the list.
        /// The internal data structure will be automatically expanded in memory if needed.
        /// </summary>
        /// <param name="y">new Y value</param>
        public void Add(double y)
        {
            if (Count >= Capacity)
                IncreaseCapacity(Capacity + CapacityIncriment);
            Ys[NextPointIndex] = y;
            MaxRenderIndex = NextPointIndex;
            NextPointIndex += 1;
        }

        /// <summary>
        /// Add a multiple points to the list.
        /// The internal data structure will be automatically expanded in memory if needed.
        /// </summary>
        /// <param name="ys">new Y values</param>
        public void AddRange(double[] ys)
        {
            // TODO: this can be optimized
            foreach (var y in ys)
                Add(y);
        }

        /// <summary>
        /// Add a multiple points to the list.
        /// The internal data structure will be automatically expanded in memory if needed.
        /// </summary>
        /// <param name="ys">new Y values</param>
        public void AddRange(IEnumerable<double> ys)
        {
            // TODO: this can be optimized
            foreach (var y in ys)
                Add(y);
        }

        /// <summary>
        /// Replace the Ys array with a new array of larger size, copying-over the old values.
        /// </summary>
        /// <param name="newCapacity">length of the new array</param>
        private void IncreaseCapacity(int newCapacity)
        {
            if (newCapacity <= Capacity)
                throw new ArgumentException("new capacity must be greater than the current capacity");

            double[] newYs = new double[newCapacity];
            Array.Copy(Ys, 0, newYs, 0, Ys.Length);
            Ys = newYs;
        }

        /// <summary>
        /// Clear the list
        /// </summary>
        /// <param name="resetCapacity">if true, the internal data structure will be reset to its initial size</param>
        public void Clear(bool resetCapacity = false)
        {
            if (resetCapacity)
                Ys = new double[CapacityIncriment];
            NextPointIndex = 0;
            MaxRenderIndex = 0; // TODO: should this be -1 to indicate no points should be rendered?
        }
    }
}
