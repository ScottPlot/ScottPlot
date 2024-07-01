using System.Text;
using System.Xml.Linq;

namespace ScottPlotTests.ApiDocs;

internal class ApiDocGeneration
{
    [Test]
    public void Test_ApiDoc_Generate()
    {
        string xmlFilePath = Path.Combine(Paths.RepoFolder, @"src/ScottPlot5/ScottPlot5/bin/Debug/net8.0/ScottPlot.xml");
        if (!File.Exists(xmlFilePath))
            throw new FileNotFoundException(xmlFilePath);

        StringBuilder sb = new();

        string xml = File.ReadAllText(xmlFilePath);
        XDocument doc = XDocument.Parse(xml);

        string ASSEMBLY_NAME = doc.Element("doc")!.Element("assembly")!.Element("name")!.Value;
        sb.AppendLine($"<h1>{ASSEMBLY_NAME}</h1>");

        foreach (XElement member in doc.Element("doc")!.Element("members")!.Elements("member")!)
        {
            string name = member.Attribute("name")!.Value;

            if (!name.Contains("ScottPlot.AxisManager."))
                continue;

            sb.AppendLine($"<h3>{name}</h3>");

            string summary = member.Element("summary")!.Value
                .Replace("<", "&lt;")
                .Replace(">", "&gt;")
                .Replace("\n", " ")
                .Trim();
            sb.AppendLine($"<p>{summary}</p>");
        }

        string saveAs = Path.GetFullPath("api.html");
        File.WriteAllText(saveAs, sb.ToString());
        Console.WriteLine(saveAs);
    }
}
