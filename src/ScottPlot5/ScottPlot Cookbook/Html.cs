using System.Text;

namespace ScottPlotCookbook;

public static class Html
{
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
