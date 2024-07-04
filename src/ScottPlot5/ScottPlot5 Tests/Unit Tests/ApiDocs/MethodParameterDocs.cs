using System.Reflection;

namespace ScottPlotTests.ApiDocs;

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
