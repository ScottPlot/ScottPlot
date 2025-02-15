using System.Data;
using System.Reflection;

namespace ScottPlotCookbook.ApiDocs;

internal class ApiDocs
{
    readonly XmlDocsDB XmlDocsDB;
    private Type[] AssemblyTypes = [];

    public ApiDocs(Type typeInAssembly, string xmlFilePath)
    {
        // TODO: improve appearance of delegates and List<T>

        XmlDocsDB = new(xmlFilePath);
        AssemblyTypes = Assembly.GetAssembly(typeInAssembly)!
            .GetTypes()
            .Where(x => x.FullName is not null && x.FullName.StartsWith("ScottPlot."))
            .Where(x => x.FullName is not null && !x.FullName.StartsWith("ScottPlot.NamedColors."))
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

    public string GetMarkdown()
    {
        return
            $"""
            ---
            Title: ScottPlot 5.0 API
            Description: All classes, fields, properties, and methods provided by the ScottPlot package
            URL: /api/5.0/
            Date: {DateTime.Now.Year:0000}-{DateTime.Now.Month:00}-{DateTime.Now.Day:00}
            ShowEditLink: false
            ---
            
            # ScottPlot {ScottPlot.Version.VersionString} API

            _Generated {DateTime.Now}_

            <div class='my-5'>&nbsp;</div>

            {GetHtml()}
            """;
    }

    public string GetHtml()
    {
        StringBuilder sb = new();
        sb.AppendLine("""
            <style>
            body {font-family: sans-serif;}
            .title{font-family: monospace; font-size: 1.5em; font-weight: 600;}
            .otherType{color: blue; font-family: monospace;}
            .scottPlotType{color: blue; font-family: monospace;}
            .name{color: black; font-family: monospace;}
            .docs{color: green; font-family: monospace;}
            a {text-decoration: none;}
            a:hover {text-decoration: underline;}
            .break-container{width:95vw;position:relative;left:calc(-1 * (95vw - 95%)/2);}
            </style>
            """);

        sb.AppendLine("<div class='break-container'>");

        foreach (Type type in AssemblyTypes)
        {
            // the type itself
            ClassDocs classDocs = new(type, XmlDocsDB);
            sb.AppendLine($"<div style='margin-top: 2em'>");
            sb.AppendLine($"<div class='title' id='{classDocs.TypeName.CleanNameHtml}'>" +
                $"<a style='color: black;' href='#{classDocs.TypeName.CleanNameHtml}'>" +
                $"{classDocs.TypeName.CleanNameHtml}" +
                $"</a>" +
                $"</div>");
            sb.AppendLine($"<div class='docs'>{classDocs.Docs}</div>");
            sb.AppendLine($"</div>");

            foreach (PropertyDocs propDocs in classDocs.GetPropertyDocs(XmlDocsDB))
            {
                string typeHtml = propDocs.TypeName.CleanNameHtml.StartsWith("ScottPlot.")
                    ? $"<a class='scottPlotType' href='#{propDocs.TypeName.CleanNameHtml}'>{propDocs.TypeName.CleanNameHtml}</a>"
                    : $"<span class='otherType'>{propDocs.TypeName.CleanNameHtml}</span>";
                string nameHtml = $"<span class='name'>{propDocs.Name}</span>";
                string docsHtml = $"<span class='docs'>{propDocs.Docs}</span>";
                sb.AppendLine($"<div>{typeHtml} {nameHtml} {docsHtml}</div>");
            }

            foreach (MethodDocs methodDocs in classDocs.GetMethodDocs(XmlDocsDB))
            {
                List<string> argsHtml = [];

                foreach (var p in methodDocs.Parameters)
                {
                    string typeHtml2 = p.TypeName.CleanNameHtml.StartsWith("ScottPlot.")
                        ? $"<a class='scottPlotType' href='#{p.TypeName.CleanNameHtml}'>{p.TypeName.CleanNameHtml}</a>"
                        : $"<span class='otherType'>{p.TypeName.CleanNameHtml}</span>";
                    string argHtml = $"<span class='name'>{p.Name}</span>";
                    argsHtml.Add($"{typeHtml2} {argHtml}");
                }

                string argLine = string.Join(", ", argsHtml);

                string typeHtml = methodDocs.ReturnTypeName.CleanNameHtml.StartsWith("ScottPlot.")
                    ? $"<a class='scottPlotType' href='#{methodDocs.ReturnTypeName.CleanNameHtml}'>{methodDocs.ReturnTypeName.CleanNameHtml}</a>"
                    : $"<span class='otherType'>{methodDocs.ReturnTypeName.CleanNameHtml}</span>";
                string nameHtml = $"<span class='name'>{methodDocs.Name}({argLine})</span>";
                string docsHtml = $"<span class='docs'>{methodDocs.Docs}</span>";

                sb.AppendLine($"<div>{typeHtml} {nameHtml} {docsHtml}</div>");
            }
        }

        sb.AppendLine("</div>");

        return sb.ToString();
    }
}
