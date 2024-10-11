using ScottPlot.Triangulation;

namespace ScottPlot.Plottables;

public class TINPlot : IPlottable, IHasLegendText
{
    public string LegendText { get; set; } = string.Empty;
    public bool IsVisible { get; set; } = true;
    public IAxes Axes { get; set; } = new Axes();

    private AxisLimits AxisLimits;
    public AxisLimits GetAxisLimits() => AxisLimits;
    public IEnumerable<LegendItem> LegendItems => LegendItem.Single(LegendText, MarkerStyle);

    Coordinates3d[] Points3D = [];
    Delaunator Delaunator = null!;

    public double ContourLineInterval { get; set; } = 0.25;

    public TINPlot(IEnumerable<Coordinates3d> coordinates)
    {
        Update(coordinates);
    }

    public void Update(IEnumerable<Coordinates3d> coordinates)
    {
        AxisLimits = new(coordinates);
        Points3D = coordinates.ToArray();
        Delaunator = new Delaunator(Points3D);
    }

    public MarkerStyle MarkerStyle { get; set; } = new(MarkerShape.FilledCircle, 5, Colors.Red);
    public LineStyle NetworkLineStyle { get; set; } = new(1, Colors.Red);
    public MarkerStyle VoronoiMarkerStyle { get; set; } = new(MarkerShape.FilledCircle, 5, Colors.Blue);
    public LineStyle VoronoiLineStyle { get; set; } = new(1, Colors.Blue);
    public LineStyle ContourLineStyle { get; set; } = new(1, Colors.Gray, LinePattern.DenselyDashed);

    private static Coordinates3d? FindPointAtZ(Coordinates3d startPoint, Coordinates3d endPoint, double z)
    {
        bool bothValuesOnSameSideOfContour = (startPoint.Z < z && endPoint.Z < z) || (startPoint.Z > z && endPoint.Z > z);
        if (bothValuesOnSameSideOfContour)
            return null;

        double t = (z - startPoint.Z) / (endPoint.Z - startPoint.Z);
        double x = startPoint.X + t * (endPoint.X - startPoint.X);
        double y = startPoint.Y + t * (endPoint.Y - startPoint.Y);
        return new Coordinates3d(x, y, z);
    }

    private void DrawNetworkLines(RenderPack rp, SKPaint paint)
    {
        foreach (var edge in Delaunator.GetEdges())
        {
            Pixel px1 = Axes.GetPixel(edge.PCoordinates);
            Pixel px2 = Axes.GetPixel(edge.QCoordinates);
            Drawing.DrawLine(rp.Canvas, paint, px1, px2, NetworkLineStyle);
        }
    }

    private void DrawVoronoi(RenderPack rp, SKPaint paint)
    {
        foreach (var cell in Delaunator.GetVoronoiCells())
        {
            IEnumerable<Pixel> polygonPixels = cell.Points.Select(x => Axes.GetPixel(x.Coordinates2d()));
            Drawing.DrawMarkers(rp.Canvas, paint, polygonPixels.Distinct(), VoronoiMarkerStyle);
            Drawing.DrawLines(rp.Canvas, paint, polygonPixels, VoronoiLineStyle);
        }
    }

    private void DrawMarkers(RenderPack rp, SKPaint paint)
    {
        IEnumerable<Pixel> markerPixels = Points3D.Select(x => Axes.GetPixel(x.Coordinates2d()));
        Drawing.DrawMarkers(rp.Canvas, paint, markerPixels, MarkerStyle);
    }

    private void DrawContours(RenderPack rp, SKPaint paint)
    {
        foreach (Triangle3D triangle in Delaunator.GetTriangles())
        {
            double zMin = Math.Floor(triangle.MinZ / ContourLineInterval) * ContourLineInterval;
            double zMax = triangle.MaxZ;

            for (double z = zMin; z <= zMax; z += ContourLineInterval)
            {
                List<Pixel> pts = [];
                for (int i = 0; i < triangle.Points.Count(); i++)
                {
                    // Find the points on the edge of the triangle at the current elevation.
                    // the third edge wraps around to the first point
                    Coordinates3d startPoint = triangle.Points.ElementAt(i);
                    Coordinates3d endPoint = triangle.Points.ElementAt(i < 2 ? i + 1 : 0);
                    Coordinates3d? pt = TINPlot.FindPointAtZ(startPoint, endPoint, z);

                    if (pt is not null)
                    {
                        pts.Add(Axes.GetPixel(new Coordinates(pt.Value.X, pt.Value.Y)));
                    }
                }

                Drawing.DrawLines(rp.Canvas, paint, pts, ContourLineStyle);
            }
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
