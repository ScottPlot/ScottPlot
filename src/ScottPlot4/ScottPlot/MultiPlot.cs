using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;

namespace ScottPlot
{
    [Obsolete("This class will be deleted in a future version. See ScottPlot FAQ for details.")]
    public class MultiPlot
    {
        public readonly Plot[] subplots;
        public readonly int rows, cols;
        public readonly int width, height;

        private readonly Bitmap bmp;
        private readonly Graphics gfx;

        public int subplotCount { get { return rows * cols; } }
        public int subplotWidth { get { return width / cols; } }
        public int subplotHeight { get { return height / rows; } }

        public MultiPlot(int width = 800, int height = 600, int rows = 1, int cols = 1)
        {
            if (rows < 1 || cols < 1)
                throw new ArgumentException("must have at least 1 row and column");

            this.width = width;
            this.height = height;
            this.rows = rows;
            this.cols = cols;

            bmp = new Bitmap(width, height);
            gfx = Graphics.FromImage(bmp);

            subplots = new Plot[subplotCount];
            for (int i = 0; i < subplotCount; i++)
                subplots[i] = new Plot(subplotWidth, subplotHeight);
        }

        private void Render()
        {
            gfx.Clear(Color.White);
            int subplotIndex = 0;
            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < cols; col++)
                {
                    Bitmap subplotBmp = subplots[subplotIndex++].Render(subplotWidth, subplotHeight, false);
                    Point pt = new Point(col * subplotWidth, row * subplotHeight);
                    gfx.DrawImage(subplotBmp, pt);
                }
            }
        }

        public Bitmap GetBitmap()
        {
            Render();
            return bmp;
        }

        public void SaveFig(string filePath)
        {
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

            Render();
            bmp.Save(filePath, imageFormat);
        }

        public Plot GetSubplot(int rowIndex, int columnIndex)
        {
            if (rowIndex < 0 || rowIndex >= rows)
                throw new ArgumentException("invalid row index");

            if (columnIndex < 0 || columnIndex >= cols)
                throw new ArgumentException("invalid column index");

            int subplotIndex = rowIndex * cols + columnIndex;
            return subplots[subplotIndex];
        }
    }
}
