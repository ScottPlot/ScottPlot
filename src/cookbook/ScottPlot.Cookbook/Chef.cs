using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ScottPlot.Cookbook
{
    /// <summary>
    /// The Chef can execute recipes (making Bitmaps) and describe recipes (reporting source code)
    /// </summary>
    public class Chef
    {
        public int Width = 600;
        public int Height = 400;

        const string Ext = ".png";
        const string ExtThumb = "_thumb.jpg";
        const string ExecutionMethod = "public void ExecuteRecipe";

        /// <summary>
        /// Use reflection to determine all IRecipe objects in the project, execute each of them, 
        /// and save the output using the recipe ID as its base filename.
        /// </summary>
        public void CreateCookbookImages(string outputPath)
        {
            outputPath = Path.GetFullPath(outputPath);
            if (!Directory.Exists(outputPath))
                Directory.CreateDirectory(outputPath);

            var recipes = Locate.GetRecipes();
            Console.WriteLine($"Cooking {recipes.Length} recipes in: {outputPath}");

            Parallel.ForEach(recipes, recipe =>
            {
                Debug.WriteLine($"Executing {recipe.ID}");
                var plt = new Plot(Width, Height);
                recipe.ExecuteRecipe(plt);

                // save full size image
                Bitmap bmp = plt.Render();
                string fileName = (recipe.ID + Ext).ToLower();
                string filePath = Path.Combine(outputPath, fileName);
                bmp.Save(filePath, System.Drawing.Imaging.ImageFormat.Png);

                // thumbnail
                int thumbHeight = 180;
                int thumbWidth = thumbHeight * bmp.Width / bmp.Height;
                Bitmap thumb = Drawing.GDI.Resize(bmp, thumbWidth, thumbHeight);
                string thumbFileName = (recipe.ID + ExtThumb).ToLower();
                string thumbFilePath = Path.Combine(outputPath, thumbFileName);
                thumb.Save(thumbFilePath, System.Drawing.Imaging.ImageFormat.Jpeg);
            });

        }

        /// <summary>
        /// Read all .cs files in the source folder to identify IRecipe source code, isolate just the recipe 
        /// component of each source file, and save the output as a text file using the recipe ID as its base filename.
        /// </summary>
        public void CreateCookbookSource(string sourcePath, string outputPath)
        {
            outputPath = Path.GetFullPath(outputPath);
            if (!Directory.Exists(outputPath))
                throw new ArgumentException($"output path does not exist: {outputPath}");

            var sources = GetRecipeSources(sourcePath);
            Console.WriteLine($"Creating source code for {sources.Count} recipes in: {outputPath}");
            Parallel.ForEach(sources, recipe =>
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("// " + recipe.title);
                sb.AppendLine("// " + recipe.description);
                sb.Append(recipe.source);

                string fileName = recipe.id + ".cs";
                string filePath = Path.Combine(outputPath, fileName.ToLower());
                File.WriteAllText(filePath, sb.ToString());
            });
        }

        /// <summary>
        /// Use a combination of file reading and reflection to get fields and source code for all recipes
        /// </summary>
        public List<(string id, string title, string description, string source)> GetRecipeSources(string sourcePath)
        {
            sourcePath = Path.GetFullPath(sourcePath);
            if (!File.Exists(Path.Combine(sourcePath, "IRecipe.cs")))
                throw new ArgumentException("IRecipe.cs can not be found in the given source colder");

            var sources = new List<(string id, string title, string description, string source)>();

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
                        throw new InvalidOperationException($"recipe structure error in: {csFilePath}");

                    // read the file's source code for primary recipe components
                    string id = GetRecipeID(singleClassSourceCode);
                    IRecipe recipe = Locate.GetRecipe(id);
                    string source = $"var plt = new ScottPlot.Plot({Width}, {Height});\n\n" +
                                    GetRecipeSource(singleClassSourceCode, csFilePath) + "\n\n" +
                                    $"plt.SaveFig(\"{id}{Ext}\");";

                    sources.Add((recipe.ID, recipe.Title, recipe.Description, source));
                }
            }

            return sources;
        }

        /// <summary>
        /// Given source code for a recipe class, return its ID
        /// </summary>
        private string GetRecipeID(string classSource)
        {
            string idDefinition = "public string ID =>";
            int idDefinitionCount = Regex.Matches(classSource, idDefinition).Count;
            if (idDefinitionCount == 0)
                throw new InvalidOperationException("recipe does not contain ID");
            if (idDefinitionCount > 1)
                throw new InvalidOperationException("recipe has more than one ID");

            return classSource
                .Split('\n')
                .Where(x => x.Contains(idDefinition))
                .First()
                .Split('>')
                .Last()
                .Trim()
                .Trim(';')
                .Trim('"');
        }

        /// <summary>
        /// Given source code for a recipe class, return its prettified execution source code
        /// </summary>
        private string GetRecipeSource(string classSource, string filePath)
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
