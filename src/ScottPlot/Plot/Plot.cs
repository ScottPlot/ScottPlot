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
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.Linq;
using System.Text;

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
            TightenLayout();
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
            plt2.Title(settings.title.text);
            plt2.XLabel(settings.xLabel.text);
            plt2.YLabel(settings.yLabel.text);

            plt2.TightenLayout();
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
            settings.gfxFigure = null;
            settings.bmpData = null;
            settings.gfxData = null;

            if (settings.figureSize.Width > 0 && settings.figureSize.Height > 0)
            {
                settings.bmpFigure = new Bitmap(settings.figureSize.Width, settings.figureSize.Height, PixelFormat.Format32bppPArgb);
                settings.gfxFigure = Graphics.FromImage(settings.bmpFigure);
            }

            if (settings.dataSize.Width > 0 && settings.dataSize.Height > 0)
            {
                settings.bmpData = new Bitmap(settings.dataSize.Width, settings.dataSize.Height, PixelFormat.Format32bppPArgb);
                settings.gfxData = Graphics.FromImage(settings.bmpData);
            }
        }

        private void RenderBitmap()
        {
            if (!settings.axes.hasBeenSet)
                settings.AxisAuto();
            else
                settings.axes.ApplyBounds();

            if (!settings.layout.tighteningOccurred)
            {
                // ticks must be populated before the layout can be tightened
                Renderer.FigureTicks(settings);
                TightenLayout();
                // only after the layout is tightened can the ticks be properly decided
            }

            settings.Benchmark.Start();
            StringBuilder errorMessages = new StringBuilder();

            if (settings.gfxFigure != null)
            {
                settings.gfxFigure.SmoothingMode = settings.misc.antiAliasFigure ? SmoothingMode.AntiAlias : SmoothingMode.None;
                settings.gfxFigure.TextRenderingHint = settings.misc.antiAliasFigure ? TextRenderingHint.AntiAliasGridFit : TextRenderingHint.SingleBitPerPixelGridFit;
                settings.FigureBackground.Render(settings);
                Renderer.FigureLabels(settings);
                Renderer.FigureTicks(settings);
                Renderer.FigureFrames(settings);
            }

            if (settings.gfxData != null)
            {
                settings.gfxData.SmoothingMode = settings.misc.antiAliasData ? SmoothingMode.AntiAlias : SmoothingMode.None;
                settings.gfxData.TextRenderingHint = settings.misc.antiAliasData ? TextRenderingHint.AntiAliasGridFit : TextRenderingHint.SingleBitPerPixelGridFit;
                settings.DataBackground.Render(settings);
                settings.HorizontalGridLines.Render(settings);
                settings.VerticalGridLines.Render(settings);

                // Construct the dimensions object to be injected into plottables during rendering.
                var dims = new PlotDimensions(settings.figureSize, settings.dataSize, settings.dataOrigin, settings.axes.Limits);
                bool lowQuality = !settings.misc.antiAliasData;

                foreach (var p in settings.plottables)
                {
                    if (p.IsVisible == false)
                        continue;

                    string error = null;
                    if (p is IValidatable pVal)
                        error = pVal.ErrorMessage(deepValidation: ValidateEveryPoint);
                    bool hasError = string.IsNullOrWhiteSpace(error) == false;
                    bool renderPlottable = true;
                    if (hasError)
                    {
                        if (ValidationErrorAction == ErrorAction.SkipRender ||
                            ValidationErrorAction == ErrorAction.DebugLog ||
                            ValidationErrorAction == ErrorAction.ShowErrorOnPlot)
                            renderPlottable = false;

                        if (ValidationErrorAction == ErrorAction.DebugLog)
                            Debug.WriteLine(error);

                        if (ValidationErrorAction == ErrorAction.ShowErrorOnPlot)
                            errorMessages.AppendLine(error);

                        if (ValidationErrorAction == ErrorAction.ThrowException)
                            throw new InvalidOperationException(error);
                    }

                    if (renderPlottable)
                    {
                        try
                        {
                            p.Render(dims, settings.bmpData, lowQuality);
                        }
                        catch (OverflowException)
                        {
                            Debug.WriteLine($"OverflowException plotting: {p}");
                        }
                    }
                }

                Renderer.MouseZoomRectangle(settings);
                Renderer.PlaceDataOntoFigure(settings);
                settings.Legend.Render(settings);
            }

            settings.Benchmark.Stop();
            settings.Benchmark.UpdateMessage(settings.plottables.Count);
            settings.Benchmark.Render(settings);
            settings.ErrorMessage.Text = errorMessages.ToString();
            settings.ErrorMessage.Render(settings);
        }

        public Bitmap GetBitmap(bool renderFirst = true, bool lowQuality = false)
        {
            if (lowQuality)
            {
                bool currentAAData = settings.misc.antiAliasData; // save currently using AA setting
                settings.misc.antiAliasData = false; // disable AA for render
                if (renderFirst)
                    RenderBitmap();
                settings.misc.antiAliasData = currentAAData; // restore saved AA setting
            }
            else
            {
                if (renderFirst)
                    RenderBitmap();
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
