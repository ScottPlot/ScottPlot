using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ScottPlot.Demo
{
    public static class Reflection
    {
        public static string[] GetDemoPlots(string namespaceStartsWith = "ScottPlot.Demo.")
        {
            var plots = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => typeof(IPlotDemo).IsAssignableFrom(p))
                .Where(p => p.IsInterface == false)
                .Where(p => p.Namespace.StartsWith(namespaceStartsWith))
                .Select(x => x.ToString())
                .ToArray();

            return plots;
        }

        public static IPlotDemo GetPlot(string plotName)
        {
            var type = Type.GetType(plotName);
            var myObject = (IPlotDemo)Activator.CreateInstance(type);
            return myObject;
        }
    }
}
