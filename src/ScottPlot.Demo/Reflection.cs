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

        public static IPlotDemo[] GetPlotsInOrder()
        {
            List<IPlotDemo> recipes = new List<IPlotDemo>();

            // define the order of cookbook examples here
            recipes.AddRange(GetPlots("ScottPlot.Demo.Quickstart"));
            recipes.AddRange(GetPlots("ScottPlot.Demo.PlotTypes"));
            recipes.AddRange(GetPlots("ScottPlot.Demo.Customize"));
            recipes.AddRange(GetPlots("ScottPlot.Demo.Advanced"));
            recipes.AddRange(GetPlots("ScottPlot.Demo.Examples"));
            recipes.AddRange(GetPlots());

            List<string> ids = new List<string>();
            List<IPlotDemo> recipes2 = new List<IPlotDemo>();
            foreach (IPlotDemo recipe in recipes)
            {
                if (!ids.Contains(recipe.id))
                {
                    recipes2.Add(recipe);
                    ids.Add(recipe.id);
                }
            }

            return recipes2.ToArray();
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
