using System.Data;
using System.Reflection;
using System.Xml;

namespace ScottPlotCookbook.ApiDocs;

public class XmlDocsDB
{
    private readonly Dictionary<string, string> DocsByKey;

    public XmlDocsDB(string xmlFilePath)
    {
        if (!File.Exists(xmlFilePath))
            throw new FileNotFoundException(xmlFilePath);
        string xml = File.ReadAllText(xmlFilePath);
        DocsByKey = GetXmlDocsByMember(xml);
    }

    private Dictionary<string, string> GetXmlDocsByMember(string xml)
    {
        Dictionary<string, string> docsByMember = [];

        using XmlReader xmlReader = XmlReader.Create(new StringReader(xml));
        while (xmlReader.Read())
        {
            if (xmlReader.NodeType == XmlNodeType.Element && xmlReader.Name == "member")
            {
                string raw_name = xmlReader["name"] ?? throw new NoNullAllowedException();
                docsByMember[raw_name] = xmlReader.ReadInnerXml();
            }
        }

        return docsByMember;
    }

    private static string? XmlToHtml(string? xml)
    {
        if (xml is null)
            return null;

        return xml.Replace("<summary>", "")
            .Replace("</summary>", "")
            .Replace("<", "&lt;")
            .Replace(">", "&gt;")
            .Trim();
    }

    public string? GetSummary(Type type)
    {
        string key = $"T:{type.FullName}";
        DocsByKey.TryGetValue(key, out string? value);
        return XmlToHtml(value);
    }

    public string? GetSummary(MethodInfo info)
    {
        string key = $"M:{info.DeclaringType!.FullName}.{info.Name}";
        DocsByKey.TryGetValue(key, out string? value);
        return XmlToHtml(value);
    }

    public string? GetSummary(PropertyInfo info)
    {
        string key = $"P:{info.DeclaringType!.FullName}.{info.Name}";
        DocsByKey.TryGetValue(key, out string? value);
        return XmlToHtml(value);
    }

    public string? GetSummary(FieldInfo info)
    {
        string key = $"F:{info.DeclaringType!.FullName}.{info.Name}";
        DocsByKey.TryGetValue(key, out string? value);
        return XmlToHtml(value);
    }
}
