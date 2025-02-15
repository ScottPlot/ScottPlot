using System.Reflection;

namespace ScottPlotCookbook.ApiDocs;

public class MethodParameterDocs
{
    public string Name { get; }
    public TypeName TypeName { get; }

    public MethodParameterDocs(ParameterInfo pi, XmlDocsDB docsDb)
    {
        Name = pi.Name ?? "ANONYMOUS";
        TypeName = new(pi.ParameterType);
    }
}
