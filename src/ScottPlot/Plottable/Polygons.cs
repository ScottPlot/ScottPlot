﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using ScottPlot.Ticks;
using ScottPlot.Drawing;
using ScottPlot.Renderable;

namespace ScottPlot.Plottable
{
    public class Polygons : IRenderable, IHasLegendItems, IUsesAxes, IValidatable
    {
        public readonly List<List<(double x, double y)>> polys;
        public string label;

        public double lineWidth;
        public Color lineColor;
        public bool fill;
        public Color fillColor;
        public double fillAlpha;
        public bool IsVisible { get; set; } = true;
        public int HorizontalAxisIndex { get; set; } = 0;
        public int VerticalAxisIndex { get; set; } = 0;

        public Color HatchColor = Color.Transparent;
        public HatchStyle HatchStyle = HatchStyle.None;

        public bool SkipOffScreenPolygons = true;
        public bool RenderSmallPolygonsAsSinglePixels = true;

        public Polygons(List<List<(double x, double y)>> polys)
        {
            this.polys = polys;
        }

        public override string ToString()
        {
            string label = string.IsNullOrWhiteSpace(this.label) ? "" : $" ({this.label})";
            return $"PlottablePolygons {label} with {PointCount} polygons";
        }

        public string ErrorMessage(bool deepValidation = false)
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
                            return e.ToString();
                        }
                    }
                }
            }

            return null;
        }

        public int PointCount { get => polys.Count; }

        public AxisLimits GetAxisLimits()
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

            return new AxisLimits(xMin, xMax, yMin, yMax);
        }

        public LegendItem[] LegendItems
        {
            get
            {
                var legendItem = new LegendItem()
                {
                    label = label,
                    color = fill ? fillColor : lineColor,
                    lineWidth = fill ? 10 : lineWidth,
                    markerShape = MarkerShape.none,
                    hatchColor = HatchColor,
                    hatchStyle = HatchStyle
                };
                return new LegendItem[] { legendItem };
            }
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

        public void Render(PlotDimensions dims, Bitmap bmp, bool lowQuality = false)
        {
            Color colorWithAlpha = Color.FromArgb((byte)(255 * fillAlpha), fillColor);
            using (Graphics gfx = GDI.Graphics(bmp, dims, lowQuality))
            using (Brush brush = GDI.Brush(fillColor, HatchColor, HatchStyle))
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
