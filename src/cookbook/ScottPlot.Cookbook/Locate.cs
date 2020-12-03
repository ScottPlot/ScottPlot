using System;
using System.Collections.Generic;
using System.Linq;

namespace ScottPlot.Cookbook
{
    public static class Locate
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

        public static string[] GetCategories() => GetRecipes().Select(x => x.Category).Distinct().ToArray();

        public static IRecipe[] GetRecipes(string category) => GetRecipes().Where(x => x.Category == category).ToArray();

        public static List<KeyValuePair<string, IRecipe[]>> GetCategorizedRecipes()
        {
            var categorizedRecipeList = new List<KeyValuePair<string, IRecipe[]>>();

            foreach (string category in GetCategories())
            {
                var recipesForCategory = new KeyValuePair<string, IRecipe[]>(category, GetRecipes(category));
                categorizedRecipeList.Add(recipesForCategory);
            }

            // move quickstart to top
            string moveCategory = "Quickstart";
            var moveThis = categorizedRecipeList.Where(x => x.Key == moveCategory).First(); ;
            categorizedRecipeList.Remove(moveThis);
            categorizedRecipeList.Insert(0, moveThis);

            return categorizedRecipeList;
        }

        public static string RecipeSourceCode(string id)
        {
            return $"fake source for {id}";
        }
    }
}
