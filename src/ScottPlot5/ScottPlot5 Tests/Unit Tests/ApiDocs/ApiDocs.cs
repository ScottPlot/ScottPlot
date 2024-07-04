using System.Data;
using System.Reflection;
using System.Text;

namespace ScottPlotTests.ApiDocs;

internal class ApiDocs
{
    readonly XmlDocsDB XmlDocsDB;
    private Type[] AssemblyTypes = [];

    public ApiDocs(Type typeInAssembly, string xmlFilePath)
    {
        // TODO: improve appearance of delegates and List<T>

        XmlDocsDB = new(xmlFilePath);
        AssemblyTypes = [
            typeof(ScottPlot.Plot),
            typeof(ScottPlot.PlottableAdder),
            typeof(ScottPlot.AxisManager),
            typeof(ScottPlot.RenderPack),
            typeof(ScottPlot.Rendering.RenderManager),
        ];

        AssemblyTypes = Assembly.GetAssembly(typeInAssembly)!
            .GetTypes()
            .Where(x => x.FullName is not null && x.FullName.StartsWith("ScottPlot."))
            .ToArray();
    }

    static string GetTypeName(Type type)
    {
        return GetName(type.FullName ?? type.Name);
    }

    static string GetParameterTypeName(ParameterInfo pi)
    {
        Type? nullableType = Nullable.GetUnderlyingType(pi.ParameterType);
        if (nullableType is not null)
        {
            return GetName(nullableType.Name) + "?";
        }

        return GetName(pi.Name!);
    }

    static string GetParameterName(ParameterInfo pi)
    {
        return GetName(pi.Name ?? "unknown");
    }

    static string GetName(string name)
    {
        return name switch
        {
            "System.Void" => "void",
            "System.Int32" => "int",
            "System.Double" => "double",
            "System.Float" => "float",
            "System.Boolean" => "bool",
            "System.String" => "string",
            "System.Object" => "object",
            _ => name
                .Replace("`1", "&lt;T&gt;").Split("+")[0]
                .Replace("`2", "&lt;T1, T2&gt;").Split("+")[0]
                .Replace("`3", "&lt;T1, T2, T3&gt;").Split("+")[0],
        };
    }

    public string GetHtml()
    {
        StringBuilder sb = new();
        sb.AppendLine("""
            <html>
            <head>
            <style>
            body {font-family: sans-serif;}
            .title{font-family: monospace; font-size: 1.5em; font-weight: 600;}
            .type{color: blue; font-family: monospace;}
            .name{color: black; font-family: monospace;}
            .docs{color: green; font-family: monospace;}
            </style>
            </head>
            <body>
            """);

        sb.AppendLine($"<h1>ScottPlot {ScottPlot.Version.VersionString} API</h1>");
        sb.AppendLine($"<div>Generated {DateTime.Now}</div>");
        sb.AppendLine($"<hr>");

        foreach (Type type in AssemblyTypes)
        {
            // the type itself
            ClassDocs classDocs = new(type, XmlDocsDB);
            sb.AppendLine($"<div style='margin-top: 2em'>");
            sb.AppendLine($"<div class='title'>{classDocs.TypeName.CleanNameHtml}</div>");
            sb.AppendLine($"<div class='docs'>{classDocs.Docs}</div>");
            sb.AppendLine($"</div>");

            foreach (PropertyDocs propDocs in classDocs.GetPropertyDocs(XmlDocsDB))
            {
                sb.AppendLine($"<div>" +
                    $"<span class='type'>{propDocs.TypeName.CleanNameHtml}</span> " +
                    $"<span class='name'>{propDocs.Name}</span> " +
                    $"<span class='docs'>{propDocs.Docs}</span> " +
                    $"</div>");
            }

            foreach (MethodDocs methodDocs in classDocs.GetMethodDocs(XmlDocsDB))
            {
                string[] args = methodDocs.Parameters
                    .Select(x =>
                        $"<span class='type'>{x.TypeName.CleanNameHtml}</span> " +
                        $"<span class='name'>{x.Name}</span>")
                    .ToArray();

                string argLine = string.Join(", ", args);

                sb.AppendLine($"<div>" +
                    $"<span class='type'>{methodDocs.ReturnTypeName.CleanNameHtml}</span> " +
                    $"<span class='name'>{methodDocs.Name}({argLine})</span> " +
                    $"<span class='docs'>{methodDocs.Docs}</span> " +
                    $"</div>");
            }
        }

        sb.AppendLine("</body></html>");

        return sb.ToString();
    }
}
