namespace ScottPlot.Triangulation
{
    public static class RectangularGrid
    {
        public static ContourLine[] GetContourLines(Coordinates3d[,] coordinateGrid, double[] zs)
        {
            List<ContourLine> paths = new List<ContourLine>();

            foreach ( double z in zs)
            {
                List<Coordinates> pts = [];
                List<EdgeLine> edgeLines = [];
                for (int j = 0; j < coordinateGrid.GetLength(0) - 1; j++)
                    for (int i = 0; i < coordinateGrid.GetLength(1) - 1; i++)
                    {
                        edgeLines.AddRange(LookupSquare(coordinateGrid, i, j, z));
                    }

                var groupedEdges = GroupEdges(edgeLines);
                paths.AddRange(groupedEdges.Select(elem => new ContourLine(CoordinatePath.Open(elem.Select(edge => edge.Interpolate(coordinateGrid, z)).ToList()), z)));
            }
            return paths.Where(p => p.Path.Points.Length > 0).ToArray();
        }
        private static EdgeLine[] LookupSquare(Coordinates3d[,] CoordinateGrid, int i, int j, double Z)
        {
            int index = 0;

            // lb - LeftBottom, lt - leftTop, rb - RightBottom, rt - RightTop
            Vertex lb = new(i, j);
            Vertex lt = new(i, j + 1);
            Vertex rb = new(i + 1, j);
            Vertex rt = new(i + 1, j + 1);

            if (CoordinateGrid[lb.j, lb.i].Z > Z)
                index += 1;
            if (CoordinateGrid[rb.j, rb.i].Z > Z)
                index += 2;
            if (CoordinateGrid[rt.j, rt.i].Z > Z)
                index += 4;
            if (CoordinateGrid[lt.j, lt.i].Z > Z)
                index += 8;

            if (index == 5 || index == 10) // saddle point fix
            {
                var midpoint = CoordinateGrid[lb.j, lb.i].Z
                               + CoordinateGrid[rb.j, rb.i].Z
                               + CoordinateGrid[rt.j, rt.i].Z
                               + CoordinateGrid[lt.j, lt.i].Z;
                midpoint /= 4;
                if (midpoint <= Z)
                {
                    if (index == 5)
                        index = 10;
                    else
                        index = 5;
                }
            }

            Edge l = new VerticalEdge(lb, lt); // Left edge
            Edge r = new VerticalEdge(rb, rt); // Right edge
            Edge b = new HorizontalEdge(lb, rb); // Bottom edge
            Edge t = new HorizontalEdge(lt, rt); // Top edge

            // all cases described in https://en.wikipedia.org/wiki/Marching_squares
            return index switch
            {
                0 => [],
                1 => [new(l, b)],
                2 => [new(b, r)],
                3 => [new(l, r)],
                4 => [new(t, r)],
                5 => [new(l, t), new(b, r)],
                6 => [new(b, t)],
                7 => [new(l, t)],
                8 => [new(l, t)],
                9 => [new(b, t)],
                10 => [new(l, b), new(t, r)],
                11 => [new(t, r)],
                12 => [new(l, r)],
                13 => [new(b, r)],
                14 => [new(l, b)],
                15 => [],
                _ => throw new Exception("Unexpected case"),
            };
        }
        private static List<List<Edge>> GroupEdges(List<EdgeLine> edgeLinesArg)
        {
            var edgeLines = edgeLinesArg.ToList();
            List<List<Edge>> result = new List<List<Edge>>();
            while (edgeLines.Count > 0)
            {
                List<Edge> currentPath = new List<Edge>();
                currentPath.Add(edgeLines[0].first);
                currentPath.Add(edgeLines[0].second);

                var current = edgeLines[0].second;
                edgeLines.RemoveAt(0);
                int candidateIndex;
                // search for a right side neighbors
                while ((candidateIndex = edgeLines.FindIndex(e => e.first.Equals(current) || e.second.Equals(current))) != -1)
                {
                    var candidate = edgeLines[candidateIndex];
                    edgeLines.RemoveAt(candidateIndex);
                    if (candidate.first.Equals(current))
                    {
                        current = candidate.second;
                        currentPath.Add(current);
                    }
                    else
                    {
                        current = candidate.first;
                        currentPath.Add(current);
                    }
                }
                current = currentPath[0];
                // search for a left side neighbors
                while ((candidateIndex = edgeLines.FindIndex(e => e.first.Equals(current) || e.second.Equals(current))) != -1)
                {
                    var candidate = edgeLines[candidateIndex];
                    edgeLines.RemoveAt(candidateIndex);
                    if (candidate.first.Equals(current))
                    {
                        current = candidate.second;
                        currentPath.Insert(0, current);
                    }
                    else
                    {
                        current = candidate.first;
                        currentPath.Insert(0, current);
                    }
                }
                result.Add(currentPath);
            }

            return result;
        }

    }
}
