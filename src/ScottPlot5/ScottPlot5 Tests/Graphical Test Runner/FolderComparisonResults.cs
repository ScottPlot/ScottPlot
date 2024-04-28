namespace GraphicalTestRunner;

public class FolderComparisonResults
{
    public string BeforeFolder { get; }
    public string AfterFolder { get; }
    public string[] Filenames { get; }
    public string[] Summaries { get; }
    public ScottPlot.Testing.ImageDiff?[] ImageDiffs { get; }

    public FolderComparisonResults(string before, string after)
    {
        BeforeFolder = before;
        AfterFolder = after;
        var BeforePaths = Directory.GetFiles(before, "*.png");
        var AfterPaths = Directory.GetFiles(after, "*.png");

        HashSet<string> allFilenames = [];
        allFilenames.UnionWith(BeforePaths.Select(x => Path.GetFileName(x)));
        allFilenames.UnionWith(AfterPaths.Select(x => Path.GetFileName(x)));
        Filenames = allFilenames.Order().ToArray();

        Summaries = new string[Filenames.Length];
        ImageDiffs = new ScottPlot.Testing.ImageDiff?[Filenames.Length];
    }

    public string GetPath1(int i)
    {
        return Path.GetFullPath(BeforeFolder + "/" + Filenames[i]);
    }

    public string GetPath2(int i)
    {
        return Path.GetFullPath(AfterFolder + "/" + Filenames[i]);
    }

    public void Analyze(int i)
    {
        string pathBefore = GetPath1(i);
        string pathAfter = GetPath2(i);

        bool img1Exists = File.Exists(pathBefore);
        bool img2Exists = File.Exists(pathAfter);

        if (img1Exists && img2Exists)
        {
            ScottPlot.Image img1 = new(pathBefore);
            ScottPlot.Image img2 = new(pathAfter);
            if (img1.Size != img2.Size)
            {
                Summaries[i] = "resized";
                return;
            }
            ImageDiffs[i] = new(img1, img2, saveDiffImage: false);
            Summaries[i] = ImageDiffs[i]!.TotalDifference == 0 ? "unchanged" : "changed";
        }
        else if (img1Exists && !img2Exists)
        {
            Summaries[i] = "deleted";
        }
        else if (!img1Exists && img2Exists)
        {
            Summaries[i] = "added";
        }
        else
        {
            throw new InvalidOperationException("neither file exists");
        }
    }
}
