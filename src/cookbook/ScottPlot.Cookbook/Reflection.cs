using System;
using System.Linq;

namespace ScottPlot.Cookbook
{
    public static class Reflection
    {
        public static Recipe[] GetRecipes() =>
            AppDomain
            .CurrentDomain
            .GetAssemblies()
            .SelectMany(s => s.GetTypes())
            .Where(x => x.IsAbstract == false)
            .Where(p => typeof(Recipe).IsAssignableFrom(p))
            .Select(x => (Recipe)Activator.CreateInstance(x))
            .ToArray();
    }
}
