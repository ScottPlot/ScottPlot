namespace ScottPlotTests.CodeTests;

public class CodeFormatTests
{
    [Test]
    public void Test_AddMethods_AreAlphabetized()
    {
        string[] methodNames = typeof(PlottableAdder)
            .GetMethods()
            .Where(x => x.ReturnType.GetInterfaces().Contains(typeof(IPlottable)))
            .Select(x => x.Name)
            .Distinct()
            .ToArray();

        string[] sorted = methodNames.OrderBy(x => x, StringComparer.InvariantCulture).ToArray();

        for (int i = 0; i < methodNames.Length; i++)
        {
            if (sorted[i] != methodNames[i])
            {
                Console.WriteLine($"Methods in {typeof(PlottableAdder)} must be in alphabetical order.");
                Console.WriteLine($"ERROR: {methodNames[i]}() is listed before {sorted[i]}()");
                Assert.Fail();
            }
        }
    }
}
