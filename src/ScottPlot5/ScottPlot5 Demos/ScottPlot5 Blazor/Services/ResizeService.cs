using ScottPlot;
using System.Drawing;

namespace Sandbox.Blazor.WebAssembly.Services
{
    public class ResizeService : IResizeService
    {
        public bool Enable { get; set; } = true;
        private readonly System.Timers.Timer SizeChangedTimer = new(TimeSpan.FromMilliseconds(50));

        public event Action<SizeF>? ResizeAction = null;

        public float PixelWidth { get; private set; } = 0;
        public float PixelHeight { get; private set; } = 0;

        public bool IsLandscape => PixelWidth > PixelHeight;
        public bool IsPortrait => PixelHeight > PixelWidth;
        public bool IsLargeScreen => PixelWidth >= 641;

        public bool IsSquare => PixelWidth > 0 && PixelHeight > 0
            ? ((IsLandscape ? PixelHeight / PixelWidth
            : PixelWidth / PixelHeight) > .97) : false;

        public void SetSize(float width, float height)
        {
            PixelSize sizeBefore = new(PixelHeight, PixelWidth);
            PixelHeight = height;
            PixelWidth = width;
            PixelSize sizeAfter = new(PixelHeight, PixelWidth);
            if (sizeBefore != sizeAfter)
            {
                SizeChangedTimer.Start();
            }
        }

        public ResizeService()
        {
            SizeChangedTimer.Elapsed += (o, e) =>
            {
                SizeChangedTimer.Enabled = false;
                SizeChangedTimer.Stop();
                if (Enable)
                    ResizeAction?.Invoke(new SizeF(PixelWidth, PixelHeight));
            };
        }
    }
}
