using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace ScottPlot.Cookbook
{
    public static class SourceParsing
    {
        const string ExecutionMethod = "public void ExecuteRecipe";

        /// <summary>
        /// Use a combination of file reading and reflection to get fields and source code for all recipes
        /// </summary>
        public static RecipeSource[] GetRecipeSources(string sourcePath, int width, int height)
        {
            sourcePath = Path.GetFullPath(sourcePath);
            if (!File.Exists(Path.Combine(sourcePath, "IRecipe.cs")))
                throw new ArgumentException($"IRecipe.cs not be found in: {sourcePath}");

            List<RecipeSource> sources = new();

            string[] projectCsFiles = Directory.GetFiles(sourcePath, "*.cs", SearchOption.AllDirectories);
            foreach (string csFilePath in projectCsFiles)
            {
                string sourceCode = File.ReadAllText(csFilePath);

                // ensure the source code is not from this file
                if (Path.GetFileName(csFilePath) == "Chef.cs")
                    continue;

                // ensure the start of the recipe is in the file
                string recipeStart = ": IRecipe";
                if (!sourceCode.Contains(recipeStart))
                    continue;

                // isolate individual recipes from files with multiple recipes
                string[] sourceCodeByClass = sourceCode.Split(
                    separator: new string[] { recipeStart },
                    options: StringSplitOptions.RemoveEmptyEntries)
                    .Skip(1)
                    .ToArray();

                foreach (string singleClassSourceCode in sourceCodeByClass)
                {
                    // ensure functions are at the correct indentation level
                    int executionMethodCount = Regex.Matches(singleClassSourceCode, ExecutionMethod).Count;
                    string indentedMethod = "\n        " + ExecutionMethod;
                    int indentedMethodCount = Regex.Matches(singleClassSourceCode, indentedMethod).Count;
                    if (executionMethodCount != indentedMethodCount)
                        throw new InvalidOperationException($"Source code parsing error in: {csFilePath}\n\n" +
                            "This is typically caused by an error in indentation and whitespace before '{ExecutionMethod}'.\n\n" +
                            "Ensure cookbook classes are standalone classes not encased by another class.");

                    // read the file's source code for primary recipe components
                    string id = GetRecipeID(singleClassSourceCode);
                    if (string.IsNullOrWhiteSpace(id))
                        continue;

                    StringBuilder sb = new();
                    sb.AppendLine($"ScottPlot.Version.ShouldBe({ScottPlot.Version.ShortString.Replace(".", ", ")});");
                    sb.AppendLine($"var plt = new ScottPlot.Plot({width}, {height});");
                    sb.AppendLine();
                    sb.AppendLine(GetRecipeSource(singleClassSourceCode, csFilePath));
                    sb.AppendLine();
                    sb.Append($"plt.SaveFig(\"{id}.png\");");

                    IRecipe recipe = Locate.GetRecipe(id);
                    sources.Add(new RecipeSource(recipe, sb.ToString()));
                }
            }

            return sources.ToArray();
        }

        /// <summary>
        /// Given source code for a recipe class, return its ID
        /// </summary>
        private static string GetRecipeID(string classSource)
        {
            string idDefinition = "public string ID =>";
            int idDefinitionCount = Regex.Matches(classSource, idDefinition).Count;
            if (idDefinitionCount == 0)
                throw new InvalidOperationException("recipe does not contain ID");
            if (idDefinitionCount > 1)
                throw new InvalidOperationException("recipe has more than one ID");

            string id = classSource
                .Split('\n')
                .Where(x => x.Contains(idDefinition))
                .First()
                .Split('>')
                .Last()
                .Trim()
                .Trim(';')
                .Trim('"');

            return id;
        }

        /// <summary>
        /// Given source code for a recipe class, return its prettified execution source code
        /// </summary>
        private static string GetRecipeSource(string classSource, string filePath)
        {
            int executionMethodCount = Regex.Matches(classSource, ExecutionMethod).Count;
            if (executionMethodCount == 0)
                throw new InvalidOperationException($"cannot located execution method in {filePath}");
            if (executionMethodCount > 1)
                throw new InvalidOperationException($"recipe has more than one execution method in {filePath}");

            string[] lines = classSource.Split('\n');

            int indexOfRecipeStart = Enumerable.Range(0, lines.Length)
                                               .Where(x => lines[x].Contains(ExecutionMethod))
                                               .First();

            string twelveSpaces = string.Concat(Enumerable.Repeat(" ", 12));

            string[] recipeLines = lines.Skip(indexOfRecipeStart)
                                        .Skip(2)
                                        .Take(lines.Length - indexOfRecipeStart - 6)
                                        .Select(x => x.Replace(twelveSpaces, ""))
                                        .ToArray();

            return string.Join("\n", recipeLines).Trim();
        }
    }
}
