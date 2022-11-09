namespace ScottPlotCookbook;

/// <summary>
/// A page of recipes groups similar recipes together and displays them in order.
/// </summary>
public abstract class RecipePage : IEquatable<RecipePage>
{
    public abstract Chapter Chapter { get; }
    public abstract string PageName { get; }
    public abstract string PageDescription { get; }

    public bool Equals(RecipePage? other)
    {
        if (other is null)
            return false;

        return PageName == other.PageName && PageDescription == other.PageDescription && Chapter == other.Chapter;
    }

    public override int GetHashCode()
    {
        return PageName.GetHashCode() ^ PageDescription.GetHashCode() ^ Chapter.GetHashCode();
    }
}
