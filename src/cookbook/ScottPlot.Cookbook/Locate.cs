using System;
using System.Collections.Generic;
using System.Linq;

namespace ScottPlot.Cookbook
{
    public static class Locate
    {
        private static IRecipe[] LocateRecipes()
        {
            var res = new List<IRecipe>();

            foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
            {
                try // Eto.Forms seem to bundle something which may not load due to missing dependencies
                {
                    foreach (var type in asm.GetTypes())
                    {
                        if (!type.IsAbstract && !type.IsInterface && typeof(IRecipe).IsAssignableFrom(type))
                        {
                            res.Add((IRecipe)Activator.CreateInstance(type));
                        }
                    }
                }
                catch
                {
                }
            }

            return res.ToArray();
        }
        private static readonly IRecipe[] Recipes = LocateRecipes();

        private static Dictionary<string, IRecipe[]> RecipesByCategory = GetRecipes()
            .GroupBy(x => x.Category)
            .ToDictionary(group => group.Key, group => group.ToArray());

        private static Dictionary<string, IRecipe> RecipesByID = GetRecipes()
            .ToDictionary(recipe => recipe.ID, recipe => recipe);

        private static readonly List<KeyValuePair<string, IRecipe[]>> RecipesByCategoryInOrder;

        public static IRecipe[] GetRecipes() => Recipes;

        public static IRecipe GetRecipe(string id) => RecipesByID[id];

        public static IRecipe[] GetRecipes(string category) => RecipesByCategory[category];

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
        };

        private static readonly string[] bottomCategories =
        {
            "Style",
            "Palette",
            "Misc"
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
