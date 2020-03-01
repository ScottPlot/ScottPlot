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
        public static IPlotDemo[] GetPlots(string namespaceStartsWith = "ScottPlot.Demo.")
        {
            var plotObjectPaths = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => typeof(IPlotDemo).IsAssignableFrom(p))
                .Where(p => p.IsInterface == false)
                .Where(p => p.ToString().StartsWith(namespaceStartsWith))
                .Select(x => x.ToString())
                .ToArray();

            IPlotDemo[] plots = new IPlotDemo[plotObjectPaths.Length];
            for (int i = 0; i < plotObjectPaths.Length; i++)
                plots[i] = GetPlot(plotObjectPaths[i]);

            return plots;
        }

        public static IPlotDemo GetPlot(string plotObjectPath)
        {
            if (!plotObjectPath.StartsWith("ScottPlot.Demo."))
                throw new ArgumentException("plot object path must start with 'ScottPlot.Demo.'");

            var type = Type.GetType(plotObjectPath);
            IPlotDemo demoPlot = (IPlotDemo)Activator.CreateInstance(type);
            return demoPlot;
        }
    }
}
