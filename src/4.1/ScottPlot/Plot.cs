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

        public Plot(int width = 600, int height = 400)
        {
            Info.UpdateSize(width, height);
            Info.UpdatePadding(left: 150, right: 100, above: 100, below: 100);
            Console.WriteLine(Info);

            AddAxes(1, 3);
            Renderables = new List<IRenderable>
            {
                new FigureBackground(),
                new DataBackground(),
                new AxisTicksTop() { Label = "Horizontal Upper" },
                new AxisTicksBottom() { Label = "Horizontal Lower" },
                new AxisTicksLeft() { Label = "Left Vertical",
                    EdgeColor = Colors.Red, MajorTickColor = Colors.Red, MinorTickColor = Colors.Red, 
                    LabelFontColor = Colors.Red, TickFontColor = Colors.Red },
                new AxisTicksRight() {
                    Label = "Right Vertical", YAxisIndex = 1, MajorGrid = false, MinorGrid = false,
                    EdgeColor = Colors.Green, MajorTickColor = Colors.Green, MinorTickColor = Colors.Green, 
                    LabelFontColor = Colors.Green, TickFontColor = Colors.Green
                },
                new AxisTicksLeft() { Label = "Floating Vertical", YAxisIndex = 2, Offset = 80, 
                    MajorGrid = false, MinorGrid = false,
                    EdgeColor = Colors.Blue, MajorTickColor = Colors.Blue, MinorTickColor = Colors.Blue,
                    LabelFontColor = Colors.Blue, TickFontColor = Colors.Blue
                },
                new Benchmark()
            };
        }

        public void AddAxes(int totalX, int totalY)
        {
            Info.AddAxes(totalX, totalY);
        }

        /// <summary>
        /// Adjust the spacing between the figure edge and data area
        /// </summary>
        public void Padding(float? left = null, float? right = null, float? above = null, float? below = null) =>
            Info.UpdatePadding(left, right, above, below);

        public void AutoAxis()
        {
            AutoX();
            AutoY();
        }

        public void AutoX()
        {
            for (int i = 0; i < Info.XAxes.Count; i++)
                AutoX(i);
        }

        public void AutoY()
        {
            for (int i = 0; i < Info.YAxes.Count; i++)
                AutoY(i);
        }

        public void AutoAxis(int xAxisIndex, int yAxisIndex)
        {
            AutoX(xAxisIndex);
            AutoY(yAxisIndex);
        }

        public void AutoX(int xAxisIndex = 0)
        {
            var ps = Renderables.Where(x => x is IPlottable)
                                .Select(x => (IPlottable)x)
                                .Where(x => x.XAxisIndex == xAxisIndex);

            if (ps.Count() == 0)
                return;

            double min = ps.Select(x => x.Limits.X1).Min();
            double max = ps.Select(x => x.Limits.X2).Max();

            Info.XAxes[xAxisIndex].SetLimits(min, max);
        }

        public void AutoY(int yAxisIndex = 0)
        {
            var ps = Renderables.Where(x => x is IPlottable)
                                .Select(x => (IPlottable)x)
                                .Where(x => x.YAxisIndex == yAxisIndex);

            if (ps.Count() == 0)
                return;

            double min = ps.Select(x => x.Limits.Y1).Min();
            double max = ps.Select(x => x.Limits.Y2).Max();

            Info.YAxes[yAxisIndex].SetLimits(min, max);
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

            for (int i = 0; i < Info.XAxes.Count; i++)
                if (Info.XAxes[i].IsValid == false)
                    AutoX(i);

            for (int i = 0; i < Info.YAxes.Count; i++)
                if (Info.YAxes[i].IsValid == false)
                    AutoY(i);

            foreach (AxisTicks axisTicks in Renderables.Where(x => x is AxisTicks))
            {
                var limits = Info.GetLimits(axisTicks.XAxisIndex, axisTicks.YAxisIndex);
                axisTicks.Recalculate(limits);
            }

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

        // TODO: replace with separated X and Y methods?
        public AxisLimits2D Axis(
            double? xMin = null,
            double? xMax = null,
            double? yMin = null,
            double? yMax = null,
            int xAxisIndex = 0,
            int yAxisIndex = 0
            )
        {
            AxisLimits2D currentLimits = Info.GetLimits(xAxisIndex, yAxisIndex);

            if (xMin is null && xMax is null && yMin is null && yMax is null)
                return currentLimits;

            double x1 = xMin ?? currentLimits.X1;
            double x2 = xMax ?? currentLimits.X2;
            double y1 = yMin ?? currentLimits.Y1;
            double y2 = yMax ?? currentLimits.Y2;
            AxisLimits2D newLimits = new AxisLimits2D(x1, x2, y1, y2);
            Info.SetLimits(newLimits, xAxisIndex, yAxisIndex);
            return newLimits;
        }
    }
}
