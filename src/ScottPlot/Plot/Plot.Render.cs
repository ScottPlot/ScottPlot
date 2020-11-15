using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;

namespace ScottPlot
{
    partial class Plot
    {
        public Bitmap Render(Bitmap renderOnThis, bool lowQuality = false) =>
            RenderBitmap(renderOnThis, lowQuality);

        public Bitmap RenderBitmap(bool lowQuality = false) =>
             RenderBitmap(settings.Width, settings.Height, lowQuality);

        public Bitmap RenderBitmap(int width, int height, bool lowQuality = false) =>
            RenderBitmap(new Bitmap(width, height, PixelFormat.Format32bppPArgb), lowQuality);

        private Bitmap RenderBitmap(Bitmap bmp, bool lowQuality = false)
        {
            settings.BenchmarkMessage.Restart();
            settings.CopyPrimaryLayoutToAllAxes();
            settings.AxisAutoUnsetAxes();
            settings.LayoutAuto();

            RenderBeforePlottables(bmp, lowQuality);
            RenderPlottables(bmp, lowQuality);
            RenderAfterPlottables(bmp, lowQuality);

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

                PlotDimensions dims = (plottable is Plottable.IUsesAxes p) ?
                    settings.GetPlotDimensions(p.HorizontalAxisIndex, p.VerticalAxisIndex) :
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
            settings.CornerLegend.UpdateLegendItems(Plottables);
            settings.CornerLegend.Render(dims, bmp, lowQuality);

            settings.BenchmarkMessage.Stop();

            settings.ZoomRectangle.Render(dims, bmp, lowQuality);
            settings.BenchmarkMessage.Render(dims, bmp, lowQuality);
            settings.ErrorMessage.Render(dims, bmp, lowQuality);
        }

        public Bitmap GetLegendBitmap(bool lowQuality = false)
        {
            RenderBitmap(lowQuality);
            return settings.CornerLegend.GetBitmap();
        }

        public void SaveFig(string filePath, bool renderFirst = true)
        {
            Bitmap bmp = RenderBitmap(lowQuality: false);

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
        }
    }
}
