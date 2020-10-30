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

        // settings the user can customize
        public readonly FigureBackground FigureBackground = new FigureBackground();
        public readonly DataBackground DataBackground = new DataBackground();
        public readonly BenchmarkMessage BenchmarkMessage = new BenchmarkMessage();
        public readonly ErrorMessage ErrorMessage = new ErrorMessage();

        // axes contain axis label, tick, and grid settings
        private readonly List<Axis> XAxes = new List<Axis>() {
            new Axis() { Edge = Edge.Bottom, PixelSize = 40, MajorGrid = true },
            new Axis() { Edge = Edge.Top, PixelSize = 40, Bold = true },
        };
        private readonly List<Axis> YAxes = new List<Axis>() {
            new Axis() { Edge = Edge.Left, PixelSize = 60, MajorGrid = true },
            new Axis() { Edge = Edge.Right, PixelSize = 60 }
        };
        private Axis[] AllAxes { get => XAxes.Concat(YAxes).ToArray(); }

        public Axis XAxis { get => XAxes[0]; } // bottom
        public Axis XAxis2 { get => XAxes[1]; } // top (title)
        public Axis YAxis { get => YAxes[0]; } // left
        public Axis YAxis2 { get => YAxes[1]; } // right

        public Plot(int width = 800, int height = 600)
        {
            if (width <= 0 || height <= 0)
                throw new ArgumentException("width and height must each be greater than 0");
            settings = new Settings();
            StyleTools.SetStyle(this, ScottPlot.Style.Default);
            Resize(width, height);
        }

        public override string ToString() =>
            $"ScottPlot ({settings.figureSize}) with {Plottables.Length:n0} plot objects";

        /// <summary>
        /// Return a new Plot with all the same Plottables (and some of the styles) of this one
        /// </summary>
        public Plot Copy()
        {
            Plot plt2 = new Plot(settings.figureSize.Width, settings.figureSize.Height);
            var settings2 = plt2.GetSettings(false);
            settings2.plottables.AddRange(settings.plottables);

            // TODO: add a Copy() method to the settings module, or perhaps Update(existingSettings).

            // copy over only the most relevant styles
            plt2.Title(XAxis2.Title);
            plt2.XLabel(XAxis.Title);
            plt2.YLabel(YAxis.Title);

            plt2.AxisAuto();
            return plt2;
        }

        public void Resize(int? width = null, int? height = null)
        {
            if (width == null)
                width = settings.figureSize.Width;
            if (height == null)
                height = settings.figureSize.Height;

            if (width < 1)
                width = 1;
            if (height < 1)
                height = 1;

            settings.Resize((int)width, (int)height);
            InitializeBitmaps();
        }

        private void InitializeBitmaps()
        {
            settings.bmpFigure = null;
            settings.bmpData = null;

            if (settings.figureSize.Width > 0 && settings.figureSize.Height > 0)
            {
                settings.bmpFigure = new Bitmap(settings.figureSize.Width, settings.figureSize.Height, PixelFormat.Format32bppPArgb);
            }

            if (settings.dataSize.Width > 0 && settings.dataSize.Height > 0)
            {
                settings.bmpData = new Bitmap(settings.dataSize.Width, settings.dataSize.Height, PixelFormat.Format32bppPArgb);
            }
        }

        public Bitmap Render(Bitmap renderOnThis)
        {
            RenderBitmap(renderOnThis);
            return renderOnThis;
        }

        private void RenderBitmap()
        {
            RenderBitmap(settings.bmpFigure);
        }

        private PlotDimensions MakeDims(float width, float height, bool autoSizeAllAxes = true)
        {
            if (autoSizeAllAxes)
                foreach (var axis in AllAxes)
                    axis.AutoSize();

            float padLeft = AllAxes.Where(x => x.Edge == Edge.Left).Select(x => x.PixelSize).Sum();
            float padRight = AllAxes.Where(x => x.Edge == Edge.Right).Select(x => x.PixelSize).Sum();
            float padBottom = AllAxes.Where(x => x.Edge == Edge.Bottom).Select(x => x.PixelSize).Sum();
            float padTop = AllAxes.Where(x => x.Edge == Edge.Top).Select(x => x.PixelSize).Sum();
            return new PlotDimensions(
                figureSize: new SizeF(width, height),
                dataSize: new SizeF(width - padLeft - padRight, height - padTop - padBottom),
                dataOffset: new PointF(padLeft, padTop),
                axisLimits: settings.axes.Limits);
        }

        private bool NeedsAutoLayout = true;
        private void RenderBitmap(Bitmap bmp)
        {
            BenchmarkMessage.Restart();
            RenderLegacyLayoutAdjustment();

            var dims = MakeDims(bmp.Width, bmp.Height);

            if (NeedsAutoLayout)
            {
                // on first layout two adjustments are requied: 
                // one for gross tick placement, and another for string measurement
                dims = MakeDims(bmp.Width, bmp.Height);
                NeedsAutoLayout = false;
            }

            bool lowQuality = !settings.misc.antiAliasData;

            RenderBeforePlottables(dims, bmp, lowQuality);
            RenderPlottables(dims, bmp, lowQuality);
            RenderAfterPlottables(dims, bmp, lowQuality);
        }

        private void RenderLegacyLayoutAdjustment()
        {
            if (!settings.axes.hasBeenSet)
                settings.AxisAuto();
            else
                settings.axes.ApplyBounds();

            if (!settings.layout.tighteningOccurred)
            {
                TightenLayout();
            }
        }

        private void RenderBeforePlottables(PlotDimensions dims, Bitmap bmp, bool lowQuality)
        {
            FigureBackground.Render(dims, bmp, lowQuality);
            DataBackground.Render(dims, bmp, lowQuality);

            settings.ticks.x.Recalculate(settings);
            XAxis.SetTicks(settings.ticks.x.tickPositionsMajor, settings.ticks.x.tickLabels, settings.ticks.x.tickPositionsMinor);
            XAxis.Render(dims, bmp, lowQuality: false);

            settings.ticks.y.Recalculate(settings);
            YAxis.SetTicks(settings.ticks.y.tickPositionsMajor, settings.ticks.y.tickLabels, settings.ticks.y.tickPositionsMinor);
            YAxis.Render(dims, bmp, lowQuality: false);

            XAxis2.Render(dims, bmp, lowQuality: false);
            YAxis2.Render(dims, bmp, lowQuality: false);
        }

        private void RenderPlottables(PlotDimensions dims, Bitmap bmp, bool lowQuality)
        {
            var plottablesToRender = settings.plottables.Where(x => x.IsVisible);
            foreach (var plottable in plottablesToRender)
            {
                try
                {
                    plottable.Render(dims, bmp, lowQuality);
                }
                catch (OverflowException)
                {
                    Debug.WriteLine($"OverflowException plotting: {plottable}");
                }
            }
        }

        private void RenderAfterPlottables(PlotDimensions dims, Bitmap bmp, bool lowQuality)
        {
            BenchmarkMessage.Stop();
            // TODO: set up validation check reporting
            //ErrorMessage.Text = "Error Message";

            BenchmarkMessage.Render(dims, bmp, lowQuality);
            ErrorMessage.Render(dims, bmp, lowQuality);
        }

        public Bitmap GetBitmap(bool renderFirst = true, bool lowQuality = false)
        {
            if (NeedsAutoLayout)
                RenderBitmap();

            if (lowQuality)
            {
                bool currentAAData = settings.misc.antiAliasData; // save currently using AA setting
                settings.misc.antiAliasData = false; // disable AA for render
                if (renderFirst)
                {
                    RenderBitmap();
                }
                settings.misc.antiAliasData = currentAAData; // restore saved AA setting
            }
            else
            {
                if (renderFirst)
                {
                    RenderBitmap();
                }
            }
            return settings.bmpFigure;
        }

        public void SaveFig(string filePath, bool renderFirst = true)
        {
            if (renderFirst)
                RenderBitmap();

            if (settings.figureSize.Width == 1 || settings.figureSize.Height == 1)
                throw new Exception("The figure has not yet been sized (it is 1px by 1px). Resize the figure and try to save again.");

            filePath = System.IO.Path.GetFullPath(filePath);
            string fileFolder = System.IO.Path.GetDirectoryName(filePath);
            if (!System.IO.Directory.Exists(fileFolder))
                throw new Exception($"ERROR: folder does not exist: {fileFolder}");

            ImageFormat imageFormat;
            string extension = System.IO.Path.GetExtension(filePath).ToUpper();
            if (extension == ".JPG" || extension == ".JPEG")
                imageFormat = ImageFormat.Jpeg; // TODO: use jpgEncoder to set custom compression level
            else if (extension == ".PNG")
                imageFormat = ImageFormat.Png;
            else if (extension == ".TIF" || extension == ".TIFF")
                imageFormat = ImageFormat.Tiff;
            else if (extension == ".BMP")
                imageFormat = ImageFormat.Bmp;
            else
                throw new NotImplementedException("Extension not supported: " + extension);

            settings.bmpFigure.Save(filePath, imageFormat);
        }

        public void Add(IRenderable plottable)
        {
            settings.plottables.Add(plottable);
        }

        [Obsolete("Access the 'Plot.Plottables' array instead", true)]
        public List<IRenderable> GetPlottables() => settings.plottables;
        public IRenderable[] Plottables { get => settings.plottables.ToArray(); }

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
            double snapWidth = GetSettings(false).xAxisUnitsPerPixel * snapDistancePixels;
            double snapHeight = GetSettings(false).yAxisUnitsPerPixel * snapDistancePixels;

            foreach (IDraggable draggable in GetDraggables())
                if (draggable.IsUnderMouse(CoordinateFromPixelX(pixelX), CoordinateFromPixelY(pixelY), snapWidth, snapHeight))
                    if (draggable.DragEnabled)
                        return draggable;

            return null;
        }

        public Settings GetSettings(bool showWarning = true)
        {
            if (showWarning)
                Debug.WriteLine("WARNING: GetSettings() is only for development and testing. Be aware its class structure changes frequently!");

            return settings;
        }
    }
}
