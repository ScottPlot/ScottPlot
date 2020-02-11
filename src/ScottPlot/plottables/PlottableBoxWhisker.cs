using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace ScottPlot
{
    public class PlottableBoxWhisker : Plottable
    {
        public double[] xs;
        public double[][] ys;
        public double lineWidth;
        public double boxWidth;
        public MarkerShape markerShape;
        private double[] yLimits;
        private double padding = 50;
        Pen pen;
        Brush brush;

        public PlottableBoxWhisker(double[] xs, double[][] ys, Color color, string label, MarkerShape markerShape, double lineWidth, double boxWidth)
        {
            if (xs == null || ys == null)
                throw new Exception("Y data cannot be null");

            if (xs.Length != ys.Length)
            {
                throw new ArgumentException("X positions must be the same length as Y values");
            }

            yLimits = new double[2];
            yLimits[0] = ys[0][0];
            yLimits[1] = ys[0][0];

            for (int i = 0; i < ys.Length; i++)
            {
                for (int j = 0; j < ys[i].Length; j++)
                {
                    if (ys[i][j] < yLimits[0])
                    {
                        yLimits[0] = ys[i][j];
                    }
                    else if (ys[i][j] > yLimits[1])
                    {
                        yLimits[1] = ys[i][j];
                    }
                }
            }

            this.xs = xs;
            this.ys = ys;
            this.color = color;
            this.label = label;
            this.lineWidth = lineWidth;
            this.boxWidth = boxWidth;
            this.markerShape = markerShape;

            pen = new Pen(color, (float)lineWidth);
            brush = new SolidBrush(color);
        }

        public override Config.AxisLimits2D GetLimits()
        {
            double[] limits = new double[4];
            limits[0] = xs.Min();
            limits[1] = xs.Max();
            limits[2] = yLimits[0];
            limits[3] = yLimits[1];

            // TODO: use features of 2d axis
            return new Config.AxisLimits2D(limits);
        }

        public override void Render(Settings settings)
        {
            for (int i = 0; i < xs.Length; i++)
            {
                double QSize = (int)Math.Floor(ys[i].Length / 4.0);
                double[] sortedYs = ys[i].OrderBy(y => y).ToArray();
                double Q1 = sortedYs[(int)Math.Floor(QSize)];
                double median = sortedYs[(int)Math.Floor(2 * QSize)];
                double Q3 = sortedYs[(int)Math.Floor(sortedYs.Length - QSize - 1)];

                double lowerBoundary = Q1 - 1.5 * (Q3 - Q1);
                double upperBoundary = Q3 + 1.5 * (Q3 - Q1);

                //These could be made faster knowing that we have already sorted the list above
                List<double> lowOutliers = ys[i].Where(y => y < lowerBoundary).ToList();
                List<double> highOutliers = ys[i].Where(y => y > upperBoundary).ToList();

                PointF topLeft = settings.GetPixel((xs[i] - (boxWidth / (2 * settings.xAxisScale))), Q3);
                PointF bottomLeft = settings.GetPixel((xs[i] - (boxWidth / (2 * settings.xAxisScale))), Q1);
                int middleX = (int)Math.Floor(topLeft.X + boxWidth / 2);
                PointF medianLine = settings.GetPixel(middleX, median);

                PointF bottomWhisker;
                PointF topWhisker;

                if (lowOutliers.Count == 0)
                {
                    bottomWhisker = settings.GetPixel(0, ys[i].Min());
                }
                else
                {
                    bottomWhisker = settings.GetPixel(0, lowerBoundary);
                }

                if (highOutliers.Count == 0)
                {
                    topWhisker = settings.GetPixel(0, ys[i].Max());
                }
                else
                {
                    topWhisker = settings.GetPixel(0, upperBoundary);
                }

                bottomWhisker.X = middleX;
                topWhisker.X = middleX;

                int whiskerSpread = (int)Math.Floor(boxWidth / 5);




                settings.gfxData.DrawLine(pen, topWhisker.X, topWhisker.Y, middleX, topLeft.Y);//Whisker Centre
                settings.gfxData.DrawLine(pen, bottomWhisker.X, bottomWhisker.Y, middleX, bottomLeft.Y);//Whisker Centre
                settings.gfxData.DrawLine(pen, bottomWhisker.X - whiskerSpread, bottomWhisker.Y, bottomWhisker.X + whiskerSpread, bottomWhisker.Y);//Whisker Cap
                settings.gfxData.DrawLine(pen, topWhisker.X - whiskerSpread, topWhisker.Y, topWhisker.X + whiskerSpread, topWhisker.Y);//Whisker Cap

                settings.gfxData.DrawRectangle(pen, topLeft.X, topLeft.Y, (int)boxWidth, (int)Math.Round((Q3 - Q1) * settings.yAxisScale));//Box
                settings.gfxData.DrawLine(pen, topLeft.X, medianLine.Y, (int)boxWidth + topLeft.X, medianLine.Y);//Median

                List<double> allOutliers = lowOutliers;
                allOutliers.AddRange(highOutliers);

                foreach (double curr in allOutliers)
                {
                    PointF point = settings.GetPixel(0, curr);
                    point.X = middleX;

                    MarkerTools.DrawMarker(settings.gfxData, point, markerShape, (float)lineWidth * 3, color);//Outliers
                }


            }

            //	throw new NotImplementedException();
        }

        public override string ToString()
        {
            return $"PlottableBoxWhisker with {xs.Length} boxes";
        }
    }
}
