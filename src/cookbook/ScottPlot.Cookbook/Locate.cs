using ScottPlot.Plottable;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ScottPlot.Cookbook
{
    public static class Locate
    {
        private static readonly IRecipe[] _recipes = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(s => s.GetTypes())
            .Where(x => x.IsAbstract == false)
            .Where(x => x.IsInterface == false)
            .Where(p => typeof(IRecipe).IsAssignableFrom(p))
            .Select(x => (IRecipe)Activator.CreateInstance(x))
            .ToArray();

        private static readonly string[] _categories = _recipes.Select(r => r.Category).Distinct().ToArray();
        private static Dictionary<string, IRecipe[]> _recipesByCategory = new Dictionary<string, IRecipe[]>();
        private static Dictionary<string, IRecipe> _recipesById = new Dictionary<string, IRecipe>();


        static Locate() // A static constructor runs exactly once and before the class or an instance of it is needed
        {
            InitializeDictionaries();
        }

        private static void InitializeDictionaries()
        {
            Dictionary<string, List<IRecipe>> byCategory = new Dictionary<string, List<IRecipe>>();
            foreach (IRecipe curr in GetRecipes())
            {
                _recipesById[curr.ID] = curr;

                if (!byCategory.ContainsKey(curr.Category))
                {
                    byCategory.Add(curr.Category, new List<IRecipe>());
                }

                byCategory[curr.Category].Add(curr);
            }

            foreach (string category in GetCategories())
            {
                _recipesByCategory[category] = byCategory[category].ToArray();
            }
        }

        public static IRecipe[] GetRecipes() => _recipes;

        public static IRecipe GetRecipe(string id) => _recipesById[id];

        public static string[] GetCategories() => _categories;

        public static string[] GetCategoriesInDisplayOrder() => GetCategorizedRecipes().Select(x => x.Key).ToArray();

        public static IRecipe[] GetRecipes(string category) => _recipesByCategory[category];

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
                "Axis and Ticks",
                "Advanced Axis Features",
                "Multi-Axis",
            };

            foreach (string category in topCategories.Reverse())
            {
                var moveThis = categorizedRecipeList.Where(x => x.Key == category).First(); ;
                categorizedRecipeList.Remove(moveThis);
                categorizedRecipeList.Insert(0, moveThis);
            }

            string[] bottomCategories =
            {
                "Style",
                "Misc"
            };

            foreach (string category in bottomCategories)
            {
                var moveThis = categorizedRecipeList.Where(x => x.Key == category).First(); ;
                categorizedRecipeList.Remove(moveThis);
                categorizedRecipeList.Add(moveThis);
            }

            return categorizedRecipeList;
        }

        public static string RecipeSourceCode(IRecipe recipe) => RecipeSourceCode(recipe.ID);

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

            // Return a message indicating exactly what the user can do to fix the problem.
            StringBuilder sb2 = new StringBuilder();
            sb2.AppendLine($"ERROR: Recipe information file was not found!");
            sb2.AppendLine($"Developers can generate these files by running the tests:");
            sb2.AppendLine($"To run tests from Visual Studio, click 'Test' and select 'Run All Tests'.");
            sb2.AppendLine($"To run tests from the command line, run 'dotnet test' in the src folder.");
            return sb2.ToString();
        }

        public static Type[] GetPlottableTypes() =>
            Assembly.GetAssembly(typeof(Plottable.ScatterPlot))
                     .GetTypes()
                     .Where(x => x.Namespace == typeof(Plottable.ScatterPlot).Namespace)
                     .Where(x => x.GetInterfaces().Contains(typeof(Plottable.IPlottable)))
                     .Where(x => !x.IsAbstract)
                     .Where(x => !x.GetCustomAttributes<ObsoleteAttribute>().Any())
                     .ToArray();

        public static string TypeName(Type type, bool urlSafe = false)
        {
            string name = type.Name.Split('`')[0];
            if (type.IsGenericType)
                name += urlSafe ? "-T" : "<T>";
            return name;
        }
        public static PropertyInfo[] GetNotablePlottableProperties(Type plottableType)
        {
            return plottableType.GetProperties()
                                .Where(x => !x.GetCustomAttributes<ObsoleteAttribute>().Any())
                                .ToArray();
        }

        public static FieldInfo[] GetNotablePlottableFields(Type plottableType)
        {
            return plottableType.GetFields()
                                .Where(x => !x.GetCustomAttributes<ObsoleteAttribute>().Any())
                                .ToArray();
        }

        public static MethodInfo[] GetNotablePlottableMethods(Type plottableType)
        {
            string[] interfaceMethodNames = typeof(IPlottable).GetMethods().Select(x => x.Name).ToArray();
            string[] ignoredMethodNames = { "ToString", "GetType", "Equals", "GetHashCode" };
            return plottableType.GetMethods()
                                .Where(x => !interfaceMethodNames.Contains(x.Name))
                                .Where(x => !ignoredMethodNames.Contains(x.Name))
                                .Where(x => !x.Name.StartsWith("get_")) // auto-properties
                                .Where(x => !x.Name.StartsWith("set_")) // auto-properties
                                .Where(x => !x.Name.StartsWith("add_")) // events
                                .Where(x => !x.Name.StartsWith("remove_")) // events
                                .Where(x => !x.GetCustomAttributes<ObsoleteAttribute>().Any())
                                .ToArray();
        }

        public static MethodInfo[] GetPlotMethods() =>
            typeof(Plot)
            .GetMethods()
            .Where(x => x.IsPublic)
            .Where(x => !x.Name.StartsWith("get_"))
            .Where(x => !x.Name.StartsWith("set_"))
            .Where(x => x.Name != "GetType")
            .Where(x => x.Name != "ToString")
            .Where(x => !x.GetCustomAttributes<ObsoleteAttribute>().Any())
            .OrderBy(x => x.Name)
            .ToArray();

        public static MethodInfo[] GetPlotMethodsOnlyAdd() =>
            GetPlotMethods().Where(x => x.Name.StartsWith("Add")).ToArray();

        public static MethodInfo[] GetPlotMethodsNoAdd() =>
            GetPlotMethods().Where(x => !x.Name.StartsWith("Add")).ToArray();

        public static PropertyInfo[] GetPlotProperties() =>
            typeof(Plot)
            .GetProperties()
            .Where(x => !x.GetCustomAttributes<ObsoleteAttribute>().Any())
            .ToArray();

        public static FieldInfo[] GetPlotFields() =>
            typeof(Plot)
            .GetFields()
            .Where(x => !x.GetCustomAttributes<ObsoleteAttribute>().Any())
            .ToArray();
    }
}
