namespace ScottPlot.Benchmarking;

public interface IBenchmark
{
    void Start();
    void Stop();
    void Reset();
    void Restart();
    TimeSpan Elapsed { get; }
    bool IsVisible { get; set; }
    void Render(SKCanvas canvas, PixelRect dataRect);
}
