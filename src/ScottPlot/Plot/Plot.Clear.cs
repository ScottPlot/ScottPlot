/* This file contains methods to remove plottables from the plot */
using System;
using ScottPlot.Plottable;

namespace ScottPlot
{
    public partial class Plot
    {
        /// <summary>
        /// Remove the given plottable from the plot
        /// </summary>
        public void Remove(IPlottable plottable) => settings.Plottables.Remove(plottable);

        /// <summary>
        /// Clear all plottables
        /// </summary>
        public void Clear()
        {
            settings.Plottables.Clear();
            settings.ResetAxisLimits();
        }

        /// <summary>
        /// Clear all plottables matching the given type
        /// </summary>
        public void Clear<T>()
        {
            settings.Plottables.RemoveAll(x => x is T);

            if (settings.Plottables.Count == 0)
                settings.ResetAxisLimits();
        }

        /// <summary>
        /// Clear all plottables matching the given type
        /// </summary>
        public void Clear(Type typeToClear)
        {
            settings.Plottables.RemoveAll(x => x.GetType() == typeToClear);

            if (settings.Plottables.Count == 0)
                settings.ResetAxisLimits();
        }

        /// <summary>
        /// Clear all plottables of the same type as the one that is given
        /// </summary>
        public void Clear(IPlottable examplePlottable)
        {
            settings.Plottables.RemoveAll(x => x.GetType() == examplePlottable.GetType());

            if (settings.Plottables.Count == 0)
                settings.ResetAxisLimits();
        }

        /// <summary>
        /// Clear all plottables matching the given types
        /// </summary>
        public void Clear(Type[] typesToClear)
        {
            if (typesToClear != null)
                foreach (var typeToClear in typesToClear)
                    Clear(typeToClear);
        }

        /// <summary>
        /// Remove the given plottables from the plot
        /// </summary>
        public void Clear(Predicate<IPlottable> plottablesToClear)
        {
            if (plottablesToClear != null)
                settings.Plottables.RemoveAll(plottablesToClear);

            if (settings.Plottables.Count == 0)
                settings.ResetAxisLimits();
        }

        [Obsolete("This overload is deprecated.", true)]
        public void Clear(bool axisLines = true, bool scatterPlots = true, bool signalPlots = true, bool text = true,
            bool bar = true, bool finance = true, bool axisSpans = true) => throw new NotImplementedException();
    }
}
