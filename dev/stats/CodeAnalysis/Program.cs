Analyze("../../../../../../src/ScottPlot4");
Analyze("../../../../../../src/ScottPlot5");

static void Analyze(string folderPath)
{
    folderPath = Path.GetFullPath(folderPath);
    if (!Directory.Exists(folderPath))
        throw new DirectoryNotFoundException(folderPath);

    Console.WriteLine(folderPath);
    string[] files = FindFiles(folderPath);
    Console.WriteLine($"Found {files.Length} C# files");

    int totalLines = 0;
    int codeLines = 0;
    foreach (string path in files)
    {
        string[] lines = File.ReadAllText(path).Split("\n");
        foreach (string line in lines)
        {
            totalLines += 1;
            if (line.StartsWith("/"))
                continue;
            if (line.Trim().Length < 5)
                continue;
            codeLines += 1;
        }

    }
    Console.WriteLine($"Total lines: {totalLines:N0}");
    Console.WriteLine($"Code lines: {codeLines:N0}");
    Console.WriteLine(Environment.NewLine);
}

static string[] FindFiles(string folder)
{
    //bool isFile = (File.GetAttributes(path) & FileAttributes.Directory) != FileAttributes.Directory;

    // add files in this directory
    List<string> files = new();
    foreach (string file in Directory.GetFiles(folder, "*.cs"))
    {
        string filename = Path.GetFileName(file);
        if (filename.EndsWith(".Designer.cs"))
            continue;
        files.Add(file);
    }

    // scan subdirectories
    foreach (string subFolder in Directory.GetDirectories(folder))
    {
        string folderName = Path.GetFileName(subFolder);
        if (folderName == "bin")
            continue;
        if (folderName == "obj")
            continue;
        files.AddRange(FindFiles(subFolder));
    }

    return files.ToArray();
}