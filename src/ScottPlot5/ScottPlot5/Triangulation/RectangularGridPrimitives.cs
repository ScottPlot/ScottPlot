namespace ScottPlot.Triangulation
{
    public record Vertex(int i, int j);
    public record EdgeLine(Edge first, Edge second);

    public abstract class Edge
    {
        public Vertex First { get; }
        public Vertex Second { get; }
        public Edge(Vertex first, Vertex second)
        {
            First = first;
            Second = second;
        }

        public virtual Coordinates Interpolate(Coordinates3d[,] coordinateGrid, double Z)
        {
            throw new NotImplementedException();
        }

        public override bool Equals(object? obj)
        {
            var other = obj as Edge;
            if (other is null)
                return false;

            return (this.First.i == other.First.i && this.First.j == other.First.j && this.Second.i == other.Second.i && this.Second.j == other.Second.j);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + First.i.GetHashCode();
                hash = hash * 23 + First.j.GetHashCode();
                hash = hash * 23 + Second.i.GetHashCode();
                hash = hash * 23 + Second.j.GetHashCode();
                return hash;
            }
        }
    }

    public class HorizontalEdge : Edge
    {
        public HorizontalEdge(Vertex first, Vertex second) : base(first, second)
        {
        }

        public override Coordinates Interpolate(Coordinates3d[,] coordinateGrid, double Z)
        {
            Coordinates3d first = coordinateGrid[First.j, First.i];
            Coordinates3d second = coordinateGrid[Second.j, Second.i];
            var x = first.X + (second.X - first.X) / (second.Z - first.Z) * (Z - first.Z);
            return new Coordinates(x, first.Y);
        }
    }

    public class VerticalEdge : Edge
    {
        public VerticalEdge(Vertex first, Vertex second) : base(first, second)
        {
        }

        public override Coordinates Interpolate(Coordinates3d[,] coordinateGrid, double Z)
        {
            Coordinates3d first = coordinateGrid[First.j, First.i];
            Coordinates3d second = coordinateGrid[Second.j, Second.i];
            var y = first.Y + (second.Y - first.Y) / (second.Z - first.Z) * (Z - first.Z);
            return new Coordinates(first.X, y);
        }
    }
}
