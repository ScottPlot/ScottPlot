namespace ScottPlotCookbook.ApiDocs;

public class ClassDocs
{
    public Type Type { get; }
    public TypeName TypeName { get; }
    public string? Docs { get; }

    public ClassDocs(Type type, XmlDocsDB docsDb)
    {
        Type = type;
        TypeName = new(Type);
        Docs = docsDb.GetSummary(type);
    }

    public PropertyDocs[] GetPropertyDocs(XmlDocsDB docsDb)
    {
        string[] ignoredNames = [
            "value__",
            "handle",
            "gcHandle",
        ];

        var fieldDocs = Type.GetFields().Select(x => new PropertyDocs(x, docsDb));
        var propertyDocs = Type.GetProperties().Select(x => new PropertyDocs(x, docsDb));

        return fieldDocs
            .Concat(propertyDocs)
            .Where(x => !ignoredNames.Contains(x.Name))
            .OrderBy(x => x.Name)
            .ToArray();
    }

    public MethodDocs[] GetMethodDocs(XmlDocsDB docsDb)
    {
        string[] ignoredNames = [
            "_value",
            "Equals",
            "HasFlag",
            "GetHashCode",
            "ToString",
            "CompareTo",
            "GetTypeCode",
            "GetType",
            "op_Inequality",
            "op_Equality",
            "Deconstruct",
        ];

        return Type.GetMethods()
            .Select(x => new MethodDocs(x, docsDb))
            .Where(x => !x.Name.StartsWith("get_"))
            .Where(x => !x.Name.StartsWith("set_"))
            .Where(x => !ignoredNames.Contains(x.Name))
            .ToArray();
    }
}
