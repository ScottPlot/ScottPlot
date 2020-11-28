/* The Plot class is the primary public interface for ScottPlot.
 * State (plottables and configuration) is stored in the settings object 
 * which is private so it can be refactored without breaking the API.
 * 
 * Helper methods for styling and plottable creation are in partial class
 * files in the Plot folder.
 */

using ScottPlot.Plottable;
using ScottPlot.Renderable;
using System;
using System.Collections.Generic;

namespace ScottPlot
{
    public partial class Plot
    {
        private readonly Settings settings = new Settings();

        /// <summary>
        /// A ScottPlot stores data in plottable objects and draws it on a bitmap when Render() is called
        /// </summary>
        public Plot(int width = 800, int height = 600)
        {
            if (width <= 0 || height <= 0)
                throw new ArgumentException("width and height must each be greater than 0");

            StyleTools.SetStyle(this, ScottPlot.Style.Default);
            Resize(width, height);
        }

        public override string ToString() =>
            $"ScottPlot ({settings.Width}x{settings.Height}) " +
            $"with {settings.Plottables.Count:n0} plottables";

        /// <summary>
        /// Return a new Plot with all the same Plottables (and some of the styles) of this one.
        /// </summary>
        public Plot Copy()
        {
            // This is typically only called when you right-click a plot in a control and hit "open in new window".
            // All state from the old plot must be copied to the new plot.

            Plot plt2 = new Plot(settings.Width, settings.Height);
            var settings2 = plt2.GetSettings(false);

            // Copying state of plottables is easy because they contain their own state.
            settings2.Plottables.AddRange(settings.Plottables);

            // TODO: copy axes, since they now carry their own state too.
            plt2.Title(settings.XAxis2.Title.Label);
            plt2.XLabel(settings.XAxis.Title.Label);
            plt2.YLabel(settings.YAxis.Title.Label);

            plt2.AxisAuto();
            return plt2;
        }

        /// <summary>
        /// Set the default size for new renders
        /// </summary>
        public void Resize(float width, float height) => settings.Resize(width, height);

        /// <summary>
        /// Add a plottable to the plot
        /// </summary>
        public void Add(IPlottable plottable) => settings.Plottables.Add(plottable);

        /// <summary>
        /// Return a copy of the list of plottables
        /// </summary>
        /// <returns></returns>
        public IPlottable[] GetPlottables() => settings.Plottables.ToArray();

        /// <summary>
        /// Return a copy of the list of draggable plottables
        /// </summary>
        /// <returns></returns>
        public IDraggable[] GetDraggables()
        {
            // TODO: linq
            List<IDraggable> draggables = new List<IDraggable>();

            foreach (var plottable in settings.Plottables)
                if (plottable is IDraggable draggable)
                    draggables.Add(draggable);

            return draggables.ToArray();
        }

        /// <summary>
        /// Return the draggable plottable under the mouse cursor (or null if there isn't one)
        /// </summary>
        public IDraggable GetDraggableUnderMouse(double pixelX, double pixelY, int snapDistancePixels = 5)
        {
            double snapWidth = GetSettings(false).XAxis.Dims.UnitsPerPx * snapDistancePixels;
            double snapHeight = GetSettings(false).YAxis.Dims.UnitsPerPx * snapDistancePixels;

            foreach (IDraggable draggable in GetDraggables())
                if (draggable.IsUnderMouse(GetCoordinateX((float)pixelX), GetCoordinateY((float)pixelY), snapWidth, snapHeight))
                    if (draggable.DragEnabled)
                        return draggable;

            return null;
        }

        /// <summary>
        /// Display render benchmark information on the plot
        /// </summary>
        public void Benchmark(bool enable = true) => settings.BenchmarkMessage.IsVisible = enable;
        public void BenchmarkToggle() => settings.BenchmarkMessage.IsVisible = !settings.BenchmarkMessage.IsVisible;

        /// <summary>
        /// Throw an exception if any plottable contains an invalid state. Deep validation is more thorough but slower.
        /// </summary>
        public void ValidatePlottableData(bool deep = true)
        {
            foreach (var plottable in settings.Plottables)
                plottable.ValidateData(deep);
        }
    }
}
