using ScottPlot.Triangulation;

namespace ScottPlot.Plottables;

public class TINPlot : IPlottable
{
    public string LegendText { get; set; } = string.Empty;
    public bool IsVisible { get; set; } = true;
    public IAxes Axes { get; set; } = new Axes();

    public AxisLimits GetAxisLimits() => TIN.AxisLimits.AxisLimits2d;
    public IEnumerable<LegendItem> LegendItems => LegendItem.Single(this, LegendText, MarkerStyle);

    public double ContourLineInterval { get; set; } = 0.25;

    TriangulatedIrregularNetwork TIN = null!;

    public TINPlot(IEnumerable<Coordinates3d> coordinates)
    {
        Update(coordinates);
    }

    public void Update(IEnumerable<Coordinates3d> coordinates)
    {
        TIN = new(coordinates);
    }

    public MarkerStyle MarkerStyle { get; set; } = new(MarkerShape.FilledCircle, 5, Colors.Red);
    public LineStyle NetworkLineStyle { get; set; } = new(1, Colors.Red);
    public MarkerStyle VoronoiMarkerStyle { get; set; } = new(MarkerShape.FilledCircle, 5, Colors.Blue);
    public LineStyle VoronoiLineStyle { get; set; } = new(1, Colors.Blue);
    public LineStyle ContourLineStyle { get; set; } = new(1, Colors.Gray, LinePattern.DenselyDashed);

    private void DrawNetworkLines(RenderPack rp, SKPaint paint)
    {
        foreach (CoordinateLine line in TIN.GetLines())
        {
            PixelLine pxLine = Axes.GetPixelLine(line);
            Drawing.DrawLine(rp.Canvas, paint, pxLine, NetworkLineStyle);
        }
    }

    private void DrawVoronoi(RenderPack rp, SKPaint paint)
    {
        foreach (CoordinatePath cell in TIN.GetCells())
        {
            Pixel[] pixels = cell.Points.Select(Axes.GetPixel).ToArray();
            Drawing.DrawMarkers(rp.Canvas, paint, pixels, VoronoiMarkerStyle);
            Drawing.DrawLines(rp.Canvas, paint, pixels, VoronoiLineStyle);
        }
    }

    private void DrawMarkers(RenderPack rp, SKPaint paint)
    {
        IEnumerable<Pixel> markerPixels = TIN.Points3D.Select(x => Axes.GetPixel(x.Coordinates2d()));
        Drawing.DrawMarkers(rp.Canvas, paint, markerPixels, MarkerStyle);
    }

    private void DrawContours(RenderPack rp, SKPaint paint)
    {
        double[] zs = TIN.GetZsByInterval(ContourLineInterval);
        foreach (ContourLine line in TIN.GetContourLines(zs))
        {
            Pixel[] pixels = line.Path.Points.Select(Axes.GetPixel).ToArray();
            Drawing.DrawLines(rp.Canvas, paint, pixels, ContourLineStyle);
        };
    }

    public virtual void Render(RenderPack rp)
    {
        using SKPaint paint = new();
        DrawNetworkLines(rp, paint);
        DrawMarkers(rp, paint);
        DrawVoronoi(rp, paint);
        DrawContours(rp, paint);
    }
}
