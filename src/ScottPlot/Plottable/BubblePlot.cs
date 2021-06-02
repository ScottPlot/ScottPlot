﻿using ScottPlot.Drawing;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Collections.Generic;
using System.Linq;

namespace ScottPlot.Plottable
{
    /// <summary>
    /// Display circles of user-defined sizes and colors at specific X/Y positions
    /// </summary>
    public class BubblePlot : IPlottable
    {
        private struct Bubble
        {
            public double X;
            public double Y;
            public float Radius;
            public Color FillColor;
            public float EdgeWidth;
            public Color EdgeColor;
        }

        private readonly List<Bubble> Bubbles = new();

        public bool IsVisible { get; set; } = true;
        public int XAxisIndex { get; set; } = 0;
        public int YAxisIndex { get; set; } = 0;
        public override string ToString() => $"BubblePlot with {Bubbles.Count} bubbles";

        /// <summary>
        /// Clear all bubbles
        /// </summary>
        public void Clear() => Bubbles.Clear();

        /// <summary>
        /// Add a single bubble
        /// </summary>
        /// <param name="x">horizontal position (in coordinate space)</param>
        /// <param name="y">horizontal vertical (in coordinate space)</param>
        /// <param name="radius">size of the bubble (in pixels)</param>
        /// <param name="fillColor"></param>
        /// <param name="edgeWidth">size of the outline (in pixels)</param>
        /// <param name="edgeColor"></param>
        public void Add(double x, double y, double radius, Color fillColor, double edgeWidth, Color edgeColor)
        {
            Bubbles.Add(new Bubble()
            {
                X = x,
                Y = y,
                Radius = (float)radius,
                FillColor = fillColor,
                EdgeWidth = (float)edgeWidth,
                EdgeColor = edgeColor
            });
        }

        /// <summary>
        /// Add many bubbles with the same size and style
        /// </summary>
        public void Add(double[] xs, double[] ys, double size, Color fillColor, double edgeWidth, Color edgeColor)
        {
            if (xs is null || ys is null)
                throw new ArgumentException("xs and ys cannot be null");

            if (xs.Length != ys.Length)
                throw new ArgumentException("xs and ys must have the same number of elements");

            for (int i = 0; i < xs.Length; i++)
            {
                Bubbles.Add(new Bubble()
                {
                    X = xs[i],
                    Y = ys[i],
                    Radius = (float)size,
                    FillColor = fillColor,
                    EdgeWidth = (float)edgeWidth,
                    EdgeColor = edgeColor
                });
            }
        }

        public LegendItem[] GetLegendItems() => null; // does not appear in legend

        public AxisLimits GetAxisLimits()
        {
            if (Bubbles.Count == 0)
                return new AxisLimits(double.NaN, double.NaN, double.NaN, double.NaN);

            var xs = Bubbles.Select(b => b.X);
            var ys = Bubbles.Select(b => b.Y);
            return new AxisLimits(xs.Min(), xs.Max(), ys.Min(), ys.Max());
        }

        public void ValidateData(bool deep = false)
        {
            foreach (Bubble bubble in Bubbles)
            {
                if (double.IsNaN(bubble.X) || double.IsNaN(bubble.Y) || double.IsNaN(bubble.Radius) || double.IsNaN(bubble.EdgeWidth))
                    throw new InvalidOperationException("Bubble positions and sizes must not be NaN");

                if (double.IsInfinity(bubble.X) || double.IsInfinity(bubble.Y) || double.IsInfinity(bubble.Radius) || double.IsInfinity(bubble.EdgeWidth))
                    throw new InvalidOperationException("Bubble position and size must real");

                if (bubble.Radius < 0 || bubble.EdgeWidth < 0)
                    throw new InvalidOperationException("Bubble sizes cannot be negative");
            }
        }

        public void Render(PlotDimensions dims, Bitmap bmp, bool lowQuality = false)
        {
            if (IsVisible == false)
                return;

            using Graphics gfx = GDI.Graphics(bmp, dims, lowQuality);
            using Brush brush = GDI.Brush(Color.Magenta);
            using Pen pen = GDI.Pen(Color.Black);

            foreach (Bubble bubble in Bubbles)
            {
                float pixelX = dims.GetPixelX(bubble.X);
                float pixelY = dims.GetPixelY(bubble.Y);
                float radiusPx = (float)bubble.Radius;

                PointF location = new(pixelX - radiusPx, pixelY - radiusPx);
                SizeF size = new(radiusPx * 2, radiusPx * 2);
                RectangleF rect = new(location, size);

                ((SolidBrush)brush).Color = bubble.FillColor;
                gfx.FillEllipse(brush, rect);

                pen.Color = bubble.EdgeColor;
                pen.Width = bubble.EdgeWidth;
                gfx.DrawEllipse(pen, rect);
            }
        }
    }
}
