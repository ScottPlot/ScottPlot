using System.Reflection;
using System.Runtime.Serialization;

namespace WinForms_Demo;

public static class DemoWindows
{
    public static List<IDemoWindow> GetDemoWindows()
    {
        List<IDemoWindow> windows = Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(x => !x.IsAbstract)
            .Where(x => x.IsSubclassOf(typeof(Form)))
            .Where(x => x.GetInterfaces().Contains(typeof(IDemoWindow)))
            .Select(x => (IDemoWindow)FormatterServices.GetUninitializedObject(x))
            .ToList();

        void MoveToTop(Type targetType)
        {
            IDemoWindow targetWindow = windows.Where(x => x.GetType() == targetType).Single();
            windows.Remove(targetWindow);
            windows.Insert(0, targetWindow);
        }

        void MoveToBottom(Type targetType)
        {
            IDemoWindow targetWindow = windows.Where(x => x.GetType() == targetType).Single();
            windows.Remove(targetWindow);
            windows.Add(targetWindow);
        }

        MoveToTop(typeof(Demos.DraggablePoints));
        MoveToTop(typeof(Demos.DraggableAxisLines));
        MoveToTop(typeof(Demos.MouseTracker));
        MoveToTop(typeof(Demos.CookbookViewer));

        MoveToBottom(typeof(Demos.OpenGL));

        return windows;
    }
}
