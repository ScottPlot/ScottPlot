using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using ScottPlot.Config;
using ScottPlot.Drawing;

namespace ScottPlot
{
    public class PlottablePolygons : Plottable, IPlottable
    {
        public readonly List<List<(double x, double y)>> polys;
        public string label;

        public double lineWidth;
        public Color lineColor;
        public bool fill;
        public Color fillColor;
        public double fillAlpha;

        public bool SkipOffScreenPolygons = true;
        public bool RenderSmallPolygonsAsSinglePixels = true;

        public PlottablePolygons(List<List<(double x, double y)>> polys)
        {
            this.polys = polys;
        }

        public override string ToString()
        {
            string label = string.IsNullOrWhiteSpace(this.label) ? "" : $" ({this.label})";
            return $"PlottablePolygons {label} with {GetPointCount()} polygons";
        }

        public string ValidationErrorMessage { get; private set; }
        public bool IsValidData(bool deepValidation = false)
        {
            if (deepValidation)
            {
                foreach (var poly in polys)
                {
                    foreach (var point in poly)
                    {
                        try
                        {
                            Validate.AssertIsReal("x", point.x);
                            Validate.AssertIsReal("y", point.y);
                        }
                        catch (ArgumentException e)
                        {
                            ValidationErrorMessage = e.ToString();
                            return false;
                        }
                    }
                }
            }

            ValidationErrorMessage = null;
            return true;
        }

        public override int GetPointCount() => polys.Count;

        public override AxisLimits2D GetLimits()
        {
            double xMin = polys[0][0].x;
            double xMax = polys[0][0].x;
            double yMin = polys[0][0].y;
            double yMax = polys[0][0].y;

            foreach (var poly in polys)
            {
                foreach (var (x, y) in poly)
                {
                    xMin = Math.Min(xMin, x);
                    xMax = Math.Max(xMax, x);
                    yMin = Math.Min(yMin, y);
                    yMax = Math.Max(yMax, y);
                }
            }

            return new AxisLimits2D(xMin, xMax, yMin, yMax);
        }

        public override LegendItem[] GetLegendItems() =>
            new LegendItem[] {
                new LegendItem(
                    label: label,
                    color: fill ? fillColor : lineColor,
                    lineWidth: fill ? 10 : lineWidth,
                    markerShape: MarkerShape.none
                )
            };

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

        public override void Render(Settings settings) => throw new InvalidOperationException("use new Render");

        public void Render(PlotDimensions dims, Bitmap bmp, bool lowQuality = false)
        {
            using (Graphics gfx = GDI.Graphics(bmp, lowQuality))
            using (Brush brush = GDI.Brush(fillColor, fillAlpha))
            using (Pen pen = GDI.Pen(lineColor, lineWidth))
            {
                foreach (List<(double x, double y)> poly in polys)
                {
                    if (SkipOffScreenPolygons &&
                        poly.Where(pt => pt.x >= dims.XMin && pt.x <= dims.XMax &&
                                         pt.y >= dims.YMin && pt.y <= dims.YMax)
                            .Count() == 0)
                        continue;

                    var polyArray = RenderSmallPolygonsAsSinglePixels && !IsBiggerThenPixel(poly, dims.UnitsPerPxX, dims.UnitsPerPxY) ?
                        new PointF[] { new PointF(dims.GetPixelX(poly[0].x), dims.GetPixelY(poly[0].y)) } :
                        poly.Select(point => new PointF(dims.GetPixelX(point.x), dims.GetPixelY(point.y))).ToArray();

                    if (fill)
                    {
                        if (polyArray.Length >= 3)
                            gfx.FillPolygon(brush, polyArray);
                        else
                            gfx.FillRectangle(brush, polyArray[0].X, polyArray[0].Y, 1, 1);
                    }

                    if (lineWidth > 0)
                        gfx.DrawPolygon(pen, polyArray);
                }
            }
        }
    }
}
