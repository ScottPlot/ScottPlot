namespace ScottPlot.Statistics.Tests;

// Useful: https://www.statstutor.ac.uk/resources/uploaded/unpaired-t-test.pdf

public class UnpairedTTest
{
    public Population Population1 { get; }
    public Population Population2 { get; }

    /// <summary>
    /// T statistic
    /// </summary>
    public double T { get; }

    /// <summary>
    /// Degrees of freedom
    /// </summary>
    public int DF { get; }

    public double P { get; }

    public UnpairedTTest(Population pop1, Population pop2)
    {
        Population1 = pop1;
        Population2 = pop2;

        // standard error on the difference between the samples
        double sed = Math.Sqrt(pop1.StandardError * pop1.StandardError + pop2.StandardError * pop2.StandardError);
        T = (pop1.Mean - pop2.Mean) / sed;
        DF = pop1.Count + pop2.Count - 2;
        P = GetStudentP(T, DF);
    }

    private static double GetStudentP(double t, double df)
    {
        // https://jamesmccaffrey.wordpress.com/2016/04/27/implementing-the-students-t-distribution-density-function-in-code/
        // for large int df or double df
        // adapted from ACM algorithm 395
        // returns 2-tail probability

        double n = df; // to sync with ACM parameter name
        double a, b, y;

        t = t * t;
        y = t / n;
        b = y + 1.0;
        if (y > 1.0E-6) y = Math.Log(b);
        a = n - 0.5;
        b = 48.0 * a * a;
        y = a * y;

        y = (((((-0.4 * y - 3.3) * y - 24.0) * y - 85.5) /
          (0.8 * y * y + 100.0 + b) +
            y + 3.0) / b + 1.0) * Math.Sqrt(y);
        return 2.0 * Gauss(-y);
    }

    public static double Gauss(double z)
    {
        // input = z-value (-inf to +inf)
        // output = p under Normal curve from -inf to z
        // e.g., if z = 0.0, function returns 0.5000
        // ACM Algorithm #209
        double y; // 209 scratch variable
        double p; // result. called 'z' in 209
        double w; // 209 scratch variable

        if (z == 0.0)
            p = 0.0;
        else
        {
            y = Math.Abs(z) / 2;
            if (y >= 3.0)
            {
                p = 1.0;
            }
            else if (y < 1.0)
            {
                w = y * y;
                p = ((((((((0.000124818987 * w
                  - 0.001075204047) * w + 0.005198775019) * w
                  - 0.019198292004) * w + 0.059054035642) * w
                  - 0.151968751364) * w + 0.319152932694) * w
                  - 0.531923007300) * w + 0.797884560593) * y * 2.0;
            }
            else
            {
                y = y - 2.0;
                p = (((((((((((((-0.000045255659 * y
                  + 0.000152529290) * y - 0.000019538132) * y
                  - 0.000676904986) * y + 0.001390604284) * y
                  - 0.000794620820) * y - 0.002034254874) * y
                  + 0.006549791214) * y - 0.010557625006) * y
                  + 0.011630447319) * y - 0.009279453341) * y
                  + 0.005353579108) * y - 0.002141268741) * y
                  + 0.000535310849) * y + 0.999936657524;
            }
        }

        if (z > 0.0)
            return (p + 1.0) / 2;
        else
            return (1.0 - p) / 2;
    }
}
