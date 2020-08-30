using ScottPlot.Plottable;
using ScottPlot.Renderable;
using ScottPlot.Renderer;
using ScottPlot.Space;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;

namespace ScottPlot
{
    public class Plot
    {
        public float Width { get { return Info.Width; } }
        public float Height { get { return Info.Height; } }

        public PlotInfo Info { get; private set; } = new PlotInfo();
        public List<IRenderable> Renderables { get; private set; } = new List<IRenderable>();

        public FigureBackground FigureBackground { get; private set; } = new FigureBackground();
        public DataBackground DataBackground { get; private set; } = new DataBackground();

        public AxisLabelTop Title { get; private set; } = new AxisLabelTop();
        public AxisLabelLeft YLabel { get; private set; } = new AxisLabelLeft();
        public AxisLabelRight Y2Label { get; private set; } = new AxisLabelRight();
        public AxisLabelBottom XLabel { get; private set; } = new AxisLabelBottom();

        public AxisTicksLeft YTicks { get; private set; } = new AxisTicksLeft();
        public AxisTicksRight Y2Ticks { get; private set; } = new AxisTicksRight();
        public AxisTicksBottom XTicks { get; private set; } = new AxisTicksBottom();
        public AxisTicksTop X2Ticks { get; private set; } = new AxisTicksTop();

        public DataBorder DataBorder { get; private set; } = new DataBorder();
        public Benchmark Benchmark { get; private set; } = new Benchmark();

        public Plot(int width = 600, int height = 400)
        {
            Info.UpdateSize(width, height);
            Info.UpdatePadding(left: 60, right: 60, above: 50, below: 50);
            Console.WriteLine(Info);

            Renderables = new List<IRenderable>
            {
                FigureBackground,
                Title,
                XLabel,
                YLabel,
                Y2Label,
                DataBackground,
                X2Ticks,
                XTicks,
                YTicks,
                Y2Ticks,
                DataBorder,
                Benchmark
            };
        }

        /// <summary>
        /// Adjust the spacing between the figure edge and data area
        /// </summary>
        public void Padding(float? left = null, float? right = null, float? above = null, float? below = null) =>
            Info.UpdatePadding(left, right, above, below);

        /// <summary>
        /// Automatically set axes based on the limits of the plotted data
        /// </summary>
        public void AutoAxis(bool autoX = true, bool autoY = true, bool tight = false)
        {
            var oldLimits = Info.GetLimits();
            var limits = new AxisLimits();
            foreach (IPlottable plottable in Renderables.Where(x => x is IPlottable))
                limits.Expand(plottable.Limits);

            // TODO: improve when tight is false so instead of zooming out blindly it zooms out to the next tick mark

            limits.X1 = autoX ? limits.X1 : oldLimits.X1;
            limits.X2 = autoX ? limits.X2 : oldLimits.X2;
            limits.Y1 = autoY ? limits.Y1 : oldLimits.Y1;
            limits.Y2 = autoY ? limits.Y2 : oldLimits.Y2;
            Info.SetLimits(limits);
        }

        /// <summary>
        /// Render a Bitmap using System.Drawing
        /// </summary>
        public System.Drawing.Bitmap GetBitmap() => GetBitmap(Info.Width, Info.Height);

        /// <summary>
        /// Render a Bitmap using System.Drawing
        /// </summary>
        public System.Drawing.Bitmap GetBitmap(float width, float height)
        {
            var pixelFormat = System.Drawing.Imaging.PixelFormat.Format32bppPArgb;
            var bmp = new System.Drawing.Bitmap((int)width, (int)height, pixelFormat);
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
            foreach (Benchmark bench in Renderables.Where(x => x is Benchmark))
                bench.Start();

            Info.UpdateSize(renderer.Width, renderer.Height);
            Console.WriteLine(Info);

            if (Info.LimitsHaveBeenSet == false)
                AutoAxis();

            foreach (AxisTicks axisTicks in Renderables.Where(x => x is AxisTicks))
                axisTicks.Recalculate(Info.GetLimits());

            foreach (var renderable in Renderables.Where(x => x.Layer == PlotLayer.BelowData))
                renderable.Render(renderer, Info);
            foreach (var plottable in Renderables.Where(x => x.Layer == PlotLayer.Data))
                plottable.Render(renderer, Info);
            foreach (var renderable in Renderables.Where(x => x.Layer == PlotLayer.AboveData))
                renderable.Render(renderer, Info);
        }

        /// <summary>
        /// Remove all plottables
        /// </summary>
        public void Clear()
        {
            var plottables = Renderables.Where(x => x.Layer == PlotLayer.Data);
            foreach (var plottable in plottables)
                Renderables.Remove(plottable);
        }

        /// <summary>
        /// Scatter plots display unordered X/Y data pairs (but they are slower than signal plots)
        /// </summary>
        public Scatter PlotScatter(double[] xs, double[] ys, Color color = null)
        {
            var scatter = new Scatter() { Color = color ?? Colors.Magenta };
            scatter.ReplaceXsAndYs(xs, ys);
            Renderables.Add(scatter);
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

        public AxisLimits Axis(
            double? xMin = null,
            double? xMax = null,
            double? yMin = null,
            double? yMax = null,
            int planeIndex = 0
            )
        {
            AxisLimits currentLimits = Info.GetLimits(planeIndex);

            if (xMin is null && xMax is null && yMin is null && yMax is null)
                return currentLimits;

            double x1 = xMin ?? currentLimits.X1;
            double x2 = xMax ?? currentLimits.X2;
            double y1 = yMin ?? currentLimits.Y1;
            double y2 = yMax ?? currentLimits.Y2;
            AxisLimits newLimits = new AxisLimits(x1, x2, y1, y2);
            Info.SetLimits(newLimits, planeIndex);
            return newLimits;
        }
    }
}
