/* The Plot class is the primary public interface for ScottPlot.
 * State (plottables and configuration) is stored in the settings object 
 * which is private so it can be refactored without breaking the API.
 * 
 * Helper methods for styling and plottable creation are in partial class
 * files in the Plot folder.
 */

using ScottPlot.Drawing;
using ScottPlot.Plottable;
using ScottPlot.Renderable;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;

namespace ScottPlot
{
    public partial class Plot
    {
        private readonly Settings settings;

        public Plot(int width = 800, int height = 600)
        {
            if (width <= 0 || height <= 0)
                throw new ArgumentException("width and height must each be greater than 0");
            settings = new Settings();
            StyleTools.SetStyle(this, ScottPlot.Style.Default);
            Resize(width, height);
        }

        public override string ToString() =>
            $"ScottPlot ({settings.Width}x{settings.Height}) with {Plottables.Length:n0} plot objects";

        /// <summary>
        /// Return a new Plot with all the same Plottables (and some of the styles) of this one
        /// </summary>
        public Plot Copy()
        {
            Plot plt2 = new Plot(settings.Width, settings.Height);
            var settings2 = plt2.GetSettings(false);
            settings2.Plottables.AddRange(settings.Plottables);

            // TODO: add a Copy() method to the settings module, or perhaps Update(existingSettings).

            // copy over only the most relevant styles
            plt2.Title(settings.XAxis2.Title.Label);
            plt2.XLabel(settings.XAxis.Title.Label);
            plt2.YLabel(settings.YAxis.Title.Label);

            plt2.AxisAuto();
            return plt2;
        }

        public void Resize(int? width = null, int? height = null)
        {
            if (width == null)
                width = settings.Width;
            if (height == null)
                height = settings.Height;

            if (width < 1)
                width = 1;
            if (height < 1)
                height = 1;

            settings.Resize((int)width, (int)height);
        }

        private void LayoutAuto(int xAxisIndex, int yAxisIndex)
        {
            // The goal of this function is to set axis pixel size to accommodate title and tick labels.

            // This is a chicken-and-egg problem:
            //   * TICK DENSITY depends on the DATA AREA SIZE
            //   * DATA AREA SIZE depends on LAYOUT PADDING
            //   * LAYOUT PADDING depends on MAXIMUM LABEL SIZE
            //   * MAXIMUM LABEL SIZE depends on TICK DENSITY
            // To solve this, start by assuming data area size == figure size, and layout padding == 0

            // axis limits shall not change
            var dims = settings.GetPlotDimensions(xAxisIndex, yAxisIndex);
            var limits = new AxisLimits2D(dims.XMin, dims.XMax, dims.YMin, dims.YMax);
            var figSize = new SizeF(settings.Width, settings.Height);

            // first-pass tick calculation based on full image size 
            var dimsFull = new PlotDimensions2D(figSize, figSize, new PointF(0, 0), limits);

            foreach (var axis in settings.Axes)
            {
                bool isMatchingXAxis = axis.IsHorizontal && axis.AxisIndex == xAxisIndex;
                bool isMatchingYAxis = axis.IsVertical && axis.AxisIndex == yAxisIndex;
                if (isMatchingXAxis || isMatchingYAxis)
                {
                    axis.RecalculateTickPositions(dimsFull);
                    axis.RecalculateAxisSize();
                }
            }

            // now adjust our layout based on measured axis sizes
            settings.TightenLayout();

            // now recalculate ticks based on new layout
            var dataSize = new SizeF(settings.DataWidth, settings.DataHeight);
            var dataOffset = new PointF(settings.DataOffsetX, settings.DataOffsetY);

            var dims3 = new PlotDimensions2D(figSize, dataSize, dataOffset, limits);
            foreach (var axis in settings.Axes)
            {
                bool isMatchingXAxis = axis.IsHorizontal && axis.AxisIndex == xAxisIndex;
                bool isMatchingYAxis = axis.IsVertical && axis.AxisIndex == yAxisIndex;
                if (isMatchingXAxis || isMatchingYAxis)
                {
                    axis.RecalculateTickPositions(dims3);
                }
            }

            // adjust the layout based on measured tick label sizes
            settings.TightenLayout();
        }

        public void Add(IRenderable plottable)
        {
            settings.Plottables.Add(plottable);
        }

        [Obsolete("Access the 'Plot.Plottables' array instead", true)]
        public List<IRenderable> GetPlottables() => settings.Plottables;
        public IRenderable[] Plottables { get => settings.Plottables.ToArray(); }

        public List<IDraggable> GetDraggables()
        {
            List<IDraggable> draggables = new List<IDraggable>();

            foreach (var plottable in Plottables)
                if (plottable is IDraggable draggable)
                    draggables.Add(draggable);

            return draggables;
        }

        public IDraggable GetDraggableUnderMouse(double pixelX, double pixelY, int snapDistancePixels = 5)
        {
            double snapWidth = GetSettings(false).XAxis.Dims.UnitsPerPx * snapDistancePixels;
            double snapHeight = GetSettings(false).YAxis.Dims.UnitsPerPx * snapDistancePixels;

            foreach (IDraggable draggable in GetDraggables())
                if (draggable.IsUnderMouse(CoordinateFromPixelX(pixelX), CoordinateFromPixelY(pixelY), snapWidth, snapHeight))
                    if (draggable.DragEnabled)
                        return draggable;

            return null;
        }
    }
}
