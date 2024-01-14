using ScottPlotCookbook.Recipes;
using ScottPlotCookbook.Website;

namespace ScottPlotCookbook;

/// <summary>
/// This class contains logic to pair recipes located using reflection with the source code parsed from .cs files.
/// </summary>
public class SourceDatabase
{
    public readonly List<RecipeInfo> Recipes = new();

    private readonly Dictionary<ICategory, IEnumerable<IRecipe>> RecipesByCategory = Query.GetRecipesByCategory();

    public SourceDatabase()
    {
        foreach (string sourceFilePath in GetRecipeSourceFilePaths())
        {
            IEnumerable<RecipeInfo> recipes = GetRecipeSources(sourceFilePath);
            Recipes.AddRange(recipes);
        }
    }

    public RecipeInfo? GetInfo(IRecipe recipe)
    {
        IEnumerable<RecipeInfo> matching = Recipes.Where(x => x.Name == recipe.Name);
        return matching.Any() ? matching.First() : null;
    }

    private static string[] GetRecipeSourceFilePaths()
    {
        List<string> paths = new();

        if (!Directory.Exists(Paths.RecipeSourceFolder))
            throw new DirectoryNotFoundException(Paths.RecipeSourceFolder);

        paths.AddRange(Directory.GetFiles(Paths.RecipeSourceFolder, "*.cs"));

        foreach (string subFolder in Directory.GetDirectories(Paths.RecipeSourceFolder))
        {
            paths.AddRange(Directory.GetFiles(subFolder, "*.cs"));
        }

        if (!paths.Any())
            throw new InvalidOperationException("no source files found");

        return paths.ToArray();
    }

    private string GetDescription(string recipeName)
    {
        foreach (var kv in RecipesByCategory)
        {
            foreach (IRecipe recipe in kv.Value)
            {
                if (recipe.Name == recipeName)
                    return recipe.Description;
            }
        }

        throw new InvalidOperationException($"unable to locate recipe named {recipeName}");
    }

    private IEnumerable<RecipeInfo> GetRecipeSources(string sourceFilePath)
    {
        string[] rawLines = File.ReadAllLines(sourceFilePath);
        sourceFilePath = sourceFilePath
            .Replace(Paths.RepoFolder, "")
            .Replace("\\", "/")
            .Trim('/')
            .Replace(" ", "%20");

        List<RecipeInfo> recipes = new();

        string recipeClassName = string.Empty;
        string categoryClassName = string.Empty;
        string chapter = string.Empty;
        string category = string.Empty;
        string recipeName = string.Empty;
        StringBuilder source = new();
        bool InRecipe = false;

        foreach (string line in rawLines)
        {
            string trimmedLine = line.Trim();

            if (trimmedLine.StartsWith("public class") && trimmedLine.EndsWith(": ICategory"))
            {
                categoryClassName = trimmedLine.Split(" ")[2];
                continue;
            }

            if (trimmedLine.StartsWith("public class") && trimmedLine.EndsWith(": RecipeBase"))
            {
                recipeClassName = trimmedLine.Split(" ")[2];
                continue;
            }

            if (trimmedLine.StartsWith("public string Chapter =>"))
            {
                chapter = line.Split('"')[1];
                continue;
            }

            if (trimmedLine.StartsWith("public string CategoryName =>"))
            {
                category = line.Split('"')[1];
                continue;
            }

            if (trimmedLine.StartsWith("public override string Name =>"))
            {
                recipeName = line.Split('"')[1];
                continue;
            }

            // NOTE: indentation-specific identification of code blocks is okay
            // becuase the CI system runs the autoformatter automatically.

            // start of the Execute() code block
            if (line.StartsWith("        {"))
            {
                InRecipe = true;
                continue;
            }

            // end of the Execute() code block
            if (InRecipe && line.StartsWith("        }"))
            {
                string shortVersionString = ScottPlot.Version.VersionString.Replace(".", ", ").Split("-")[0];

                StringBuilder sb = new();
                //sb.AppendLine($"ScottPlot.Version.ShouldBe({shortVersionString});");
                sb.AppendLine("ScottPlot.Plot myPlot = new();");
                sb.AppendLine();
                sb.AppendLine(source.ToString().Trim());
                sb.AppendLine();
                sb.AppendLine($"myPlot.SavePng(\"demo.png\", 400, 300);");

                string description = GetDescription(recipeName);
                RecipeInfo thisRecipe = new(chapter, category, recipeName, description, sb.ToString(), categoryClassName, recipeClassName, sourceFilePath);
                recipes.Add(thisRecipe);

                InRecipe = false;
                source.Clear();
                continue;
            }

            if (InRecipe)
            {
                string newSourceLine = line.Trim().Length == 0
                    ? string.Empty // preserve double linebreaks in recipe sources
                    : line[12..]; // de-indent recipe sources

                source.AppendLine(newSourceLine);
            }
        }

        return recipes;
    }
}
