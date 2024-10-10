using ScottPlot.DataSources;

namespace ScottPlot.Plottables
{
    public class TINPlot(TINSourceCoordinates3dArray data) : IPlottable, IHasLine, IHasMarker, IHasLegendText
    {
        // TODO: the outer boundary (convex hull) can be easily added with its own style, as can the voronoi points and lines

        public string LegendText { get; set; } = string.Empty;
        public bool IsVisible { get; set; } = true;
        public IAxes Axes { get; set; } = new Axes();
        public AxisLimits GetAxisLimits() => Data.GetLimits();
        public IEnumerable<LegendItem> LegendItems => LegendItem.Single(LegendText, MarkerStyle);
        public double MinorInterval { get; set; } = 0.25;
        public double MajorInterval { get; set; } = 5;
        public TINSourceCoordinates3dArray Data { get; } = data;

        #region MarkerStyle

        public MarkerStyle MarkerStyle { get; set; } = new() { LineWidth = 1, Size = 3, Shape = MarkerShape.Eks, MarkerColor = Colors.Red };
        public MarkerShape MarkerShape { get => MarkerStyle.Shape; set => MarkerStyle.Shape = value; }
        public float MarkerSize { get => MarkerStyle.Size; set => MarkerStyle.Size = value; }
        public Color MarkerFillColor { get => MarkerStyle.FillColor; set => MarkerStyle.FillColor = value; }
        public Color MarkerLineColor { get => MarkerStyle.LineColor; set => MarkerStyle.LineColor = value; }
        public Color MarkerColor { get => MarkerStyle.MarkerColor; set => MarkerStyle.MarkerColor = value; }
        public float MarkerLineWidth { get => MarkerStyle.LineWidth; set => MarkerStyle.LineWidth = value; }

        #endregion

        #region LineStyle

        public LineStyle LineStyle { get; set; } = new() { Width = 1, Color = Colors.LightGray };
        public float LineWidth { get => LineStyle.Width; set => LineStyle.Width = value; }
        public LinePattern LinePattern { get => LineStyle.Pattern; set => LineStyle.Pattern = value; }
        public Color LineColor { get => LineStyle.Color; set => LineStyle.Color = value; }

        #endregion

        #region VoronoiMarkerStyle

        public MarkerStyle VoronoiMarkerStyle { get; set; } = new() { LineWidth = 1, Size = 0, Shape = MarkerShape.OpenCircle, MarkerColor = Colors.Blue };
        public MarkerShape VoronoiMarkerShape { get => VoronoiMarkerStyle.Shape; set => VoronoiMarkerStyle.Shape = value; }
        public float VoronoiMarkerSize { get => VoronoiMarkerStyle.Size; set => VoronoiMarkerStyle.Size = value; }
        public Color VoronoiMarkerFillColor { get => VoronoiMarkerStyle.FillColor; set => VoronoiMarkerStyle.FillColor = value; }
        public Color VoronoiMarkerLineColor { get => VoronoiMarkerStyle.LineColor; set => VoronoiMarkerStyle.LineColor = value; }
        public Color VoronoiMarkerColor { get => VoronoiMarkerStyle.MarkerColor; set => VoronoiMarkerStyle.MarkerColor = value; }
        public float VoronoiMarkerLineWidth { get => VoronoiMarkerStyle.LineWidth; set => VoronoiMarkerStyle.LineWidth = value; }

        #endregion

        #region VoronoiLineStyle

        public LineStyle VoronoiLineStyle { get; set; } = new() { Width = 0, Color = Colors.Black };
        public float VoronoiLineWidth { get => VoronoiLineStyle.Width; set => VoronoiLineStyle.Width = value; }
        public LinePattern VoronoiLinePattern { get => VoronoiLineStyle.Pattern; set => VoronoiLineStyle.Pattern = value; }
        public Color VoronoiLineColor { get => VoronoiLineStyle.Color; set => VoronoiLineStyle.Color = value; }

        #endregion       

        #region ContourLineStyle

        public LineStyle ContourLineStyle { get; set; } = new() { Width = 1, Color = Colors.Red };
        public float ContourLineWidth { get => ContourLineStyle.Width; set => ContourLineStyle.Width = value; }
        public LinePattern ContourLinePattern { get => ContourLineStyle.Pattern; set => ContourLineStyle.Pattern = value; }
        public Color ContourLineColor { get => ContourLineStyle.Color; set => ContourLineStyle.Color = value; }

        #endregion

        #region IndexContourLineStyle

        public LineStyle IndexContourLineStyle { get; set; } = new() { Width = 0, Color = Colors.Green };
        public float IndexContourLineWidth { get => IndexContourLineStyle.Width; set => IndexContourLineStyle.Width = value; }
        public LinePattern IndexContourLinePattern { get => IndexContourLineStyle.Pattern; set => IndexContourLineStyle.Pattern = value; }
        public Color IndexContourLineColor { get => IndexContourLineStyle.Color; set => IndexContourLineStyle.Color = value; }

        #endregion

        private Triangulation.Point? FindPointAtElevation(Triangulation.Point startPoint, Triangulation.Point endPoint, double elev)
        {
            // If both Z values are on the same side of the contour, skip this edge
            if ((startPoint.Z < elev && endPoint.Z < elev) || (startPoint.Z > elev && endPoint.Z > elev))
            {
                return null;
            }

            // Otherwise, find the point on the edge at the elevation
            double t = (elev - startPoint.Z) / (endPoint.Z - startPoint.Z);
            double x = startPoint.X + t * (endPoint.X - startPoint.X);
            double y = startPoint.Y + t * (endPoint.Y - startPoint.Y);
            return new Triangulation.Point(x, y, elev);
        }

        private void DrawTINLines(RenderPack rp, SKPaint paint, Triangulation.Delaunator delaunator)
        {
            if ((LineStyle != LineStyle.None) && LineWidth > 0)
            {
                delaunator.ForEachTriangleEdge(edge =>
                {
                    Drawing.DrawLine(rp.Canvas, paint, Axes.GetPixel(new Coordinates((float)edge.P.X, (float)edge.P.Y)), Axes.GetPixel(new Coordinates((float)edge.Q.X, (float)edge.Q.Y)), LineStyle);
                });
            }
        }

        private void DrawVoronoi(RenderPack rp, SKPaint paint, Triangulation.Delaunator delaunator)
        {
            // If the lines AND the markers are either invisible or have no size, there is nothing to draw
            if (((VoronoiLineStyle == LineStyle.None) || (VoronoiLineStyle.Width == 0)) &&
                ((VoronoiMarkerStyle == MarkerStyle.None) || (VoronoiMarkerStyle.Size == 0)))
                return;
            delaunator.ForEachVoronoiCell((cell) =>
            {
                IEnumerable<Pixel> polygonPixels = Enumerable.Range(0, cell.Points.Count()).Select(x => Axes.GetPixel(new Coordinates(cell.Points[x].X, cell.Points[x].Y)));
                if ((VoronoiMarkerStyle != MarkerStyle.None) || (VoronoiMarkerStyle.Size > 0))
                {
                    // TODO: there has to be an efficient way to remove duplicates.  
                    Drawing.DrawMarkers(rp.Canvas, paint, polygonPixels, VoronoiMarkerStyle);
                }
                if ((VoronoiLineStyle != LineStyle.None) || (VoronoiLineStyle.Width > 0))
                {
                    Drawing.DrawLines(rp.Canvas, paint, polygonPixels, VoronoiLineStyle);
                }
            });
        }
        private void DrawMarkers(RenderPack rp, SKPaint paint, IReadOnlyList<Coordinates3d> points, MarkerStyle style)
        {
            if ((style != MarkerStyle.None) && (style.Size > 0))
            {
                IEnumerable<Pixel> markerPixels = Enumerable.Range(0, points.Count).Select(x => Axes.GetPixel(new Coordinates(points[x].X, points[x].Y)));
                Drawing.DrawMarkers(rp.Canvas, paint, markerPixels, style);
            }
        }

        private void DrawContours(RenderPack rp, SKPaint paint, Triangulation.Delaunator delaunator)
        {
            if ((ContourLineStyle != LineStyle.None) && (ContourLineWidth != 0))
            {
                delaunator.ForEachTriangle(t =>
                {
                    // Find the minimum and maximum Z values of the triangle
                    //var zValues = t.Points.Select(p => p.Z);
                    double minZ = t.Points.Min(p => p.Z);
                    double maxZ = t.Points.Max(p => p.Z);

                    for (double z = Math.Floor(minZ / MinorInterval) * MinorInterval; z <= maxZ; z += MinorInterval)
                    {
                        List<Pixel> pts = new();
                        for (int i = 0; i < t.Points.Count(); i++)
                        {
                            // Find the points on the edge of the triangle at the current elevation.
                            // the third edge wraps around to the first point
                            Triangulation.Point? pt = FindPointAtElevation(t.Points.ElementAt(i), t.Points.ElementAt(i < 2 ? i + 1 : 0), z);

                            if (pt is not null)
                            {
                                pts.Add(Axes.GetPixel(new Coordinates(pt.Value.X, pt.Value.Y)));
                            }
                        }

                        if (pts.Count > 1)
                        {
                            // Use the index style if this is an index contour
                            Drawing.DrawLines(rp.Canvas, paint, pts, (MajorInterval != 0) && (z % MajorInterval == 0) ? IndexContourLineStyle : ContourLineStyle);
                        }
                    }
                });
            }
        }
        public virtual void Render(RenderPack rp)
        {
            IReadOnlyList<Coordinates3d> points = Data.GetTINPoints();

            if (points.Count == 0)
                return;

            // convert points to an array of DelaunatorSharp.Point objects and create a Delaunator object
            // TODO: there has to be a more efficient way to do this, but it works
            Triangulation.Point[] pts = points.Select(pt => (Triangulation.Point)new Triangulation.Point(pt.X, pt.Y, pt.Z)).ToArray();
            Triangulation.Delaunator delaunator = new(pts);
            //SmoothTINSurface(ref delaunator);
            using SKPaint paint = new();
            DrawTINLines(rp, paint, delaunator);
            DrawMarkers(rp, paint, points, MarkerStyle);
            DrawVoronoi(rp, paint, delaunator);
            DrawContours(rp, paint, delaunator);
        }
    }
}
