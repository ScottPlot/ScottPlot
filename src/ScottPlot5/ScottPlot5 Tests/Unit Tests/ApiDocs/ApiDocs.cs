using Microsoft.VisualStudio.TestPlatform.Common.Utilities;
using System;
using System.Data;
using System.Reflection;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

namespace ScottPlotTests.ApiDocs;

internal class ApiDocs
{
    readonly XmlDocsDB XmlDocsDB;
    private Type[] AssemblyTypes = [];

    public ApiDocs(Type typeInAssembly, string xmlFilePath)
    {
        XmlDocsDB = new(xmlFilePath);
        //AssemblyTypes = Assembly.GetAssembly(typeInAssembly)!.GetTypes();
        AssemblyTypes = [
            typeof(ScottPlot.Plot),
            typeof(ScottPlot.PlottableAdder),
            typeof(ScottPlot.AxisManager),
            typeof(ScottPlot.RenderPack),
            typeof(ScottPlot.Rendering.RenderManager),
        ];
    }

    public string GetHtml()
    {
        StringBuilder sb = new();
        sb.AppendLine("""
            <html>
            <head>
            <style>
            body {font-family: sans-serif;}
            blockquote {color: green; }
            .type{color: blue; font-family: monospace;}
            .name{color: black; font-family: monospace;}
            </style>
            </head>
            <body>
            """);

        foreach (Type type in AssemblyTypes)
        {
            // the type itself
            sb.AppendLine($"<h1>{type}</h1>");
            sb.AppendLine($"<blockquote>{XmlDocsDB.GetSummary(type)}</blockquote>");

            foreach (PropertyInfo pi in type.GetProperties())
            {
                sb.AppendLine($"<div>" +
                    $"<span class='type'>{pi.PropertyType.Name}</span> " +
                    $"<span class='name'>{pi.Name}</span> " +
                    $"</div>");
            }

            foreach (FieldInfo fi in type.GetFields())
            {           
                sb.AppendLine($"<div>" +
                    $"<span class='type'>{fi.FieldType.Name}</span> " +
                    $"<span class='name'>{fi.Name}</span> " +
                    $"</div>");
            }

            foreach (MethodInfo mi in type.GetMethods())
            {
                // properties
                if (mi.Name.StartsWith("get_")) continue;
                if (mi.Name.StartsWith("set_")) continue;

                // TODO: figure out the generic type
                string returnTypeName = mi.ReturnType.Name.Replace("`1", "&lt;T&gt;");

                string[] argNames = mi.GetParameters()
                    .Select(x => x.Name!)
                    .ToArray();

                string[] argTypeNames = mi.GetParameters()
                    .Select(x => x.ParameterType.Name)
                    .ToArray();

                string[] args = Enumerable
                    .Range(0, argNames.Length)
                    .Select(x => 
                        $"<span class='type'>{argTypeNames[x]}</span> " +
                        $"<span class='name'>{argNames[x]}</span>")
                    .ToArray();

                string argLine = string.Join(", ", args);

                sb.AppendLine($"<div>" +
                    $"<span class='type'>{returnTypeName}</span> " +
                    $"<span class='name'>{mi.Name}({argLine})</span> " +
                    $"</div>");
            }
        }

        sb.AppendLine("</body></html>");

        return sb.ToString();
    }
}
