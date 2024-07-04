using System.Reflection;

namespace ScottPlotTests.ApiDocs;

internal class ApiDocGeneration
{
    [Test]
    public void Test_Docs()
    {
        string xmlFilePath = Paths.GetScottPlotXmlFilePath();
        ApiDocs docs = new(typeof(Plot), xmlFilePath);

        string savePath = Path.GetFullPath("test.html");
        File.WriteAllText(savePath, docs.GetHtml());
        Console.WriteLine(xmlFilePath);
        Console.WriteLine(savePath);
    }
}
