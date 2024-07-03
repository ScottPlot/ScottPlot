using System.Data;
using System.Xml;

namespace ScottPlotTests.ApiDocs;

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

    public string GetSummary(Type type)
    {
        string key = $"T:{type.FullName}";
        DocsByKey.TryGetValue(key, out string? value);
        return value ?? "unknown";
    }
}
