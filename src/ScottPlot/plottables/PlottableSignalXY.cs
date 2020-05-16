using ScottPlot.Config;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;

namespace ScottPlot
{
    public class PlottableSignalXY : PlottableSignal
    {
        public double[] xs;
        public PlottableSignalXY(double[] xs, double[] ys, Color color, double lineWidth, double markerSize, string label, bool useParallel, int maxRenderIndex)
            : base(ys, 1, 0, 0, color, lineWidth, markerSize, label, useParallel, null, maxRenderIndex)
        {
            if ((xs == null) || (ys == null))
                throw new ArgumentException("X and Y data cannot be null");

            if ((xs.Length == 0) || (ys.Length == 0))
                throw new ArgumentException("xs and ys must have at least one element");

            if (xs.Length != ys.Length)
                throw new ArgumentException("Xs and Ys must have same length");

            // TODO: should XS be scanned here to ensure they are all in ascending order?

            this.xs = xs;
        }

        public override AxisLimits2D GetLimits()
        {
            var limits = base.GetLimits();
            limits.SetX(xs[0], xs[xs.Length - 1]);
            return limits;
        }

        public class IntervalValues
        {
            public double yStart;
            public double yEnd;
            public double yMin;
            public double yMax;
            public int pointsCount;
            public int pixelIndex;
            public int distanceToLeftNeighbor;
            public int distanceToRightNeighbor;
        }

        public override void Render(Settings settings)
        {
            pen.Color = color;
            pen.Width = (float)lineWidth;
            brush = new SolidBrush(color);

            // x locations of pixels borders
            var xBorders = Enumerable.Range(0, settings.dataSize.Width + 1)
                .Select(x => settings.axes.x.min + settings.axes.x.span / settings.dataSize.Width * x)
                .ToArray();

            int? PointBeforeDisplayedIndex = null;
            int? PointAfterDisplayedIndex = null;

            int CurrentIndex = 0;
            int[] xBordersIndexes = new int[settings.dataSize.Width + 1]; // indexes of points close to pixels borders
            for (int i = 0; i < settings.dataSize.Width + 1; i++)
            {
                for (; CurrentIndex < xs.Length && xs[CurrentIndex] < xBorders[i]; CurrentIndex++)
                    ;

                if (PointBeforeDisplayedIndex == null) // point index before first pixel column
                    PointBeforeDisplayedIndex = CurrentIndex - 1;
                xBordersIndexes[i] = CurrentIndex;
            }

            PointAfterDisplayedIndex = CurrentIndex;

            IntervalValues[] yParams = CalcIntervalParams(xBordersIndexes, settings);
            List<PointF> PointsToDraw = GetPointsToDraw(yParams, PointBeforeDisplayedIndex, PointAfterDisplayedIndex, settings);

            // Draw lines
            if (PointsToDraw.Count > 1)
                settings.gfxData.DrawLines(pen, PointsToDraw.ToArray());
            else if (xs[0] > xBorders[0] && xs[xs.Length - 1] < xBorders[xBorders.Length - 1])
                settings.gfxData.DrawLine(pen,
                    settings.GetPixel(xs[0], ys.Min()),
                    settings.GetPixel(xs[0], ys.Max()));

            FindNeigborDistances(yParams);

            // draw markers
            foreach (var pixelInterval in yParams)
            {
                if (pixelInterval?.pointsCount == 1) // single point in pixel
                {
                    // draw marker only then has free space to left and right
                    // TODO smooth animation can be implemented like in Signal
                    if (markerSize > 0)
                    {
                        float pixelsBetweenPoints = Math.Min(pixelInterval.distanceToLeftNeighbor, pixelInterval.distanceToRightNeighbor);
                        float zoomTransitionScale = Math.Min(1, pixelsBetweenPoints / 10);
                        float markerPxDiameter = markerSize * zoomTransitionScale;
                        float markerPxRadius = markerPxDiameter / 2;
                        if (markerPxRadius > .25)
                        {
                            // adjust marker offset to improve rendering on Linux and MacOS
                            float markerOffsetX = (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) ? 0 : 1;

                            settings.gfxData.FillEllipse(brush: brush,
                                  x: pixelInterval.pixelIndex - markerPxRadius + markerOffsetX,
                                  y: (int)settings.GetPixelY(pixelInterval.yStart) - markerPxRadius,
                                    width: markerPxDiameter, height: markerPxDiameter);
                        };
                    }
                }
            }
        }

        private IntervalValues[] CalcIntervalParams(int[] xBordersIndexes, Settings settings)
        {
            IntervalValues[] yParams = new IntervalValues[settings.dataSize.Width];
            for (int i = 0; i < settings.dataSize.Width; i++)
            {
                if (xBordersIndexes[i] == xBordersIndexes[i + 1])
                {
                    yParams[i] = null;
                    continue;
                }

                double min = ys[xBordersIndexes[i]];
                double max = ys[xBordersIndexes[i]];
                for (int j = xBordersIndexes[i]; j < xBordersIndexes[i + 1]; j++)
                {
                    if (ys[j] < min)
                        min = ys[j];
                    if (ys[j] > max)
                        max = ys[j];
                }

                yParams[i] = new IntervalValues()
                {
                    yStart = ys[xBordersIndexes[i]],
                    yEnd = ys[xBordersIndexes[i + 1] - 1],
                    yMin = min,
                    yMax = max,
                    pointsCount = xBordersIndexes[i + 1] - xBordersIndexes[i],
                    pixelIndex = i,
                };
            }

            return yParams;
        }

        private List<PointF> GetPointsToDraw(IntervalValues[] yParams, int? PointBeforeDisplayedIndex, int? PointAfterDisplayedIndex, Settings settings)
        {
            List<PointF> PointsToDraw = new List<PointF>();

            if (PointBeforeDisplayedIndex >= 0)
            {
                PointsToDraw.Add(settings.GetPixel(xs[PointBeforeDisplayedIndex.Value], ys[PointBeforeDisplayedIndex.Value]));
            }

            for (int i = 0; i < settings.dataSize.Width; i++)
            {
                if (yParams[i] == null)
                    continue;

                PointsToDraw.Add(new PointF(yParams[i].pixelIndex, (float)settings.GetPixelY(yParams[i].yStart)));
                if (yParams[i].pointsCount > 1)
                {
                    PointsToDraw.Add(new PointF(yParams[i].pixelIndex, (float)settings.GetPixelY(yParams[i].yMin)));
                    PointsToDraw.Add(new PointF(yParams[i].pixelIndex, (float)settings.GetPixelY(yParams[i].yMax)));
                    PointsToDraw.Add(new PointF(yParams[i].pixelIndex, (float)settings.GetPixelY(yParams[i].yEnd)));
                }
            }

            if (PointAfterDisplayedIndex < xs.Length && PointsToDraw.Count >= 1)
            {
                PointF lastPoint = PointsToDraw[PointsToDraw.Count - 1];
                PointF afterPoint = settings.GetPixel(xs[PointAfterDisplayedIndex.Value], ys[PointAfterDisplayedIndex.Value]);

                float x1 = settings.dataSize.Width;
                float y1 = lastPoint.Y + (afterPoint.Y - lastPoint.Y) * (x1 - lastPoint.X) / (afterPoint.X - lastPoint.X);
                PointsToDraw.Add(new PointF(x1, y1));
            }

            if (PointBeforeDisplayedIndex >= 0 && PointsToDraw.Count >= 2)
            {
                float x0 = -1;
                float y0 = PointsToDraw[1].Y + (PointsToDraw[0].Y - PointsToDraw[1].Y) * (x0 - PointsToDraw[1].X) / (PointsToDraw[0].X - PointsToDraw[1].X);
                PointsToDraw[0] = new PointF(x0, y0);
            }

            return PointsToDraw;
        }

        private static void FindNeigborDistances(IntervalValues[] yParams)
        {
            const int distanceOnEdges = 100; // using Int.Max can produce overflow

            int Counter = distanceOnEdges;
            for (int i = 0; i < yParams.Length; i++)
            {
                if (yParams[i] == null)
                    Counter++;
                else
                {
                    yParams[i].distanceToLeftNeighbor = Counter;
                    Counter = 1;
                }
            }
            Counter = distanceOnEdges;
            for (int i = yParams.Length - 1; i > 0; i--)
            {
                if (yParams[i] == null)
                    Counter++;
                else
                {
                    yParams[i].distanceToRightNeighbor = Counter;
                    Counter = 1;
                }
            }
        }
    }
}
