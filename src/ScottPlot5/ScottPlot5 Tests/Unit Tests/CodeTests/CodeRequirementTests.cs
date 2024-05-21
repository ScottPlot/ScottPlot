using System.Reflection;
using System.Security.Cryptography.X509Certificates;

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

    [Test]
    public void Test_Plottables_RenderMethodIsVirtual()
    {
        // https://github.com/ScottPlot/ScottPlot/issues/3693

        var plottableTypes = Assembly.GetAssembly(typeof(ScottPlot.Plot))!
            .GetTypes()
            .Where(x => x.IsAssignableTo(typeof(ScottPlot.IPlottable)))
            .Where(x => x.IsClass);

        foreach (Type type in plottableTypes)
        {
            MethodInfo[] mis = type.GetMethods().Where(x => x.Name == "Render").ToArray();

            foreach (MethodInfo mi in mis)
            {
                ParameterInfo[] pis = mi.GetParameters();
                if (pis.Length != 1)
                    continue;

                ParameterInfo pi = pis[0];
                if (pi.ParameterType.Name != "RenderPack")
                    continue;

                if (mi.IsFinal)
                {
                    Assert.Fail($"{type.Namespace}.{type.Name}.Render() must be virtual void");
                }
            }
        }
    }

    [Test]
    public void Test_RenderActions_ArePublic()
    {
        var actionTypes = Assembly.GetAssembly(typeof(ScottPlot.Plot))!
            .GetTypes()
            .Where(x => x.IsAssignableTo(typeof(ScottPlot.IRenderAction)))
            .Where(x => x.IsClass);

        foreach (Type type in actionTypes)
        {
            TypeInfo classInfo = type.GetTypeInfo();
            if (!classInfo.IsVisible)
            {
                throw new InvalidOperationException($"{type.Namespace}.{type.Name} should be public");
            }
        }
    }
}
