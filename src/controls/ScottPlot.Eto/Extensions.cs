using Eto.Drawing;

namespace ScottPlot.Eto
{
    public static class Extensions
    {
        public static Bitmap ToEto(this System.Drawing.Bitmap bmp)
        {
            // using System.Buffer.MemoryCopy does not make it quicker since both bitmaps needs expensive lock() to access buffers

            using var memory = new System.IO.MemoryStream();
            bmp.Save(memory, System.Drawing.Imaging.ImageFormat.Png);
            memory.Position = 0;

            return new Bitmap(memory);
        }
    }
}
