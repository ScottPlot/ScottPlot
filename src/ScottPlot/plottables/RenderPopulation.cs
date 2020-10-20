using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using ScottPlot.Drawing;
using ScottPlot.Statistics;

namespace ScottPlot
{
    public static class RenderPopulation
    {
        public enum Position { Hide, Center, Left, Right }

        public static void Scatter(PlotDimensions dims, Bitmap bmp, bool lowQuality, Population pop, Random rand,
            double popLeft, double popWidth, Color fillColor, Color edgeColor, byte alpha, Position position)
        {
            using (Graphics gfx = GDI.Graphics(bmp))
            {
                // adjust edges to accomodate special positions
                if (position == Position.Hide) return;
                if (position == Position.Left || position == Position.Right) popWidth /= 2;
                if (position == Position.Right) popLeft += popWidth;

                // contract edges slightly to encourage padding between elements
                double edgePaddingFrac = 0.2;
                popLeft += popWidth * edgePaddingFrac;
                popWidth -= (popWidth * edgePaddingFrac) * 2;

                edgeColor = Color.FromArgb(alpha, edgeColor.R, edgeColor.G, edgeColor.B);
                fillColor = Color.FromArgb(alpha, fillColor.R, fillColor.G, fillColor.B);

                Pen penEdge = new Pen(edgeColor);
                Brush brushFill = new SolidBrush(fillColor);
                float radius = 5;

                foreach (double value in pop.values)
                {
                    double yPx = dims.GetPixelY(value);
                    double xPx = dims.GetPixelX(popLeft + rand.NextDouble() * popWidth);
                    gfx.FillEllipse(brushFill, (float)(xPx - radius), (float)(yPx - radius), radius * 2, radius * 2);
                    gfx.DrawEllipse(penEdge, (float)(xPx - radius), (float)(yPx - radius), radius * 2, radius * 2);
                }
            }
        }

        public static void Distribution(PlotDimensions dims, Bitmap bmp, bool lowQuality, Population pop, Random rand,
            double popLeft, double popWidth, Color color, Position position, LineStyle lineStyle)
        {
            using (Graphics gfx = GDI.Graphics(bmp))
            {
                // adjust edges to accomodate special positions
                if (position == Position.Hide) return;
                if (position == Position.Left || position == Position.Right) popWidth /= 2;
                if (position == Position.Right) popLeft += popWidth;

                // contract edges slightly to encourage padding between elements
                double edgePaddingFrac = 0.2;
                popLeft += popWidth * edgePaddingFrac;
                popWidth -= (popWidth * edgePaddingFrac) * 2;

                Pen pen = GDI.Pen(color, 1, lineStyle, true);

                double[] ys = DataGen.Range(pop.minus3stDev, pop.plus3stDev, dims.UnitsPerPxY);
                double[] ysFrac = pop.GetDistribution(ys);

                PointF[] points = new PointF[ys.Length];
                for (int i = 0; i < ys.Length; i++)
                {
                    float x = (float)dims.GetPixelX(popLeft + popWidth * ysFrac[i]);
                    float y = (float)dims.GetPixelY(ys[i]);
                    points[i] = new PointF(x, y);
                }

                if (points.Length > 1)
                    gfx.DrawLines(pen, points);
            }
        }

        public static void MeanAndError(PlotDimensions dims, Bitmap bmp, bool lowQuality, Population pop, Random rand,
            double popLeft, double popWidth, Color color, Position position, bool useStdErr = false)
        {

            using (Graphics gfx = GDI.Graphics(bmp))
            {
                // adjust edges to accomodate special positions
                if (position == Position.Hide) return;
                if (position == Position.Left || position == Position.Right) popWidth /= 2;
                if (position == Position.Right) popLeft += popWidth;

                // determine the center point and calculate bounds
                double centerX = popLeft + popWidth / 2;
                double xPx = dims.GetPixelX(centerX);
                double yPx = dims.GetPixelY(pop.mean);

                double errorMaxPx, errorMinPx;
                if (useStdErr)
                {
                    errorMaxPx = dims.GetPixelY(pop.mean + pop.stdErr);
                    errorMinPx = dims.GetPixelY(pop.mean - pop.stdErr);
                }
                else
                {
                    errorMaxPx = dims.GetPixelY(pop.mean + pop.stDev);
                    errorMinPx = dims.GetPixelY(pop.mean - pop.stDev);
                }

                // make cap width a fraction of available space
                double capWidthFrac = .38;
                double capWidth = popWidth * capWidthFrac;
                double capPx1 = dims.GetPixelX(centerX - capWidth / 2);
                double capPx2 = dims.GetPixelX(centerX + capWidth / 2);

                Pen pen = new Pen(color, 2);
                Brush brush = new SolidBrush(color);
                float radius = 5;

                gfx.FillEllipse(brush, (float)(xPx - radius), (float)(yPx - radius), radius * 2, radius * 2);
                gfx.DrawLine(pen, (float)xPx, (float)errorMinPx, (float)xPx, (float)errorMaxPx);
                gfx.DrawLine(pen, (float)capPx1, (float)errorMinPx, (float)capPx2, (float)errorMinPx);
                gfx.DrawLine(pen, (float)capPx1, (float)errorMaxPx, (float)capPx2, (float)errorMaxPx);
            }
        }

        public static void Bar(PlotDimensions dims, Bitmap bmp, bool lowQuality, Population pop, Random rand,
            double popLeft, double popWidth, Color color, Position position, bool useStdErr = false)
        {
            using (Graphics gfx = GDI.Graphics(bmp))
            {
                // adjust edges to accomodate special positions
                if (position == Position.Hide) return;
                if (position == Position.Left || position == Position.Right) popWidth /= 2;
                if (position == Position.Right) popLeft += popWidth;

                // determine the center point and calculate bounds
                double centerX = popLeft + popWidth / 2;
                double xPx = dims.GetPixelX(centerX);
                double yPxTop = dims.GetPixelY(pop.mean);
                double yPxBase = dims.GetPixelY(0);

                double errorMaxPx, errorMinPx;
                if (useStdErr)
                {
                    errorMaxPx = dims.GetPixelY(pop.mean + pop.stdErr);
                    errorMinPx = dims.GetPixelY(pop.mean - pop.stdErr);
                }
                else
                {
                    errorMaxPx = dims.GetPixelY(pop.mean + pop.stDev);
                    errorMinPx = dims.GetPixelY(pop.mean - pop.stDev);
                }

                // make cap width a fraction of available space
                double capWidthFrac = .38;
                double capWidth = popWidth * capWidthFrac;
                double capPx1 = dims.GetPixelX(centerX - capWidth / 2);
                double capPx2 = dims.GetPixelX(centerX + capWidth / 2);

                // contract edges slightly to encourage padding between elements
                double edgePaddingFrac = 0.2;
                popLeft += popWidth * edgePaddingFrac;
                popWidth -= (popWidth * edgePaddingFrac) * 2;
                double leftPx = dims.GetPixelX(popLeft);
                double rightPx = dims.GetPixelX(popLeft + popWidth);

                Pen pen = new Pen(Color.Black, 1);
                Brush brush = new SolidBrush(color);

                RectangleF rect = new RectangleF((float)leftPx, (float)yPxTop, (float)(rightPx - leftPx), (float)(yPxBase - yPxTop));
                gfx.FillRectangle(brush, rect.X, rect.Y, rect.Width, rect.Height);
                gfx.DrawRectangle(pen, rect.X, rect.Y, rect.Width, rect.Height);
                gfx.DrawLine(pen, (float)xPx, (float)errorMinPx, (float)xPx, (float)errorMaxPx);
                gfx.DrawLine(pen, (float)capPx1, (float)errorMinPx, (float)capPx2, (float)errorMinPx);
                gfx.DrawLine(pen, (float)capPx1, (float)errorMaxPx, (float)capPx2, (float)errorMaxPx);
            }
        }

        public enum BoxFormat { StdevStderrMean, OutlierQuartileMedian }
        public enum HorizontalAlignment { Left, Center, Right }

        public static void Box(PlotDimensions dims, Bitmap bmp, bool lowQuality, Population pop, Random rand,
            double popLeft, double popWidth, Color color, Position position, BoxFormat boxFormat, HorizontalAlignment errorAlignment = HorizontalAlignment.Right)
        {
            using (Graphics gfx = GDI.Graphics(bmp))
            {
                // adjust edges to accomodate special positions
                if (position == Position.Hide) return;
                if (position == Position.Left || position == Position.Right) popWidth /= 2;
                if (position == Position.Right) popLeft += popWidth;

                double errorMaxPx, errorMinPx;
                double yPxTop, yPxBase;
                double yPx;
                if (boxFormat == BoxFormat.StdevStderrMean)
                {
                    errorMaxPx = dims.GetPixelY(pop.mean + pop.stDev);
                    errorMinPx = dims.GetPixelY(pop.mean - pop.stDev);
                    yPxTop = dims.GetPixelY(pop.mean + pop.stdErr);
                    yPxBase = dims.GetPixelY(pop.mean - pop.stdErr);
                    yPx = dims.GetPixelY(pop.mean);
                }
                else if (boxFormat == BoxFormat.OutlierQuartileMedian)
                {
                    errorMaxPx = dims.GetPixelY(pop.maxNonOutlier);
                    errorMinPx = dims.GetPixelY(pop.minNonOutlier);
                    yPxTop = dims.GetPixelY(pop.Q3);
                    yPxBase = dims.GetPixelY(pop.Q1);
                    yPx = dims.GetPixelY(pop.median);
                }
                else
                {
                    throw new NotImplementedException();
                }

                // make cap width a fraction of available space
                double capWidthFrac = .38;
                double capWidth = popWidth * capWidthFrac;

                // contract edges slightly to encourage padding between elements
                double edgePaddingFrac = 0.2;
                popLeft += popWidth * edgePaddingFrac;
                popWidth -= (popWidth * edgePaddingFrac) * 2;
                double leftPx = dims.GetPixelX(popLeft);
                double rightPx = dims.GetPixelX(popLeft + popWidth);

                Pen pen = new Pen(Color.Black, 1);
                Brush brush = new SolidBrush(color);

                // draw the box
                RectangleF rect = new RectangleF((float)leftPx, (float)yPxTop, (float)(rightPx - leftPx), (float)(yPxBase - yPxTop));
                gfx.FillRectangle(brush, rect.X, rect.Y, rect.Width, rect.Height);
                gfx.DrawRectangle(pen, rect.X, rect.Y, rect.Width, rect.Height);

                // draw the line in the center
                gfx.DrawLine(pen, rect.X, (float)yPx, rect.X + rect.Width, (float)yPx);

                // determine location of errorbars and caps
                double capPx1, capPx2, errorPxX;
                switch (errorAlignment)
                {
                    case HorizontalAlignment.Center:
                        double centerX = popLeft + popWidth / 2;
                        errorPxX = dims.GetPixelX(centerX);
                        capPx1 = dims.GetPixelX(centerX - capWidth / 2);
                        capPx2 = dims.GetPixelX(centerX + capWidth / 2);
                        break;
                    case HorizontalAlignment.Right:
                        errorPxX = dims.GetPixelX(popLeft + popWidth);
                        capPx1 = dims.GetPixelX(popLeft + popWidth - capWidth / 2);
                        capPx2 = dims.GetPixelX(popLeft + popWidth);
                        break;
                    case HorizontalAlignment.Left:
                        errorPxX = dims.GetPixelX(popLeft);
                        capPx1 = dims.GetPixelX(popLeft);
                        capPx2 = dims.GetPixelX(popLeft + capWidth / 2);
                        break;
                    default:
                        throw new NotImplementedException();
                }

                // draw errorbars and caps
                gfx.DrawLine(pen, (float)errorPxX, (float)errorMinPx, (float)errorPxX, rect.Y + rect.Height);
                gfx.DrawLine(pen, (float)errorPxX, (float)errorMaxPx, (float)errorPxX, rect.Y);
                gfx.DrawLine(pen, (float)capPx1, (float)errorMinPx, (float)capPx2, (float)errorMinPx);
                gfx.DrawLine(pen, (float)capPx1, (float)errorMaxPx, (float)capPx2, (float)errorMaxPx);
            }
        }
    }
}
