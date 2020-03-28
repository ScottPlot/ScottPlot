using ScottPlot.Statistics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace ScottPlot
{
    public static class RenderPopulation
    {
        public enum Position { Hide, Overlap, Left, Right }

        public static void Scatter(Settings settings, Population pop, Random rand, double popLeft, double popWidth, Color color, Position position)
        {
            // adjust edges to accomodate special positions
            if (position == Position.Hide) return;
            if (position == Position.Left || position == Position.Right) popWidth /= 2;
            if (position == Position.Right) popLeft += popWidth;

            // contract edges slightly to encourage padding between elements
            double edgePaddingFrac = 0.2;
            popLeft += popWidth * edgePaddingFrac;
            popWidth -= (popWidth * edgePaddingFrac) * 2;

            Pen pen = new Pen(color);
            float radius = 5;

            foreach (double value in pop.values)
            {
                double yPx = settings.GetPixelY(value);
                double xPx = settings.GetPixelX(popLeft + rand.NextDouble() * popWidth);
                settings.gfxData.DrawEllipse(pen, (float)(xPx - radius), (float)(yPx - radius), radius * 2, radius * 2);
            }
        }

        public static void MeanAndError(Settings settings, Population pop, Random rand, double popLeft, double popWidth, Color color, Position position)
        {
            // adjust edges to accomodate special positions
            if (position == Position.Hide) return;
            if (position == Position.Left || position == Position.Right) popWidth /= 2;
            if (position == Position.Right) popLeft += popWidth;

            // determine the center point and calculate bounds
            double centerX = popLeft + popWidth / 2;
            double xPx = settings.GetPixelX(centerX);
            double yPx = settings.GetPixelY(pop.mean);
            double errorMaxPx = settings.GetPixelY(pop.mean + pop.stDev);
            double errorMinPx = settings.GetPixelY(pop.mean - pop.stDev);

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

        public static void Bar(Settings settings, Population pop, Random rand, double popLeft, double popWidth, Color color, Position position)
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
            double errorMaxPx = settings.GetPixelY(pop.mean + pop.stDev);
            double errorMinPx = settings.GetPixelY(pop.mean - pop.stDev);

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

        public static void Box(Settings settings, Population pop, Random rand, double popLeft, double popWidth, Color color, Position position, BoxFormat boxFormat)
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

            // determine the center point and calculate bounds
            double centerX = popLeft + popWidth / 2;
            double xPx = settings.GetPixelX(centerX);

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

            // vertical errorbars
            settings.gfxData.DrawLine(pen, (float)xPx, (float)errorMinPx, (float)xPx, rect.Y + rect.Height);
            settings.gfxData.DrawLine(pen, (float)xPx, (float)errorMaxPx, (float)xPx, rect.Y);

            // errorbar caps
            settings.gfxData.DrawLine(pen, (float)capPx1, (float)errorMinPx, (float)capPx2, (float)errorMinPx);
            settings.gfxData.DrawLine(pen, (float)capPx1, (float)errorMaxPx, (float)capPx2, (float)errorMaxPx);

            // horizontal line in center
            settings.gfxData.DrawLine(pen, rect.X, (float)yPx, rect.X + rect.Width, (float)yPx);
        }
    }
}
