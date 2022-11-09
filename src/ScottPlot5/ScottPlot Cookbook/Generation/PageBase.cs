using System.Text;

namespace ScottPlotCookbook.Generation;

internal abstract class PageBase
{
    public static string OutputFolder => Path.GetFullPath(Path.Combine(TestContext.CurrentContext.TestDirectory, "cookbook-output"));

    protected readonly StringBuilder MD = new();

    public abstract void Generate();

    public PageBase()
    {
        Generate();
        Export();
    }

    protected void Export(string subFolder = "")
    {
        string folder = Path.Combine(OutputFolder, subFolder);
        Directory.CreateDirectory(folder);

        string saveAsMD = Path.Combine(folder, "index.md");
        File.WriteAllText(saveAsMD, MD.ToString());
        TestContext.WriteLine(saveAsMD);
    }

    public static string UrlSafe(string text)
    {
        StringBuilder sb = new();

        string charsToReplaceWithDash = " _-+:";

        foreach (char c in text.ToLower().ToCharArray())
        {
            if (charsToReplaceWithDash.Contains(c))
                sb.Append("-");
            else if (char.IsLetterOrDigit(c))
                sb.Append(c);
        }

        return sb.ToString();
    }
}
