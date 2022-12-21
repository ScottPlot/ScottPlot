using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ScottPlot
{
    internal class SKBitmapHelpers
    {
        public static SKBitmap BitmapFromArgbs(uint[] argbs, int width, int height)
        {
            GCHandle handle = GCHandle.Alloc(argbs, GCHandleType.Pinned);

            var imageInfo = new SKImageInfo(width, height);
            var bmp = new SKBitmap(imageInfo);
            bmp.InstallPixels(
                info: imageInfo,
                pixels: handle.AddrOfPinnedObject(),
                rowBytes: imageInfo.RowBytes,
                releaseProc: (IntPtr _, object _) => handle.Free());

            return bmp;
        }
    }
}
