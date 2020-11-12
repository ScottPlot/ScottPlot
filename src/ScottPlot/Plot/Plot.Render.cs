using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;

namespace ScottPlot
{
    partial class Plot
    {

        public Bitmap Render(Bitmap renderOnThis, bool lowQuality)
        {
            RenderBitmap(renderOnThis, lowQuality);
            return renderOnThis;
        }

        private Bitmap RenderBitmap(bool lowQuality) =>
             RenderBitmap(settings.Width, settings.Height, lowQuality);

        private Bitmap RenderBitmap(int width, int height, bool lowQuality) =>
            RenderBitmap(new Bitmap(width, height, PixelFormat.Format32bppPArgb), lowQuality);

        public int RenderCount { get; private set; } = 0;
        private Bitmap RenderBitmap(Bitmap bmp, bool lowQuality)
        {
            settings.BenchmarkMessage.Restart();

            // pre-render axis adjustments
            if (!settings.AxesHaveBeenSet)
                settings.AxisAuto();

            // auto-layout before every single frame
            LayoutAuto();

            PlotDimensions2D dims = settings.GetPlotDimensions();
            RenderBeforePlottables(dims, bmp, lowQuality);
            RenderPlottables(dims, bmp, lowQuality);
            RenderAfterPlottables(dims, bmp, lowQuality);

            RenderCount += 1;
            return bmp;
        }

        private void RenderBeforePlottables(PlotDimensions2D dims, Bitmap bmp, bool lowQuality)
        {
            settings.FigureBackground.Render(dims, bmp, lowQuality);
            settings.DataBackground.Render(dims, bmp, lowQuality);

            try
            {
                settings.XAxis.Render(dims, bmp, lowQuality);
                settings.YAxis.Render(dims, bmp, lowQuality);
                settings.XAxis2.Render(dims, bmp, lowQuality);
                settings.YAxis2.Render(dims, bmp, lowQuality);
            }
            catch (OverflowException)
            {
                throw new InvalidOperationException("data cannot contain Infinity");
            }
        }

        private void RenderPlottables(PlotDimensions2D dims, Bitmap bmp, bool lowQuality)
        {
            var plottablesToRender = settings.Plottables.Where(x => x.IsVisible);
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

        private void RenderAfterPlottables(PlotDimensions2D dims, Bitmap bmp, bool lowQuality)
        {
            settings.CornerLegend.UpdateLegendItems(Plottables);
            settings.CornerLegend.Render(dims, bmp, lowQuality);

            settings.BenchmarkMessage.Stop();
            // TODO: set up validation check reporting
            //ErrorMessage.Text = "Error Message";

            settings.ZoomRectangle.Render(dims, bmp, lowQuality);
            settings.BenchmarkMessage.Render(dims, bmp, lowQuality);
            settings.ErrorMessage.Render(dims, bmp, lowQuality);
        }

        public Bitmap GetBitmap(bool renderFirst = true, bool lowQuality = false)
        {
            if (renderFirst)
                RenderBitmap(lowQuality: false);
            return RenderBitmap(lowQuality);
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
