using System;
using System.Linq;

namespace ScottPlot.Statistics
{
    public class LinearRegressionLine
    {
        public readonly double slope;
        public readonly double offset;

        private readonly int pointCount;
        private readonly double firstX;
        private readonly double xSpacing;
        private readonly double meanX;

        public LinearRegressionLine(double[] xs, double[] ys)
        {
            if ((xs.Length != ys.Length) || (xs.Length < 2))
            {
                throw new ArgumentException("xs and ys must be the same length and have at least 2 points");
            }

            // TODO: Should we need to add a check to ensure every X value is evenly spaced? 

            pointCount = ys.Length;
            firstX = xs[0];
            xSpacing = xs[1] - xs[0];
            meanX = xs.Average();
            (slope, offset) = GetCoefficients(ys, firstX, xSpacing, meanX);
        }

        public LinearRegressionLine(double[] ys, double firstX, double xSpacing, double meanX)
        {
            // this constructor doesn't require an X array to be passed in at all
            pointCount = ys.Length;
            this.firstX = firstX;
            this.xSpacing = xSpacing;
            this.meanX = meanX;
            (slope, offset) = GetCoefficients(ys, firstX, xSpacing, meanX);
        }

        private static (double, double) GetCoefficients(double[] ys, double firstX, double xSpacing, double meanX)
        {
            int pointCount = ys.Length;
            double meanY = ys.Average();
            double sumXYResidual = 0;
            double sumXSquareResidual = 0;

            for (int i = 0; i < pointCount; i++)
            {
                double thisX = firstX + xSpacing * i;
                double diffFromMean = thisX - meanX;
                sumXYResidual += diffFromMean * (ys[i] - meanY);
                sumXSquareResidual += diffFromMean * diffFromMean;
            }

            // Note: least-squares regression line always passes through (x̅,y̅)
            double slope = sumXYResidual / (sumXSquareResidual);
            double offset = meanY - (slope * meanX); 

            return (slope, offset);
        }

        public double GetValueAt(double x)
        {
            return offset + slope * x;
        }

        public double[] GetValues()
        {
            double[] values = new double[pointCount];
            for (int i = 0; i < pointCount; i++)
            {
                double x = firstX + xSpacing * i;
                values[i] = GetValueAt(x);
            }
            return values;
        }

        public double[] GetResiduals(double[] ys)
        {
            // the residual is the difference between the actual and predicted value

            double[] residuals = new double[ys.Length];

            for (int i = 0; i < ys.Length; i++)
            {
                double x = firstX + xSpacing * i;
                residuals[i] = ys[i] - GetValueAt(x);
            }

            return residuals;
        }
    }
}
