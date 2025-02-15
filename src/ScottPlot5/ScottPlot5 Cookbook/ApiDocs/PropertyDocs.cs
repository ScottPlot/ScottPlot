using System.Reflection;

namespace ScottPlotCookbook.ApiDocs;

public class PropertyDocs
{
    public string Name { get; }
    public TypeName TypeName { get; }
    public string? Docs { get; private set; }

    public PropertyDocs(FieldInfo fi, XmlDocsDB docsDb)
    {
        Name = fi.Name;
        TypeName = new(fi.FieldType);
        Docs = docsDb.GetSummary(fi);
    }

    public PropertyDocs(PropertyInfo pi, XmlDocsDB docsDb)
    {
        Name = pi.Name;
        TypeName = new(pi.PropertyType);
        Docs = docsDb.GetSummary(pi);
    }
}
