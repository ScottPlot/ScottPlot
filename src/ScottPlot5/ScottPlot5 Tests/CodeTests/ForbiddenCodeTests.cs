using System.Text;

namespace ScottPlotTests.CodeTests;

internal class ForbiddenCodeTests
{
    private readonly string[] SourceFilePaths = SourceCodeParsing.GetSourceFilePaths();

    [Test]
    public void Test_CanvasSave_IsNotCalledDirectly()
    {
        StringBuilder errorMessages = new();
        foreach (string filePath in SourceFilePaths)
        {
            string[] lines = File.ReadAllLines(filePath);
            for (int i = 0; i < lines.Length; i++)
            {
                string file2 = filePath.Replace(SourceCodeParsing.SourceFolder, string.Empty);
                string line = lines[i];
                if (line.Contains(".Save();"))
                {
                    errorMessages.AppendLine($"{file2} line {i + 1}");
                    errorMessages.AppendLine(line.Trim());
                    errorMessages.AppendLine();
                }
            }
        }

        errorMessages.ToString().Should().BeEmpty(
            $"SKCanvas.Save() should never be called directly.\n" +
            $"{errorMessages}");
    }
}
