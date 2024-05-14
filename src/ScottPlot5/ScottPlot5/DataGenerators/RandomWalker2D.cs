namespace ScottPlot.DataGenerators;

public class RandomWalker2D(int? seed = null)
{
    private RandomDataGenerator Gen { get; } = new(seed);

    private double X = 0;
    private double Y = 0;
    private double VX = 0;
    private double VY = 0;

    public Coordinates Next()
    {
        VX += Gen.RandomNumber() - .5;
        VY += Gen.RandomNumber() - .5;

        double absX = Math.Abs(VX);
        if (absX > 1) VX *= 1 - absX;

        double absY = Math.Abs(VY);
        if (absY > 1) VY *= 1 - absY;

        X += VX;
        Y += VY;

        return new(X, Y);
    }

    public IEnumerable<Coordinates> Next(int count) => Enumerable.Range(0, count).Select(x => Next());
}
