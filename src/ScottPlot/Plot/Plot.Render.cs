using ScottPlot.Drawing;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;

namespace ScottPlot
{
    partial class Plot
    {
        #region primary render system

        /// <summary>
        /// Render the plot onto an existing bitmap
        /// </summary>
        /// <param name="bmp">an existing bitmap to render onto</param>
        /// <param name="lowQuality"></param>
        /// <param name="scale">scale the size of the output image by this fraction (without resizing the plot)</param>
        /// <returns>the same bitmap that was passed in (but was rendered onto)</returns>
        public Bitmap Render(Bitmap bmp, bool lowQuality = false, double scale = 1.0)
        {
            while (IsRenderLocked) { }
            IsRendering = true;

            settings.BenchmarkMessage.Restart();
            settings.Resize((int)(bmp.Width / scale), (int)(bmp.Height / scale));
            settings.CopyPrimaryLayoutToAllAxes();
            settings.AxisAutoUnsetAxes();
            settings.EnforceEqualAxisScales();
            settings.LayoutAuto();
            if (settings.EqualScaleMode != EqualScaleMode.Disabled)
            {
                settings.EnforceEqualAxisScales();
                settings.LayoutAuto();
            }

            PlotDimensions primaryDims = settings.GetPlotDimensions(0, 0, scale);

            using var gfx = GDI.Graphics(bmp, primaryDims, lowQuality);

            RenderClear(gfx, primaryDims);
            if (primaryDims.DataWidth > 0 && primaryDims.DataHeight > 0)
            {
                RenderBeforePlottables(gfx, primaryDims);
                RenderPlottables(gfx, scale);
                RenderAfterPlottables(gfx, primaryDims);
            }

            IsRendering = false;
            return bmp;
        }

        /// <summary>
        /// Render the plot onto an existing Graphics element
        /// </summary>
        /// <param name="gfx">an existing Graphics element to render onto</param>
        /// <param name="rectPrintArea">Area where to render the plot</param>
        public void Render(Graphics gfx, RectangleF rectPrintArea)
        {
            settings.BenchmarkMessage.Restart();
            settings.Resize((int)(rectPrintArea.Width), (int)(rectPrintArea.Height), (int)(rectPrintArea.X), (int)(rectPrintArea.Y));
            settings.CopyPrimaryLayoutToAllAxes();
            settings.AxisAutoUnsetAxes();
            settings.EnforceEqualAxisScales();
            settings.LayoutAuto();
            if (settings.EqualScaleMode != EqualScaleMode.Disabled)
            {
                settings.EnforceEqualAxisScales();
                settings.LayoutAuto();
            }

            PlotDimensions primaryDims = settings.GetPlotDimensions(0, 0, 1);

            RenderClear(gfx, primaryDims);
            if (primaryDims.DataWidth > 0 && primaryDims.DataHeight > 0)
            {
                RenderBeforePlottables(gfx, primaryDims);
                RenderPlottables(gfx, 1);
                RenderAfterPlottables(gfx, primaryDims);
            }
        }

        private void RenderClear(Graphics gfx, PlotDimensions primaryDims)
        {
            settings.FigureBackground.Render(primaryDims, gfx);
        }

        private void RenderBeforePlottables(Graphics gfx, PlotDimensions dims)
        {
            settings.DataBackground.Render(dims, gfx);

            foreach (var axis in settings.Axes)
            {
                PlotDimensions dims2 = axis.IsHorizontal ?
                    settings.GetPlotDimensions(axis.AxisIndex, 0, dims.ScaleFactor) :
                    settings.GetPlotDimensions(0, axis.AxisIndex, dims.ScaleFactor);

                try
                {
                    axis.Render(dims2, gfx);
                }
                catch (OverflowException)
                {
                    throw new InvalidOperationException("data cannot contain Infinity");
                }
            }
        }

        private void RenderPlottables(Graphics gfx, double scaleFactor)
        {
            foreach (var plottable in settings.Plottables)
            {
                if (plottable.IsVisible == false)
                    continue;

                plottable.ValidateData(deep: false);

                PlotDimensions dims = (plottable is Plottable.IPlottable p) ?
                    settings.GetPlotDimensions(p.XAxisIndex, p.YAxisIndex, scaleFactor) :
                    settings.GetPlotDimensions(0, 0, scaleFactor);

                try
                {
                    plottable.Render(dims, gfx);
                }
                catch (OverflowException)
                {
                    Debug.WriteLine($"OverflowException plotting: {plottable}");
                }
            }
        }

        private void RenderAfterPlottables(Graphics gfx, PlotDimensions dims)
        {
            settings.CornerLegend.UpdateLegendItems(GetPlottables());
            settings.CornerLegend.Render(dims, gfx);

            settings.BenchmarkMessage.Stop();

            settings.ZoomRectangle.Render(dims, gfx);
            settings.BenchmarkMessage.Render(dims, gfx);
            settings.ErrorMessage.Render(dims, gfx);
        }

        #endregion

        #region render lock

        private bool IsRendering = false; // Becomes true only while the render loop is running and not locked
        private bool IsRenderLocked = false; // RenderBitmap() will hang infinitely while this is true

        /// <summary>
        /// Wait for the current render to finish, then prevent future renders until RenderUnlock() is called.
        /// Locking rendering is required if you intend to modify plottables while rendering is occurring in another thread.
        /// </summary>
        public void RenderLock()
        {
            IsRenderLocked = true; // prevent new renders from starting
            while (IsRendering) { } // wait for the current render to finish
        }

        /// <summary>
        /// Release the render lock, allowing renders to proceed.
        /// </summary>
        public void RenderUnlock()
        {
            IsRenderLocked = false; // allow new renders to occur
        }

        #endregion

        #region render helper methods

        /// <summary>
        /// Render the plot onto a new Bitmap (using the size given when the plot was created or resized)
        /// </summary>
        /// <param name="lowQuality">if true, anti-aliasing will be disabled for this render</param>
        /// <returns>the Bitmap that was created</returns>
        public Bitmap Render(bool lowQuality = false) =>
             Render(settings.Width, settings.Height, lowQuality);

        /// <summary>
        /// Render the plot onto a new Bitmap of the given dimensions
        /// </summary>
        /// <param name="width">resize the plot to this width (pixels) before rendering</param>
        /// <param name="height">resize the plot to this height (pixels) before rendering</param>
        /// <param name="lowQuality">if true, anti-aliasing will be disabled for this render</param>
        /// <param name="scale">scale the size of the output image by this fraction (without resizing the plot)</param>
        /// <returns>the Bitmap that was created</returns>
        public Bitmap Render(int width, int height, bool lowQuality = false, double scale = 1.0)
        {
            // allow a bitmap to always be created even if invalid dimensions are provided
            width = Math.Max(1, (int)(width * scale));
            height = Math.Max(1, (int)(height * scale));

            Bitmap bmp = new(width, height, PixelFormat.Format32bppPArgb);
            Render(bmp, lowQuality, scale);
            return bmp;
        }

        /// <summary>
        /// Create a new Bitmap, render the plot onto it, and return it
        /// </summary>
        public Bitmap GetBitmap(bool lowQuality = false, double scale = 1.0) => Render(settings.Width, settings.Height, lowQuality, scale);

        /// <summary>
        /// Render the plot and return the bytes for a PNG file.
        /// This method is useful for rendering in stateless cloud environments that do not use a traditional filesystem.
        /// </summary>
        /// <returns></returns>
        public byte[] GetImageBytes(bool lowQuality = false, double scale = 1.0)
        {
            using MemoryStream stream = new();
            Bitmap bmp = GetBitmap(lowQuality, scale);
            bmp.Save(stream, ImageFormat.Png);
            byte[] imageBytes = stream.ToArray();
            return imageBytes;
        }

        /// <summary>
        /// Return a new Bitmap containing only the legend
        /// </summary>
        /// <returns>new bitmap containing the legend</returns>
        public Bitmap RenderLegend(bool lowQuality = false, double scale = 1.0)
        {
            Render(lowQuality);
            var originalEdgeColor = settings.CornerLegend.OutlineColor;
            settings.CornerLegend.OutlineColor = Color.Transparent;
            var bmp = settings.CornerLegend.GetBitmap(lowQuality, scale);
            settings.CornerLegend.OutlineColor = originalEdgeColor;
            return bmp;
        }

        /// <summary>
        /// Save the plot as an image
        /// </summary>
        /// <param name="filePath">file path for the images (existing files will be overwritten)</param>
        /// <param name="width">resize the plot to this width (pixels) before rendering</param>
        /// <param name="height">resize the plot to this height (pixels) before rendering</param>
        /// <param name="lowQuality">if true, anti-aliasing will be disabled for this render. Default false</param>
        /// <param name="scale">scale the size of the output image by this fraction (without resizing the plot)</param>
        /// <returns>Full path for the image that was saved</returns>
        public string SaveFig(string filePath, int? width = null, int? height = null, bool lowQuality = false, double scale = 1.0)
        {
            Bitmap bmp = Render(width: width ?? settings.Width, height: height ?? settings.Height, lowQuality, scale);

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

            bmp.Save(filePath, imageFormat);
            return filePath;
        }

        #endregion
    }
}
