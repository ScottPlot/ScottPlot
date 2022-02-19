using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;

namespace ScottPlot
{
    public enum MarkerShape
    {
        // TODO: replace this with ScottPlot.Marker in the next major version of ScottPlot
        none,
        filledCircle,
        filledSquare,
        openCircle,
        openSquare,
        filledDiamond,
        openDiamond,
        asterisk,
        hashTag,
        cross,
        eks,
        verticalBar,
        triUp,
        triDown,
    }

    public class MarkerTools
    {
        public static void DrawMarker(Graphics gfx, PointF pixelLocation, MarkerShape shape, float size, Color color)
        {
            if (size == 0 || shape == MarkerShape.none)
                return;

            // Improve marker vs. line alignment on Linux and MacOS
            // https://github.com/ScottPlot/ScottPlot/issues/340
            // https://github.com/ScottPlot/ScottPlot/pull/1660
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                pixelLocation = new PointF(pixelLocation.X + .5f, pixelLocation.Y);
            }

            RectangleF rect = new(x: pixelLocation.X - size / 2, y: pixelLocation.Y - size / 2, width: size, height: size);

            // NOTE: refactored in Feb 2022 to make it easier to convert this to individual marker classes soon
            switch (shape)
            {
                case MarkerShape.filledCircle:
                    DrawFilledCircle(gfx, rect, color);
                    break;
                case MarkerShape.openCircle:
                    DrawOpenCircle(gfx, rect, color);
                    break;
                case MarkerShape.filledSquare:
                    DrawFilledSquare(gfx, rect, color);
                    break;
                case MarkerShape.openSquare:
                    DrawOpenSquare(gfx, rect, color);
                    break;
                case MarkerShape.filledDiamond:
                    DrawFilledDiamond(gfx, rect, color);
                    break;
                case MarkerShape.openDiamond:
                    DrawOpenDiamond(gfx, rect, color);
                    break;
                case MarkerShape.asterisk:
                    DrawString(gfx, rect, color, "*");
                    break;
                case MarkerShape.hashTag:
                    DrawString(gfx, rect, color, "#");
                    break;
                case MarkerShape.cross:
                    DrawString(gfx, rect, color, "+");
                    break;
                case MarkerShape.eks:
                    DrawString(gfx, rect, color, "x");
                    break;
                case MarkerShape.verticalBar:
                    DrawString(gfx, rect, color, "|");
                    break;
                case MarkerShape.triUp:
                    DrawTriStarUp(gfx, rect, color);
                    break;
                case MarkerShape.triDown:
                    DrawTriStarDown(gfx, rect, color);
                    break;
                case MarkerShape.none:
                    break;
                default:
                    throw new NotImplementedException($"unsupported marker type: {shape}");
            }
        }

        public static void DrawMarkers(Graphics gfx, ICollection<PointF> pixelLocations, MarkerShape shape, float size, Color color)
        {
            foreach (PointF pt in pixelLocations)
            {
                DrawMarker(gfx, pt, shape, size, color);
            }
        }

        private static void DrawFilledCircle(Graphics gfx, RectangleF rect, Color color)
        {
            using Brush brush = new SolidBrush(color);
            gfx.FillEllipse(brush, rect);
        }

        private static void DrawOpenCircle(Graphics gfx, RectangleF rect, Color color)
        {
            using Pen pen = new(color);
            gfx.DrawEllipse(pen, rect);
        }

        private static void DrawFilledSquare(Graphics gfx, RectangleF rect, Color color)
        {
            using Brush brush = new SolidBrush(color);
            gfx.FillRectangle(brush, rect);
        }

        private static void DrawOpenSquare(Graphics gfx, RectangleF rect, Color color)
        {
            using Pen pen = new(color);
            gfx.DrawRectangle(pen, rect.Left, rect.Top, rect.Width, rect.Height);
        }

        private static PointF[] DiamondPoints(RectangleF rect)
        {
            float centerX = rect.Left + rect.Width / 2;
            float centerY = rect.Top + rect.Height / 2;

            PointF[] points =
            {
                new PointF(rect.Left, centerY),
                new PointF(centerX, rect.Top),
                new PointF(rect.Right, centerY),
                new PointF(centerX, rect.Bottom),
            };

            return points;
        }

        private static void DrawFilledDiamond(Graphics gfx, RectangleF rect, Color color)
        {
            PointF[] points = DiamondPoints(rect);
            using Brush brush = new SolidBrush(color);
            gfx.FillPolygon(brush, points);
        }

        private static void DrawOpenDiamond(Graphics gfx, RectangleF rect, Color color)
        {
            PointF[] points = DiamondPoints(rect);
            using Pen pen = new(color);
            gfx.DrawPolygon(pen, points);
        }

        private static void DrawString(Graphics gfx, RectangleF rect, Color color, string str)
        {
            float centerX = rect.Left + rect.Width / 2;
            float centerY = rect.Top + rect.Height / 2;

            string fontName = Drawing.InstalledFont.Monospace();
            Font font = new(fontName, rect.Width * 2);

            using Brush brush = new SolidBrush(color);
            var sf = Drawing.GDI.StringFormat(HorizontalAlignment.Center, VerticalAlignment.Middle);
            gfx.DrawString(str, font, brush, centerX, centerY, sf);
        }

        private static void DrawTriStarUp(Graphics gfx, RectangleF rect, Color color)
        {
            float centerX = rect.Left + rect.Width / 2;
            float centerY = rect.Top + rect.Height / 2;
            float size = rect.Width;

            using Pen pen = new(color);
            gfx.DrawLine(pen, centerX, centerY, centerX, centerY - size);
            gfx.DrawLine(pen, centerX, centerY, centerX - size * (float)0.866, centerY + size * (float).5);
            gfx.DrawLine(pen, centerX, centerY, centerX + size * (float)0.866, centerY + size * (float).5);
        }

        private static void DrawTriStarDown(Graphics gfx, RectangleF rect, Color color)
        {
            float centerX = rect.Left + rect.Width / 2;
            float centerY = rect.Top + rect.Height / 2;
            float size = rect.Width;

            using Pen pen = new(color);
            gfx.DrawLine(pen, centerX, centerY, centerX, centerY + size);
            gfx.DrawLine(pen, centerX, centerY, centerX - size * (float)0.866, centerY - size * (float).5);
            gfx.DrawLine(pen, centerX, centerY, centerX + size * (float)0.866, centerY - size * (float).5);
        }
    }
}
