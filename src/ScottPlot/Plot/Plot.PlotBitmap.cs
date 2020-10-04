/* Code here extends Plot module with methods to construct plottables.
 *   - Plottables created here are added to the plottables list and returned.
 *   - Long lists of optional arguments (matplotlib style) are permitted.
 *   - Use one line per argument to simplify the tracking of changes.
 */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace ScottPlot
{
    public partial class Plot
    {

        public PlottableImage PlotBitmap(
           Bitmap bitmap,
           double x,
           double y,
           string label = null,
           ImageAlignment alignment = ImageAlignment.middleLeft,
           double rotation = 0,
           Color? frameColor = null,
           int frameSize = 0
           )
        {
            PlottableImage plottableImage = new PlottableImage()
            {
                image = bitmap,
                x = x,
                y = y,
                label = label,
                alignment = alignment,
                rotation = rotation,
                frameColor = frameColor ?? Color.White,
                frameSize = frameSize
            };

            settings.plottables.Add(plottableImage);
            return plottableImage;
        }

        public PlottableHeatmap PlotHeatmap(
            double[,] intensities,
            Drawing.Colormap colormap = null,
            string label = null,
            double[] axisOffsets = null,
            double[] axisMultipliers = null,
            double? scaleMin = null,
            double? scaleMax = null,
            double? transparencyThreshold = null,
            Bitmap backgroundImage = null,
            bool displayImageAbove = false,
            bool drawAxisLabels = true
            )
        {
            if (colormap == null)
                colormap = Drawing.Colormap.Viridis;

            if (axisOffsets == null)
                axisOffsets = new double[] { 0, 0 };

            if (axisMultipliers == null)
                axisMultipliers = new double[] { 1, 1 };

            PlottableHeatmap heatmap = new PlottableHeatmap(intensities, colormap, label, axisOffsets, axisMultipliers, scaleMin, scaleMax, transparencyThreshold, backgroundImage, displayImageAbove, drawAxisLabels);
            Add(heatmap);
            MatchAxis(this);
            Ticks(false, false);
            Layout(y2LabelWidth: 180);

            return heatmap;
        }
    }
}
