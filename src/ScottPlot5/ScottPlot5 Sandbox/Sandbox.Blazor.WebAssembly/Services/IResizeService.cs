using System.Drawing;

namespace Sandbox.Blazor.WebAssembly.Services
{
    public interface IResizeService
    {
        public event Action<SizeF>? Resize;

        public float PixelWidth { get; }
        public float PixelHeight { get; }
        public void SetSize(float width, float height);
    }
}
