using ScottPlot5_WinForms_Demo;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;

namespace WinForms_Demo;

public static class DemoWindows
{
    public static Dictionary<string, Type> GetDemoTypesByTitle() => GetDemoTypes()
        .Select(x => (IDemoForm)FormatterServices.GetUninitializedObject(x))
        .ToDictionary(keySelector: x => x.Title, elementSelector: x => x.GetType());

    public static Type[] GetDemoTypes() => Assembly.GetExecutingAssembly()
        .GetTypes()
        .Where(x => !x.IsAbstract)
        .Where(x => x.IsSubclassOf(typeof(Form)))
        .Where(x => x.GetInterfaces().Contains(typeof(IDemoForm)))
        .ToArray();
}
