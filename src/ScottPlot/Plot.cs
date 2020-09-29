/* The Plot class is the primary public interface to ScottPlot.
 * - This should be the only class the user interacts with.
 * - Internal refactoring can occur as long as these functions remain fixed.
 * - This file is intentionally spaced out to make code changes easier to review.
 * - Very little processing occurs here. This interface mostly calls private methods.
 */

using ScottPlot.Diagnostic;
using ScottPlot.Drawing;
using ScottPlot.Statistics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;

namespace ScottPlot
{
    public class Plot
    {
        public PixelFormat pixelFormat = PixelFormat.Format32bppPArgb;
        private readonly Settings settings;
        public bool DiagnosticMode = false;

        public Plot(int width = 800, int height = 600)
        {
            if (width <= 0 || height <= 0)
                throw new ArgumentException("width and height must each be greater than 0");
            settings = new Settings();
            StyleTools.SetStyle(this, ScottPlot.Style.Default);
            Resize(width, height);
            TightenLayout();
        }

        public override string ToString()
        {
            return string.Format($"ScottPlot ({0:n0} x {1:n0}) with {2:n0} objects ({3:n0} points)",
                settings.figureSize.Width, settings.figureSize.Height,
                GetPlottables().Count, GetTotalPoints());
        }

        /// <summary>
        /// Return a new Plot with all the same Plottables (and some of the styles) of this one
        /// </summary>
        public Plot Copy()
        {
            Plot plt2 = new ScottPlot.Plot(settings.figureSize.Width, settings.figureSize.Height);
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

        #region Bitmaps, Graphics, and Drawing Settings

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
                settings.bmpFigure = new Bitmap(settings.figureSize.Width, settings.figureSize.Height, pixelFormat);
                settings.gfxFigure = Graphics.FromImage(settings.bmpFigure);
            }

            if (settings.dataSize.Width > 0 && settings.dataSize.Height > 0)
            {
                settings.bmpData = new Bitmap(settings.dataSize.Width, settings.dataSize.Height, pixelFormat);
                settings.gfxData = Graphics.FromImage(settings.bmpData);
            }
        }

        private void UpdateAntiAliasingSettings()
        {

            if (settings.gfxFigure != null)
            {
                if (settings.misc.antiAliasFigure)
                {
                    settings.gfxFigure.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                    settings.gfxFigure.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
                }
                else
                {
                    settings.gfxFigure.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
                    settings.gfxFigure.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SingleBitPerPixelGridFit;
                }
            }

            if (settings.gfxData != null)
            {
                if (settings.misc.antiAliasData)
                {
                    settings.gfxData.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                    settings.gfxData.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
                }
                else
                {
                    settings.gfxData.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
                    settings.gfxData.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SingleBitPerPixelGridFit;
                }
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

            UpdateAntiAliasingSettings();

            settings.Benchmark.Start();
            if (settings.gfxFigure != null)
            {
                settings.FigureBackground.Render(settings);
                Renderer.FigureLabels(settings);
                Renderer.FigureTicks(settings);
                Renderer.FigureFrames(settings);
            }

            if (settings.gfxData != null)
            {
                settings.DataBackground.Render(settings);
                settings.HorizontalGridLines.Render(settings);
                settings.VerticalGridLines.Render(settings);
                if (DiagnosticMode)
                {
                    new DiagnosticDataChecker().CheckPlottables(settings.plottables);
                }
                Renderer.DataPlottables(settings);
                Renderer.MouseZoomRectangle(settings);
                Renderer.PlaceDataOntoFigure(settings);
                settings.Legend.Render(settings);
            }
            settings.Benchmark.Stop();
            settings.Benchmark.UpdateMessage(settings.plottables.Count, settings.GetTotalPointCount());
            settings.Benchmark.Render(settings);
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

        #endregion

        #region Managing Plot Objects

        public void Add(Plottable plottable)
        {
            settings.plottables.Add(plottable);
        }

        /// <summary>
        /// Clear all plottables
        /// </summary>
        public void Clear()
        {
            settings.plottables.Clear();
            settings.axes.Reset();
        }

        /// <summary>
        /// Clear all plottables matching the given type
        /// </summary>
        public void Clear<T>()
        {
            settings.plottables.RemoveAll(x => x is T);

            if (settings.plottables.Count == 0)
                settings.axes.Reset();
        }

        /// <summary>
        /// Clear all plottables matching the given type
        /// </summary>
        public void Clear(Type typeToClear)
        {
            settings.plottables.RemoveAll(x => x.GetType() == typeToClear);

            if (settings.plottables.Count == 0)
                settings.axes.Reset();
        }

        /// <summary>
        /// Clear all plottables of the same type as the one that is given
        /// </summary>
        public void Clear(Plottable examplePlottable)
        {
            settings.plottables.RemoveAll(x => x.GetType() == examplePlottable.GetType());

            if (settings.plottables.Count == 0)
                settings.axes.Reset();
        }

        /// <summary>
        /// Clear all plottables matching the given types
        /// </summary>
        public void Clear(Type[] typesToClear)
        {
            if (typesToClear != null)
                foreach (var typeToClear in typesToClear)
                    Clear(typeToClear);
        }

        /// <summary>
        /// Remove the given plottables from the plot
        /// </summary>
        public void Clear(Predicate<Plottable> plottablesToClear)
        {
            if (plottablesToClear != null)
                settings.plottables.RemoveAll(plottablesToClear);

            if (settings.plottables.Count == 0)
                settings.axes.Reset();
        }

        [Obsolete("This overload is deprecated. Clear plots using a different overload of the Clear() method.")]
        public void Clear(
            bool axisLines = true,
            bool scatterPlots = true,
            bool signalPlots = true,
            bool text = true,
            bool bar = true,
            bool finance = true,
            bool axisSpans = true
            )
        {
            List<int> indicesToDelete = new List<int>();
            for (int i = 0; i < settings.plottables.Count; i++)
            {
                if ((settings.plottables[i] is PlottableVLine || settings.plottables[i] is PlottableHLine) && axisLines)
                    indicesToDelete.Add(i);
                else if (settings.plottables[i] is PlottableScatter && scatterPlots)
                    indicesToDelete.Add(i);
                else if (settings.plottables[i] is PlottableSignal && signalPlots)
                    indicesToDelete.Add(i);
                else if (settings.plottables[i].GetType().IsGenericType && settings.plottables[i].GetType().GetGenericTypeDefinition() == typeof(PlottableSignalConst<>) && signalPlots)
                    indicesToDelete.Add(i);
                else if (settings.plottables[i] is PlottableText && text)
                    indicesToDelete.Add(i);
                else if (settings.plottables[i] is PlottableBar && bar)
                    indicesToDelete.Add(i);
                else if (settings.plottables[i] is PlottableOHLC && finance)
                    indicesToDelete.Add(i);
                else if ((settings.plottables[i] is PlottableVSpan || settings.plottables[i] is PlottableHSpan) && axisSpans)
                    indicesToDelete.Add(i);
            }

            indicesToDelete.Reverse();
            for (int i = 0; i < indicesToDelete.Count; i++)
                settings.plottables.RemoveAt(indicesToDelete[i]);

            settings.axes.Reset();
        }

        /// <summary>
        /// Remove the given plottable from the plot
        /// </summary>
        public void Remove(Plottable plottable)
        {
            settings.plottables.Remove(plottable);
        }

        public Colorset Colorset(Colorset colorset = null)
        {
            if (colorset != null)
                settings.colorset = colorset;

            return settings.colorset;
        }

        public PlottableText PlotText(
            string text,
            double x,
            double y,
            Color? color = null,
            string fontName = null,
            double fontSize = 12,
            bool bold = false,
            string label = null,
            TextAlignment alignment = TextAlignment.middleLeft,
            double rotation = 0,
            bool frame = false,
            Color? frameColor = null
            )
        {
            if (color == null)
                color = settings.GetNextColor();

            if (fontName == null)
                fontName = Config.Fonts.GetDefaultFontName();

            if (frameColor == null)
                frameColor = Color.White;

            fontName = Config.Fonts.GetValidFontName(fontName);

            PlottableText plottableText = new PlottableText(
                text: text,
                x: x,
                y: y,
                color: (Color)color,
                fontName: fontName,
                fontSize: fontSize,
                bold: bold,
                label: label,
                alignment: alignment,
                rotation: rotation,
                frame: frame,
                frameColor: (Color)frameColor
                );

            Add(plottableText);
            return plottableText;
        }

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

        public PlottableScatter PlotPoint(
            double x,
            double y,
            Color? color = null,
            double markerSize = 5,
            string label = null,
            double? errorX = null,
            double? errorY = null,
            double errorLineWidth = 1,
            double errorCapSize = 3,
            MarkerShape markerShape = MarkerShape.filledCircle,
            LineStyle lineStyle = LineStyle.Solid
            )
        {
            if (color == null)
                color = settings.GetNextColor();

            double[] errorXarray = (errorX != null) ? new double[] { (double)errorX } : null;
            double[] errorYarray = (errorY != null) ? new double[] { (double)errorY } : null;

            PlottableScatter scatterPlot = new PlottableScatter(
                xs: new double[] { x },
                ys: new double[] { y },
                color: (Color)color,
                lineWidth: 0,
                markerSize: markerSize,
                label: label,
                errorX: errorXarray,
                errorY: errorYarray,
                errorLineWidth: errorLineWidth,
                errorCapSize: errorCapSize,
                stepDisplay: false,
                markerShape: markerShape,
                lineStyle: lineStyle
                );

            Add(scatterPlot);
            return scatterPlot;
        }

        public PlottablePolygon PlotFill(
            double[] xs,
            double[] ys,
            string label = null,
            double lineWidth = 0,
            Color? lineColor = null,
            bool fill = true,
            Color? fillColor = null,
            double fillAlpha = 1,
            double baseline = 0
            )
        {
            if (xs.Length != ys.Length)
                throw new ArgumentException("xs and ys must all have the same length");

            double[] xs2 = Tools.Pad(xs, cloneEdges: true);
            double[] ys2 = Tools.Pad(ys, padWithLeft: baseline, padWithRight: baseline);

            return PlotPolygon(xs2, ys2, label, lineWidth, lineColor, fill, fillColor, fillAlpha);
        }

        public PlottablePolygon PlotFill(
            double[] xs1,
            double[] ys1,
            double[] xs2,
            double[] ys2,
            string label = null,
            double lineWidth = 0,
            Color? lineColor = null,
            bool fill = true,
            Color? fillColor = null,
            double fillAlpha = 1,
            double baseline = 0
            )
        {
            if ((xs1.Length != ys1.Length) || (xs2.Length != ys2.Length))
                throw new ArgumentException("xs and ys for each dataset must have the same length");

            int pointCount = xs1.Length + xs2.Length;
            double[] bothX = new double[pointCount];
            double[] bothY = new double[pointCount];

            // copy the first dataset as-is
            Array.Copy(xs1, 0, bothX, 0, xs1.Length);
            Array.Copy(ys1, 0, bothY, 0, ys1.Length);

            // copy the second dataset in reverse order
            for (int i = 0; i < xs2.Length; i++)
            {
                bothX[xs1.Length + i] = xs2[xs2.Length - 1 - i];
                bothY[ys1.Length + i] = ys2[ys2.Length - 1 - i];
            }

            return PlotPolygon(bothX, bothY, label, lineWidth, lineColor, fill, fillColor, fillAlpha);
        }

        public (PlottablePolygon, PlottablePolygon) PlotFillAboveBelow(
            double[] xs,
            double[] ys,
            string labelAbove = null,
            string labelBelow = null,
            double lineWidth = 1,
            Color? lineColor = null,
            bool fill = true,
            Color? fillColorAbove = null,
            Color? fillColorBelow = null,
            double fillAlpha = 1,
            double baseline = 0
            )
        {
            if (xs.Length != ys.Length)
                throw new ArgumentException("xs and ys must all have the same length");

            double[] xs2 = Tools.Pad(xs, cloneEdges: true);
            double[] ys2 = Tools.Pad(ys, padWithLeft: baseline, padWithRight: baseline);

            double[] ys2below = new double[ys2.Length];
            double[] ys2above = new double[ys2.Length];
            for (int i = 0; i < ys2.Length; i++)
            {
                if (ys2[i] < baseline)
                {
                    ys2below[i] = ys2[i];
                    ys2above[i] = baseline;
                }
                else
                {
                    ys2above[i] = ys2[i];
                    ys2below[i] = baseline;
                }
            }

            if (fillColorAbove is null)
                fillColorAbove = Color.Green;
            if (fillColorBelow is null)
                fillColorBelow = Color.Red;
            if (lineColor is null)
                lineColor = Color.Black;

            var polyAbove = PlotPolygon(xs2, ys2above, labelAbove, lineWidth, lineColor, fill, fillColorAbove, fillAlpha);
            var polyBelow = PlotPolygon(xs2, ys2below, labelBelow, lineWidth, lineColor, fill, fillColorBelow, fillAlpha);

            return (polyBelow, polyAbove);
        }

        public PlottableFunction PlotFunction(
            Func<double, double?> function,
            Color? color = null,
            double lineWidth = 1,
            double markerSize = 0,
            string label = "f(x)",
            MarkerShape markerShape = MarkerShape.filledCircle,
            LineStyle lineStyle = LineStyle.Solid
        )
        {
            if (color == null)
            {
                color = settings.GetNextColor();
            }

            PlottableFunction functionPlot = new PlottableFunction(function, color.Value, lineWidth, markerSize, label, markerShape, lineStyle);

            Add(functionPlot);
            return functionPlot;
        }

        public PlottableScaleBar PlotScaleBar(
            double sizeX,
            double sizeY,
            string labelX = null,
            string labelY = null,
            double thickness = 2,
            double fontSize = 12,
            Color? color = null,
            double padPx = 10
            )
        {
            color = (color is null) ? Color.Black : color.Value;
            var scalebar = new PlottableScaleBar(sizeX, sizeY, labelX, labelY, thickness, fontSize, color.Value, padPx);
            Add(scalebar);
            return scalebar;
        }

        [Obsolete("This method is experimental and may change in subsequent versions")]
        public PlottableHeatmap PlotHeatmap(
            double[,] intensities,
            Colormap colormap = null,
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
                colormap = Colormap.Viridis;

            if (axisOffsets == null)
                axisOffsets = new double[] { 0, 0 };

            if (axisMultipliers == null)
                axisMultipliers = new double[] { 1, 1 };

            PlottableHeatmap heatmap = new PlottableHeatmap(intensities, colormap, label, axisOffsets, axisMultipliers, scaleMin, scaleMax, transparencyThreshold, backgroundImage, displayImageAbove, drawAxisLabels);
            Add(heatmap);
            MatchAxis(this);
            Ticks(false, false); //I think we need to sort out our own labelling with System.Drawing
            Layout(y2LabelWidth: 180);

            return heatmap;
        }

        public PlottableScatter PlotScatter(
            double[] xs,
            double[] ys,
            Color? color = null,
            double lineWidth = 1,
            double markerSize = 5,
            string label = null,
            double[] errorX = null,
            double[] errorY = null,
            double errorLineWidth = 1,
            double errorCapSize = 3,
            MarkerShape markerShape = MarkerShape.filledCircle,
            LineStyle lineStyle = LineStyle.Solid
            )
        {
            if (color == null)
                color = settings.GetNextColor();

            PlottableScatter scatterPlot = new PlottableScatter(
                xs: xs,
                ys: ys,
                color: (Color)color,
                lineWidth: lineWidth,
                markerSize: markerSize,
                label: label,
                errorX: errorX,
                errorY: errorY,
                errorLineWidth: errorLineWidth,
                errorCapSize: errorCapSize,
                stepDisplay: false,
                markerShape: markerShape,
                lineStyle: lineStyle
                );

            Add(scatterPlot);
            return scatterPlot;
        }

        public PlottableScatterHighlight PlotScatterHighlight(
           double[] xs,
           double[] ys,
           Color? color = null,
           double lineWidth = 1,
           double markerSize = 5,
           string label = null,
           double[] errorX = null,
           double[] errorY = null,
           double errorLineWidth = 1,
           double errorCapSize = 3,
           MarkerShape markerShape = MarkerShape.filledCircle,
           LineStyle lineStyle = LineStyle.Solid,
           MarkerShape highlightedShape = MarkerShape.openCircle,
           Color? highlightedColor = null,
           double? highlightedMarkerSize = null
           )
        {
            if (color is null)
                color = settings.GetNextColor();

            if (highlightedColor is null)
                highlightedColor = Color.Red;

            if (highlightedMarkerSize is null)
                highlightedMarkerSize = 2 * markerSize;

            PlottableScatterHighlight scatterPlot = new PlottableScatterHighlight(
                xs: xs,
                ys: ys,
                color: (Color)color,
                lineWidth: lineWidth,
                markerSize: markerSize,
                label: label,
                errorX: errorX,
                errorY: errorY,
                errorLineWidth: errorLineWidth,
                errorCapSize: errorCapSize,
                stepDisplay: false,
                markerShape: markerShape,
                lineStyle: lineStyle,
                highlightedShape: highlightedShape,
                highlightedColor: highlightedColor.Value,
                highlightedMarkerSize: highlightedMarkerSize.Value
                );

            Add(scatterPlot);
            return scatterPlot;
        }

        public PlottableErrorBars PlotErrorBars(
            double[] xs,
            double[] ys,
            double[] xPositiveError = null,
            double[] xNegativeError = null,
            double[] yPositiveError = null,
            double[] yNegativeError = null,
            Color? color = null,
            double lineWidth = 1,
            double capWidth = 3,
            string label = null
            )
        {
            if (color is null)
                color = settings.GetNextColor();

            PlottableErrorBars errorBars = new PlottableErrorBars(
                xs,
                ys,
                xPositiveError,
                xNegativeError,
                yPositiveError,
                yNegativeError,
                color.Value,
                lineWidth,
                capWidth,
                label
                );

            Add(errorBars);
            return errorBars;
        }

        public PlottableRadar PlotRadar(
            double[,] values,
            string[] categoryNames = null,
            string[] groupNames = null,
            Color[] fillColors = null,
            double fillAlpha = .4,
            Color? webColor = null
            )
        {
            fillColors = fillColors ?? Enumerable.Range(0, values.Length).Select(i => settings.colorset.GetColor(i)).ToArray();
            webColor = webColor ?? Color.Gray;

            var plottable = new PlottableRadar(values, categoryNames, groupNames, fillColors, (byte)(fillAlpha * 256), webColor.Value);
            Add(plottable);
            MatchAxis(this);

            return plottable;
        }

        public PlottableAnnotation PlotAnnotation(
            string label,
            double xPixel = 10,
            double yPixel = 10,
            double fontSize = 12,
            string fontName = "Segoe UI",
            Color? fontColor = null,
            double fontAlpha = 1,
            bool fill = true,
            Color? fillColor = null,
            double fillAlpha = .2,
            double lineWidth = 1,
            Color? lineColor = null,
            double lineAlpha = 1,
            bool shadow = false
            )
        {

            fontColor = (fontColor is null) ? Color.Black : fontColor.Value;
            fillColor = (fillColor is null) ? Color.Yellow : fillColor.Value;
            lineColor = (lineColor is null) ? Color.Black : lineColor.Value;

            fontColor = Color.FromArgb((int)(255 * fontAlpha), fontColor.Value.R, fontColor.Value.G, fontColor.Value.B);
            fillColor = Color.FromArgb((int)(255 * fillAlpha), fillColor.Value.R, fillColor.Value.G, fillColor.Value.B);
            lineColor = Color.FromArgb((int)(255 * lineAlpha), lineColor.Value.R, lineColor.Value.G, lineColor.Value.B);

            var plottable = new PlottableAnnotation(
                    xPixel: xPixel,
                    yPixel: yPixel,
                    label: label,
                    fontSize: fontSize,
                    fontName: fontName,
                    fontColor: fontColor.Value,
                    fill: fill,
                    fillColor: fillColor.Value,
                    lineWidth: lineWidth,
                    lineColor: lineColor.Value,
                    shadow: shadow
                );

            Add(plottable);
            return plottable;
        }

        [Obsolete("This method is experimental and may change in subsequent versions")]
        public PlottableVectorField PlotVectorField(
            Vector2[,] vectors,
            double[] xs,
            double[] ys,
            string label = null,
            Color? color = null,
            Colormap colormap = null,
            double scaleFactor = 1
            )
        {
            if (!color.HasValue)
            {
                color = settings.GetNextColor();
            }

            var vectorField = new PlottableVectorField(vectors, xs, ys, label, color.Value, colormap, scaleFactor);

            Add(vectorField);
            return vectorField;
        }

        public PlottableScatter PlotArrow(
            double tipX,
            double tipY,
            double baseX,
            double baseY,
            double lineWidth = 5,
            float arrowheadWidth = 3,
            float arrowheadLength = 3,
            Color? color = null,
            string label = null
            )
        {

            var arrow = PlotScatter(
                xs: new double[] { baseX, tipX },
                ys: new double[] { baseY, tipY },
                color: color,
                lineWidth: lineWidth,
                label: label,
                markerSize: 0
                );


            AdjustableArrowCap arrowCap = new AdjustableArrowCap(arrowheadWidth, arrowheadLength, isFilled: true);

            arrow.penLine.CustomEndCap = arrowCap;
            arrow.penLine.StartCap = LineCap.Flat;

            return arrow;
        }

        public PlottableScatter PlotLine(
            double x1,
            double y1,
            double x2,
            double y2,
            Color? color = null,
            double lineWidth = 1,
            string label = null,
            LineStyle lineStyle = LineStyle.Solid
            )
        {
            return PlotScatter(
                xs: new double[] { x1, x2 },
                ys: new double[] { y1, y2 },
                color: color,
                lineWidth: lineWidth,
                label: label,
                lineStyle: lineStyle,
                markerSize: 0
                );
        }

        public PlottableScatter PlotLine(
            double slope,
            double offset,
            (double x1, double x2) xLimits,
            Color? color = null,
            double lineWidth = 1,
            string label = null,
            LineStyle lineStyle = LineStyle.Solid
            )
        {
            double y1 = xLimits.x1 * slope + offset;
            double y2 = xLimits.x2 * slope + offset;
            return PlotScatter(
                xs: new double[] { xLimits.x1, xLimits.x2 },
                ys: new double[] { y1, y2 },
                color: color,
                lineWidth: lineWidth,
                label: label,
                lineStyle: lineStyle,
                markerSize: 0
                );
        }

        public PlottableScatter PlotStep(
            double[] xs,
            double[] ys,
            Color? color = null,
            double lineWidth = 1,
            string label = null
            )
        {
            if (color == null)
                color = settings.GetNextColor();

            PlottableScatter stepPlot = new PlottableScatter(
                xs: xs,
                ys: ys,
                color: (Color)color,
                lineWidth: lineWidth,
                markerSize: 0,
                label: label,
                errorX: null,
                errorY: null,
                errorLineWidth: 0,
                errorCapSize: 0,
                stepDisplay: true,
                markerShape: MarkerShape.none,
                lineStyle: LineStyle.Solid
                );

            Add(stepPlot);
            return stepPlot;
        }
        public PlottableSignalXY PlotSignalXY(
            double[] xs,
            double[] ys,
            Color? color = null,
            double lineWidth = 1,
            double markerSize = 5,
            string label = null,
            int? minRenderIndex = null,
            int? maxRenderIndex = null,
            LineStyle lineStyle = LineStyle.Solid,
            bool useParallel = true
            )
        {
            if (color == null)
                color = settings.GetNextColor();

            if (minRenderIndex == null)
                minRenderIndex = 0;
            if (maxRenderIndex == null)
                maxRenderIndex = ys.Length - 1;

            PlottableSignalXY signal = new PlottableSignalXY(
                xs: xs,
                ys: ys,
                color: (Color)color,
                lineWidth: lineWidth,
                markerSize: markerSize,
                label: label,
                minRenderIndex: minRenderIndex.Value,
                maxRenderIndex: maxRenderIndex.Value,
                lineStyle: lineStyle,
                useParallel: useParallel
                );
            Add(signal);
            return signal;
        }

        public PlottableSignalXYConst<TX, TY> PlotSignalXYConst<TX, TY>(
            TX[] xs,
            TY[] ys,
            Color? color = null,
            double lineWidth = 1,
            double markerSize = 5,
            string label = null,
            int? minRenderIndex = null,
            int? maxRenderIndex = null,
            LineStyle lineStyle = LineStyle.Solid,
            bool useParallel = true
            ) where TX : struct, IComparable where TY : struct, IComparable

        {
            if (color == null)
                color = settings.GetNextColor();

            if (minRenderIndex == null)
                minRenderIndex = 0;
            if (maxRenderIndex == null)
                maxRenderIndex = ys.Length - 1;

            PlottableSignalXYConst<TX, TY> signal = new PlottableSignalXYConst<TX, TY>(
                xs: xs,
                ys: ys,
                color: (Color)color,
                lineWidth: lineWidth,
                markerSize: markerSize,
                label: label,
                minRenderIndex: minRenderIndex.Value,
                maxRenderIndex: maxRenderIndex.Value,
                lineStyle: lineStyle,
                useParallel: useParallel
                );

            Add(signal);
            return signal;
        }

        [Obsolete]
        public PlottableSignalOld PlotSignalOld(
            double[] ys,
            double sampleRate = 1,
            double xOffset = 0,
            double yOffset = 0,
            Color? color = null,
            double lineWidth = 1,
            double markerSize = 5,
            string label = null,
            Color[] colorByDensity = null,
            int? minRenderIndex = null,
            int? maxRenderIndex = null,
            LineStyle lineStyle = LineStyle.Solid,
            bool useParallel = true
            )
        {
            if (color == null)
                color = settings.GetNextColor();

            if (minRenderIndex == null)
                minRenderIndex = 0;

            if (maxRenderIndex == null)
                maxRenderIndex = ys.Length - 1;

            PlottableSignalOld signal = new PlottableSignalOld(
                ys: ys,
                sampleRate: sampleRate,
                xOffset: xOffset,
                yOffset: yOffset,
                color: (Color)color,
                lineWidth: lineWidth,
                markerSize: markerSize,
                label: label,
                colorByDensity: colorByDensity,
                minRenderIndex: minRenderIndex.Value,
                maxRenderIndex: maxRenderIndex.Value,
                lineStyle: lineStyle,
                useParallel: useParallel
                );

            Add(signal);
            return signal;
        }
        public PlottableSignal PlotSignal(
            double[] ys,
            double sampleRate = 1,
            double xOffset = 0,
            double yOffset = 0,
            Color? color = null,
            double lineWidth = 1,
            double markerSize = 5,
            string label = null,
            Color[] colorByDensity = null,
            int? minRenderIndex = null,
            int? maxRenderIndex = null,
            LineStyle lineStyle = LineStyle.Solid,
            bool useParallel = true
            )
        {
            if (color == null)
                color = settings.GetNextColor();

            if (minRenderIndex == null)
                minRenderIndex = 0;

            if (maxRenderIndex == null)
                maxRenderIndex = ys.Length - 1;

            PlottableSignal signal = new PlottableSignal(
                ys: ys,
                sampleRate: sampleRate,
                xOffset: xOffset,
                yOffset: yOffset,
                color: (Color)color,
                lineWidth: lineWidth,
                markerSize: markerSize,
                label: label,
                colorByDensity: colorByDensity,
                minRenderIndex: minRenderIndex.Value,
                maxRenderIndex: maxRenderIndex.Value,
                lineStyle: lineStyle,
                useParallel: useParallel
                );
            Add(signal);
            return signal;
        }

        public PlottableSignalConst<T> PlotSignalConst<T>(
            T[] ys,
            double sampleRate = 1,
            double xOffset = 0,
            double yOffset = 0,
            Color? color = null,
            double lineWidth = 1,
            double markerSize = 5,
            string label = null,
            Color[] colorByDensity = null,
            int? minRenderIndex = null,
            int? maxRenderIndex = null,
            LineStyle lineStyle = LineStyle.Solid,
            bool useParallel = true
            ) where T : struct, IComparable
        {
            if (color == null)
                color = settings.GetNextColor();

            if (minRenderIndex == null)
                minRenderIndex = 0;

            if (maxRenderIndex == null)
                maxRenderIndex = ys.Length - 1;

            PlottableSignalConst<T> signal = new PlottableSignalConst<T>(
                ys: ys,
                sampleRate: sampleRate,
                xOffset: xOffset,
                yOffset: yOffset,
                color: (Color)color,
                lineWidth: lineWidth,
                markerSize: markerSize,
                label: label,
                colorByDensity: colorByDensity,
                minRenderIndex: minRenderIndex.Value,
                maxRenderIndex: maxRenderIndex.Value,
                lineStyle: lineStyle,
                useParallel: useParallel
                );

            Add(signal);
            return signal;
        }

        public PlottablePie PlotPie(
            double[] values,
            string[] sliceLabels = null,
            Color[] colors = null,
            bool explodedChart = false,
            bool showValues = false,
            bool showPercentages = false,
            bool showLabels = true,
            string label = null
            )
        {
            if (colors is null)
                colors = Enumerable.Range(0, values.Length).Select(i => settings.colorset.GetColor(i)).ToArray();

            PlottablePie pie = new PlottablePie(values, sliceLabels, colors, explodedChart, showValues, showPercentages, showLabels, label);

            Add(pie);
            return pie;
        }

        public PlottableBar PlotWaterfall(
            double[] xs,
            double[] ys,
            double[] errorY = null,
            string label = null,
            double barWidth = .8,
            double xOffset = 0,
            bool fill = true,
            Color? fillColor = null,
            double outlineWidth = 1,
            Color? outlineColor = null,
            double errorLineWidth = 1,
            double errorCapSize = .38,
            Color? errorColor = null,
            bool horizontal = false,
            bool showValues = false,
            Color? valueColor = null,
            bool autoAxis = true,
            Color? negativeColor = null
            )
        {
            double[] yOffsets = Enumerable.Range(0, ys.Length).Select(count => ys.Take(count).Sum()).ToArray();
            return PlotBar(
                xs,
                ys,
                errorY,
                label,
                barWidth,
                xOffset,
                fill,
                fillColor,
                outlineWidth,
                outlineColor,
                errorLineWidth,
                errorCapSize,
                errorColor,
                horizontal,
                showValues,
                valueColor,
                autoAxis,
                yOffsets,
                negativeColor
            );
        }

        public PlottableBar PlotBar(
            double[] xs,
            double[] ys,
            double[] errorY = null,
            string label = null,
            double barWidth = .8,
            double xOffset = 0,
            bool fill = true,
            Color? fillColor = null,
            double outlineWidth = 1,
            Color? outlineColor = null,
            double errorLineWidth = 1,
            double errorCapSize = .38,
            Color? errorColor = null,
            bool horizontal = false,
            bool showValues = false,
            Color? valueColor = null,
            bool autoAxis = true,
            double[] yOffsets = null,
            Color? negativeColor = null
            )
        {
            if (fillColor == null)
                fillColor = settings.GetNextColor();

            if (outlineColor == null)
                outlineColor = Color.Black;

            if (errorColor == null)
                errorColor = Color.Black;

            if (valueColor == null)
                valueColor = Color.Black;

            if (!negativeColor.HasValue)
            {
                negativeColor = fillColor;
            }

            PlottableBar barPlot = new PlottableBar(
                xs: xs,
                ys: ys,
                barWidth: barWidth,
                xOffset: xOffset,
                fill: fill,
                fillColor: fillColor.Value,
                label: label,
                yErr: errorY,
                errorLineWidth: errorLineWidth,
                errorCapSize: errorCapSize,
                errorColor: errorColor.Value,
                outlineWidth: outlineWidth,
                outlineColor: outlineColor.Value,
                horizontal: horizontal,
                showValues: showValues,
                valueColor: valueColor.Value,
                yOffsets: yOffsets,
                negativeColor: negativeColor.Value
                );

            Add(barPlot);

            if (autoAxis)
            {
                // perform a tight axis adjustment
                AxisAuto(0, 0);
                double[] tightAxisLimits = Axis();

                // now loosen it up a bit
                AxisAuto();

                if (horizontal)
                {
                    if (tightAxisLimits[0] == 0)
                        Axis(x1: 0);
                    else if (tightAxisLimits[1] == 0)
                        Axis(x2: 0);
                }
                else
                {
                    if (tightAxisLimits[2] == 0)
                        Axis(y1: 0);
                    else if (tightAxisLimits[3] == 0)
                        Axis(y2: 0);
                }
            }

            return barPlot;
        }

        /// <summary>
        /// Create a series of bar plots given a 2D dataset
        /// </summary>
        /// <param name="groupLabels">displayed as horizontal axis tick labels</param>
        /// <param name="seriesLabels">displayed in the legend</param>
        /// <param name="ys">Array of arrays (one per series) that contan one point per group</param>
        /// <returns></returns>
        public PlottableBar[] PlotBarGroups(
                string[] groupLabels,
                string[] seriesLabels,
                double[][] ys,
                double[][] yErr = null,
                double groupWidthFraction = 0.8,
                double barWidthFraction = 0.8,
                double errorCapSize = 0.38,
                bool showValues = false
            )
        {
            if (groupLabels is null || seriesLabels is null || ys is null)
                throw new ArgumentException("labels and ys cannot be null");

            if (seriesLabels.Length != ys.Length)
                throw new ArgumentException("groupLabels and ys must be the same length");

            foreach (double[] subArray in ys)
                if (subArray.Length != groupLabels.Length)
                    throw new ArgumentException("all arrays inside ys must be the same length as groupLabels");

            int seriesCount = ys.Length;
            double barWidth = groupWidthFraction / seriesCount;
            PlottableBar[] bars = new PlottableBar[seriesCount];
            bool containsNegativeY = false;
            for (int i = 0; i < seriesCount; i++)
            {
                double offset = i * barWidth;
                double[] barYs = ys[i];
                double[] barYerr = yErr?[i];
                double[] barXs = DataGen.Consecutive(barYs.Length);
                containsNegativeY |= barYs.Where(y => y < 0).Any();
                bars[i] = PlotBar(barXs, barYs, barYerr, seriesLabels[i], barWidth * barWidthFraction, offset,
                    errorCapSize: errorCapSize, showValues: showValues);
            }

            if (containsNegativeY)
            {
                AxisAuto();
            }
            XTicks(DataGen.Consecutive(groupLabels.Length, offset: (groupWidthFraction - barWidth) / 2), groupLabels);

            return bars;
        }

        public PlottableOHLC PlotOHLC(
            OHLC[] ohlcs,
            Color? colorUp = null,
            Color? colorDown = null,
            bool autoWidth = true,
            bool sequential = false
            )
        {
            if (colorUp is null)
                colorUp = ColorTranslator.FromHtml("#26a69a");
            if (colorDown is null)
                colorDown = ColorTranslator.FromHtml("#ef5350");

            PlottableOHLC ohlc = new PlottableOHLC(ohlcs, false, autoWidth, colorUp.Value, colorDown.Value, sequential);
            Add(ohlc);
            return ohlc;
        }

        public PlottableOHLC PlotCandlestick(
            OHLC[] ohlcs,
            Color? colorUp = null,
            Color? colorDown = null,
            bool autoWidth = true,
            bool sequential = false
            )
        {
            if (colorUp is null)
                colorUp = ColorTranslator.FromHtml("#26a69a");
            if (colorDown is null)
                colorDown = ColorTranslator.FromHtml("#ef5350");

            PlottableOHLC ohlc = new PlottableOHLC(ohlcs, true, autoWidth, colorUp.Value, colorDown.Value, sequential);
            Add(ohlc);
            return ohlc;
        }

        public PlottableVLine PlotVLine(
            double x,
            Color? color = null,
            double lineWidth = 1,
            string label = null,
            bool draggable = false,
            double dragLimitLower = double.NegativeInfinity,
            double dragLimitUpper = double.PositiveInfinity,
            LineStyle lineStyle = LineStyle.Solid
            )
        {
            if (color == null)
                color = settings.GetNextColor();

            PlottableVLine axLine = new PlottableVLine(
                position: x,
                color: (Color)color,
                lineWidth: lineWidth,
                label: label,
                draggable: draggable,
                dragLimitLower: dragLimitLower,
                dragLimitUpper: dragLimitUpper,
                lineStyle: lineStyle
                );

            Add(axLine);
            return axLine;
        }

        public PlottableVSpan PlotVSpan(
            double y1,
            double y2,
            Color? color = null,
            double alpha = .5,
            string label = null,
            bool draggable = false,
            bool dragFixedSize = false,
            double dragLimitLower = double.NegativeInfinity,
            double dragLimitUpper = double.PositiveInfinity
            )
        {
            if (color == null)
                color = settings.GetNextColor();

            var axisSpan = new PlottableVSpan(
                position1: y1,
                position2: y2,
                color: (Color)color,
                alpha: alpha,
                label: label,
                draggable: draggable,
                dragFixedSize: dragFixedSize,
                dragLimitLower: dragLimitLower,
                dragLimitUpper: dragLimitUpper
                );

            Add(axisSpan);
            return axisSpan;
        }

        public PlottableHLine PlotHLine(
            double y,
            Color? color = null,
            double lineWidth = 1,
            string label = null,
            bool draggable = false,
            double dragLimitLower = double.NegativeInfinity,
            double dragLimitUpper = double.PositiveInfinity,
            LineStyle lineStyle = LineStyle.Solid
            )
        {
            if (color == null)
                color = settings.GetNextColor();

            PlottableHLine axLine = new PlottableHLine(
                position: y,
                color: (Color)color,
                lineWidth: lineWidth,
                label: label,
                draggable: draggable,
                dragLimitLower: dragLimitLower,
                dragLimitUpper: dragLimitUpper,
                lineStyle: lineStyle
                );

            Add(axLine);
            return axLine;
        }

        public PlottableHSpan PlotHSpan(
            double x1,
            double x2,
            Color? color = null,
            double alpha = .5,
            string label = null,
            bool draggable = false,
            bool dragFixedSize = false,
            double dragLimitLower = double.NegativeInfinity,
            double dragLimitUpper = double.PositiveInfinity
            )
        {
            if (color == null)
                color = settings.GetNextColor();

            var axisSpan = new PlottableHSpan(
                    position1: x1,
                    position2: x2,
                    color: (Color)color,
                    alpha: alpha,
                    label: label,
                    draggable: draggable,
                    dragFixedSize: dragFixedSize,
                    dragLimitLower: dragLimitLower,
                    dragLimitUpper: dragLimitUpper
                    );

            Add(axisSpan);
            return axisSpan;
        }

        public PlottablePolygon PlotPolygon(
            double[] xs,
            double[] ys,
            string label = null,
            double lineWidth = 0,
            Color? lineColor = null,
            bool fill = true,
            Color? fillColor = null,
            double fillAlpha = 1
            )
        {
            if (lineColor is null)
                lineColor = settings.GetNextColor();

            if (fillColor is null)
                fillColor = settings.GetNextColor();

            var plottable = new ScottPlot.PlottablePolygon(
                    xs: xs,
                    ys: ys,
                    label: label,
                    lineWidth: lineWidth,
                    lineColor: lineColor.Value,
                    fill: fill,
                    fillColor: fillColor.Value,
                    fillAlpha: fillAlpha
                );

            Add(plottable);

            return plottable;
        }

        public PlottablePolygons PlotPolygons(
            List<List<(double x, double y)>> polys,
            string label = null,
            double lineWidth = 0,
            Color? lineColor = null,
            bool fill = true,
            Color? fillColor = null,
            double fillAlpha = 1
            )
        {
            if (lineColor is null)
                lineColor = settings.GetNextColor();

            if (fillColor is null)
                fillColor = settings.GetNextColor();

            var plottable = new ScottPlot.PlottablePolygons(
                    polys: polys,
                    label: label,
                    lineWidth: lineWidth,
                    lineColor: lineColor.Value,
                    fill: fill,
                    fillColor: fillColor.Value,
                    fillAlpha: fillAlpha
                );

            Add(plottable);

            return plottable;
        }

        public PlottablePopulations PlotPopulations(Statistics.Population population, string label = null)
        {
            var plottable = new PlottablePopulations(population, label, settings.GetNextColor());
            Add(plottable);
            return plottable;
        }

        public PlottablePopulations PlotPopulations(Statistics.Population[] populations, string label = null)
        {
            var plottable = new PlottablePopulations(populations, label);
            Add(plottable);
            return plottable;
        }

        public PlottablePopulations PlotPopulations(Statistics.PopulationSeries series, string label = null)
        {
            series.color = settings.GetNextColor();
            if (label != null)
                series.seriesLabel = label;
            var plottable = new PlottablePopulations(series, label);
            Add(plottable);
            return plottable;
        }

        public PlottablePopulations PlotPopulations(Statistics.PopulationMultiSeries multiSeries)
        {
            for (int i = 0; i < multiSeries.multiSeries.Length; i++)
                multiSeries.multiSeries[i].color = settings.colorset.GetColor(i);

            var plottable = new PlottablePopulations(multiSeries);
            Add(plottable);
            return plottable;
        }

        public List<Plottable> GetPlottables()
        {
            return settings.plottables;
        }

        public List<IDraggable> GetDraggables()
        {
            List<IDraggable> draggables = new List<IDraggable>();

            foreach (Plottable plottable in GetPlottables())
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
                Debug.WriteLine("WARNING: GetSettings() is only for development and testing as its contents change frequently");

            return settings;
        }

        public int GetTotalPoints()
        {
            int totalPoints = 0;
            foreach (Plottable plottable in settings.plottables)
                totalPoints += plottable.GetPointCount();
            return totalPoints;
        }

        #endregion

        #region Axis Settings

        public double[] Axis(
            double? x1 = null,
            double? x2 = null,
            double? y1 = null,
            double? y2 = null
            )
        {
            bool someValuesAreNull = (x1 == null) || (x2 == null) || (y1 == null) || (y2 == null);
            if (someValuesAreNull && !settings.axes.hasBeenSet)
                settings.AxisAuto();

            settings.axes.Set(x1, x2, y1, y2);
            return settings.axes.limits;
        }

        // TODO: alias Axis() for LRTB labels

        public double[] Axis(double[] axisLimits)
        {
            if ((axisLimits == null) || (axisLimits.Length != 4))
                throw new ArgumentException("axis limits must contain 4 elements");
            Axis(axisLimits[0], axisLimits[1], axisLimits[2], axisLimits[3]);
            return settings.axes.limits;
        }

        public void AxisBounds(
            double minX = double.NegativeInfinity,
            double maxX = double.PositiveInfinity,
            double minY = double.NegativeInfinity,
            double maxY = double.PositiveInfinity)
        {
            settings.axes.x.boundMin = minX;
            settings.axes.x.boundMax = maxX;
            settings.axes.y.boundMin = minY;
            settings.axes.y.boundMax = maxY;
        }

        public double[] AxisScale(double? unitsPerPixelX = null, double? unitsPerPixelY = null)
        {
            if (unitsPerPixelX != null)
            {
                double spanX = unitsPerPixelX.Value * settings.dataSize.Width;
                Axis(x1: settings.axes.x.center - spanX / 2, x2: settings.axes.x.center + spanX / 2);
            }

            if (unitsPerPixelY != null)
            {
                double spanY = unitsPerPixelY.Value * settings.dataSize.Height;
                Axis(y1: settings.axes.y.center - spanY / 2, y2: settings.axes.y.center + spanY / 2);
            }

            return settings.axes.limits;
        }

        public double[] AxisEqual(bool preserveY = true)
        {
            if (preserveY)
                AxisScale(unitsPerPixelX: settings.yAxisUnitsPerPixel);
            else
                AxisScale(unitsPerPixelY: settings.xAxisUnitsPerPixel);
            return settings.axes.limits;
        }

        public bool EqualAxis
        {
            get => settings.axes.equalAxes;
            set
            {
                settings.axes.equalAxes = value;
                if (value)
                    settings.AxisAuto();
            }
        }

        [Obsolete("use AxisAuto() instead")]
        public double[] AutoAxis()
        {
            return AxisAuto();
        }

        [Obsolete("use AxisAuto() instead")]
        public double[] AutoScale()
        {
            return AxisAuto();
        }

        public double[] AxisAuto(
            double horizontalMargin = .05,
            double verticalMargin = .1,
            bool xExpandOnly = false,
            bool yExpandOnly = false,
            bool tightenLayout = true
            )
        {
            settings.AxisAuto(horizontalMargin, verticalMargin, xExpandOnly, yExpandOnly);
            if (tightenLayout)
                TightenLayout();
            else
                settings.layout.tighteningOccurred = true;
            return settings.axes.limits;
        }

        public double[] AxisAutoX(
            double margin = .05,
            bool expandOnly = false
            )
        {
            if (settings.axes.hasBeenSet == false)
                AxisAuto();

            double[] originalLimits = Axis();
            double[] newLimits = AxisAuto(horizontalMargin: margin, xExpandOnly: expandOnly);
            return Axis(newLimits[0], newLimits[1], originalLimits[2], originalLimits[3]);
        }

        public double[] AxisAutoY(
            double margin = .1,
            bool expandOnly = false
            )
        {
            if (settings.axes.hasBeenSet == false)
                AxisAuto();

            double[] originalLimits = Axis();
            double[] newLimits = AxisAuto(verticalMargin: margin, yExpandOnly: expandOnly);
            return Axis(originalLimits[0], originalLimits[1], newLimits[2], newLimits[3]);
        }

        public double[] AxisZoom(
            double xFrac = 1,
            double yFrac = 1,
            double? zoomToX = null,
            double? zoomToY = null
            )
        {
            if (!settings.axes.hasBeenSet)
                settings.AxisAuto();

            if (zoomToX is null)
                zoomToX = settings.axes.x.center;

            if (zoomToY is null)
                zoomToY = settings.axes.y.center;

            settings.axes.Zoom(xFrac, yFrac, zoomToX, zoomToY);
            return settings.axes.limits;
        }

        public double[] AxisPan(double dx = 0, double dy = 0)
        {
            if (!settings.axes.hasBeenSet)
                settings.AxisAuto();
            settings.axes.x.Pan(dx);
            settings.axes.y.Pan(dy);
            return settings.axes.limits;
        }

        public double CoordinateFromPixelX(double pixelX)
        {
            return settings.GetLocationX(pixelX);
        }

        public double CoordinateFromPixelY(double pixelY)
        {
            return settings.GetLocationY(pixelY);
        }

        [Obsolete("use CoordinateFromPixelX and CoordinateFromPixelY for improved precision")]
        public PointF CoordinateFromPixel(int pixelX, int pixelY)
        {
            return settings.GetLocation(pixelX, pixelY);
        }

        [Obsolete("use CoordinateFromPixelX and CoordinateFromPixelY for improved precision")]
        public PointF CoordinateFromPixel(float pixelX, float pixelY)
        {
            return settings.GetLocation(pixelX, pixelY);
        }

        [Obsolete("use CoordinateFromPixelX and CoordinateFromPixelY for improved precision")]
        public PointF CoordinateFromPixel(double pixelX, double pixelY)
        {
            return settings.GetLocation(pixelX, pixelY);
        }

        [Obsolete("use CoordinateFromPixelX and CoordinateFromPixelY for improved precision")]
        public PointF CoordinateFromPixel(Point pixel)
        {
            return CoordinateFromPixel(pixel.X, pixel.Y);
        }

        [Obsolete("use CoordinateFromPixelX and CoordinateFromPixelY for improved precision")]
        public PointF CoordinateFromPixel(PointF pixel)
        {
            return CoordinateFromPixel(pixel.X, pixel.Y);
        }

        public PointF CoordinateToPixel(double locationX, double locationY)
        {
            PointF pixelLocation = settings.GetPixel(locationX, locationY);
            pixelLocation.X += settings.dataOrigin.X;
            pixelLocation.Y += settings.dataOrigin.Y;
            return pixelLocation;
        }

        public PointF CoordinateToPixel(PointF location)
        {
            return CoordinateToPixel(location.X, location.Y);
        }

        #endregion

        #region Labels

        public void Title(
            string title = null,
            bool? enable = null,
            string fontName = null,
            float? fontSize = null,
            Color? color = null,
            bool? bold = null
            )
        {

            settings.title.text = title ?? settings.title.text;
            settings.title.visible = enable ?? settings.title.visible;
            settings.title.fontName = fontName ?? settings.title.fontName;
            settings.title.fontSize = fontSize ?? settings.title.fontSize;
            settings.title.color = color ?? settings.title.color;
            settings.title.bold = bold ?? settings.title.bold;

            TightenLayout();
        }

        public void XLabel(
            string xLabel = null,
            Color? color = null,
            bool? enable = null,
            string fontName = null,
            float? fontSize = null,
            bool? bold = null
            )
        {
            settings.xLabel.text = xLabel ?? settings.xLabel.text;
            settings.xLabel.color = color ?? settings.xLabel.color;
            settings.xLabel.visible = enable ?? settings.xLabel.visible;
            settings.xLabel.fontName = fontName ?? settings.xLabel.fontName;
            settings.xLabel.fontSize = fontSize ?? settings.xLabel.fontSize;
            settings.xLabel.bold = bold ?? settings.xLabel.bold;

            TightenLayout();
        }

        public void YLabel(
            string yLabel = null,
            bool? enable = null,
            string fontName = null,
            float? fontSize = null,
            Color? color = null,
            bool? bold = null
            )
        {
            settings.yLabel.text = yLabel ?? settings.yLabel.text;
            settings.yLabel.color = color ?? settings.yLabel.color;
            settings.yLabel.visible = enable ?? settings.yLabel.visible;
            settings.yLabel.fontName = fontName ?? settings.yLabel.fontName;
            settings.yLabel.fontSize = fontSize ?? settings.yLabel.fontSize;
            settings.yLabel.bold = bold ?? settings.yLabel.bold;

            TightenLayout();
        }

        public void Legend(
            bool enableLegend = true,
            string fontName = null,
            float? fontSize = null,
            bool? bold = null,
            Color? fontColor = null,
            Color? backColor = null,
            Color? frameColor = null,
            legendLocation location = legendLocation.lowerRight,
            shadowDirection shadowDirection = shadowDirection.lowerRight,
            bool? fixedLineWidth = null,
            bool? reverseOrder = null
            )
        {
            settings.Legend.Visible = enableLegend;
            if (fontName != null)
                settings.Legend.FontName = fontName;
            if (fontSize != null)
                settings.Legend.FontSize = fontSize.Value;
            if (fontColor != null)
                settings.Legend.FontColor = fontColor.Value;
            if (backColor != null)
                settings.Legend.FillColor = backColor.Value;
            if (frameColor != null)
                settings.Legend.OutlineColor = frameColor.Value;
            if (reverseOrder != null)
                settings.Legend.ReverseOrder = reverseOrder.Value;
            if (bold != null)
                settings.Legend.FontBold = bold.Value;
            if (fixedLineWidth != null)
                settings.Legend.FixedLineWidth = fixedLineWidth.Value;

            // TODO: In ScottPlot 4.1 change these arguments
            if (location == legendLocation.upperLeft)
                settings.Legend.Location = Direction.NW;
            else if (location == legendLocation.upperCenter)
                settings.Legend.Location = Direction.N;
            else if (location == legendLocation.upperRight)
                settings.Legend.Location = Direction.NE;
            else if (location == legendLocation.middleRight)
                settings.Legend.Location = Direction.E;
            else if (location == legendLocation.lowerRight)
                settings.Legend.Location = Direction.SE;
            else if (location == legendLocation.lowerCenter)
                settings.Legend.Location = Direction.S;
            else if (location == legendLocation.lowerLeft)
                settings.Legend.Location = Direction.SW;
            else if (location == legendLocation.middleLeft)
                settings.Legend.Location = Direction.W;
        }

        public Bitmap GetLegendBitmap()
        {
            if (settings.bmpData is null)
                RenderBitmap();
            return settings.Legend.GetBitmap(settings);
        }

        #endregion

        #region Styling and Misc Graph Settings

        public void Ticks(
            bool? displayTicksX = null,
            bool? displayTicksY = null,
            bool? displayTicksXminor = null,
            bool? displayTicksYminor = null,
            bool? displayTickLabelsX = null,
            bool? displayTickLabelsY = null,
            Color? color = null,
            bool? useMultiplierNotation = null,
            bool? useOffsetNotation = null,
            bool? useExponentialNotation = null,
            bool? dateTimeX = null,
            bool? dateTimeY = null,
            bool? rulerModeX = null,
            bool? rulerModeY = null,
            bool? invertSignX = null,
            bool? invertSignY = null,
            string fontName = null,
            float? fontSize = null,
            double? xTickRotation = null,
            bool? logScaleX = null,
            bool? logScaleY = null,
            string numericFormatStringX = null,
            string numericFormatStringY = null,
            bool? snapToNearestPixel = null,
            int? baseX = null,
            int? baseY = null,
            string prefixX = null,
            string prefixY = null,
            string dateTimeFormatStringX = null,
            string dateTimeFormatStringY = null
            )
        {
            if (displayTicksX != null)
                settings.ticks.displayXmajor = (bool)displayTicksX;
            if (displayTicksY != null)
                settings.ticks.displayYmajor = (bool)displayTicksY;
            if (color != null)
                settings.ticks.color = (Color)color;
            if (useMultiplierNotation != null)
                settings.ticks.useMultiplierNotation = (bool)useMultiplierNotation;
            if (useOffsetNotation != null)
                settings.ticks.useOffsetNotation = (bool)useOffsetNotation;
            if (useExponentialNotation != null)
                settings.ticks.useExponentialNotation = (bool)useExponentialNotation;
            if (displayTicksXminor != null)
                settings.ticks.displayXminor = (bool)displayTicksXminor;
            if (displayTicksYminor != null)
                settings.ticks.displayYminor = (bool)displayTicksYminor;
            if (dateTimeX != null)
                settings.ticks.x.dateFormat = (bool)dateTimeX;
            if (dateTimeY != null)
                settings.ticks.y.dateFormat = (bool)dateTimeY;
            if (rulerModeX != null)
                settings.ticks.rulerModeX = (bool)rulerModeX;
            if (rulerModeY != null)
                settings.ticks.rulerModeY = (bool)rulerModeY;
            if (invertSignX != null)
                settings.ticks.x.invertSign = (bool)invertSignX;
            if (invertSignY != null)
                settings.ticks.y.invertSign = (bool)invertSignY;
            if (fontSize != null)
                settings.ticks.fontSize = (float)fontSize;
            if (fontName != null)
                settings.ticks.fontName = fontName;
            if (displayTickLabelsX != null)
                settings.ticks.displayXlabels = (bool)displayTickLabelsX;
            if (displayTickLabelsY != null)
                settings.ticks.displayYlabels = (bool)displayTickLabelsY;
            if (xTickRotation != null)
                settings.ticks.rotationX = xTickRotation.Value;
            if (logScaleX != null)
                settings.ticks.x.logScale = logScaleX.Value;
            if (logScaleY != null)
                settings.ticks.y.logScale = logScaleY.Value;
            if (numericFormatStringX != null)
                settings.ticks.x.numericFormatString = numericFormatStringX;
            if (numericFormatStringY != null)
                settings.ticks.y.numericFormatString = numericFormatStringY;
            if (snapToNearestPixel != null)
                settings.ticks.snapToNearestPixel = snapToNearestPixel.Value;
            if (dateTimeFormatStringX != null)
                settings.ticks.x.dateTimeFormatString = dateTimeFormatStringX;
            if (dateTimeFormatStringY != null)
                settings.ticks.y.dateTimeFormatString = dateTimeFormatStringY;

            if (baseX != null)
            {
                settings.ticks.x.radix = baseX.Value;
                settings.ticks.x.prefix = prefixX;
            }
            if (baseY != null)
            {
                settings.ticks.y.radix = baseY.Value;
                settings.ticks.y.prefix = prefixY;
            }

            // dont use offset notation if the sign is inverted
            if (settings.ticks.x.invertSign || settings.ticks.y.invertSign)
                settings.ticks.useOffsetNotation = false;

            if (dateTimeX != null || dateTimeY != null)
            {
                // why these in this order? voodoo magic
                TightenLayout();
                RenderBitmap();
            }

            TightenLayout();
        }

        public void XTicks(string[] labels)
        {
            if (labels is null)
                throw new ArgumentException("labels cannot be null");

            XTicks(DataGen.Consecutive(labels.Length), labels);
        }

        public void XTicks(double[] positions = null, string[] labels = null)
        {
            TightenLayout();
            settings.ticks.x.manualTickPositions = positions;
            settings.ticks.x.manualTickLabels = labels;
        }

        public void YTicks(string[] labels)
        {
            if (labels is null)
                throw new ArgumentException("labels cannot be null");

            YTicks(DataGen.Consecutive(labels.Length), labels);
        }

        public void YTicks(double[] positions = null, string[] labels = null)
        {
            TightenLayout();
            settings.ticks.y.manualTickPositions = positions;
            settings.ticks.y.manualTickLabels = labels;
        }

        public void Grid(
            bool? enable = null,
            Color? color = null,
            double? xSpacing = null,
            double? ySpacing = null,
            bool? enableHorizontal = null,
            bool? enableVertical = null,
            Config.DateTimeUnit? xSpacingDateTimeUnit = null,
            Config.DateTimeUnit? ySpacingDateTimeUnit = null,
            double? lineWidth = null,
            LineStyle? lineStyle = null,
            bool? snapToNearestPixel = null
            )
        {
            if (enable != null)
            {
                settings.HorizontalGridLines.Visible = enable.Value;
                settings.VerticalGridLines.Visible = enable.Value;
            }

            if (enableHorizontal != null)
                settings.HorizontalGridLines.Visible = enableHorizontal.Value;

            if (enableVertical != null)
                settings.VerticalGridLines.Visible = enableVertical.Value;

            if (color != null)
            {
                settings.HorizontalGridLines.Color = color.Value;
                settings.VerticalGridLines.Color = color.Value;
            }

            if (xSpacing != null)
                settings.ticks.manualSpacingX = xSpacing.Value;

            if (ySpacing != null)
                settings.ticks.manualSpacingY = ySpacing.Value;

            if (xSpacingDateTimeUnit != null)
                settings.ticks.manualDateTimeSpacingUnitX = xSpacingDateTimeUnit.Value;

            if (ySpacingDateTimeUnit != null)
                settings.ticks.manualDateTimeSpacingUnitY = ySpacingDateTimeUnit.Value;

            if (lineWidth != null)
            {
                settings.HorizontalGridLines.LineWidth = (float)lineWidth.Value;
                settings.VerticalGridLines.LineWidth = (float)lineWidth.Value;
            }

            if (lineStyle != null)
            {
                settings.HorizontalGridLines.LineStyle = lineStyle.Value;
                settings.VerticalGridLines.LineStyle = lineStyle.Value;
            }

            if (snapToNearestPixel != null)
            {
                settings.HorizontalGridLines.SnapToNearestPixel = snapToNearestPixel.Value;
                settings.VerticalGridLines.SnapToNearestPixel = snapToNearestPixel.Value;
            }
        }

        public void Frame(
            bool? drawFrame = true,
            Color? frameColor = null,
            bool? left = true,
            bool? right = true,
            bool? bottom = true,
            bool? top = true
            )
        {
            if (drawFrame != null)
                settings.layout.displayAxisFrames = (bool)drawFrame;
            if (frameColor != null)
                settings.ticks.color = (Color)frameColor;
            if (left != null)
                settings.layout.displayFrameByAxis[0] = (bool)left;
            if (right != null)
                settings.layout.displayFrameByAxis[1] = (bool)right;
            if (bottom != null)
                settings.layout.displayFrameByAxis[2] = (bool)bottom;
            if (top != null)
                settings.layout.displayFrameByAxis[3] = (bool)top;
            TightenLayout();
        }

        public void Benchmark(bool show = true, bool toggle = false)
        {
            if (toggle)
                settings.Benchmark.Visible = !settings.Benchmark.Visible;
            else
                settings.Benchmark.Visible = show;
        }

        public void AntiAlias(bool figure = true, bool data = false, bool legend = false)
        {
            settings.misc.antiAliasFigure = figure;
            settings.misc.antiAliasData = data;
            settings.Legend.AntiAlias = legend;
        }

        public void TightenLayout(int? padding = null, bool render = false)
        {
            if (settings.gfxData is null)
                return;

            if (render)
                GetBitmap();
            if (!settings.axes.hasBeenSet && settings.plottables.Count > 0)
                settings.AxisAuto();

            settings.ticks?.x?.Recalculate(settings); // this probably never happens
            settings.ticks?.y?.Recalculate(settings); // this probably never happens

            int pad = (padding is null) ? 15 : (int)padding;
            settings.TightenLayout(pad, pad, pad, pad);

            Resize();
        }

        public void Layout(
                double? yLabelWidth = null,
                double? yScaleWidth = null,
                double? y2LabelWidth = null,
                double? y2ScaleWidth = null,
                double? titleHeight = null,
                double? xLabelHeight = null,
                double? xScaleHeight = null
            )
        {
            TightenLayout(render: true);

            if (yLabelWidth != null) settings.layout.yLabelWidth = (int)yLabelWidth;
            if (yScaleWidth != null) settings.layout.yScaleWidth = (int)yScaleWidth;
            if (y2LabelWidth != null) settings.layout.y2LabelWidth = (int)y2LabelWidth;
            if (y2ScaleWidth != null) settings.layout.y2ScaleWidth = (int)y2ScaleWidth;
            if (titleHeight != null) settings.layout.titleHeight = (int)titleHeight;
            if (xLabelHeight != null) settings.layout.xLabelHeight = (int)xLabelHeight;
            if (xScaleHeight != null) settings.layout.xScaleHeight = (int)xScaleHeight;

            Resize();
        }

        public void MatchLayout(Plot sourcePlot, bool horizontal = true, bool vertical = true)
        {
            if (!sourcePlot.GetSettings(showWarning: false).axes.hasBeenSet)
                sourcePlot.AxisAuto();

            if (!settings.axes.hasBeenSet)
                AxisAuto();

            Resize();

            var sourceLayout = sourcePlot.GetSettings(false).layout;

            if (horizontal)
            {
                settings.layout.yLabelWidth = sourceLayout.yLabelWidth;
                settings.layout.y2LabelWidth = sourceLayout.y2LabelWidth;
                settings.layout.yScaleWidth = sourceLayout.yScaleWidth;
                settings.layout.y2ScaleWidth = sourceLayout.y2ScaleWidth;
            }

            if (vertical)
            {
                settings.layout.titleHeight = sourceLayout.titleHeight;
                settings.layout.xLabelHeight = sourceLayout.xLabelHeight;
                settings.layout.xScaleHeight = sourceLayout.xScaleHeight;
            }
        }

        public void MatchAxis(Plot sourcePlot, bool horizontal = true, bool vertical = true)
        {
            if (!sourcePlot.GetSettings(showWarning: false).axes.hasBeenSet)
                sourcePlot.AxisAuto();

            if (!settings.axes.hasBeenSet)
                AxisAuto();

            if (horizontal)
            {
                settings.axes.x.min = sourcePlot.settings.axes.x.min;
                settings.axes.x.max = sourcePlot.settings.axes.x.max;
            }
            if (vertical)
            {
                settings.axes.y.min = sourcePlot.settings.axes.y.min;
                settings.axes.y.max = sourcePlot.settings.axes.y.max;
            }
            TightenLayout();
        }

        // TODO: create a new Style()
        /*
        It should only have arguments to configure styles of things which
        can't be set any other way. Be mindful not to duplicate arguments from
        Ticks(), Grid(), Title, XLabel(), YLabel(), Frame(), etc.

        Probably the only two arguments should be:
            figureBackground
            dataBackground
        */

        public void Style(
            Color? figBg = null,
            Color? dataBg = null,
            Color? grid = null,
            Color? tick = null,
            Color? label = null,
            Color? title = null
            )
        {
            if (figBg != null)
                settings.FigureBackground.Color = figBg.Value;
            if (dataBg != null)
                settings.DataBackground.Color = dataBg.Value;
            if (grid != null)
            {
                settings.HorizontalGridLines.Color = grid.Value;
                settings.VerticalGridLines.Color = grid.Value;
            }
            if (tick != null)
                settings.ticks.color = (Color)tick;
            if (label != null)
                settings.xLabel.color = (Color)label;
            if (label != null)
                settings.yLabel.color = (Color)label;
            if (title != null)
                settings.title.color = (Color)title;
            if (dataBg != null)
                settings.Legend.FillColor = (Color)dataBg;
            if (tick != null)
                settings.Legend.OutlineColor = (Color)tick;
            if (label != null)
                settings.Legend.FontColor = (Color)label;
        }

        public void Style(Style style)
        {
            StyleTools.SetStyle(this, style);
        }

        public void SetCulture(System.Globalization.CultureInfo culture)
        {
            settings.culture = culture;
        }

        /// <summary>
        /// Updates the used culture to match your requirements.
        /// </summary>
        /// <param name="shortDatePattern">
        /// https://docs.microsoft.com/dotnet/standard/base-types/custom-date-and-time-format-strings
        /// </param>
        /// <param name="decimalSeparator">
        /// Separates the decimal digits.
        /// </param>
        /// <param name="numberGroupSeparator">
        /// Separates large numbers ito groups of digits for readability.
        /// </param>
        /// <param name="decimalDigits">
        /// Number of digits after the numberDecimalSeparator.
        /// </param>
        /// <param name="numberNegativePattern">
        /// https://docs.microsoft.com/dotnet/api/system.globalization.numberformatinfo.numbernegativepattern
        /// </param>
        /// <param name="numberGroupSizes">
        /// Sizes of decimal groups which are separated by the numberGroupSeparator.
        /// https://docs.microsoft.com/dotnet/api/system.globalization.numberformatinfo.numbergroupsizes
        /// </param>
        public void SetCulture(
            string shortDatePattern = null,
            string decimalSeparator = null,
            string numberGroupSeparator = null,
            int? decimalDigits = null,
            int? numberNegativePattern = null,
            int[] numberGroupSizes = null)
        {

            // settings.culture may be null if the thread culture is the same is the system culture.
            // If it is null, assigning it to a clone of the current culture solves this and also makes it mutable.
            if (settings.culture is null)
                settings.culture = (System.Globalization.CultureInfo)System.Globalization.CultureInfo.CurrentCulture.Clone();

            if (shortDatePattern != null)
                settings.culture.DateTimeFormat.ShortDatePattern = shortDatePattern;

            if (decimalDigits != null)
                settings.culture.NumberFormat.NumberDecimalDigits = decimalDigits.Value;

            if (decimalSeparator != null)
                settings.culture.NumberFormat.NumberDecimalSeparator = decimalSeparator;

            if (numberGroupSeparator != null)
                settings.culture.NumberFormat.NumberGroupSeparator = numberGroupSeparator;

            if (numberGroupSizes != null)
                settings.culture.NumberFormat.NumberGroupSizes = numberGroupSizes;

            if (numberNegativePattern != null)
                settings.culture.NumberFormat.NumberNegativePattern = numberNegativePattern.Value;
        }

        #endregion

    }
}
