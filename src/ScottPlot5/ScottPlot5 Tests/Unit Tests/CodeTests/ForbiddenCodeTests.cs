using System.Reflection;
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
                offense |= line.Contains(".Save();") && !line.Contains("CanvasState.Save();", StringComparison.OrdinalIgnoreCase);
                offense |= line.Contains(".Restore();") && !line.Contains("CanvasState.Restore();", StringComparison.OrdinalIgnoreCase);
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

    [Test]
    public void Test_PrimitivesNamespace_IsNeverUsed()
    {
        Assembly.GetAssembly(typeof(Plot))!
            .GetTypes()
            .Where(x => x.Namespace is not null && x.Namespace.Contains("ScottPlot.Primitives"))
            .ToList()
            .ForEach(t => Assert.Fail($"{t.Name} should be in the namespace ScottPlot (not ScottPlot.Primitives)"));
    }

    [Test]
    public void Test_InterfacesNamespace_IsNeverUsed()
    {
        Assembly.GetAssembly(typeof(Plot))!
            .GetTypes()
            .Where(x => x.Namespace is not null && x.Namespace.Contains("ScottPlot.Interfaces"))
            .ToList()
            .ForEach(t => Assert.Fail($"{t.Name} should be in the namespace ScottPlot (not ScottPlot.Interfaces)"));
    }

    [Test]
    public void Test_Canvas_DoNotCallDrawText()
    {
        int offences = 0;
        StringBuilder errorMessages = new();
        foreach (string filePath in SourceFilePaths)
        {
            if (Path.GetFileName(filePath) == "Label.cs")
                continue;

            if (Path.GetFileName(filePath) == "MeasuredText.cs")
                continue;

            string[] lines = File.ReadAllLines(filePath);
            for (int i = 0; i < lines.Length; i++)
            {
                string file2 = filePath.Replace(SourceCodeParsing.SourceFolder, string.Empty);
                string line = lines[i];

                if (line.Contains(".DrawText("))
                {
                    offences += 1;
                    errorMessages.AppendLine($"{file2} line {i + 1}");
                    errorMessages.AppendLine(line.Trim());
                    errorMessages.AppendLine();
                }
            }
        }

        offences.Should().Be(0,
            $"SKCanvas.DrawText() must never be called directly." +
            $"Create a Label, style it as desired, and call it's Render() method." +
            $"{offences} offences:\n" +
            $"{errorMessages}");
    }


    [Test]
    public void Test_Paint_FontSpacing()
    {
        int offences = 0;
        StringBuilder errorMessages = new();
        foreach (string filePath in SourceFilePaths)
        {
            if (Path.GetFileName(filePath) == "Label.cs")
                continue;

            string[] lines = File.ReadAllLines(filePath);
            for (int i = 0; i < lines.Length; i++)
            {
                string file2 = filePath.Replace(SourceCodeParsing.SourceFolder, string.Empty);
                string line = lines[i];

                if (line.Contains(".FontSpacing"))
                {
                    offences += 1;
                    errorMessages.AppendLine($"{file2} line {i + 1}");
                    errorMessages.AppendLine(line.Trim());
                    errorMessages.AppendLine();
                }
            }
        }

        offences.Should().Be(0,
            $"SKPaint.FontSpacing must never be accessed." +
            $"Create a Label, style it as desired, use its Measeure() method." +
            $"{offences} offences:\n" +
            $"{errorMessages}");
    }
}
