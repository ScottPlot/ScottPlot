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
        public static IPlotDemo[] GetPlots(string namespaceStartsWith = "ScottPlot.Demo.", bool useDLL = false)
        {
            if (useDLL)
            {
                var plotObjects = Assembly.LoadFrom("ScottPlot.Demo.dll").GetTypes();
                return plotObjects
                    .Where(p => typeof(IPlotDemo).IsAssignableFrom(p))
                    .Where(p => p.IsInterface == false)
                    .Where(p => p.ToString().StartsWith(namespaceStartsWith))
                    .Select(x => x.ToString())
                    .Select(path => GetPlot(path))
                    .ToArray();
            }
            else
            {
                var plotObjects = AppDomain.CurrentDomain.GetAssemblies();
                return plotObjects
                        .SelectMany(s => s.GetTypes())
                        .Where(p => typeof(IPlotDemo).IsAssignableFrom(p))
                        .Where(p => p.IsInterface == false)
                        .Where(p => p.ToString().StartsWith(namespaceStartsWith))
                        .Select(x => x.ToString())
                        .Select(path => GetPlot(path))
                        .ToArray();
            }


        }

        public static IPlotDemo[] GetPlotsInOrder(bool useDLL = false)
        {
            List<IPlotDemo> recipes = new List<IPlotDemo>();

            // define the order of cookbook examples here
            recipes.AddRange(GetPlots("ScottPlot.Demo.Quickstart", useDLL));
            recipes.AddRange(GetPlots("ScottPlot.Demo.PlotTypes", useDLL));
            recipes.AddRange(GetPlots("ScottPlot.Demo.Customize", useDLL));
            recipes.AddRange(GetPlots("ScottPlot.Demo.Advanced", useDLL));
            recipes.AddRange(GetPlots("ScottPlot.Demo.Examples", useDLL));
            recipes.AddRange(GetPlots("ScottPlot.Demo.", useDLL));

            return recipes.GroupBy(recipe => recipe.id)
                          .Select(g => g.First())
                          .ToArray();
        }

        public static List<DemoNodeItem> GetPlotNodeItems(bool expandAndSelectDefaultNode = true)
        {
            IPlotDemo[] plots = GetPlotsInOrder();

            var nodeItems = plots
                .GroupBy(x => x.categoryMajor)
                .Select(majorCategory =>
                    new DemoNodeItem
                    {
                        Header = majorCategory.Key,
                        IsExpanded = true,
                        Items = majorCategory
                            .GroupBy(x => x.categoryMinor)
                            .Select(minorCategory =>
                                new DemoNodeItem
                                {
                                    Header = minorCategory.Key,
                                    IsExpanded = false,
                                    Items = minorCategory
                                        .Select(demoPlot =>
                                                    new DemoNodeItem
                                                    {
                                                        Header = demoPlot.name,
                                                        Tag = demoPlot.classPath.ToString()
                                                    })
                                        .ToList()
                                })
                            .ToList()
                    })
                .ToList();

            if (expandAndSelectDefaultNode)
            {
                nodeItems[0].Items[0].IsExpanded = true;
                nodeItems[0].Items[0].Items[0].IsSelected = true;
            }

            return nodeItems;
        }

        public static IPlotDemo GetPlot(string plotObjectPath)
        {
            if (!plotObjectPath.StartsWith("ScottPlot.Demo."))
                throw new ArgumentException("plot object path must start with 'ScottPlot.Demo.'");

            var type = Type.GetType(plotObjectPath);
            IPlotDemo demoPlot = (IPlotDemo)Activator.CreateInstance(type);
            return demoPlot;
        }

        /// <summary>
        /// return the path to the folder containing IPlotDemo.cs
        /// </summary>
        /// <returns></returns>
        public static string FindDemoSourceFolder()
        {
            const int searchDepth = 10;

            string exePath = Assembly.GetEntryAssembly().Location;
            string folderPath = System.IO.Path.GetDirectoryName(exePath);

            // first try in every folder up from here
            for (int i = 0; i < searchDepth; i++)
            {
                string testPath = folderPath + "/src/ScottPlot.Demo/";
                if (System.IO.File.Exists(testPath + "IPlotDemo.cs"))
                    return System.IO.Path.GetFullPath(testPath);
                else
                    folderPath = System.IO.Path.GetDirectoryName(folderPath);
            }

            // finally try the source folder down from here
            folderPath = System.IO.Path.GetDirectoryName(exePath);
            string sourcePathDown = folderPath + "/source/";
            if (System.IO.File.Exists(sourcePathDown + "IPlotDemo.cs"))
                return System.IO.Path.GetFullPath(sourcePathDown);

            return null;
        }
    }
}
