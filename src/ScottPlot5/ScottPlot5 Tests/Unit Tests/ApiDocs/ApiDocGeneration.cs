using System.Reflection;

namespace ScottPlotTests.ApiDocs;

internal class ApiDocGeneration
{
    [Test]
    public void Test_Docs()
    {
        string xmlFilePath = Path.Combine(Paths.RepoFolder, @"src/ScottPlot5/ScottPlot5/bin/Debug/net8.0/ScottPlot.xml");
        ApiDocs docs = new(typeof(Plot), xmlFilePath);

        string savePath = Path.GetFullPath("test.html");
        File.WriteAllText(savePath, docs.GetHtml());
        Console.WriteLine(xmlFilePath);
        Console.WriteLine(savePath);
    }
}
