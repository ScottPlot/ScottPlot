using System.Drawing;

namespace Sandbox.Blazor.WebAssembly.Services
{
    public class ResizeService : IResizeService
    {
        private System.Timers.Timer? _refresgDelay = null;
        public event Action<SizeF>? Resize = null;

        public float PixelWidth { get; private set; } = 0;
        public float PixelHeight { get; private set; } = 0;
        public void SetSize(float width, float height)
        {
            bool hasChanged = false;
            if (PixelHeight != height)
            {
                PixelHeight = height;
                hasChanged = true;
            }
            if (PixelWidth != width)
            {
                PixelWidth = width;
                hasChanged = true;
            }
            if (hasChanged)
            {
                _refresgDelay?.Stop();
                _refresgDelay?.Start();
            }
        }

        public ResizeService()
        {
            _refresgDelay = new(TimeSpan.FromMilliseconds(50));
            _refresgDelay.Elapsed += (o, e) =>
            {
                _refresgDelay.Enabled = false;
                Resize?.Invoke(new SizeF(PixelWidth, PixelHeight));
            };
        }
    }
}
