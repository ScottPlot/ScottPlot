using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using ScottPlot.Config;

namespace ScottPlot
{
    public class PlottablePolygons : Plottable
    {
        public List<List<(double x, double y)>> polys;
        public string label;

        public double lineWidth;
        System.Drawing.Color lineColor;
        public System.Drawing.Pen pen;
        public bool fill;
        System.Drawing.Color fillColor;
        public System.Drawing.Brush brush;

        private bool checkOnInit = true;
        private bool cutOffscreenPoly = true;
        private bool smallPolySinglePixel = true;

        public PlottablePolygons(List<List<(double x, double y)>> polys, string label,
            double lineWidth, System.Drawing.Color lineColor,
            bool fill, System.Drawing.Color fillColor, double fillAlpha)
        {
            if (polys is null)
                throw new ArgumentException("polys cannot be null");

            if (checkOnInit)
            {
                foreach (var poly in polys)
                {
                    if (poly.Count < 3)
                        throw new ArgumentException("polygons must contain at least 3 points");
                }
            }

            this.polys = polys;

            this.label = label;
            this.lineWidth = lineWidth;
            this.lineColor = lineColor;
            this.fill = fill;

            pen = new System.Drawing.Pen(lineColor, (float)lineWidth)
            {
                LineJoin = System.Drawing.Drawing2D.LineJoin.Round, // prevent sharp corners
            };
            this.fillColor = System.Drawing.Color.FromArgb((int)(255 * fillAlpha), fillColor.R, fillColor.G, fillColor.B);
            brush = new System.Drawing.SolidBrush(this.fillColor);
        }

        public override string ToString()
        {
            string label = string.IsNullOrWhiteSpace(this.label) ? "" : $" ({this.label})";
            return $"PlottablePolygons {label} with {GetPointCount()} polygons";
        }

        public override int GetPointCount()
        {
            return polys.Count;
        }

        public override AxisLimits2D GetLimits()
        {
            double xMin = polys[0][0].x;
            double xMax = polys[0][0].x;
            double yMin = polys[0][0].y;
            double yMax = polys[0][0].y;

            foreach (var poly in polys)
            {
                foreach (var point in poly)
                {
                    xMin = Math.Min(xMin, point.x);
                    xMax = Math.Max(xMax, point.x);
                    yMin = Math.Min(yMin, point.y);
                    yMax = Math.Max(yMax, point.y);
                }
            }

            return new AxisLimits2D(xMin, xMax, yMin, yMax);
        }

        public override LegendItem[] GetLegendItems()
        {
            if (fill)
                return new LegendItem[] { new LegendItem(label, fillColor, lineWidth: 10, markerShape: MarkerShape.none) };
            else
                return new LegendItem[] { new LegendItem(label, lineColor, lineWidth: lineWidth, markerShape: MarkerShape.none) };
        }

        private bool IsBiggerThenPixel(List<(double x, double y)> poly, double UnitsPerPixelX, double UnitsPerPixelY)
        {
            double minX = poly[0].x;
            double maxX = poly[0].x;
            double minY = poly[0].y;
            double maxY = poly[0].y;
            double smallerThenPixelX = 0.5 * UnitsPerPixelX;
            double smallerThenPixelY = 0.5 * UnitsPerPixelY;
            for (int i = 1; i < poly.Count; i++)
            {
                if (poly[i].x < minX)
                {
                    minX = poly[i].x;
                    if (maxX - minX > smallerThenPixelX)
                        return true;
                }
                if (poly[i].x > maxX)
                {
                    maxX = poly[i].x;
                    if (maxX - minX > smallerThenPixelX)
                        return true;
                }
                if (poly[i].y < minX)
                {
                    minY = poly[i].y;
                    if (maxY - minY > smallerThenPixelY)
                        return true;
                }
                if (poly[i].y > maxX)
                {
                    maxY = poly[i].y;
                    if (maxY - minY > smallerThenPixelY)
                        return true;
                }
            }
            return (maxX - minX > smallerThenPixelX || maxY - minY > smallerThenPixelY);
        }

        public override void Render(Settings settings)
        {
            IEnumerable<List<(double x, double y)>> polysToRender;

            if (cutOffscreenPoly)
            {
                polysToRender = polys.Where(poly => poly
                       .Any(p =>
                                 p.x >= settings.axes.limits[0]
                              && p.x <= settings.axes.limits[1]
                              && p.y >= settings.axes.limits[2]
                              && p.y <= settings.axes.limits[3]
                               ));
            }
            else
            {
                polysToRender = polys;
            }

            var plotPoints = polysToRender.Select(poly =>
            {
                if (!smallPolySinglePixel || IsBiggerThenPixel(poly, settings.xAxisUnitsPerPixel, settings.yAxisUnitsPerPixel))
                {
                    return poly.Select(point => settings.GetPixel(point.x, point.y));
                }
                else
                    return new PointF[] { settings.GetPixel(poly[0].x, poly[0].y) };
            });

            foreach (var poly in plotPoints)
            {
                var polyArray = poly.ToArray();
                if (fill)
                {
                    if (polyArray.Length >= 3)
                        settings.gfxData.FillPolygon(brush, polyArray);
                    else
                    {
                        if (polyArray[0].X >= 0 && polyArray[0].X < settings.bmpData.Width
                            && polyArray[0].Y >= 0 && polyArray[0].Y < settings.bmpData.Height)
                            settings.bmpData.SetPixel((int)polyArray[0].X, (int)polyArray[0].Y, fillColor);
                    }
                }
                if (lineWidth > 0)
                    settings.gfxData.DrawPolygon(pen, polyArray);
            }
        }
    }
}
