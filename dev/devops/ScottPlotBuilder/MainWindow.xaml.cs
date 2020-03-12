using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ScottPlotBuilder
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ProjectFileVersion maybeProjVersion;
        string projectFilePath = @"../../../../../../src/ScottPlot/ScottPlot.csproj";

        public MainWindow()
        {
            InitializeComponent();
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            maybeProjVersion = new ProjectFileVersion(projectFilePath);
            VersionStartedText.Text = $"initial: {maybeProjVersion}";
            VersionCurrentText.Text = $"current: {maybeProjVersion}";

            // return early if this function got called manually
            if (sender is null)
                return;

            VersionNugetText.Text = $"nuget.org: searching...";

            BackgroundWorker worker = new BackgroundWorker();
            worker.WorkerReportsProgress = true;
            worker.DoWork += Worker_DoWork;
            worker.RunWorkerCompleted += Worker_RunWorkerCompleted;
            worker.RunWorkerAsync();
        }

        private string DownloadTextFile(string url)
        {
            url = url.Trim();
            WebClient client = new WebClient();
            MemoryStream stream = new MemoryStream(client.DownloadData(url));
            StreamReader reader = new StreamReader(stream);
            string text = reader.ReadToEnd();
            reader.Close();
            return text;
        }

        Version nugetVersion = new Version();
        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            // do NOT access GUI components from this thread
            string html = DownloadTextFile("https://www.nuget.org/packages/ScottPlot/");
            foreach (string line in html.Split("\n"))
            {
                if (line.Contains("<small"))
                {
                    string[] parts = line.Trim().Replace("<", "|").Replace(">", "|").Split("|");
                    string versionString = parts[2];
                    Debug.WriteLine($"version string from NuGet website: {versionString}");
                    nugetVersion = new Version(versionString);
                }
            }
        }

        private void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            // you may access GUI components from this thread
            VersionNugetText.Text = (nugetVersion.Major > 0) ? $"nuget.org: {nugetVersion}" : "nuget.org: HTML parse error";
        }

        private void VersionReset(object sender, RoutedEventArgs e)
        {
            OnLoaded(null, null);
        }

        private void VersionIncriment(object sender, RoutedEventArgs e)
        {
            maybeProjVersion.Incriment();
            VersionCurrentText.Text = $"current: {maybeProjVersion}";
        }

        private void VersionApply(object sender, RoutedEventArgs e)
        {
            string[] projectPaths = new string[]
            {
                @"../../../../../../src/ScottPlot/ScottPlot.csproj",
                @"../../../../../../src/ScottPlot.WinForms/ScottPlot.WinForms.csproj",
                @"../../../../../../src/ScottPlot.WPF/ScottPlot.WPF.csproj",
            };

            foreach (string projectPath in projectPaths)
            {
                var thisProjVersion = new ProjectFileVersion(projectPath);
                thisProjVersion.version = maybeProjVersion.version;
                thisProjVersion.Save();
            }
        }
    }
}
