using ScottPlot.Plottable;
using ScottPlot.Renderable;
using System;
using System.Collections.Generic;
using System.Drawing;
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
                new Benchmark()
            };
        }

        private void ResizeLayout(float width, float height)
        {
            if ((width < 1) || (height < 1))
                throw new ArgumentException("Width and height must be greater than 1");

            // TODO: determine these values by measuring axis labels and tick labels
            float dataPadL = 20;
            float dataPadR = 10;
            float dataPadB = 20;
            float dataPadT = 30;
            float dataWidth = width - dataPadR - dataPadL;
            float dataHeight = height - dataPadB - dataPadT;

            info.Resize(width, height, dataWidth, dataHeight, dataPadL, dataPadT);
        }

        public Bitmap Render()
        {
            Bitmap bmp = new Bitmap((int)Width, (int)Height, System.Drawing.Imaging.PixelFormat.Format32bppPArgb);
            Render(bmp);
            return bmp;
        }

        public Bitmap Render(Bitmap bmp)
        {
            ResizeLayout(bmp.Width, bmp.Height);

            foreach (var renderable in renderables.Where(x => x.Layer == PlotLayer.BelowData))
                renderable.Render(bmp, info);

            foreach (var renderable in renderables.Where(x => x.Layer == PlotLayer.Data))
                renderable.Render(bmp, info);

            foreach (var renderable in renderables.Where(x => x.Layer == PlotLayer.AboveData))
                renderable.Render(bmp, info);

            return bmp;
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
