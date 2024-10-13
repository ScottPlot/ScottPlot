#nullable disable
namespace ScottPlot.Triangulation
{
    public static class RectangularGrid
    {
        public static ContourLine[] GetContourLines(Coordinates3d[,] coordinateGrid, double[] zs)
        {
            List<ContourLine> paths = new List<ContourLine>();

            foreach (double z in zs)
            {
                List<Coordinates> pts = [];
                List<EdgeLine> edgeLines = [];
                for (int j = 0; j < coordinateGrid.GetLength(0) - 1; j++)
                    for (int i = 0; i < coordinateGrid.GetLength(1) - 1; i++)
                    {
                        edgeLines.AddRange(LookupSquare(coordinateGrid, i, j, z));
                    }

                var mergedPaths = MergeContourParts(edgeLines, coordinateGrid.GetLength(0));
                paths.AddRange(mergedPaths.Select(elem => new ContourLine(CoordinatePath.Open(elem.Select(edge => edge.Interpolate(coordinateGrid, z)).ToList()), z)));
            }
            return paths.Where(p => p.Path.Points.Length > 0).ToArray();
        }
        private static EdgeLine[] LookupSquare(Coordinates3d[,] CoordinateGrid, int i, int j, double Z)
        {
            // lb - LeftBottom, lt - leftTop, rb - RightBottom, rt - RightTop
            Vertex lb = new(i, j);
            Vertex lt = new(i, j + 1);
            Vertex rb = new(i + 1, j);
            Vertex rt = new(i + 1, j + 1);

            int cellID = j * CoordinateGrid.GetLength(0) + i;

            int index = 0;

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

            IEdge l = new VerticalEdge(lb); // Left edge
            IEdge r = new VerticalEdge(rb); // Right edge
            IEdge b = new HorizontalEdge(lb); // Bottom edge
            IEdge t = new HorizontalEdge(lt); // Top edge

            // all cases described in https://en.wikipedia.org/wiki/Marching_squares
            return index switch
            {
                0 => [],
                1 => [new(l, b, cellID)],
                2 => [new(b, r, cellID)],
                3 => [new(l, r, cellID)],
                4 => [new(t, r, cellID)],
                5 => [new EdgeLinePair(l, t, b, r, cellID)],
                6 => [new(b, t, cellID)],
                7 => [new(l, t, cellID)],
                8 => [new(l, t, cellID)],
                9 => [new(b, t, cellID)],
                10 => [new EdgeLinePair(l, b, t, r, cellID)],
                11 => [new(t, r, cellID)],
                12 => [new(l, r, cellID)],
                13 => [new(b, r, cellID)],
                14 => [new(l, b, cellID)],
                15 => [],
                _ => throw new Exception("Unexpected case"),
            };
        }
        private static List<List<IEdge>> MergeContourParts(List<EdgeLine> edgeLines, int GridHeight)
        {
            var edgeLineLookup = edgeLines.ToDictionary(el => el.CellID);

            List<List<IEdge>> result = new List<List<IEdge>>();
            while (edgeLineLookup.Count > 0)
            {
                List<IEdge> currentPath = new List<IEdge>();

                var startELEntry = edgeLineLookup.First();
                var startEL = startELEntry.Value;

                edgeLineLookup.Remove(startELEntry.Key);

                var rightSearch = FindNeigbhorsChain(startEL.second, startEL.CellID, edgeLineLookup, GridHeight);
                var leftSearch = FindNeigbhorsChain(startEL.first, startEL.CellID, edgeLineLookup, GridHeight);

                leftSearch.Reverse();
                currentPath.AddRange(leftSearch);
                currentPath.Add(startEL.first);
                currentPath.Add(startEL.second);
                currentPath.AddRange(rightSearch);

                result.Add(currentPath);
            }
            return result;
        }

        private static List<IEdge> FindNeigbhorsChain(IEdge startEdge, int startCellId, Dictionary<int, EdgeLine> edgeLineLookup, int GridHeight)
        {
            List<IEdge> currentPath = new List<IEdge>();
            IEdge current = startEdge;
            int currentCellId = startCellId;
            bool found = false;
            do
            {
                EdgeLine candidate;
                var key = GetNeigbhorCell(current, currentCellId, GridHeight);
                found = edgeLineLookup.TryGetValue(key, out candidate);
                if (found)
                {
                    edgeLineLookup.Remove(key);
                    if (candidate is EdgeLinePair candidatePair)
                    {
                        if (candidate.first.Equals(current))
                        {
                            current = candidate.second;
                            currentPath.Add(current);
                            edgeLineLookup.Add(key, new EdgeLine(candidatePair.first1, candidatePair.second1, candidatePair.CellID));
                        }
                        else if (candidate.second.Equals(current))
                        {
                            current = candidate.first;
                            currentPath.Add(current);
                            edgeLineLookup.Add(key, new EdgeLine(candidatePair.first1, candidatePair.second1, candidatePair.CellID));
                        }
                        else if (candidatePair.first1.Equals(current))
                        {
                            current = candidatePair.second1;
                            currentPath.Add(current);
                            edgeLineLookup.Add(key, new EdgeLine(candidatePair.first, candidatePair.second, candidatePair.CellID));
                        }
                        else if (candidatePair.second1.Equals(current))
                        {
                            current = candidatePair.first1;
                            currentPath.Add(current);
                            edgeLineLookup.Add(key, new EdgeLine(candidatePair.first, candidatePair.second, candidatePair.CellID));
                        }
                    }

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
                    currentCellId = candidate.CellID;
                }
            } while (found);
            return currentPath;
        }

        private static int GetNeigbhorCell(IEdge edge, int edgeCell, int GridHeight)
        {
            if (edge is VerticalEdge)
            {
                var edgeInCell = edge.First.j * GridHeight + edge.First.i;
                if (edgeInCell == edgeCell)
                    return edgeCell - 1;
                else
                    return edgeCell + 1;
            }
            if (edge is HorizontalEdge)
            {
                var edgeInCell = edge.First.j * GridHeight + edge.First.i;
                if (edgeInCell == edgeCell)
                    return edgeCell - GridHeight;
                else
                    return edgeCell + GridHeight;
            }
            return 0;
        }
    }
}
