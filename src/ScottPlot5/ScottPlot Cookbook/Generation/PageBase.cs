using System.Text;

namespace ScottPlotCookbook.Generation;

internal abstract class PageBase
{
    protected readonly string OutputFolder = Path.GetFullPath(Path.Combine(TestContext.CurrentContext.TestDirectory, "cookbook-output"));

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
}
