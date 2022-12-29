using System.Reflection;
using System.Runtime.Serialization;

namespace WinForms_Demo;

public static class DemoWindows
{
    public static Dictionary<string, Type> GetDemoTypesByTitle() => GetDemoTypes()
        .Select(x => (IDemoWindow)FormatterServices.GetUninitializedObject(x))
        .ToDictionary(keySelector: x => x.Title, elementSelector: x => x.GetType());

    public static Type[] GetDemoTypes() => Assembly.GetExecutingAssembly()
        .GetTypes()
        .Where(x => !x.IsAbstract)
        .Where(x => x.IsSubclassOf(typeof(Form)))
        .Where(x => x.GetInterfaces().Contains(typeof(IDemoWindow)))
        .ToArray();
}
