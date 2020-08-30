using ScottPlot.Plottable;
using ScottPlot.Renderable;
using ScottPlot.Renderer;
using ScottPlot.Space;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ScottPlot
{
    public class Plot
    {
        public float Width { get { return info.Width; } }
        public float Height { get { return info.Height; } }

        private readonly PlotInfo info = new PlotInfo();
        private readonly List<IRenderable> renderables;

        public Plot(float width = 600, float height = 400)
        {
            ResizeLayout(width, height);

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

        public PlotInfo GetInfo(bool warn = true)
        {
            if (warn)
                Console.WriteLine("Interacting with the Info module is for developers only");
            return info;
        }

        private void ResizeLayout(float width, float height)
        {
            if ((width < 1) || (height < 1))
                throw new ArgumentException("Width and height must be greater than 1");

            // TODO: determine these values by measuring axis labels and tick labels
            float dataPadL = 50;
            float dataPadR = 10;
            float dataPadB = 50;
            float dataPadT = 30;
            float dataWidth = width - dataPadR - dataPadL;
            float dataHeight = height - dataPadB - dataPadT;

            info.Resize(width, height, dataWidth, dataHeight, dataPadL, dataPadT);
        }

        public void AxisAuto()
        {
            var autoAxisLimits = new AxisLimits();
            foreach (IPlottable plottable in renderables.Where(x => x is IPlottable))
                autoAxisLimits.Expand(plottable.Limits);
            Console.WriteLine(autoAxisLimits);
            info.SetLimits(autoAxisLimits);
        }

        /// <summary>
        /// Render a Bitmap using System.Drawing
        /// </summary>
        public System.Drawing.Bitmap Render(float width, float height)
        {
            ResizeLayout(width, height);
            return Render();
        }

        /// <summary>
        /// Render a Bitmap using System.Drawing
        /// </summary>
        public System.Drawing.Bitmap Render()
        {
            var bmp = new System.Drawing.Bitmap((int)Width, (int)Height, System.Drawing.Imaging.PixelFormat.Format32bppPArgb);
            using (var renderer = new SystemDrawingRenderer(bmp))
            {
                Render(renderer);
            }
            return bmp;
        }

        /// <summary>
        /// Render a Bitmap using a custom renderer
        /// </summary>
        public void Render(IRenderer renderer)
        {
            // reset benchmarks
            foreach (Benchmark bench in renderables.Where(x => x is Benchmark))
                bench.Start();

            // only resize the layout if the dimensions have changed
            if (renderer.Width != info.Width || renderer.Height != info.Height)
                ResizeLayout(renderer.Width, renderer.Height);

            // ensure our axes are valid
            if (info.GetLimits().IsValid == false)
                AxisAuto();

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

        public void Clear()
        {
            var plottables = renderables.Where(x => x.Layer == PlotLayer.Data);
            foreach (var plottable in plottables)
                renderables.Remove(plottable);
        }

        public Scatter PlotScatter(double[] xs, double[] ys, Color color = null)
        {
            var scatter = new Scatter() { Color = color ?? Colors.Magenta };
            scatter.ReplaceXsAndYs(xs, ys);
            renderables.Add(scatter);
            return scatter;
        }

        public System.Drawing.Bitmap SaveFig(string path)
        {
            return SaveFig(info.Width, info.Height, path);
        }

        public System.Drawing.Bitmap SaveFig(float width, float height, string path)
        {
            System.Drawing.Bitmap bmp = Render(width, height);
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
