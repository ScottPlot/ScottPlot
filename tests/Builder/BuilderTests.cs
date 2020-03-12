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
            string projectFilePath = @"../../../data/sample.csproj";
            var ver = new ScottPlotBuilder.ProjectFileVersion(projectFilePath);
            Console.WriteLine(ver);
        }

        [Test]
        public void Test_Version_Incriment()
        {
            string projectFilePath = @"../../../data/sample.csproj";

            var projVer1 = new ScottPlotBuilder.ProjectFileVersion(projectFilePath);
            Version v1 = new Version(projVer1.version.ToString());

            Console.WriteLine($"Version in file: {projVer1}");
            projVer1.Incriment();
            Console.WriteLine($"Incrimented to: {projVer1}");
            projVer1.Save();
            Console.WriteLine($"Saved");

            var projVer2 = new ScottPlotBuilder.ProjectFileVersion(projectFilePath);
            Version v2 = new Version(projVer1.version.ToString());

            Console.WriteLine($"Version in file: {projVer2}");

            Assert.AreEqual(v1.Major, v2.Major);
            Assert.AreEqual(v1.Minor, v2.Minor);
            Assert.Greater(v2.Build, v1.Build);
        }
    }
}
