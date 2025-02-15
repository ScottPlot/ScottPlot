using System.Drawing;

namespace Sandbox.Blazor.WebAssembly.Services
{
    public interface IResizeService
    {
        public event Action<SizeF>? ResizeAction;
        public float PixelWidth { get; }
        public float PixelHeight { get; }
        public bool IsLandscape { get; }
        public bool IsPortrait { get; }
        public bool IsLargeScreen { get; }
        public bool IsSquare { get; }
        public void SetSize(float width, float height);
        public bool Enable { get; set; }
    }
}
