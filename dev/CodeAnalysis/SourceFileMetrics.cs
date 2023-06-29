namespace CodeAnalysis;

public class SourceFileMetrics
{
    public readonly string FilePath;
    public readonly int SourceLines;
    public readonly int LinesOfCode;

    public SourceFileMetrics(string csFilePath)
    {
        FilePath = csFilePath;
        string code = File.ReadAllText(csFilePath);
        SourceLines = code.Split("\n").Length;
        LinesOfCode = RemoveEmptyLines(StripComments(code)).Split("\n").Length;
    }

    static string StripComments(string s) =>
        System.Text.RegularExpressions.Regex.Replace(
            input: s,
            pattern: @"(@(?:""[^""]*"")+|""(?:[^""\n\\]+|\\.)*""|'(?:[^'\n\\]+|\\.)*')|//.*|/\*(?s:.*?)\*/",
            replacement: "$1");

    static string RemoveEmptyLines(string s) =>
        string.Join("\n", s.Split("\n").Where(x => x.Trim().Length > 0));

    public bool IsInFolder(string folderName) =>
        FilePath.Contains(Path.DirectorySeparatorChar + folderName + Path.DirectorySeparatorChar);
}
