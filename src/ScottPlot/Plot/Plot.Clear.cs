/* This file contains API methods to remote plottables from the list */
using System;
using System.Collections.Generic;
using System.Text;
using ScottPlot.Plottable;
using ScottPlot.Renderable;

namespace ScottPlot
{
    public partial class Plot
    {
        /// <summary>
        /// Remove the given plottable from the plot
        /// </summary>
        public void Remove(IRenderable plottable)
        {
            settings.Plottables.Remove(plottable);
        }

        /// <summary>
        /// Clear all plottables
        /// </summary>
        public void Clear()
        {
            settings.Plottables.Clear();
            settings.axes.Reset();
        }

        /// <summary>
        /// Clear all plottables matching the given type
        /// </summary>
        public void Clear<T>()
        {
            settings.Plottables.RemoveAll(x => x is T);

            if (settings.Plottables.Count == 0)
                settings.axes.Reset();
        }

        /// <summary>
        /// Clear all plottables matching the given type
        /// </summary>
        public void Clear(Type typeToClear)
        {
            settings.Plottables.RemoveAll(x => x.GetType() == typeToClear);

            if (settings.Plottables.Count == 0)
                settings.axes.Reset();
        }

        /// <summary>
        /// Clear all plottables of the same type as the one that is given
        /// </summary>
        public void Clear(IRenderable examplePlottable)
        {
            settings.Plottables.RemoveAll(x => x.GetType() == examplePlottable.GetType());

            if (settings.Plottables.Count == 0)
                settings.axes.Reset();
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
        public void Clear(Predicate<IRenderable> plottablesToClear)
        {
            if (plottablesToClear != null)
                settings.Plottables.RemoveAll(plottablesToClear);

            if (settings.Plottables.Count == 0)
                settings.axes.Reset();
        }

        [Obsolete("This overload is deprecated. Clear plots using a different overload of the Clear() method.")]
        public void Clear(
            bool axisLines = true,
            bool scatterPlots = true,
            bool signalPlots = true,
            bool text = true,
            bool bar = true,
            bool finance = true,
            bool axisSpans = true
            )
        {
            List<int> indicesToDelete = new List<int>();
            for (int i = 0; i < settings.Plottables.Count; i++)
            {
                if ((settings.Plottables[i] is VLine || settings.Plottables[i] is HLine) && axisLines)
                    indicesToDelete.Add(i);
                else if (settings.Plottables[i] is ScatterPlot && scatterPlots)
                    indicesToDelete.Add(i);
                else if (settings.Plottables[i] is SignalPlot && signalPlots)
                    indicesToDelete.Add(i);
                else if (settings.Plottables[i].GetType().IsGenericType && settings.Plottables[i].GetType().GetGenericTypeDefinition() == typeof(SignalPlotConst<>) && signalPlots)
                    indicesToDelete.Add(i);
                else if (settings.Plottables[i] is Text && text)
                    indicesToDelete.Add(i);
                else if (settings.Plottables[i] is BarPlot && bar)
                    indicesToDelete.Add(i);
                else if (settings.Plottables[i] is FinancePlot && finance)
                    indicesToDelete.Add(i);
                else if ((settings.Plottables[i] is VSpan || settings.Plottables[i] is HSpan) && axisSpans)
                    indicesToDelete.Add(i);
            }

            indicesToDelete.Reverse();
            for (int i = 0; i < indicesToDelete.Count; i++)
                settings.Plottables.RemoveAt(indicesToDelete[i]);

            settings.axes.Reset();
        }
    }
}
