namespace ScottPlot.Triangulation;

public class TriangulatedIrregularNetwork
{
    public AxisLimits3d AxisLimits { get; private set; }

    public Coordinates3d[] Points3D { get; private set; } = [];

    private Delaunator Delaunator = null!;

    public TriangulatedIrregularNetwork(IEnumerable<Coordinates3d> coordinates)
    {
        Update(coordinates);
    }

    public void Update(IEnumerable<Coordinates3d> coordinates)
    {
        AxisLimits = AxisLimits3d.FromPoints(coordinates);
        Points3D = coordinates.ToArray();
        Delaunator = new Delaunator(Points3D);
    }

    public CoordinateLine[] GetLines() => Delaunator.GetEdges().Select(x => x.CoordinateLine).ToArray();

    public CoordinatePath[] GetCells() => Delaunator.GetVoronoiCells().Select(cell => cell.Path).ToArray();

    public CoordinatePath[] GetContourLines(double zInterval)
    {
        List<CoordinatePath> paths = [];

        foreach (Triangle3D triangle in Delaunator.GetTriangles())
        {
            double zMin = Math.Floor(triangle.MinZ / zInterval) * zInterval;
            double zMax = triangle.MaxZ;

            for (double z = zMin; z <= zMax; z += zInterval)
            {
                List<Coordinates> pts = [];
                for (int i = 0; i < triangle.Points.Count(); i++)
                {
                    Coordinates3d startPoint = triangle.Points.ElementAt(i);
                    Coordinates3d endPoint = triangle.Points.ElementAt(i < 2 ? i + 1 : 0);
                    Coordinates3d? pt = FindPointAtZ(startPoint, endPoint, z);

                    if (pt is not null)
                    {
                        pts.Add(new Coordinates(pt.Value.X, pt.Value.Y));
                    }
                }

                paths.Add(CoordinatePath.Open(pts));
            }
        };

        return [.. paths];
    }

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
}
