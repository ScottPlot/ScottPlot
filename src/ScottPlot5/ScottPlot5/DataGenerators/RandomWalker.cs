namespace ScottPlot.DataGenerators;

public class RandomWalker(int? seed = null, double firstValue = 0, double multiplier = 1)
{
    readonly private RandomDataGenerator Gen = new(seed);
    readonly double Multiplier = multiplier;
    private double LastNumber = firstValue;
    public double Next() => LastNumber += (Gen.RandomNumber() - .5) * Multiplier;
    public IEnumerable<double> Next(int count) => Enumerable.Range(0, count).Select(x => Next());
}
