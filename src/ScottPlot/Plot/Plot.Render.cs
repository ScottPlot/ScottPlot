using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;

namespace ScottPlot
{
    partial class Plot
    {
        #region primary render system

        /// <summary>
        /// Render the plot onto an existing bitmap
        /// </summary>
        public Bitmap Render(Bitmap bmp, bool lowQuality = false)
        {
            while (IsRenderLocked) { }
            IsRendering = true;

            settings.BenchmarkMessage.Restart();
            settings.Resize(bmp.Width, bmp.Height);
            settings.CopyPrimaryLayoutToAllAxes();
            settings.AxisAutoUnsetAxes();
            settings.EnforceEqualAxisScales();
            settings.LayoutAuto();
            settings.EnforceEqualAxisScales();

            RenderBeforePlottables(bmp, lowQuality);
            RenderPlottables(bmp, lowQuality);
            RenderAfterPlottables(bmp, lowQuality);

            IsRendering = false;
            return bmp;
        }


        private void RenderBeforePlottables(Bitmap bmp, bool lowQuality)
        {
            PlotDimensions dims = settings.GetPlotDimensions(0, 0);
            settings.FigureBackground.Render(dims, bmp, lowQuality);
            settings.DataBackground.Render(dims, bmp, lowQuality);

            foreach (var axis in settings.Axes)
            {
                PlotDimensions dims2 = axis.IsHorizontal ?
                    settings.GetPlotDimensions(axis.AxisIndex, 0) :
                    settings.GetPlotDimensions(0, axis.AxisIndex);

                try
                {
                    axis.Render(dims2, bmp, lowQuality);
                }
                catch (OverflowException)
                {
                    throw new InvalidOperationException("data cannot contain Infinity");
                }
            }
        }

        private void RenderPlottables(Bitmap bmp, bool lowQuality)
        {
            foreach (var plottable in settings.Plottables)
            {
                if (plottable.IsVisible == false)
                    continue;

                plottable.ValidateData(deep: false);

                PlotDimensions dims = (plottable is Plottable.IPlottable p) ?
                    settings.GetPlotDimensions(p.XAxisIndex, p.YAxisIndex) :
                    settings.GetPlotDimensions(0, 0);

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

        private void RenderAfterPlottables(Bitmap bmp, bool lowQuality)
        {
            PlotDimensions dims = settings.GetPlotDimensions(0, 0);
            settings.CornerLegend.UpdateLegendItems(GetPlottables());
            settings.CornerLegend.Render(dims, bmp, lowQuality);

            settings.BenchmarkMessage.Stop();

            settings.ZoomRectangle.Render(dims, bmp, lowQuality);
            settings.BenchmarkMessage.Render(dims, bmp, lowQuality);
            settings.ErrorMessage.Render(dims, bmp, lowQuality);
        }

        #endregion

        #region render lock

        private bool IsRendering = false; // Becomes true only while the render loop is running and not locked
        private bool IsRenderLocked = false; // RenderBitmap() will hang infinitely while this is true

        /// <summary>
        /// Wait for the current render to finish, then prevent future renders until RenderUnlock() is called.
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
        /// Render the plot onto a new Bitmap (using size defined by Plot.Width and Plot.Height)
        /// </summary>
        public Bitmap Render(bool lowQuality = false) =>
             Render(settings.Width, settings.Height, lowQuality);

        /// <summary>
        /// Render the plot onto a new Bitmap of the given dimensions
        /// </summary>
        public Bitmap Render(int width, int height, bool lowQuality = false) =>
            Render(new Bitmap(Math.Max(1, width), Math.Max(1, height), PixelFormat.Format32bppPArgb), lowQuality);

        [Obsolete("Call the Render() method", true)]
        public Bitmap GetBitmap() => null; // bitmaps are no longer pre-rendered and stored

        /// <summary>
        /// Return a new Bitmap containing only the legend
        /// </summary>
        /// <returns></returns>
        public Bitmap GetLegendBitmap()
        {
            Render();
            return settings.CornerLegend.GetBitmap();
        }

        /// <summary>
        /// Save the plot as an image file and return the full path of the new file
        /// </summary>
        public string SaveFig(string filePath)
        {
            Bitmap bmp = Render();

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
