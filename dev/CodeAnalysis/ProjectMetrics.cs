using System.Collections.Generic;

namespace CodeAnalysis;

public class ProjectMetrics
{
    public readonly List<SourceFileMetrics> Files = new();
    public readonly string FolderPath;

    public ProjectMetrics(string sourceFolder)
    {
        FolderPath = sourceFolder;

        string[] csFilePaths = Directory.GetFiles(sourceFolder, "*.cs", SearchOption.AllDirectories)
            .Where(x => !x.EndsWith("Designer.cs"))
            .Where(x => !x.Contains(Path.DirectorySeparatorChar + "obj" + Path.DirectorySeparatorChar))
            .Where(x => !x.EndsWith("AssemblyInfo.cs"))
            .OrderBy(x => x)
            .ToArray();

        Console.WriteLine($"Analyzing {csFilePaths.Length:N0} files...");

        foreach (string csFilePath in csFilePaths)
        {
            SourceFileMetrics metrics = new(csFilePath);
            Files.Add(metrics);
        }

        Console.WriteLine(
            $"Analyzed {Files.Select(x => x.SourceLines).Sum():N0} source lines " +
            $"({Files.Select(x => x.LinesOfCode).Sum():N0} lines of code)");
    }

    public SourceFileMetrics[] FilesInFolder(string folderName) =>
        Files
        .Where(x => x.IsInFolder(folderName))
        .ToArray();

    public SourceFileMetrics[] FilesInFolders(string folderName1, string folderName2) =>
        Files
        .Where(x => x.IsInFolder(folderName1))
        .Where(x => x.IsInFolder(folderName2))
        .ToArray();

    public string GetLines(string rootFolder = "", string subFolder = "")
    {
        SourceFileMetrics[] files = Files.ToArray();

        if (rootFolder != "")
        {
            files = subFolder == ""
              ? FilesInFolder(rootFolder)
              : FilesInFolders(rootFolder, subFolder);

            if (rootFolder == subFolder)
            {
                string match = Path.DirectorySeparatorChar +
                    rootFolder + Path.DirectorySeparatorChar +
                    subFolder + Path.DirectorySeparatorChar;

                files = Files.Where(x => x.FilePath.Contains(match)).ToArray();
            }
        }

        int lines = files.Select(x => x.SourceLines).Sum();
        int coding = files.Select(x => x.LinesOfCode).Sum();
        return $"{lines:N0} lines " +
            $"({coding:N0} coding) " +
            $"across {files.Length:N0} files";
    }
}
