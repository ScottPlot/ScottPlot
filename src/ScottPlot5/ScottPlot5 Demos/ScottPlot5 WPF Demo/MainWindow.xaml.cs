using System.Text;
using System.Windows;
using System.Windows.Media.Imaging;

namespace WPF_Demo;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        string[] args = Environment.GetCommandLineArgs();
        if (args.Length > 1 && args[1].EndsWith(".html"))
        {
            GenerateHtml(args[1]);
            Application.Current.Shutdown();
        }

        InitializeComponent();

        string logoImagePath = System.IO.Path.GetFullPath("Resources/logo-128.png");
        Uri logoImageUri = new(logoImagePath, UriKind.Absolute);
        LogoImage.Source = new BitmapImage(logoImageUri);
        Subtitle.Content = $"ScottPlot.WPF Version {ScottPlot.Version.VersionString}";

        DemoItemPanel.Children.Clear();
        GetDemoWindowTypes().ForEach(x => DemoItemPanel.Children.Add(new DemoMenuItem(x)));
    }

    private List<Type> GetDemoWindowTypes()
    {
        var demoWindows = System.Reflection.Assembly.GetAssembly(typeof(MainWindow))!
            .GetTypes()
            .Where(x => x.IsAssignableTo(typeof(IDemoWindow)))
            .Where(x => !x.IsInterface)
            .ToList();

        void MoveToTop(Type t) { demoWindows.Remove(t); demoWindows.Insert(0, t); }
        MoveToTop(typeof(DemoWindows.Quickstart));
        return demoWindows;
    }

    private void GenerateHtml(string saveAs)
    {
        StringBuilder sb = new();
        sb.AppendLine("<ul>");
        foreach (Type type in GetDemoWindowTypes())
        {
            IDemoWindow demo = (IDemoWindow)Activator.CreateInstance(type)!;
            string url = $"https://github.com/ScottPlot/ScottPlot/tree/main/src/ScottPlot5/ScottPlot5%20Demos/ScottPlot5%20WPF%20Demo/DemoWindows/{demo.GetType().Name}.xaml.cs";
            sb.AppendLine($"<li><strong><a href='{url}' target='_blank'>{demo.DemoTitle}</a></strong> - {demo.Description}</li>");
        }
        sb.AppendLine("</ul>");

        saveAs = System.IO.Path.GetFullPath(saveAs);
        System.IO.File.WriteAllText(saveAs, sb.ToString());
        System.Diagnostics.Debug.WriteLine(saveAs);
    }
}
