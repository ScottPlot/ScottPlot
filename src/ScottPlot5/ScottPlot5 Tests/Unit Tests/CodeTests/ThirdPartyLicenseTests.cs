using System.Text;

namespace ScottPlotTests.CodeTests;

internal class ThirdPartyLicenseTests
{
    readonly string NoticeFolder = Path.Combine(SourceCodeParsing.SourceFolder, "Notices");

    [Test]
    public void Test_Identify_SourceFilesWithLicenses()
    {
        Dictionary<string, List<string>> licenseToFiles = [];

        // ensure no old licenses stick around
        if (Directory.Exists(NoticeFolder))
        {
            Directory.Delete(NoticeFolder, true);
        }

        Directory.CreateDirectory(NoticeFolder);

        string readmePath = Path.Combine(NoticeFolder, "readme.md");
        File.WriteAllText(readmePath, """
            Notices here are copyright and license details extracted from
            source code comments in files named according to the file they
            were sourced from. Licenses in this collection are not duplicated.
            """);

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

            string relPath = sourceFile.Replace(SourceCodeParsing.SourceFolder, "").Replace("\\", "/");

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
        output.AppendLine("ScottPlot includes third-party content from the projects listed below, alongside their licenses.");
        output.AppendLine("");
        
        foreach (var (license, files) in orderedNotices)
        {
            var divider = new string('#', 80);
            var header = $"# Used in: {String.Join(", ", files.Order())}";

            output.AppendLine(divider);
            output.AppendLine(header);
            output.AppendLine(divider);
            output.AppendLine("");
            output.AppendLine(license);
            output.AppendLine(divider);
            output.AppendLine("");
        }
        
        var noticesPath = Path.Combine(NoticeFolder, "THIRD-PARTY-NOTICES.txt");
        File.WriteAllText(noticesPath, output.ToString());
        Console.WriteLine(new Uri(noticesPath).AbsoluteUri);
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
