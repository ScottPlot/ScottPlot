namespace GraphicalTestRunner;

public class FolderComparisonResults
{
    public ImageComparisonDetails[] Images { get; }

    public FolderComparisonResults(string before, string after)
    {
        HashSet<string> allFilenames = [];
        allFilenames.UnionWith(Directory.GetFiles(before, "*.png").Select(x => Path.GetFileName(x)).ToHashSet());
        allFilenames.UnionWith(Directory.GetFiles(before, "*.png").Select(x => Path.GetFileName(x)).ToHashSet());
        Images = allFilenames.Order().Select(x => new ImageComparisonDetails(before, after, x)).ToArray();
    }
}
