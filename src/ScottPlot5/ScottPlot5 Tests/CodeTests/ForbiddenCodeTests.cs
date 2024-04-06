using System.Text;

namespace ScottPlotTests.CodeTests;

internal class ForbiddenCodeTests
{
    private readonly string[] SourceFilePaths = SourceCodeParsing.GetSourceFilePaths();

    [Test]
    public void Test_CanvasSave_IsNotCalledDirectly()
    {
        int offences = 0;
        StringBuilder errorMessages = new();
        foreach (string filePath in SourceFilePaths)
        {
            if (filePath.EndsWith("CanvasState.cs"))
                continue;

            string[] lines = File.ReadAllLines(filePath);
            for (int i = 0; i < lines.Length; i++)
            {
                string file2 = filePath.Replace(SourceCodeParsing.SourceFolder, string.Empty);
                string line = lines[i];

                bool offense = false;
                offense |= line.Contains(".Save();") && !line.Contains("CanvasState.Save();");
                offense |= line.Contains(".Restore();") && !line.Contains("CanvasState.Restore();");

                if (offense && line.Contains("WARNING", StringComparison.InvariantCultureIgnoreCase))
                    continue;

                if (!offense)
                    continue;

                offences += 1;
                errorMessages.AppendLine($"{file2} line {i + 1}");
                errorMessages.AppendLine(line.Trim());
                errorMessages.AppendLine();
            }
        }

        offences.Should().Be(0,
            $"SKCanvas Save() and Restore() should never be called directly. " +
            $"Call RenderPack.CanvasState Save() and Restore() instead." +
            $"{offences} offences:\n" +
            $"{errorMessages}");
    }
}
