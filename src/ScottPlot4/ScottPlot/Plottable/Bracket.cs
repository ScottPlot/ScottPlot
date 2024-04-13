using ScottPlot.Drawing;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace ScottPlot.Plottable
{
    public class Bracket : IPlottable
    {
        /// <summary>
        /// Horizontal location (in pixel units) relative to the data area
        /// </summary>
        public double X1 { get; set; }

        /// <summary>
        /// Vertical position (in pixel units) relative to the data area
        /// </summary>
        public double Y1 { get; set; }

        /// <summary>
        /// Horizontal location (in pixel units) relative to the data area
        /// </summary>
        public double X2 { get; set; }

        /// <summary>
        /// Vertical position (in pixel units) relative to the data area
        /// </summary>
        public double Y2 { get; set; }

        /// <summary>
        /// Size of the small lines (in pixels) placed the edges of the bracket and between the center of the bracket and the label
        /// </summary>
        public float EdgeLength = 5;

        /// <summary>
        /// Text displayed in the annotation
        /// </summary>
        public string Label { get; set; }

        public readonly Drawing.Font Font = new();

        /// <summary>
        /// Color of the bracket lines and text
        /// </summary>
        public Color Color { get; set; } = Color.Black;

        /// <summary>
        /// Thickness (in pixels) of the lines
        /// </summary>
        public float LineWidth { get; set; } = 1;

        /// <summary>
        /// Controls whether the tip of the bracket is counter-clockwise from the line formed by the bracket base.
        /// </summary>
        public bool LabelCounterClockwise { get; set; } = false;

        public Bracket(double x1, double y1, double x2, double y2)
        {
            X1 = x1;
            Y1 = y1;
            X2 = x2;
            Y2 = y2;
        }

        public bool IsVisible { get; set; } = true;
        public int XAxisIndex { get; set; } = 0;
        public int YAxisIndex { get; set; } = 0;

        public AxisLimits GetAxisLimits()
        {
            return new(Math.Min(X1, X2), Math.Max(X1, X2), Math.Min(Y1, Y2), Math.Max(Y1, Y2));
        }

        private double AngleBetweenVectors(Vector2 reference, Vector2 v)
        {
            reference = Vector2.Normalize(reference);
            v = Vector2.Normalize(v);
            return Math.Acos(Vector2.Dot(reference, v));
        }

        public void Render(PlotDimensions dims, Bitmap bmp, bool lowQuality = false)
        {
            if (!IsVisible)
                return;

            using var gfx = GDI.Graphics(bmp, dims, lowQuality);
            using var pen = GDI.Pen(Color, LineWidth);
            using var brush = GDI.Brush(Color);
            using var font = GDI.Font(Font);

            var v = new Vector2((float)(X2 - X1), (float)(Y2 - Y1));
            var vPixel = new Vector2((float)(v.X * dims.PxPerUnitX), (float)(v.Y * dims.PxPerUnitY));
            var vDirectionVector = Vector2.Normalize(vPixel);

            if (v.X < 0 || (v.X == 0 && v.Y < 0)) // To prevent switching the order of the points from changing label position
            {
                vDirectionVector = Vector2.Negate(vDirectionVector);
            }

            Vector2 normal = Vector2.Normalize(new(vPixel.Y, vPixel.X));
            Vector2 antiNormal = Vector2.Negate(normal);

            var clockwiseNormalVector = AngleBetweenVectors(vDirectionVector, normal) > 0 ? normal : antiNormal;
            var counterClockwiseNormalVector = normal == clockwiseNormalVector ? antiNormal : normal;

            var edgeVector = LabelCounterClockwise ? counterClockwiseNormalVector : clockwiseNormalVector;

            var globalTranslation = edgeVector * EdgeLength;
            gfx.TranslateTransform(globalTranslation.X, globalTranslation.Y);

            var pxStart1 = dims.GetPixel(new(X1, Y1));
            var pxEnd1 = dims.GetPixel(new(X2, Y2));

            var bracketHeadTranslation = edgeVector * EdgeLength;

            var pxStart2 = pxStart1.WithTranslation(bracketHeadTranslation.X, bracketHeadTranslation.Y);
            var pxEnd2 = pxEnd1.WithTranslation(bracketHeadTranslation.X, bracketHeadTranslation.Y);

            gfx.DrawLine(pen, pxStart1.X, pxStart1.Y, pxStart2.X, pxStart2.Y);
            gfx.DrawLine(pen, pxEnd1.X, pxEnd1.Y, pxEnd2.X, pxEnd2.Y);
            gfx.DrawLine(pen, pxStart2.X, pxStart2.Y, pxEnd2.X, pxEnd2.Y);

            if (!string.IsNullOrWhiteSpace(Label))
            {
                // draw the "sub" line between center of bracket and center of base of label
                var halfVector = new Vector2((float)X1, (float)Y1) + 0.5f * v;

                Pixel stubPixel1 = dims.GetPixel(new(halfVector.X, halfVector.Y))
                    .WithTranslation(bracketHeadTranslation.X, bracketHeadTranslation.Y);

                Pixel stubPixel2 = stubPixel1
                    .WithTranslation(bracketHeadTranslation.X, bracketHeadTranslation.Y);

                gfx.DrawLine(pen, stubPixel1.X, stubPixel1.Y, stubPixel2.X, stubPixel2.Y);

                // draw label text
                gfx.TranslateTransform(stubPixel2.X, stubPixel2.Y);
                var angle = (float)(-Math.Atan2(v.Y * dims.PxPerUnitY, v.X * dims.PxPerUnitX) * 180 / Math.PI);
                if (angle < 0)
                    angle += 360;

                bool flippedText = false;
                if (angle > 90 && angle < 270)
                {
                    flippedText = true;
                    angle -= 180; // Keep the text upright
                }

                gfx.RotateTransform(angle);

                bool IsInverted = edgeVector == antiNormal;

                var labelHeight = gfx.MeasureString(Label, font).Height;
                gfx.TranslateTransform(0, labelHeight * ((IsInverted && !flippedText || !IsInverted && flippedText) ? 0 : 1));

                gfx.DrawString(Label, font, brush, 0, 0, new() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Far });
            }
        }

        public LegendItem[] GetLegendItems() => LegendItem.None;

        public void ValidateData(bool deep = false) { }
    }
}
