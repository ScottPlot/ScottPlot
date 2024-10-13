#nullable disable
namespace ScottPlot.Triangulation
{
    public static class RectangularGrid
    {
        public static ContourLine[] GetContourLines(Coordinates3d[,] coordinateGrid, double[] zs)
        {
            var paths = zs.Select(z =>
            {
                var edgeLines = LookupSquare(coordinateGrid, z).ToDictionary(e => e.CellID);
                var mergedPaths = MergeContourParts(edgeLines, coordinateGrid.GetLength(0));
                return mergedPaths.Select(elem => new ContourLine(CoordinatePath.Open(elem.Select(edge => edge.Interpolate(coordinateGrid, z))), z));
            });
            return paths.SelectMany(elem => elem).ToArray();
        }

        private static IEnumerable<EdgeLine> LookupSquare(Coordinates3d[,] CoordinateGrid, double Z)
        {
            for (int j = 0; j < CoordinateGrid.GetLength(0) - 1; j++)
                for (int i = 0; i < CoordinateGrid.GetLength(1) - 1; i++)
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
                    switch (index)
                    {
                        case 0:
                            break;
                        case 1:
                            yield return new(l, b, cellID);
                            break;
                        case 2:
                            yield return new(b, r, cellID);
                            break;
                        case 3:
                            yield return new(l, r, cellID);
                            break;
                        case 4:
                            yield return new(t, r, cellID);
                            break;
                        case 5:
                            yield return new EdgeLinePair(l, t, b, r, cellID);
                            break;
                        case 6:
                            yield return new(b, t, cellID);
                            break;
                        case 7:
                            yield return new(l, t, cellID);
                            break;
                        case 8:
                            yield return new(l, t, cellID);
                            break;
                        case 9:
                            yield return new(b, t, cellID);
                            break;
                        case 10:
                            yield return new EdgeLinePair(l, b, t, r, cellID);
                            break;
                        case 11:
                            yield return new(t, r, cellID);
                            break;
                        case 12:
                            yield return new(l, r, cellID);
                            break;
                        case 13:
                            yield return new(b, r, cellID);
                            break;
                        case 14:
                            yield return new(l, b, cellID);
                            break;
                        case 15:
                            break;
                        default: throw new Exception("Unexpected case");
                    }
                }
        }
        private static IEnumerable<IEnumerable<IEdge>> MergeContourParts(Dictionary<int, EdgeLine> edgeLines, int GridHeight)
        {
            while (edgeLines.Count > 0)
            {
                List<IEdge> currentPath = new List<IEdge>();

                var startELEntry = edgeLines.First();
                var startEL = startELEntry.Value;

                edgeLines.Remove(startELEntry.Key);

                var rightSearch = FindNeigbhorsChain(startEL.second, startEL.CellID, edgeLines, GridHeight);
                var leftSearch = FindNeigbhorsChain(startEL.first, startEL.CellID, edgeLines, GridHeight);

                var contour =
                    leftSearch.Reverse()
                    .Concat([startEL.first, startEL.second])
                    .Concat(rightSearch);
                yield return contour;
            }
        }

        private static IEnumerable<IEdge> FindNeigbhorsChain(IEdge startEdge, int startCellId, Dictionary<int, EdgeLine> edgeLineLookup, int GridHeight)
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
                            edgeLineLookup.Add(key, new EdgeLine(candidatePair.first1, candidatePair.second1, candidatePair.CellID));
                        }
                        else if (candidate.second.Equals(current))
                        {
                            current = candidate.first;
                            edgeLineLookup.Add(key, new EdgeLine(candidatePair.first1, candidatePair.second1, candidatePair.CellID));
                        }
                        else if (candidatePair.first1.Equals(current))
                        {
                            current = candidatePair.second1;
                            edgeLineLookup.Add(key, new EdgeLine(candidatePair.first, candidatePair.second, candidatePair.CellID));
                        }
                        else if (candidatePair.second1.Equals(current))
                        {
                            current = candidatePair.first1;
                            edgeLineLookup.Add(key, new EdgeLine(candidatePair.first, candidatePair.second, candidatePair.CellID));
                        }
                        currentCellId = candidate.CellID;

                    }
                    else
                    {
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

                    yield return current;
                    currentCellId = candidate.CellID;
                }
            } while (found);
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
