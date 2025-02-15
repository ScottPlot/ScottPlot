#nullable disable
namespace ScottPlot.Triangulation
{
    public static class RectangularGrid
    {
        public static List<ContourLine> GetContourLines(Coordinates3d[,] coordinateGrid, double[] zs, bool fixContourDirections = true)
        {
            List<ContourLine> result;
            result = zs.Select(z =>
            {
                var edgeLines = MarchingSquares(coordinateGrid, z).ToDictionary(e => e.CellID);
                var mergedPaths = MergeContourParts(edgeLines, coordinateGrid.GetLength(1));
                return mergedPaths.Select(elem => new ContourLine(CoordinatePath.Open(elem.Select(edge => edge.Interpolate(coordinateGrid, z))), z));
            })
            .SelectMany(elem => elem)
            .ToList();

            if (fixContourDirections)
                return FixContourDirections(result);
            else
                return result;
        }

        public static List<ContourLine> FixContourDirections(List<ContourLine> contours)
        {
            for (int i = 0; i < contours.Count; i++)
            {
                // closed contour, make it starts from a top 
                if (Math.Abs(contours[i].Path.Points[0].X - contours[i].Path.Points[^1].X) < 0.0000000001
                    && Math.Abs(contours[i].Path.Points[0].Y - contours[i].Path.Points[^1].Y) < 0.0000000001)
                {
                    var points = contours[i].Path.Points;
                    var max = points[0].Y;
                    var maxIndex = 0;
                    for (int j = 0; j < points.Length; j++)
                    {
                        if (points[j].Y > max)
                        {
                            max = points[j].Y;
                            maxIndex = j;
                        }
                    }
                    var rotatedPoints = points.Skip(maxIndex).Concat(points.Skip(1).Take(maxIndex - 1)).Concat([points[maxIndex]]).ToArray();
                    contours[i] = new ContourLine(CoordinatePath.Open(rotatedPoints), contours[i].Z);
                }
                else
                {
                    // open contour, make it starts from left
                    if (contours[i].Path.Points[0].X > contours[i].Path.Points[^1].X)
                        contours[i] = new ContourLine(CoordinatePath.Open(contours[i].Path.Points.Reverse().ToArray()), contours[i].Z);
                }
            }
            return contours;
        }

        private static IEnumerable<EdgeLine> MarchingSquares(Coordinates3d[,] CoordinateGrid, double Z)
        {
            for (int j = 0; j < CoordinateGrid.GetLength(0) - 1; j++)
                for (int i = 0; i < CoordinateGrid.GetLength(1) - 1; i++)
                {
                    Vertex lb = new(i, j);         // LeftBottom
                    Vertex lt = new(i, j + 1);     // LeftTop
                    Vertex rb = new(i + 1, j);     // RigthtBottom
                    Vertex rt = new(i + 1, j + 1); // RightTop

                    int cellID = j * CoordinateGrid.GetLength(1) + i;

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
                            index = 15 - index; // swap 5 <-> 10
                        }
                    }

                    IEdge l = new VerticalEdge(lb); // Left edge
                    IEdge r = new VerticalEdge(rb); // Right edge
                    IEdge b = new HorizontalEdge(lb); // Bottom edge
                    IEdge t = new HorizontalEdge(lt); // Top edge

                    // all cases described in https://en.wikipedia.org/wiki/Marching_squares
                    if (index == 0 || index == 15) // empty cell
                    {
                        continue;
                    }
                    yield return index switch
                    {
                        1 or 14 => new EdgeLine(l, b, cellID),
                        2 or 13 => new EdgeLine(b, r, cellID),
                        3 or 12 => new EdgeLine(l, r, cellID),
                        4 or 11 => new EdgeLine(t, r, cellID),
                        5 => new EdgeLinePair(l, t, b, r, cellID),
                        6 or 9 => new EdgeLine(b, t, cellID),
                        7 or 8 => new EdgeLine(l, t, cellID),
                        10 => new EdgeLinePair(l, b, t, r, cellID),
                        _ => throw new Exception("Unexpected case"),
                    };
                }
        }
        private static IEnumerable<IEnumerable<IEdge>> MergeContourParts(Dictionary<int, EdgeLine> edgeLines, int GridHeight)
        {
            while (edgeLines.Count > 0)
            {
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
            IEdge current = startEdge;
            int currentCellId = startCellId;
            bool found;
            do
            {
                EdgeLine candidate;
                var key = GetNeigbhorCell(current, currentCellId, GridHeight);
                found = edgeLineLookup.TryGetValue(key, out candidate);
                if (found)
                {
                    edgeLineLookup.Remove(key);
                    if (candidate is EdgeLinePair candidatePair) // 2 lines in cell. We take the right one, and we put the other one back.
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

                    }
                    else // single line in cell
                    {
                        if (candidate.first.Equals(current))
                        {
                            current = candidate.second;
                        }
                        else
                        {
                            current = candidate.first;
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
