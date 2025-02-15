namespace ScottPlotCookbook;

/// <summary>
/// A recipe category object contains many individual recipes
/// </summary>
public interface ICategory
{
    public abstract Chapter Chapter { get; }
    public abstract string CategoryName { get; } // keep this redundant name to distinguish it from recipe names
    public abstract string CategoryDescription { get; } // keep this redundant name to distinguish it from recipe names
}
