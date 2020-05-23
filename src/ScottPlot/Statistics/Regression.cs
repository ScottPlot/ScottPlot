using System;
using System.Linq;

namespace ScottPlot.Statistics
{
    public class LinearRegressionLine
    {
        public readonly double slope;
        public readonly double offset;
        public readonly double rSquared;

        private readonly int pointCount;
        private readonly double[] xs;
        private readonly double[] ys;

        public LinearRegressionLine(double[] xs, double[] ys)
        {
            if ((xs.Length != ys.Length) || (xs.Length < 2))
            {
                throw new ArgumentException("xs and ys must be the same length and have at least 2 points");
            }

            pointCount = ys.Length;
            this.xs = xs;
            this.ys = ys;
            (slope, offset, rSquared) = GetCoefficients(xs, ys);
        }

        public LinearRegressionLine(double[] ys, double firstX, double xSpacing)
        {
            // this constructor doesn't require an X array to be passed in at all
            pointCount = ys.Length;
            double[] xs = new double[pointCount];
            for (int i = 0; i < pointCount; i++)
            {
                xs[i] = firstX + xSpacing * i;
            }
            this.xs = xs;
            this.ys = ys;
            (slope, offset, rSquared) = GetCoefficients(xs, ys);
        }

        public override string ToString()
        {
            return $"Linear fit for {pointCount} points: Y = {slope}x + {offset} (R² = {rSquared})";
        }

        private static (double, double, double) GetCoefficients(double[] xs, double[] ys)
        {
            double sumXYResidual = 0;
            double sumXSquareResidual = 0;

            double meanX = xs.Average();
            double meanY = ys.Average();

            for (int i = 0; i < xs.Length; i++)
            {
                sumXYResidual += (xs[i] - meanX) * (ys[i] - meanY);
                sumXSquareResidual += (xs[i] - meanX) * (xs[i] - meanX);
            }

            // Note: least-squares regression line always passes through (x̅,y̅)
            double slope = sumXYResidual / sumXSquareResidual;
            double offset = meanY - (slope * meanX);

            // calcualte R squared (https://en.wikipedia.org/wiki/Coefficient_of_determination)
            double ssTot = 0;
            double ssRes = 0;
            for (int i = 0; i < ys.Length; i++)
            {
                double thisY = ys[i];

                double distanceFromMeanSquared = Math.Pow(thisY - meanY, 2);
                ssTot += distanceFromMeanSquared;

                double modelY = slope * xs[i] + offset;
                double distanceFromModelSquared = Math.Pow(thisY - modelY, 2);
                ssRes += distanceFromModelSquared;
            }
            double rSquared = 1 - ssRes / ssTot;

            return (slope, offset, rSquared);
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
                values[i] = GetValueAt(xs[i]);
            }
            return values;
        }

        public double[] GetResiduals()
        {
            // the residual is the difference between the actual and predicted value

            double[] residuals = new double[ys.Length];

            for (int i = 0; i < ys.Length; i++)
            {
                residuals[i] = ys[i] - GetValueAt(xs[i]);
            }

            return residuals;
        }
    }
}
