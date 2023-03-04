using System;
using System.Collections.Generic;
using System.Linq;

namespace ScottPlot.Cookbook
{
    public static class Locate
    {
        private static readonly IRecipe[] Recipes = TryLocateRecipes();

        /// <summary>
        /// Carefully locate recipes using try/catch to improve support for platforms like Eto
        /// </summary>
        public static IRecipe[] TryLocateRecipes()
        {
            List<IRecipe> recipes = new();

            foreach (var assemblies in AppDomain.CurrentDomain.GetAssemblies())
            {
                try
                {
                    foreach (Type assemblyType in assemblies.GetTypes())
                    {
                        if (assemblyType.IsAbstract || assemblyType.IsInterface)
                            continue;

                        if (typeof(IRecipe).IsAssignableFrom(assemblyType))
                        {
                            IRecipe instantiatedRecipe = (IRecipe)Activator.CreateInstance(assemblyType);
                            recipes.Add(instantiatedRecipe);
                        }
                    }
                }
                catch (Exception ex)
                {
                    if (ex is System.Reflection.ReflectionTypeLoadException)
                    {
                        continue; // Eto seem to bundle something which may not load due to missing dependencies
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            return recipes.ToArray();
        }

        private static Dictionary<string, IRecipe[]> RecipesByCategory = GetRecipes()
            .GroupBy(x => x.Category.Name)
            .OrderBy(x => x.Key.ToLower())
            .ToDictionary(group => group.Key, group => group.ToArray());

        private static Dictionary<string, IRecipe> RecipesByID = GetRecipes()
            .ToDictionary(recipe => recipe.ID, recipe => recipe);

        private static readonly List<KeyValuePair<string, IRecipe[]>> RecipesByCategoryInOrder;

        public static IRecipe[] GetRecipes() => Recipes;

        public static IRecipe GetRecipe(string id) => RecipesByID[id];

        public static IRecipe[] GetRecipes(string category) => RecipesByCategory[category];

        /// <summary>
        /// Returns all recipies by category in proper order (e.g., quickstart is first)
        /// </summary>
        public static List<KeyValuePair<string, IRecipe[]>> GetCategorizedRecipes() => RecipesByCategoryInOrder;

        static Locate() // A static constructor runs exactly once and before the class or an instance of it is needed
        {
            RecipesByCategoryInOrder = RecipesByCategory.OrderBy(CategoryIndex).ThenBy(IndexWithinCategory).ToList();
        }

        private static readonly string[] topCategories =
        {
            "Quickstart",
            "Axis and Ticks",
            "Advanced Axis Features",
            "Multi-Axis",
            "Style",
            "Palette",
        };

        private static readonly string[] bottomCategories =
        {
            "Miscellaneous",
        };

        private static int CategoryIndex(KeyValuePair<string, IRecipe[]> input)
        {
            string category = input.Key;
            if (topCategories.Contains(category))
                return 0;

            if (bottomCategories.Contains(category))
                return 2;

            return 1;
        }

        private static int IndexWithinCategory(KeyValuePair<string, IRecipe[]> input)
        {
            string category = input.Key;

            for (int i = 0; i < topCategories.Length; i++)
                if (topCategories[i] == category)
                    return i;

            for (int i = 0; i < bottomCategories.Length; i++)
                if (topCategories[i] == category)
                    return i;

            return 0;
        }
    }
}
