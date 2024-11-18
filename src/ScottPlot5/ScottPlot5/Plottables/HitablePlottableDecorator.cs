namespace ScottPlot.Plottables
{
    public class HitablePlottableDecorator : IPlottable, IDisposable
    {
        private SKBitmap? _bitmap;
        public IPlottable Source { get; }
        public bool IsVisible { get => Source.IsVisible; set => Source.IsVisible = value; }
        public IAxes Axes { get => Source.Axes; set => Source.Axes = value; }

        public IEnumerable<LegendItem> LegendItems => Source.LegendItems;

        public AxisLimits GetAxisLimits()
        {
            return Source.GetAxisLimits();
        }

        public HitablePlottableDecorator(IPlottable plottable)
        {
            Source = plottable;
        }

        public bool IsHit(float mouseX, float mouseY, float span)
        {
            if (_bitmap == null)
                return false;

            for (int i = (int)(mouseX - span / 2); i < mouseX + span / 2; i++)
            {
                for (int j = (int)(mouseY - span / 2); j < mouseY + span / 2; j++)
                {
                    if (i >= 0 && i < _bitmap.Width && j >= 0 && j < _bitmap.Height)
                    {
                        if (_bitmap.GetPixel(i, j).Alpha != 0)
                            return true;
                    }
                }
            }
            return false;
        }

        public void Render(RenderPack rp)
        {
            Source.Render(rp);
            _bitmap?.Dispose();
            _bitmap = new SKBitmap((int)rp.FigureRect.Width, (int)rp.FigureRect.Height, SKColorType.Rgba8888, SKAlphaType.Premul);
            using (var hitCanvas = new SKCanvas(_bitmap))
            {
                hitCanvas.Clear(new SKColor(255, 255, 255, 0));
                using (RenderPack rpHittable = new RenderPack(rp.Plot, rp.FigureRect, hitCanvas))
                {
                    rpHittable.CalculateLayout();
                    rpHittable.CanvasState.Clip(rp.DataRect);
                    Source.Render(rpHittable);
                    rpHittable.CanvasState.DisableClipping();
                }
            }
        }

        public void Dispose()
        {
            _bitmap?.Dispose();
            _bitmap = null;
        }
    }
}
