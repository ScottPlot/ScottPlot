namespace ScottPlot.Statistics;

public readonly struct LinearRegression
{
    public readonly double Slope;
    public readonly double Offset;
    public readonly double Rsquared;
    public string Formula => $"y = {Slope:0.###}x + {Offset:0.###}";
    public string FormulaWithRSquared => $"{Formula} (r²={Rsquared:0.###})";

    /// <summary>
    /// Calculate the linear regression from a collection of X/Y coordinates
    /// </summary>
    public LinearRegression(Coordinates[] coordinates)
    {
        if (coordinates.Length < 2)
        {
            throw new ArgumentException($"{nameof(coordinates)} must have at least 2 points");
        }

        double[] xs = coordinates.Select(x => x.X).ToArray();
        double[] ys = coordinates.Select(x => x.Y).ToArray();
        (Slope, Offset, Rsquared) = GetCoefficients(xs, ys);
    }

    /// <summary>
    /// Calculate the linear regression a paired collection of X and Y points
    /// </summary>
    public LinearRegression(double[] xs, double[] ys)
    {
        if (xs.Length != ys.Length)
        {
            throw new ArgumentException($"{nameof(xs)} and {nameof(ys)} must be the same length");
        }

        if (ys.Length < 2)
        {
            throw new ArgumentException($"{nameof(xs)} and {nameof(ys)} must have at least 2 points");
        }

        (Slope, Offset, Rsquared) = GetCoefficients(xs, ys);
    }

    /// <summary>
    /// Calculate the linear regression from a collection of evenly-spaced Y values
    /// </summary>
    public LinearRegression(double[] ys, double firstX = 0, double xSpacing = 1)
    {
        if (xSpacing == 0)
        {
            throw new ArgumentException($"{nameof(xSpacing)} cannot be zero");
        }

        if (ys.Length < 2)
        {
            throw new ArgumentException($"{nameof(ys)} must have at least 2 points");
        }

        double[] xs = Generate.Consecutive(count: ys.Length, delta: xSpacing, first: firstX);
        (Slope, Offset, Rsquared) = GetCoefficients(xs, ys);
    }

    private static (double slope, double offset, double rSquared) GetCoefficients(double[] xs, double[] ys)
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

    /// <summary>
    /// Return the Y point of the regression line for the given X position
    /// </summary>
    public double GetValue(double x)
    {
        return Offset + Slope * x;
    }

    /// <summary>
    /// Return the Y points of the regression line for the given X positions
    /// </summary>
    public double[] GetValues(double[] xs)
    {
        double[] ys = new double[xs.Length];

        for (int i = 0; i < ys.Length; i++)
        {
            ys[i] = GetValue(xs[i]);
        }

        return ys;
    }
}
