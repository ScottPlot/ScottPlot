// This file tests what happens when you have "using" statements for two libraries with the same Color

using ScottPlot;
using System.Drawing;

using NUnit.Framework;
using System;

namespace ScottPlotTests;

internal class NamespaceTests
{
    private void DoSomething(System.Drawing.Color color) => Console.WriteLine(color);

    private void DoSomething(ScottPlot.Color color) => Console.WriteLine(color);

    [Test]
    public void Test_Namespace_Check()
    {
        ScottPlot.Color color1 = new();
        DoSomething(color1);

        System.Drawing.Color color2 = new();
        DoSomething(color2);

        // NOT OK (AMBIGUOUS)
        // var c = new Color();
    }
}
