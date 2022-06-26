using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;

namespace ScottPlot
{
    public static class MarkerTools
    {
        public static void DrawMarker(Graphics gfx, PointF pixelLocation, MarkerShape shape, float size, Brush brush, Pen pen)
        {
            if (size == 0 || shape == MarkerShape.none)
                return;

            IMarker marker = Marker.Create(shape);
            DrawMarker(gfx, pixelLocation, marker, size, brush, pen);
        }

        public static void DrawMarker(Graphics gfx, PointF pixelLocation, IMarker marker, float size, Brush brush, Pen pen)
        {
            if (size == 0)
                return;

            float diameter = size;
            float radius = diameter / 2;

            /* Improve marker vs. line alignment on Linux and MacOS
             * https://github.com/ScottPlot/ScottPlot/issues/340
             * https://github.com/ScottPlot/ScottPlot/pull/1660
             */
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                pixelLocation = new PointF(pixelLocation.X + .5f, pixelLocation.Y);

            marker.Draw(gfx, pixelLocation, radius, brush, pen);
        }

        public static void DrawMarker(Graphics gfx, PointF pixelLocation, MarkerShape shape, float size, Color color, float linewidth = 1)
        {
            using Brush brush = new SolidBrush(color);
            using Pen pen = new(color, linewidth);
            DrawMarker(gfx, pixelLocation, shape, size, brush, pen);
        }

        public static void DrawMarkers(Graphics gfx, ICollection<PointF> pixelLocations, MarkerShape shape, float size, Color color, float linewidth = 1)
        {
            using SolidBrush brush = new(color);
            using Pen pen = new(color, linewidth);
            IMarker marker = Marker.Create(shape);

            foreach (PointF pt in pixelLocations)
                DrawMarker(gfx, pt, marker, size, brush, pen);
        }

        internal static PointF[] DiamondPoints(PointF center, float radius)
        {
            PointF[] points =
            {
            new PointF(center.X - radius, center.Y),
            new PointF(center.X, center.Y - radius),
            new PointF(center.X + radius, center.Y),
            new PointF(center.X, center.Y + radius),
        };

            return points;
        }

        internal static PointF[] DiamondPoints(RectangleF rect)
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

        internal static void DrawRadial(Graphics gfx, Pen pen, PointF center, PointF[] points)
        {
            foreach (PointF point in points)
            {
                gfx.DrawLine(pen, center, point);
            }
        }

        internal static PointF[] TriangleUpPoints(PointF center, float radius)
        {
            RectangleF rect = new(center.X - radius, center.Y - radius, radius * 2, radius * 2);
            return TriangleUpPoints(rect).Item1;
        }

        internal static PointF[] TriangleDownPoints(PointF center, float radius)
        {
            RectangleF rect = new(center.X - radius, center.Y - radius, radius * 2, radius * 2);
            return TriangleDownPoints(rect).Item1;
        }

        private static (PointF[], PointF) TriangleUpPoints(RectangleF rect)
        {
            float centerX = rect.Left + rect.Width / 2;
            float centerY = rect.Top + rect.Height / 2;
            float size = rect.Width / 2;

            PointF[] points =
            {
                new PointF(centerX, centerY - size ),
                new PointF(centerX - size * 0.866f, centerY + size/2),
                new PointF(centerX + size * 0.866f, centerY + size/2),
            };

            return (points, new PointF(centerX, centerY));
        }

        private static (PointF[], PointF) TriangleDownPoints(RectangleF rect)
        {
            float centerX = rect.Left + rect.Width / 2;
            float centerY = rect.Top + rect.Height / 2;
            float size = rect.Width / 2;

            PointF[] points =
            {
                new PointF(centerX, centerY + size ),
                new PointF(centerX - size * 0.866f, centerY - size /2),
                new PointF(centerX + size * 0.866f, centerY - size /2),
            };

            return (points, new PointF(centerX, centerY));
        }
    }
}
