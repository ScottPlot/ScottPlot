namespace ScottPlotCookbook;

internal static class UrlTools
{
    [Obsolete]
    internal static string UrlSafe(string text)
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
