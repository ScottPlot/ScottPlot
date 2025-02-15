namespace ScottPlotCookbook.ApiDocs;

public class TypeName
{
    public string Name { get; }
    public string FullName { get; }
    public string CleanName { get; }
    public string CleanNameHtml => CleanName.Replace("<", "&lt;").Replace(">", "&gt;");

    public TypeName(Type type)
    {
        Type? nullableType = Nullable.GetUnderlyingType(type);
        if (nullableType is not null)
        {
            FullName = nullableType.FullName ?? nullableType.Name;
            Name = FullName.Split(".").Last();
            CleanName = GetCleanName(FullName) + "?";
        }
        else
        {
            FullName = type.FullName ?? type.Name;
            Name = FullName.Split(".").Last();
            CleanName = GetCleanName(FullName);
        }

        if (type.IsGenericType)
        {
            Type genericType = type.GetGenericArguments()[0];
            string genericTypeCleanName = GetCleanName(genericType.FullName ?? genericType.Name);
            CleanName = CleanName.Replace("<T>", $"<{genericTypeCleanName}>");
        }
    }

    private string GetCleanName(string name)
    {
        // TODO: show the type of <T> where possible

        name = name switch
        {
            "System.Void" => "void",
            "System.Threading.Tasks.Task" => "Task",
            "System.Byte" => "byte",
            "System.Byte[]" => "byte[]",
            "System.Byte[,]" => "byte[,]",
            "System.Byte[,,]" => "byte[,,]",
            "System.Char" => "char",
            "System.Char[]" => "char[]",
            "System.UInt32" => "uint",
            "System.UInt32[]" => "uint[]",
            "System.Int16" => "Int16",
            "System.Int32" => "int",
            "System.Int32[]" => "int[]",
            "System.Double" => "double",
            "System.Double[]" => "double[]",
            "System.Double[,]" => "double[,]",
            "System.Double[,,]" => "double[,,]",
            "System.Single" => "single",
            "System.Float" => "float",
            "System.Float[]" => "float[]",
            "System.Boolean" => "bool",
            "System.String" => "string",
            "System.String[]" => "string[]",
            "System.TimeSpan" => "TimeSpan",
            "System.DateTime" => "DateTime",
            "System.DateTime[]" => "DateTime[]",
            "System.Object" => "object",
            "System.Type" => "type",
            _ => name,
        };

        return name
            .Replace("System.EventHandler", "EventHandler")
            .Replace("System.Func", "Func")
            .Replace("System.Action", "Action")
            .Replace("System.Collections.Generic.", "")
            .Replace("`1", "<T>")
            .Replace("`2", "<T1, T2>")
            .Replace("`3", "<T1, T2, T3>")
            .Replace("System.Nullable<T>", "T?")
            .Replace("System.ValueTuple", "ValueTuple")
            .Split("[[")[0]
            .Split("+")[0];
    }
}
