using System.Text;

namespace ScottPlotTests.CodeTests;

internal class ThirdPartyLicenseTests
{
    readonly string NoticeFolder = Path.Combine(SourceCodeParsing.SourceFolder, "Notices");

    [Test]
    public void Test_Identify_SourceFilesWithLicenses()
    {
        // ensure the notices folder exists
        if (!Directory.Exists(NoticeFolder))
            Directory.CreateDirectory(NoticeFolder);
        Console.WriteLine($"Putting notices into: {NoticeFolder}");

        string noticesDestPath = Path.Combine(NoticeFolder, "NOTICES.txt");
        string ourLicenseSourcePath = Path.Combine(SourceCodeParsing.RepoFolder, "LICENSE");
        string ourLicenseDestPath = Path.Combine(NoticeFolder, "LICENSE.txt");

        // clear all other files from the notices folder
        foreach (string path in Directory.GetFiles(NoticeFolder))
        {
            if (path == ourLicenseDestPath || path == noticesDestPath)
                continue;
            Console.WriteLine($"Deleting notice file: {path}");
            File.Delete(path);
        }

        // copy our license into the notices folder
        File.Copy(ourLicenseSourcePath, ourLicenseDestPath, overwrite: true);

        // scan source code files for third party licenses to include in the notice folder
        Dictionary<string, List<string>> licenseToFiles = [];
        foreach (var sourceFile in SourceCodeParsing.GetSourceFilePaths())
        {
            if (sourceFile.Contains(nameof(ThirdPartyLicenseTests)))
                continue;

            string text = File.ReadAllText(sourceFile);
            bool hasLicense = text.Contains("license", StringComparison.InvariantCultureIgnoreCase);
            bool hasCopyright = text.Contains("copyright", StringComparison.InvariantCultureIgnoreCase);
            if (hasLicense == false && hasCopyright == false)
                continue;

            if (text.Contains("CC0 license", StringComparison.InvariantCultureIgnoreCase))
                continue;

            if (text.Contains(".NET Foundation", StringComparison.InvariantCultureIgnoreCase))
            {
                //continue;
            }

            string relPath = sourceFile.Replace(SourceCodeParsing.SourceFolder, "").Replace("\\", "/")
                                      .TrimStart('/');

            // These markers denote the top and bottom of what to include in the notice file
            string errorMessage = $"""
                {relPath} does not contain a notice designator.
                Files with license or copyright must include markers above and below
                the portion that needs to be reproduced as a distinct file. 
                Add a single empty line formatted like one of the following:
                --- NOTICE BEGIN ---
                --- NOTICE END ---
                """;
            Assert.That(text, Contains.Substring("--- NOTICE BEGIN ---"), errorMessage);
            Assert.That(text, Contains.Substring("--- NOTICE END ---"), errorMessage);
            string notice = ExtractNotice(text, relPath);

            if (!licenseToFiles.ContainsKey(notice))
                licenseToFiles[notice] = [];

            licenseToFiles[notice].Add(relPath);
        }

        var orderedNotices = licenseToFiles.Select(item => (item.Key, item.Value.Order().ToArray()))
                                          .OrderBy(item => item.Item2[0])
                                          .ToArray();
        StringBuilder output = new();
        output.AppendLine("""
            ScottPlot is provided under the MIT License. 
            https://scottplot.net
            https://github.com/scottplot/scottplot
            Some third-party components it includes require attribution text 
            to accompany the binary builds included in this package. Although 
            this information is present in the source code of the relevant files,
            it is also reproduced here so it can be distributed as plain text.
            """);

        output.AppendLine("");

        string separator = new('\n', 4); // above and below sections
        string divider = new('#', 80); // decorative horizontal line

        foreach (var (license, files) in orderedNotices)
        {
            output.Append(separator);
            output.AppendLine(divider);

            output.AppendLine("# The following notice is included in:");
            foreach (string file in files.Order())
            {
                output.AppendLine($"#   {file}");
            }
            output.AppendLine(divider);
            output.AppendLine(license);
            output.Append(separator);
        }

        File.WriteAllText(noticesDestPath, output.ToString());
        Console.WriteLine(new Uri(noticesDestPath).AbsoluteUri);
    }

    public string ExtractNotice(string text, string relPath)
    {
        StringBuilder sb = new();
        bool isInsideNotice = false;
        foreach (string line in text.Split("\n"))
        {
            if (line.Contains("--- NOTICE BEGIN ---"))
            {
                isInsideNotice = true;
            }
            else if (line.Contains("--- NOTICE END ---"))
            {
                isInsideNotice = false;
                sb.AppendLine();
            }
            else if (isInsideNotice)
            {
                sb.AppendLine(line.Trim());
            }
        }

        return sb.ToString().Trim();
    }
}
