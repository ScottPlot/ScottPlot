using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

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

            string[] topCategories =
            {
                "Quickstart",
                "Managing Plottables",
                "Axis",
                "Ticks"
            };

            foreach (string category in topCategories.Reverse())
            {
                var moveThis = categorizedRecipeList.Where(x => x.Key == category).First(); ;
                categorizedRecipeList.Remove(moveThis);
                categorizedRecipeList.Insert(0, moveThis);
            }

            return categorizedRecipeList;
        }

        public static string RecipeSourceCode(string id)
        {
            string[] possiblePaths = {
                "cookbook/source", // same folder as this EXE
                "../../../../../tests/bin/Debug/net5.0/cookbook/source", // tests output if we are running from VS
            };

            StringBuilder sb = new StringBuilder();
            foreach (string possiblePath in possiblePaths)
            {
                string recipeSourceFile = System.IO.Path.Combine(possiblePath, id + ".cs");
                recipeSourceFile = System.IO.Path.GetFullPath(recipeSourceFile);
                sb.AppendLine($"Looking for: {recipeSourceFile}");
                if (System.IO.File.Exists(recipeSourceFile))
                {
                    var codeLines = System.IO.File.ReadLines(recipeSourceFile).Skip(2);
                    var codeText = string.Join(Environment.NewLine, codeLines);
                    return codeText;
                }
            }
            Debug.WriteLine(sb.ToString());
            return "Could not locate cookbook source code. Run tests to generate it.";
        }

        // TODO: use this for all controls
        public static List<DemoNodeItem> GetPlotNodeItems()
        {
            List<DemoNodeItem> nodes = new List<DemoNodeItem>();
            foreach (var dict in GetCategorizedRecipes())
            {
                string category = dict.Key;
                IRecipe[] recipes = dict.Value;

                var categoryNode = new DemoNodeItem() { Header = category, Items = new List<DemoNodeItem>() };
                nodes.Add(categoryNode);

                foreach (IRecipe recipe in recipes)
                {
                    var recipeNode = new DemoNodeItem() { Header = recipe.Title, Tag = recipe.ID };
                    categoryNode.Items.Add(recipeNode);
                }
            }

            return nodes;
        }
    }

    // TODO: use this for all controls
    public class DemoNodeItem
    {
        public string Header { get; set; }
        public string Tag { get; set; }
        public bool IsExpanded { get; set; }
        public bool IsSelected { get; set; }
        public List<DemoNodeItem> Items { get; set; }
    }
}
