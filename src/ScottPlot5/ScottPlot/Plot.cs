using ScottPlot.Drawing;
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
        public float Width { get { return Dims.Width; } }
        public float Height { get { return Dims.Height; } }

        public Dimensions Dims { get; private set; } = new Dimensions();
        private void EnsureAxesExistForEveryPlottable() => Dims.CreateAxes(Plottables);

        public List<IRenderable> Renderables { get; private set; } = new List<IRenderable>();
        public List<IPlottable> Plottables { get => Renderables.Where(x => x is IPlottable).Select(x => (IPlottable)x).ToList(); }

        public readonly FigureBackground FigureBackground = new FigureBackground();
        public readonly DataBackground DataBackground = new DataBackground();
        public readonly Benchmark Benchmark = new Benchmark();
        public readonly List<Axis> Axes = new List<Axis>();
        public readonly AxisLeft AxisLeft = new AxisLeft();
        public readonly AxisRight AxisRight = new AxisRight();
        public readonly AxisTop AxisTop = new AxisTop();
        public readonly AxisBottom AxisBottom = new AxisBottom();
        public readonly ZoomRectangle ZoomRectangle = new ZoomRectangle();

        public ColorPalette Palette = ColorPalette.Category10;
        private Color GetNextColor() => Palette.GetColor(Plottables.Count());

        public Plot(int width = 600, int height = 400)
        {
            Dims.UpdateSize(width, height);
            ResetAxisStyles();
            Layout();
        }

        public void Layout(IRenderer renderer)
        {
            // use a renderer to measure the size of every axis label and tick
            foreach (Axis axis in Axes)
                axis.AutoSize(renderer);

            // then update the layout based on the new sizes
            Layout();
        }

        public void Layout(float? padLeft = null, float? padRight = null, float? padTop = null, float? padBottom = null)
        {
            float L = padLeft ?? Axes.Where(x => x is AxisLeft).Select(x => x.Size.Width).Sum();
            float R = padRight ?? Axes.Where(x => x is AxisRight).Select(x => x.Size.Width).Sum();
            float T = padTop ?? Axes.Where(x => x is AxisTop).Select(x => x.Size.Height).Sum();
            float B = padBottom ?? Axes.Where(x => x is AxisBottom).Select(x => x.Size.Height).Sum();
            Dims.UpdatePadding(L, R, T, B);
            LayoutMultipleAxes();
            RecalculateTickPositions();
        }

        private void LayoutMultipleAxes()
        {
            // place the first of each axis right next to the data
            // offset additional axes outward

            {
                AxisLeft[] leftAxes = Axes.Where(x => x is AxisLeft).Select(x => (AxisLeft)x).ToArray();
                float offset = 0;
                for (int i = 0; i < leftAxes.Count(); i++)
                {
                    leftAxes[i].Offset = offset;
                    offset += leftAxes[i].Size.Width;
                }
            }

            {
                AxisRight[] rightAxes = Axes.Where(x => x is AxisRight).Select(x => (AxisRight)x).ToArray();
                float offset = 0;
                for (int i = 0; i < rightAxes.Count(); i++)
                {
                    rightAxes[i].Offset = offset;
                    offset += rightAxes[i].Size.Width;
                }
            }

            {
                AxisTop[] topAxis = Axes.Where(x => x is AxisTop).Select(x => (AxisTop)x).ToArray();
                float offset = 0;
                for (int i = 0; i < topAxis.Count(); i++)
                {
                    topAxis[i].Offset = offset;
                    offset += topAxis[i].Size.Height;
                }
            }

            {
                AxisBottom[] bottomAxis = Axes.Where(x => x is AxisBottom).Select(x => (AxisBottom)x).ToArray();
                float offset = 0;
                for (int i = 0; i < bottomAxis.Count(); i++)
                {
                    bottomAxis[i].Offset = offset;
                    offset += bottomAxis[i].Size.Height;
                }
            }
        }

        public void ResetAxisStyles()
        {
            Axes.Clear();
            Axes.Add(AxisLeft);
            Axes.Add(AxisRight);
            Axes.Add(AxisTop);
            Axes.Add(AxisBottom);

            AxisLeft.Label = "Vertical Axis Label";
            AxisLeft.MajorGrid = true;
            AxisLeft.MinorGrid = true;

            AxisBottom.Label = "Horizontal Axis Label";
            AxisBottom.MajorGrid = true;
            AxisBottom.MinorGrid = true;

            AxisTop.Label = "Title";
            AxisTop.TickLabel = false;
            AxisTop.Size.Height = 28;

            AxisRight.Label = null;
            AxisRight.TickLabel = false;
            AxisRight.Size.Width = 15;
        }

        /// <summary>
        /// Adjust the spacing between the figure edge and data area
        /// </summary>
        public void Padding(float? left = null, float? right = null, float? above = null, float? below = null) =>
            Dims.UpdatePadding(left, right, above, below);

        public void AutoScale(double marginX = .05, double marginY = .1)
        {
            AutoScaleX(marginX);
            AutoScaleY(marginY);
        }

        public void AutoScaleX(double margin = .05)
        {
            for (int i = 0; i < Dims.XAxes.Count; i++)
                AutoScaleX(i, margin);
        }

        public void AutoScaleY(double margin = .1)
        {
            for (int i = 0; i < Dims.YAxes.Count; i++)
                AutoScaleY(i, margin);
        }

        public void AutoScale(int xAxisIndex, int yAxisIndex, double marginX = .05, double marginY = .1)
        {
            AutoScaleX(xAxisIndex, marginX);
            AutoScaleY(yAxisIndex, marginY);
        }

        public void AutoScale(int[] xAxisIndexes, int[] yAxisIndexes, double marginX = .05, double marginY = .1)
        {
            foreach (int x in xAxisIndexes)
                AutoScaleX(x, marginX);
            foreach (int y in yAxisIndexes)
                AutoScaleY(y, marginY);
        }

        private (int[] scaledXs, int[] scaledYs) AutoScaleInvalidAxes(double marginX = .05, double marginY = .1)
        {
            EnsureAxesExistForEveryPlottable();
            int[] invalidAxesX = Enumerable.Range(0, Dims.XAxes.Count).Where(x => !Dims.XAxes[x].IsValid).ToArray();
            int[] invalidAxesY = Enumerable.Range(0, Dims.YAxes.Count).Where(x => !Dims.YAxes[x].IsValid).ToArray();

            foreach (int xAxisIndex in invalidAxesX)
                AutoScaleX(xAxisIndex, marginX);
            foreach (int yAxisIndex in invalidAxesY)
                AutoScaleY(yAxisIndex, marginY);

            return (invalidAxesX, invalidAxesY);
        }

        public void AutoScaleX(int xAxisIndex = 0, double margin = .05)
        {
            EnsureAxesExistForEveryPlottable();
            var ps = Renderables.Where(x => x is IPlottable)
                                .Select(x => (IPlottable)x)
                                .Where(x => x.XAxisIndex == xAxisIndex);

            if (ps.Count() == 0)
                return;

            double min = ps.Select(x => x.Limits.X1).Min();
            double max = ps.Select(x => x.Limits.X2).Max();

            Dims.XAxes[xAxisIndex].SetLimits(min, max);
            Dims.XAxes[xAxisIndex].Zoom(1 - margin);
        }

        public void AutoScaleY(int yAxisIndex = 0, double margin = .1)
        {
            EnsureAxesExistForEveryPlottable();
            var ps = Renderables.Where(x => x is IPlottable)
                                .Select(x => (IPlottable)x)
                                .Where(x => x.YAxisIndex == yAxisIndex);

            if (ps.Count() == 0)
                return;

            double min = ps.Select(x => x.Limits.Y1).Min();
            double max = ps.Select(x => x.Limits.Y2).Max();

            Dims.YAxes[yAxisIndex].SetLimits(min, max);
            Dims.YAxes[yAxisIndex].Zoom(1 - margin);
        }

        private void RecalculateTickPositions()
        {
            EnsureAxesExistForEveryPlottable();
            foreach (Axis axis in Axes)
                axis.CalculateTicks(Dims.GetLimits(axis.XAxisIndex, axis.YAxisIndex));
        }

        /// <summary>
        /// Render a Bitmap using System.Drawing
        /// </summary>
        public System.Drawing.Bitmap GetBitmap() => GetBitmap(Dims.Width, Dims.Height);

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
        public void Render(IRenderer renderer, bool recalculateLayout = true, bool lowQuality = false)
        {
            // update figure details based on the given renderer
            Dims.UpdateSize(renderer.Width, renderer.Height);
            AutoScaleInvalidAxes();
            RecalculateTickPositions();
            if (recalculateLayout)
                Layout(renderer);

            // draw everything using the given renderer
            Benchmark.Start();
            FigureBackground.Render(renderer, Dims, lowQuality);
            DataBackground.Render(renderer, Dims, lowQuality);
            foreach (var renderable in Renderables)
                renderable.Render(renderer, Dims, lowQuality);
            foreach (Axis axis in Axes)
                axis.Render(renderer, Dims, lowQuality);
            Benchmark.Render(renderer, Dims, lowQuality);
            ZoomRectangle.Render(renderer, Dims, lowQuality);
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

        public void AddRenderable(IRenderable renderable)
        {
            Renderables.Add(renderable);
        }

        public void AddPlottable(IPlottable plottable)
        {
            Renderables.Add(plottable);
            EnsureAxesExistForEveryPlottable();
        }

        public void AddAxis(Axis axis)
        {
            Axes.Add(axis);
        }

        /// <summary>
        /// Scatter plots display unordered X/Y data pairs (but they are slower than signal plots)
        /// </summary>
        public Scatter PlotScatter(double[] xs, double[] ys)
        {
            var scatter = new Scatter() { Color = GetNextColor() };
            scatter.ReplaceXsAndYs(xs, ys);
            AddPlottable(scatter);
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

        public void ScaleX(double? xMin = null, double? xMax = null, int xAxisIndex = 0) =>
            Dims.XAxes[xAxisIndex].SetLimits(xMin ?? Dims.XAxes[xAxisIndex].Min, xMax ?? Dims.XAxes[xAxisIndex].Max);

        public void ScaleY(double? yMin = null, double? yMax = null, int yAxisIndex = 0) =>
            Dims.YAxes[yAxisIndex].SetLimits(yMin ?? Dims.YAxes[yAxisIndex].Min, yMax ?? Dims.YAxes[yAxisIndex].Max);

        public void Scale(AxisLimits2D limits, int xAxisIndex = 0, int yAxisIndex = 0) =>
            Dims.SetLimits(limits, xAxisIndex, yAxisIndex);

        public AxisLimits2D Scale(double? xMin = null, double? xMax = null, double? yMin = null, double? yMax = null,
                                  int xAxisIndex = 0, int yAxisIndex = 0)
        {
            ScaleX(xMin, xMax, xAxisIndex);
            ScaleY(yMin, yMax, yAxisIndex);
            return Dims.GetLimits(xAxisIndex, yAxisIndex);
        }
    }
}
