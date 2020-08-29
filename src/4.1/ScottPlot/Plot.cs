using ScottPlot.Plottable;
using ScottPlot.Renderable;
using ScottPlot.Renderer;
using ScottPlot.Space;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace ScottPlot
{
    public class Plot
    {
        private readonly PlotInfo info = new PlotInfo();
        private readonly List<IRenderable> renderables;

        public Plot()
        {
            renderables = new List<IRenderable>
            {
                new FigureBackground(),
                new DataBackground(),
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
            float dataPadB = 20;
            float dataPadT = 30;
            float dataWidth = width - dataPadR - dataPadL;
            float dataHeight = height - dataPadB - dataPadT;

            info.Resize(width, height, dataWidth, dataHeight, dataPadL, dataPadT);
        }

        public Bitmap Render(int width, int height)
        {
            Bitmap bmp = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format32bppPArgb);
            using (var renderer = new SystemDrawingRenderer(bmp))
            {
                Render(renderer);
            }
            return bmp;
        }

        public void AxisAuto()
        {
            var autoAxisLimits = new AxisLimits();
            foreach (IPlottable plottable in renderables.Where(x => x is IPlottable))
                autoAxisLimits.Expand(plottable.Limits);
            Console.WriteLine(autoAxisLimits);
            info.SetLimits(autoAxisLimits);
        }

        public void Render(IRenderer renderer)
        {
            // reset benchmarks
            foreach (Benchmark bench in renderables.Where(x => x is Benchmark))
                bench.Start();

            if (renderer.Width != info.Width || renderer.Height != info.Height)
                ResizeLayout(renderer.Width, renderer.Height);

            // ensure our axes are valid
            if (info.GetLimits().IsValid == false)
                AxisAuto();

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

        public Scatter PlotScatter(double[] xs, double[] ys)
        {
            var scatter = new Scatter();
            scatter.ReplaceXsAndYs(xs, ys);
            renderables.Add(scatter);
            return scatter;
        }
    }
}
