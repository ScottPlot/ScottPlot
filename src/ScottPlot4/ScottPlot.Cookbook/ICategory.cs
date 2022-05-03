namespace ScottPlot.Cookbook;

public interface ICategory
{
    public string Name { get; }
    public string Description { get; }

    /// <summary>
    /// Name of the URL subfolder to place this category in.
    /// Used for back-compatibility to avoid breaking existing URLs.
    /// In the future the lowercase class name is sufficient.
    /// </summary>
    public string Folder { get; }
}
