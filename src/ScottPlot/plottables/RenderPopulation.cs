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

        public static void Scatter(Settings settings, Population pop, Random rand, double popLeft, double popWidth, Color fillColor, Color edgeColor, byte alpha, Position position)
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
                double yPx = settings.GetPixelY(value);
                double xPx = settings.GetPixelX(popLeft + rand.NextDouble() * popWidth);
                settings.gfxData.FillEllipse(brushFill, (float)(xPx - radius), (float)(yPx - radius), radius * 2, radius * 2);
                settings.gfxData.DrawEllipse(penEdge, (float)(xPx - radius), (float)(yPx - radius), radius * 2, radius * 2);
            }
        }

        public static void Distribution(Settings settings, Population pop, Random rand, double popLeft, double popWidth, Color color, Position position, LineStyle lineStyle)
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

            double[] ys = DataGen.Range(pop.minus3stDev, pop.plus3stDev, settings.yAxisUnitsPerPixel);
            double[] ysFrac = pop.GetDistribution(ys);

            PointF[] points = new PointF[ys.Length];
            for (int i = 0; i < ys.Length; i++)
            {
                float x = (float)settings.GetPixelX(popLeft + popWidth * ysFrac[i]);
                float y = (float)settings.GetPixelY(ys[i]);
                points[i] = new PointF(x, y);
            }
            settings.gfxData.DrawLines(pen, points);
        }

        public static void MeanAndError(Settings settings, Population pop, Random rand, double popLeft, double popWidth, Color color, Position position, bool useStdErr = false)
        {
            // adjust edges to accomodate special positions
            if (position == Position.Hide) return;
            if (position == Position.Left || position == Position.Right) popWidth /= 2;
            if (position == Position.Right) popLeft += popWidth;

            // determine the center point and calculate bounds
            double centerX = popLeft + popWidth / 2;
            double xPx = settings.GetPixelX(centerX);
            double yPx = settings.GetPixelY(pop.mean);

            double errorMaxPx, errorMinPx;
            if (useStdErr)
            {
                errorMaxPx = settings.GetPixelY(pop.mean + pop.stdErr);
                errorMinPx = settings.GetPixelY(pop.mean - pop.stdErr);
            }
            else
            {
                errorMaxPx = settings.GetPixelY(pop.mean + pop.stDev);
                errorMinPx = settings.GetPixelY(pop.mean - pop.stDev);
            }

            // make cap width a fraction of available space
            double capWidthFrac = .38;
            double capWidth = popWidth * capWidthFrac;
            double capPx1 = settings.GetPixelX(centerX - capWidth / 2);
            double capPx2 = settings.GetPixelX(centerX + capWidth / 2);

            Pen pen = new Pen(color, 2);
            Brush brush = new SolidBrush(color);
            float radius = 5;

            settings.gfxData.FillEllipse(brush, (float)(xPx - radius), (float)(yPx - radius), radius * 2, radius * 2);
            settings.gfxData.DrawLine(pen, (float)xPx, (float)errorMinPx, (float)xPx, (float)errorMaxPx);
            settings.gfxData.DrawLine(pen, (float)capPx1, (float)errorMinPx, (float)capPx2, (float)errorMinPx);
            settings.gfxData.DrawLine(pen, (float)capPx1, (float)errorMaxPx, (float)capPx2, (float)errorMaxPx);
        }

        public static void Bar(Settings settings, Population pop, Random rand, double popLeft, double popWidth, Color color, Position position, bool useStdErr = false)
        {
            // adjust edges to accomodate special positions
            if (position == Position.Hide) return;
            if (position == Position.Left || position == Position.Right) popWidth /= 2;
            if (position == Position.Right) popLeft += popWidth;

            // determine the center point and calculate bounds
            double centerX = popLeft + popWidth / 2;
            double xPx = settings.GetPixelX(centerX);
            double yPxTop = settings.GetPixelY(pop.mean);
            double yPxBase = settings.GetPixelY(0);

            double errorMaxPx, errorMinPx;
            if (useStdErr)
            {
                errorMaxPx = settings.GetPixelY(pop.mean + pop.stdErr);
                errorMinPx = settings.GetPixelY(pop.mean - pop.stdErr);
            }
            else
            {
                errorMaxPx = settings.GetPixelY(pop.mean + pop.stDev);
                errorMinPx = settings.GetPixelY(pop.mean - pop.stDev);
            }

            // make cap width a fraction of available space
            double capWidthFrac = .38;
            double capWidth = popWidth * capWidthFrac;
            double capPx1 = settings.GetPixelX(centerX - capWidth / 2);
            double capPx2 = settings.GetPixelX(centerX + capWidth / 2);

            // contract edges slightly to encourage padding between elements
            double edgePaddingFrac = 0.2;
            popLeft += popWidth * edgePaddingFrac;
            popWidth -= (popWidth * edgePaddingFrac) * 2;
            double leftPx = settings.GetPixelX(popLeft);
            double rightPx = settings.GetPixelX(popLeft + popWidth);

            Pen pen = new Pen(Color.Black, 1);
            Brush brush = new SolidBrush(color);

            RectangleF rect = new RectangleF((float)leftPx, (float)yPxTop, (float)(rightPx - leftPx), (float)(yPxBase - yPxTop));
            settings.gfxData.FillRectangle(brush, rect.X, rect.Y, rect.Width, rect.Height);
            settings.gfxData.DrawRectangle(pen, rect.X, rect.Y, rect.Width, rect.Height);
            settings.gfxData.DrawLine(pen, (float)xPx, (float)errorMinPx, (float)xPx, (float)errorMaxPx);
            settings.gfxData.DrawLine(pen, (float)capPx1, (float)errorMinPx, (float)capPx2, (float)errorMinPx);
            settings.gfxData.DrawLine(pen, (float)capPx1, (float)errorMaxPx, (float)capPx2, (float)errorMaxPx);
        }

        public enum BoxFormat { StdevStderrMean, OutlierQuartileMedian }
        public enum HorizontalAlignment { Left, Center, Right }

        public static void Box(Settings settings, Population pop, Random rand, double popLeft, double popWidth, Color color, Position position, BoxFormat boxFormat, HorizontalAlignment errorAlignment = HorizontalAlignment.Right)
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
                errorMaxPx = settings.GetPixelY(pop.mean + pop.stDev);
                errorMinPx = settings.GetPixelY(pop.mean - pop.stDev);
                yPxTop = settings.GetPixelY(pop.mean + pop.stdErr);
                yPxBase = settings.GetPixelY(pop.mean - pop.stdErr);
                yPx = settings.GetPixelY(pop.mean);
            }
            else if (boxFormat == BoxFormat.OutlierQuartileMedian)
            {
                errorMaxPx = settings.GetPixelY(pop.maxNonOutlier);
                errorMinPx = settings.GetPixelY(pop.minNonOutlier);
                yPxTop = settings.GetPixelY(pop.Q3);
                yPxBase = settings.GetPixelY(pop.Q1);
                yPx = settings.GetPixelY(pop.median);
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
            double leftPx = settings.GetPixelX(popLeft);
            double rightPx = settings.GetPixelX(popLeft + popWidth);

            Pen pen = new Pen(Color.Black, 1);
            Brush brush = new SolidBrush(color);

            // draw the box
            RectangleF rect = new RectangleF((float)leftPx, (float)yPxTop, (float)(rightPx - leftPx), (float)(yPxBase - yPxTop));
            settings.gfxData.FillRectangle(brush, rect.X, rect.Y, rect.Width, rect.Height);
            settings.gfxData.DrawRectangle(pen, rect.X, rect.Y, rect.Width, rect.Height);

            // draw the line in the center
            settings.gfxData.DrawLine(pen, rect.X, (float)yPx, rect.X + rect.Width, (float)yPx);

            // determine location of errorbars and caps
            double capPx1, capPx2, errorPxX;
            switch (errorAlignment)
            {
                case HorizontalAlignment.Center:
                    double centerX = popLeft + popWidth / 2;
                    errorPxX = settings.GetPixelX(centerX);
                    capPx1 = settings.GetPixelX(centerX - capWidth / 2);
                    capPx2 = settings.GetPixelX(centerX + capWidth / 2);
                    break;
                case HorizontalAlignment.Right:
                    errorPxX = settings.GetPixelX(popLeft + popWidth);
                    capPx1 = settings.GetPixelX(popLeft + popWidth - capWidth / 2);
                    capPx2 = settings.GetPixelX(popLeft + popWidth);
                    break;
                case HorizontalAlignment.Left:
                    errorPxX = settings.GetPixelX(popLeft);
                    capPx1 = settings.GetPixelX(popLeft);
                    capPx2 = settings.GetPixelX(popLeft + capWidth / 2);
                    break;
                default:
                    throw new NotImplementedException();
            }

            // draw errorbars and caps
            settings.gfxData.DrawLine(pen, (float)errorPxX, (float)errorMinPx, (float)errorPxX, rect.Y + rect.Height);
            settings.gfxData.DrawLine(pen, (float)errorPxX, (float)errorMaxPx, (float)errorPxX, rect.Y);
            settings.gfxData.DrawLine(pen, (float)capPx1, (float)errorMinPx, (float)capPx2, (float)errorMinPx);
            settings.gfxData.DrawLine(pen, (float)capPx1, (float)errorMaxPx, (float)capPx2, (float)errorMaxPx);
        }
    }
}
