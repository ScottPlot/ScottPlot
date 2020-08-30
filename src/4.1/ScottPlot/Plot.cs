using ScottPlot.Plottable;
using ScottPlot.Renderable;
using ScottPlot.Renderer;
using ScottPlot.Space;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace ScottPlot
{
    public class Plot
    {
        public readonly PlotInfo info = new PlotInfo();
        public readonly List<IRenderable> renderables;

        public Plot()
        {
            renderables = new List<IRenderable>
            {
                new FigureBackground(),
                new DataBackground(),
                new AxisTicksLeft(),
                new AxisTicksBottom(),
                new DataBorder(),
                new Benchmark()
            };
        }

        /// <summary>
        /// Resize the data area to accomodate axis labels and ticks
        /// </summary>
        private void AutoLayout(float width, float height)
        {
            // TODO: determine these values by measuring axis labels and tick labels
            float dataPadL = 50;
            float dataPadR = 10;
            float dataPadB = 50;
            float dataPadT = 30;
            float dataWidth = width - dataPadR - dataPadL;
            float dataHeight = height - dataPadB - dataPadT;

            info.Resize(width, height, dataWidth, dataHeight, dataPadL, dataPadT);
        }

        /// <summary>
        /// Automatically set axes based on the limits of the plotted data
        /// </summary>
        public void AutoAxis(bool autoX = true, bool autoY = true, bool tight = false)
        {
            var oldLimits = info.GetLimits();
            var newLimits = new AxisLimits();
            foreach (IPlottable plottable in renderables.Where(x => x is IPlottable))
                newLimits.Expand(plottable.Limits);

            // TODO: improve when tight is false so instead of zooming out blindly it zooms out to the next tick mark

            if (autoX == false)
            {
                newLimits.X1 = oldLimits.X1;
                newLimits.X2 = oldLimits.X2;
            }

            if (autoY == false)
            {
                newLimits.Y1 = oldLimits.Y1;
                newLimits.Y2 = oldLimits.Y2;
            }

            info.SetLimits(newLimits);
        }

        /// <summary>
        /// Render a Bitmap using System.Drawing
        /// </summary>
        public System.Drawing.Bitmap GetBitmap(float width, float height)
        {
            AutoLayout(width, height);
            var bmp = new System.Drawing.Bitmap((int)info.Width, (int)info.Height, System.Drawing.Imaging.PixelFormat.Format32bppPArgb);
            using (var renderer = new SystemDrawingRenderer(bmp))
            {
                Render(renderer);
            }
            return bmp;
        }

        /// <summary>
        /// Draw the plot with a custom renderer
        /// </summary>
        public void Render(IRenderer renderer)
        {
            // reset benchmarks
            foreach (Benchmark bench in renderables.Where(x => x is Benchmark))
                bench.Start();

            // only resize the layout if the dimensions have changed
            if (renderer.Width != info.Width || renderer.Height != info.Height)
                AutoLayout(renderer.Width, renderer.Height);

            // ensure our axes are valid
            if (info.GetLimits().IsValid == false)
                AutoAxis();

            // calculate ticks based on new layout
            foreach (AxisTicks axisTicks in renderables.Where(x => x is AxisTicks))
                axisTicks.Recalculate(info.GetLimits());

            // render each of the layers
            foreach (var renderable in renderables.Where(x => x.Layer == PlotLayer.BelowData))
                renderable.Render(renderer, info);

            foreach (var plottable in renderables.Where(x => x.Layer == PlotLayer.Data))
                plottable.Render(renderer, info);

            foreach (var renderable in renderables.Where(x => x.Layer == PlotLayer.AboveData))
                renderable.Render(renderer, info);
        }

        /// <summary>
        /// Remove all plottables
        /// </summary>
        public void Clear()
        {
            var plottables = renderables.Where(x => x.Layer == PlotLayer.Data);
            foreach (var plottable in plottables)
                renderables.Remove(plottable);
        }

        /// <summary>
        /// Scatter plots display unordered X/Y data pairs (but they are slower than signal plots)
        /// </summary>
        public Scatter PlotScatter(double[] xs, double[] ys, Color color = null)
        {
            var scatter = new Scatter() { Color = color ?? Colors.Magenta };
            scatter.ReplaceXsAndYs(xs, ys);
            renderables.Add(scatter);
            return scatter;
        }

        /// <summary>
        /// Render the plot and save it as an image file
        /// </summary>
        public System.Drawing.Bitmap SaveFig(string path, float width, float height)
        {
            System.Drawing.Bitmap bmp = GetBitmap(width, height);
            path = System.IO.Path.GetFullPath(path);
            string ext = System.IO.Path.GetExtension(path).ToLower();
            if (ext == ".bmp")
                bmp.Save(path, System.Drawing.Imaging.ImageFormat.Bmp);
            else if (ext == ".png")
                bmp.Save(path, System.Drawing.Imaging.ImageFormat.Png);
            else if (ext == ".jpg" || ext == ".jpeg")
                bmp.Save(path, System.Drawing.Imaging.ImageFormat.Jpeg);
            else
                throw new ArgumentException("file format not supported");
            return bmp;
        }
    }
}
