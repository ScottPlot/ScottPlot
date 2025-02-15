namespace ScottPlot.Triangulation;

public class TriangulatedIrregularNetwork
{
    public AxisLimits3d AxisLimits { get; private set; }

    public Coordinates3d[] Points3D { get; private set; } = [];
    public double MinZ { get; private set; }
    public double MaxZ { get; private set; }

    private Delaunator Delaunator = null!;

    public TriangulatedIrregularNetwork(IEnumerable<Coordinates3d> coordinates)
    {
        Update(coordinates);
    }

    public void Update(IEnumerable<Coordinates3d> coordinates)
    {
        AxisLimits = AxisLimits3d.FromPoints(coordinates);
        Points3D = coordinates.ToArray();
        MinZ = coordinates.Select(x => x.Z).Min();
        MaxZ = coordinates.Select(x => x.Z).Max();
        Delaunator = new Delaunator(Points3D);
    }

    public double[] GetZsByCount(int count)
    {
        double interval = (MaxZ - MinZ) / (count + 1);
        return GetZsByInterval(interval);
    }

    public double[] GetZsByInterval(double interval)
    {
        int count = (int)((MaxZ - MinZ) / interval) + 1;
        return Enumerable.Range(0, count).Select(x => MinZ + interval * x).ToArray();
    }

    public CoordinateLine[] GetLines() => Delaunator.GetEdges().Select(x => x.CoordinateLine).ToArray();

    public CoordinatePath[] GetCells() => Delaunator.GetVoronoiCells().Select(cell => cell.Path).ToArray();

    public ContourLine[] GetContourLines(double[] zs)
    {
        List<ContourLine> paths = [];

        foreach (Triangle3D triangle in Delaunator.GetTriangles())
        {
            foreach (double z in zs)
            {
                if (z < triangle.MinZ || z > triangle.MaxZ)
                    continue;

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

                if (pts.Count > 0)
                {
                    CoordinatePath path = CoordinatePath.Open(pts);
                    paths.Add(new(path, z));
                }
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
