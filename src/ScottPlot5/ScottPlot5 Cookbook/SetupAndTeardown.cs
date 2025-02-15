namespace ScottPlotCookbook;

[SetUpFixture]
internal class SetupAndTeardown
{
    [OneTimeSetUp]
    public void RunBeforeAnyTests()
    {
        // delete all old cookbook content
        if (Directory.Exists(Paths.OutputFolder))
            Directory.Delete(Paths.OutputFolder, true);

        // create a fresh cookbook folder
        Directory.CreateDirectory(Paths.OutputFolder);
        Directory.CreateDirectory(Paths.OutputImageFolder);
    }

    [OneTimeTearDown]
    public void RunAfterAllTests()
    {
        StringBuilder sb = new();
        sb.AppendLine("""
            <html>
            <head>
            <style>
            body { font-family: sans-serif; }
            </style>
            </head>
            <body>
            """);
        sb.AppendLine($"<div style='display: flex; flex-wrap: wrap;'>");
        foreach (string imagePath in Directory.GetFiles(Paths.OutputImageFolder, "*.png"))
        {
            string filename = Path.GetFileName(imagePath);
            sb.AppendLine($"<div style='border: 1px solid black; margin: 1em; text-align: center;'>");
            sb.AppendLine($"<h3>{filename}</h3>");
            sb.AppendLine($"<a href='images/{filename}'><img src='images/{filename}'></a>");
            sb.AppendLine($"</div>");
        }
        sb.AppendLine($"</div>");
        sb.AppendLine("</body>");
        sb.AppendLine("</html>");
        string saveAs = Path.Combine(Paths.OutputFolder, ".test.html");
        File.WriteAllText(saveAs, sb.ToString());
    }
}
