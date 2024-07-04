using Microsoft.VisualStudio.TestPlatform.Common.Utilities;
using System;
using System.Data;
using System.Diagnostics;
using System.Reflection;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;

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

        static string GetTypeName(Type type)
        {
            return GetName(type.Name);
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
                "Void" => "void",
                "Int32" => "int",
                "Double" => "double",
                "Float" => "float",
                "Boolean" => "bool",
                "String" => "string",
                "Object" => "object",
                _ => name.Replace("`1", "&lt;T&gt;"),
            };
        }

        foreach (Type type in AssemblyTypes)
        {
            // the type itself
            sb.AppendLine($"<div style='margin: 2em 0 1em 0;'>");
            sb.AppendLine($"<div class='title'>{GetTypeName(type)}</div>");
            sb.AppendLine($"<div class='docs'>{XmlDocsDB.GetSummary(type)}</div>");
            sb.AppendLine($"</div>");

            foreach (PropertyInfo pi in type.GetProperties())
            {
                sb.AppendLine($"<div>" +
                    $"<span class='type'>{GetTypeName(pi.PropertyType)}</span> " +
                    $"<span class='name'>{pi.Name}</span> " +
                    $"<span class='docs'>{XmlDocsDB.GetSummary(pi)}</span> " +
                    $"</div>");
            }

            foreach (FieldInfo fi in type.GetFields())
            {
                sb.AppendLine($"<div>" +
                    $"<span class='type'>{GetTypeName(fi.FieldType)}</span> " +
                    $"<span class='name'>{fi.Name}</span> " +
                    $"<span class='docs'>{XmlDocsDB.GetSummary(fi)}</span> " +
                    $"</div>");
            }

            foreach (MethodInfo mi in type.GetMethods())
            {
                if (mi.Name.StartsWith("get_")) continue;
                if (mi.Name.StartsWith("set_")) continue;

                string[] argNames = mi.GetParameters()
                    .Select(GetParameterName)
                    .ToArray();

                string[] argTypeNames = mi.GetParameters()
                    .Select(GetParameterTypeName)
                    .ToArray();

                string[] args = Enumerable
                    .Range(0, argNames.Length)
                    .Select(x =>
                        $"<span class='type'>{argTypeNames[x]}</span> " +
                        $"<span class='name'>{argNames[x]}</span>")
                    .ToArray();

                string argLine = string.Join(", ", args);

                sb.AppendLine($"<div>" +
                    $"<span class='type'>{GetTypeName(mi.ReturnType)}</span> " +
                    $"<span class='name'>{mi.Name}({argLine})</span> " +
                    $"<span class='docs'>{XmlDocsDB.GetSummary(mi)}</span> " +
                    $"</div>");
            }
        }

        sb.AppendLine("</body></html>");

        return sb.ToString();
    }
}
