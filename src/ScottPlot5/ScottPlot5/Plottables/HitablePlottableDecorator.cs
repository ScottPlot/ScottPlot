using System.Threading.Tasks;

namespace ScottPlot.Plottables
{
    public class HitablePlottableDecorator : IPlottable, IDisposable
    {
        private object _lock = new object();
        private SKBitmap? _bitmap;
        public IPlottable Source { get; }
        public bool IsVisible { get => Source.IsVisible; set => Source.IsVisible = value; }
        public IAxes Axes { get => Source.Axes; set => Source.Axes = value; }

        public IEnumerable<LegendItem> LegendItems => Source.LegendItems;

        public AxisLimits GetAxisLimits() => Source.GetAxisLimits();

        public HitablePlottableDecorator(IPlottable plottable)
        {
            Source = plottable;
        }

        public bool IsHit(float mouseX, float mouseY, float span)
        {
            if (_bitmap == null)
                return false;

            lock (_lock)
            {
                int fromX = Math.Max(0, (int)(mouseX - span / 2));
                int toX = Math.Min(_bitmap.Width, (int)(mouseX + span / 2));
                int fromY = Math.Max(0, (int)(mouseY - span / 2));
                int toY = Math.Min(_bitmap.Height, (int)(mouseY + span / 2));
                for (int i = fromX; i < toX; i++)
                {
                    for (int j = fromY; j < toY; j++)
                    {
                        if (_bitmap.GetPixel(i, j).Alpha != 0)
                            return true;
                    }
                }
                return false;
            }
        }

        public virtual void Render(RenderPack rp)
        {
            Source.Render(rp);
            Task.Run(() =>
            {
                SKBitmap bitmapBuf = new SKBitmap((int)rp.FigureRect.Width, (int)rp.FigureRect.Height, SKColorType.Rgba8888, SKAlphaType.Premul);
                using (var hitCanvas = new SKCanvas(bitmapBuf))
                {
                    hitCanvas.Clear(new SKColor(255, 255, 255, 0));
                    using (RenderPack rpHitable = new RenderPack(rp.Plot, rp.FigureRect, hitCanvas))
                    {
                        rpHitable.CalculateLayout();
                        rpHitable.CanvasState.Clip(rpHitable.DataRect);
                        Source.Render(rpHitable);
                        rpHitable.CanvasState.DisableClipping();
                    }
                }
                lock (_lock)
                {
                    _bitmap?.Dispose();
                    _bitmap = bitmapBuf;
                }
            });
        }

        public void Dispose()
        {
            _bitmap?.Dispose();
            _bitmap = null;
        }
    }
}
