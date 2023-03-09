using System.Xml.Linq;

namespace CodeAnalysis;

public class ProjectReport
{
    public string ProjectName { get; }
    public List<Metrics> Metrics { get; } = new();

    public int LinesOfCode => Metrics.Select(x => x.LinesOfCode).Where(x => x.HasValue).Select(x => x!.Value).Sum();
    public int CyclomaticComplexity => Metrics.Select(x => x.CyclomaticComplexity).Where(x => x.HasValue).Select(x => x!.Value).Sum();

    public int Types => Metrics.Count;

    public ProjectReport(string xmlFilePath)
    {
        ProjectName = Path.GetFileName(xmlFilePath).Replace(".Metrics.xml", "");

        string xmlText = File.ReadAllText(xmlFilePath);
        XDocument doc = XDocument.Parse(xmlText);
        XElement assembly = doc.Descendants("Assembly").First();

        foreach (XElement namespaceElement in assembly.Element("Namespaces")!.Elements("Namespace"))
        {
            string namespaceName = namespaceElement.Attribute("Name")!.Value;

            foreach (XElement namedType in namespaceElement.Elements("Types").Elements("NamedType"))
            {
                string typeName = namedType.Attribute("Name")!.Value;

                var metricsElement = namedType.Element("Metrics");
                if (metricsElement is not null)
                {
                    Metrics.Add(CodeAnalysis.Metrics.FromElement(metricsElement, namespaceName, typeName));
                }
            }
        }
    }

    public override string ToString()
    {
        return $"{ProjectName} with {Metrics.Count} metrics";
    }
}
