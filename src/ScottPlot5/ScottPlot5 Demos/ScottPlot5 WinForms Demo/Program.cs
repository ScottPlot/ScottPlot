using System.Text;
using System.Text.Json;

namespace WinForms_Demo;

static class Program
{
    [STAThread]
    static void Main(string[] args)
    {
        if (args.Length == 0)
        {
            LaunchWinFormsApp();
        }
        else
        {
            if (args[0].EndsWith(".json"))
            {
                SaveJsonFile(args[0]);
            }
            else if (args[0].EndsWith(".html"))
            {
                SaveHtmlFile(args[0]);
            }
            else
            {
                throw new ArgumentException("JSON or HTML file path expected");
            }
        }
    }
    static void LaunchWinFormsApp()
    {
        Application.EnableVisualStyles();
        ApplicationConfiguration.Initialize();
        Application.EnableVisualStyles();
        Type testingFormType = typeof(Demos.TransparentBackground);
        Application.Run(new MainMenuForm(testingFormType));
    }

    static void SaveJsonFile(string saveAs)
    {
        using MemoryStream stream = new();
        JsonWriterOptions options = new() { Indented = true };
        using Utf8JsonWriter writer = new(stream, options);

        writer.WriteStartObject();
        foreach (IDemoWindow demo in DemoWindows.GetDemoWindows())
        {
            writer.WriteStartObject(demo.GetType().Name);
            writer.WriteString("title", demo.Title);
            writer.WriteString("description", demo.Description);
            writer.WriteEndObject();
        }
        writer.WriteEndObject();

        writer.Flush();
        string json = Encoding.UTF8.GetString(stream.ToArray());

        saveAs = Path.GetFullPath(saveAs);
        File.WriteAllText(saveAs, json);
        System.Diagnostics.Debug.WriteLine(saveAs);
    }

    static void SaveHtmlFile(string saveAs)
    {
        StringBuilder sb = new();
        sb.AppendLine("<ul>");
        foreach (IDemoWindow demo in DemoWindows.GetDemoWindows())
        {
            string url = $"https://github.com/ScottPlot/ScottPlot/blob/main/src/ScottPlot5/ScottPlot5%20Demos/ScottPlot5%20WinForms%20Demo/Demos/{demo.GetType().Name}.cs";
            sb.AppendLine($"<li><strong><a href='{url}' target='_blank'>{demo.Title}</a></strong> - {demo.Description}</li>");
        }
        sb.AppendLine("</ul>");

        saveAs = Path.GetFullPath(saveAs);
        File.WriteAllText(saveAs, sb.ToString());
        System.Diagnostics.Debug.WriteLine(saveAs);
    }
}
