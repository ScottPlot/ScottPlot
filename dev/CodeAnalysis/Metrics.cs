using System.Xml.Linq;

namespace CodeAnalysis;

public class Metrics
{
    public string? NamespaceName { get; set; } = null;
    public string? TypeName { get; set; } = null;
    public string FullTypeName => $"{NamespaceName}.{TypeName}";

    public int? CyclomaticComplexity { get; set; } = null;
    public int? LinesOfCode { get; set; } = null;
    public int? ExecutableLines { get; set; } = null;
    public int? Maintainability { get; set; } = null;

    public static Metrics FromElement(XElement metricsElement, string namespaceName, string typeName)
    {
        Metrics ms = new();

        foreach (XElement metric in metricsElement.Elements("Metric"))
        {
            string name = metric.Attribute("Name")!.Value.ToString();
            int value = int.Parse(metric.Attribute("Value")!.Value);
            if (name == "CyclomaticComplexity")
                ms.CyclomaticComplexity = value;
            else if (name == "MaintainabilityIndex")
                ms.Maintainability = value;
            else if (name == "SourceLines")
                ms.LinesOfCode = value;
            else if (name == "ExecutableLines")
                ms.ExecutableLines = value;
        }

        return ms;
    }
}
