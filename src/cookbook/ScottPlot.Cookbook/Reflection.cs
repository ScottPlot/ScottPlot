using System;
using System.Linq;

namespace ScottPlot.Cookbook
{
    public static class Reflection
    {
        public static IRecipe[] GetRecipes() =>
            AppDomain
            .CurrentDomain
            .GetAssemblies()
            .SelectMany(s => s.GetTypes())
            .Where(x => x.IsAbstract == false)
            .Where(x => x.IsInterface == false)
            .Where(p => typeof(IRecipe).IsAssignableFrom(p))
            .Select(x => (IRecipe)Activator.CreateInstance(x))
            .ToArray();

        public static IRecipe GetRecipe(string id) => GetRecipes().Where(x => x.ID == id).First();
    }
}
