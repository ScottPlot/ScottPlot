using System.Reflection;

namespace ScottPlotTests.CodeTests;

internal class CodeRequirementTests
{
    [Test]
    public void Test_AllTestMethods_HaveTestAttribute()
    {
        var testMethods = Assembly.GetAssembly(typeof(CodeRequirementTests))!
            .GetTypes()
            .SelectMany(t => t.GetMethods())
            .Where(x => x.Name.StartsWith("Test_"));

        foreach (MethodInfo mi in testMethods)
        {
            bool hasTestAttribute = mi.CustomAttributes
                .Select(x => x.AttributeType)
                .Contains(typeof(TestAttribute));

            bool hasParameterizedTestAttribute = mi.CustomAttributes
                .Select(x => x.AttributeType)
                .Contains(typeof(TestCaseAttribute));

            bool hasRequireAttribute = hasTestAttribute || hasParameterizedTestAttribute;

            if (!hasRequireAttribute)
            {
                string name = $"{mi.DeclaringType}." + mi.ToString()!.Split(" ")[1];
                string message = $"{name} is missing the [Test] attribute.";
                Assert.Fail(message);
            }
        }
    }
}
