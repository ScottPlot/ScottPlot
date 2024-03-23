﻿namespace ScottPlot;

public class RenderPack
{
    public SKCanvas Canvas { get; }
    public PixelRect FigureRect { get; }
    public PixelRect DataRect { get; private set; }
    public Layout Layout { get; private set; }
    public Plot Plot { get; }
    private Stopwatch Stopwatch { get; }
    public TimeSpan Elapsed => Stopwatch.Elapsed;

    /// <summary>
    /// This object is passed through the render system.
    /// The plot will be drawn on the canvas to create a figure of the given size.
    /// </summary>
    public RenderPack(Plot plot, PixelRect figureRect, SKCanvas canvas)
    {
        Canvas = canvas;
        FigureRect = figureRect;
        Plot = plot;
        Stopwatch = Stopwatch.StartNew();
    }

    public void CalculateLayout()
    {
        if (DataRect.HasArea)
            throw new InvalidOperationException("DataRect must only be calculated once per render");

        PixelRect scaledRect = new(
            left: FigureRect.Left / Plot.ScaleFactorF,
            right: FigureRect.Right / Plot.ScaleFactorF,
            bottom: FigureRect.Bottom / Plot.ScaleFactorF,
            top: FigureRect.Top / Plot.ScaleFactorF);

        Layout = Plot.Layout.LayoutEngine.GetLayout(scaledRect, Plot.Axes.GetPanels());
        DataRect = Layout.DataRect;
    }

    public override string ToString()
    {
        return $"RenderPack FigureRect={FigureRect} DataRect={DataRect}";
    }

    public RestoreState PushClip(PixelRect rect)
    {
        Canvas.Save();
        Canvas.ClipRect(rect.ToSKRect());

        return new(this);
    }

    public RestoreState PushClipToDataArea()
    {
        return PushClip(DataRect);
    }

    [Obsolete($"Use {nameof(PushClipToDataArea)} instead.")]
    public void ClipToDataArea()
    {
        Canvas.ClipRect(DataRect.ToSKRect());
    }

    [Obsolete($"Use {nameof(PushClipToDataArea)} instead.")]
    public void DisableClipping()
    {
        Canvas.Restore();
    }

    public readonly struct RestoreState : IDisposable
    {
        private readonly RenderPack _rp;

        public RestoreState(RenderPack rp)
        {
            _rp = rp;
        }

        public void Dispose()
        {
            _rp.Canvas.Restore();
        }
    }
}
