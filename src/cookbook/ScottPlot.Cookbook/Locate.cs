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
        private static Dictionary<string, IRecipe[]> _recipesByCategory = GetRecipes().GroupBy(x => x.Category).ToDictionary(gk => gk.Key, gk => gk.ToArray());
        private static Dictionary<string, IRecipe> _recipesById = GetRecipes().ToDictionary(r => r.ID, r => r);
        private static List<KeyValuePair<string, IRecipe[]>> _categorizedRecipeList;

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


        static Locate() // A static constructor runs exactly once and before the class or an instance of it is needed
        {
            InitializeCategoryList();
        }

        private static void InitializeCategoryList()
        {
            _categorizedRecipeList = _recipesByCategory.OrderBy(CategoryIndex).ThenBy(IndexWithinCategory).ToList();
        }

        public static IRecipe[] GetRecipes() => _recipes;

        public static IRecipe GetRecipe(string id) => _recipesById[id];

        public static string[] GetCategories() => _categories;

        public static string[] GetCategoriesInDisplayOrder() => GetCategorizedRecipes().Select(x => x.Key).ToArray();

        public static IRecipe[] GetRecipes(string category) => _recipesByCategory[category];

        public static List<KeyValuePair<string, IRecipe[]>> GetCategorizedRecipes() => _categorizedRecipeList;

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

        private static readonly Type[] _plottableTypes = Assembly.GetAssembly(typeof(Plottable.ScatterPlot))
                     .GetTypes()
                     .Where(x => x.Namespace == typeof(Plottable.ScatterPlot).Namespace)
                     .Where(x => x.GetInterfaces().Contains(typeof(Plottable.IPlottable)))
                     .Where(x => !x.IsAbstract)
                     .Where(x => !x.GetCustomAttributes<ObsoleteAttribute>().Any())
                     .ToArray();

        public static Type[] GetPlottableTypes() => _plottableTypes;

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

        private static readonly MethodInfo[] _plotMethods = typeof(Plot)
            .GetMethods()
            .Where(x => x.IsPublic)
            .Where(x => !x.Name.StartsWith("get_"))
            .Where(x => !x.Name.StartsWith("set_"))
            .Where(x => x.Name != "GetType")
            .Where(x => x.Name != "ToString")
            .Where(x => !x.GetCustomAttributes<ObsoleteAttribute>().Any())
            .OrderBy(x => x.Name)
            .ToArray();

        public static MethodInfo[] GetPlotMethods() => _plotMethods;

        public static MethodInfo[] GetPlotMethodsOnlyAdd() =>
            GetPlotMethods().Where(x => x.Name.StartsWith("Add")).ToArray();

        public static MethodInfo[] GetPlotMethodsNoAdd() =>
            GetPlotMethods().Where(x => !x.Name.StartsWith("Add")).ToArray();

        private static readonly PropertyInfo[] _plotProperies = typeof(Plot)
            .GetProperties()
            .Where(x => !x.GetCustomAttributes<ObsoleteAttribute>().Any())
            .ToArray();

        public static PropertyInfo[] GetPlotProperties() => _plotProperies;

        private static readonly FieldInfo[] _plotFields = typeof(Plot)
            .GetFields()
            .Where(x => !x.GetCustomAttributes<ObsoleteAttribute>().Any())
            .ToArray();

        public static FieldInfo[] GetPlotFields() => _plotFields;
    }
}
