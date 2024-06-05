namespace ScottPlotTests.CodeTests;

public class CodeFormatTests
{
    [Test]
    public void Test_AddMethods_AreAlphabetized()
    {
        List<string> methodNames = SourceCodeParsing.GetMethodNames("PlottableAdder.cs");

        methodNames.Remove("GetNextColor");

        string lastMethodName = string.Empty;
        foreach (string methodName in methodNames)
        {
            if (string.Compare(methodName, lastMethodName) < 0)
            {
                throw new InvalidOperationException($"PlottableAdder.cs methods must be in alphabetical order. " +
                    $"{lastMethodName} is currently before {methodName}.");
            }

            lastMethodName = methodName;
        }
    }
}
