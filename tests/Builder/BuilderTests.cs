using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace ScottPlotTests.Builder
{
    class BuilderTests
    {
        [Test]
        public void Test_Version_ParsesCsprojFile()
        {
            string projectFilePath = @"../../../../src/ScottPlot.WinForms/ScottPlot.WinForms.csproj";
            var ver = new ScottPlotBuilder.ProjectFileVersion(projectFilePath);
            Console.WriteLine(ver);
        }
    }
}
