namespace ScottPlot.Triangulation
{
    public record Vertex(int i, int j);
    public class EdgeLine
    {
        public IEdge first { get; }
        public IEdge second { get; }
        public int CellID { get; set; }

        public EdgeLine(IEdge first, IEdge second, int cellId)
        {
            this.first = first;
            this.second = second;
            CellID = cellId;
        }
    }

    public class EdgeLinePair : EdgeLine
    {
        public IEdge first1 { get; }
        public IEdge second1 { get; }
        public EdgeLinePair(IEdge first, IEdge second, IEdge first1, IEdge second1, int cellId) : base(first, second, cellId)
        {
            this.first1 = first1;
            this.second1 = second1;
        }
    }

    public interface IEdge
    {
        Vertex First { get; }
        Coordinates Interpolate(Coordinates3d[,] coordinateGrid, double Z);
    }

    public class HorizontalEdge : IEdge
    {
        public Vertex First { get; }
        public HorizontalEdge(Vertex first)
        {
            First = first;
        }
        public override bool Equals(object? obj)
        {
            var other = obj as HorizontalEdge;
            if (other is null)
                return false;

            return this.First.i == other.First.i && this.First.j == other.First.j;
        }


        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + First.i.GetHashCode();
                hash = hash * 23 + First.j.GetHashCode();
                return hash;
            }
        }

        public Coordinates Interpolate(Coordinates3d[,] coordinateGrid, double Z)
        {
            Coordinates3d first = coordinateGrid[First.j, First.i];
            Coordinates3d second = coordinateGrid[First.j, First.i + 1];
            var x = first.X + (second.X - first.X) / (second.Z - first.Z) * (Z - first.Z);
            return new Coordinates(x, first.Y);
        }
    }

    public class VerticalEdge : IEdge
    {
        public Vertex First { get; }
        public VerticalEdge(Vertex first)
        {
            First = first;
        }

        public override bool Equals(object? obj)
        {
            var other = obj as VerticalEdge;
            if (other is null)
                return false;

            return (this.First.i == other.First.i && this.First.j == other.First.j);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + First.i.GetHashCode();
                hash = hash * 23 + First.j.GetHashCode();
                return hash;
            }
        }

        public Coordinates Interpolate(Coordinates3d[,] coordinateGrid, double Z)
        {
            Coordinates3d first = coordinateGrid[First.j, First.i];
            Coordinates3d second = coordinateGrid[First.j + 1, First.i];
            var y = first.Y + (second.Y - first.Y) / (second.Z - first.Z) * (Z - first.Z);
            return new Coordinates(first.X, y);
        }
    }
}
