namespace ScottPlot.Statistics;

public class LinearRegression
{
    public readonly double Slope;
    public readonly double Offset;
    public readonly double Rsquared;

    private readonly int pointCount;
    private readonly double[] Xs;
    private readonly double[] Ys;

    public LinearRegression(double[] xs, double[] ys)
    {
        if (xs.Length != ys.Length)
        {
            throw new ArgumentException("xs and ys must be the same length");
        }

        if (ys.Length < 2)
        {
            throw new ArgumentException("xs and ys must have at least 2 points");
        }

        pointCount = ys.Length;
        Xs = xs;
        Ys = ys;
        (Slope, Offset, Rsquared) = GetCoefficients(xs, ys);
    }

    public LinearRegression(double[] ys, double firstX, double xSpacing)
    {
        if (ys.Length < 2)
        {
            throw new ArgumentException("xs and ys must have at least 2 points");
        }

        pointCount = ys.Length;
        double[] xs = new double[pointCount];
        for (int i = 0; i < pointCount; i++)
        {
            xs[i] = firstX + xSpacing * i;
        }
        Xs = xs;
        Ys = ys;
        (Slope, Offset, Rsquared) = GetCoefficients(xs, ys);
    }

    public static (double slope, double offset, double rSquared) GetCoefficients(double[] xs, double[] ys)
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

        // Note: least-squares regression line always passes through (x̄, ȳ)
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
        return Offset + Slope * x;
    }

    public double[] GetValues()
    {
        double[] values = new double[pointCount];
        for (int i = 0; i < pointCount; i++)
        {
            values[i] = GetValueAt(Xs[i]);
        }
        return values;
    }

    /// <summary>
    /// Residual is the difference between the actual and predicted value
    /// </summary>
    public double[] GetResiduals()
    {
        double[] residuals = new double[Ys.Length];

        for (int i = 0; i < Ys.Length; i++)
        {
            residuals[i] = Ys[i] - GetValueAt(Xs[i]);
        }

        return residuals;
    }
}
