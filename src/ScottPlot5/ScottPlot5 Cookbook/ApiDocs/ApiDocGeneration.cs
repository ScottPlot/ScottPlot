namespace ScottPlotCookbook.ApiDocs;

internal class ApiDocGeneration
{
    [Test]
    public void Test_Docs()
    {
        string xmlFilePath = Paths.GetScottPlotXmlFilePath();
        ApiDocs docs = new(typeof(Plot), xmlFilePath);

        string savePath = Path.Combine(Paths.OutputFolder, "API.md");
        File.WriteAllText(savePath, docs.GetMarkdown());
        Console.WriteLine(savePath);
    }
}
