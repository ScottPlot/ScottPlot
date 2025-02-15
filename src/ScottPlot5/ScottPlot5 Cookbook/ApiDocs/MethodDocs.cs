using System.Reflection;

namespace ScottPlotCookbook.ApiDocs;

public class MethodDocs
{
    public string Name { get; }
    public TypeName ReturnTypeName { get; }
    public string? Docs { get; private set; }
    public MethodParameterDocs[] Parameters { get; }

    public MethodDocs(MethodInfo fi, XmlDocsDB docsDb)
    {
        Name = fi.Name;
        ReturnTypeName = new(fi.ReturnType);
        Docs = docsDb.GetSummary(fi);
        Parameters = fi.GetParameters().Select(x => new MethodParameterDocs(x, docsDb)).ToArray();
    }
}
