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
            (slope, offset) = GetCoefficients(ys, firstX, xSpacing);
        }

        public LinearRegressionLine(double[] ys, double firstX, double xSpacing)
        {
            // this constructor doesn't require an X array to be passed in at all
            pointCount = ys.Length;
            this.firstX = firstX;
            this.xSpacing = xSpacing;
            (slope, offset) = GetCoefficients(ys, firstX, xSpacing);
        }

        public override string ToString()
        {
            return $"Linear fit for {pointCount} points: Y = {slope}x + {offset}";
        }

        private static (double, double) GetCoefficients(double[] ys, double firstX, double xSpacing)
        {
            int pointCount = ys.Length;
            double meanY = ys.Average();
            double sumXYResidual = 0;
            double sumXSquareResidual = 0;
            double spanX = firstX + pointCount * xSpacing;
            double meanX = spanX / 2 + firstX;

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
