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
        ProjectFileVersion projVersion;
        string projectFilePath = @"../../../../../../src/ScottPlot/ScottPlot.csproj";
        Version versionAtStart;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void VersionReset(object sender, RoutedEventArgs e)
        {
            if (versionAtStart is null)
            {
                // this gets called when the program actually loads
                projVersion = new ProjectFileVersion(projectFilePath);
                versionAtStart = new Version(projVersion.version.ToString());

                VersionNugetText.Text = $"nuget.org: searching...";
                BackgroundWorker worker = new BackgroundWorker();
                worker.WorkerReportsProgress = true;
                worker.DoWork += Worker_DoWork;
                worker.RunWorkerCompleted += Worker_RunWorkerCompleted;
                worker.RunWorkerAsync();
            }
            else
            {
                projVersion.version = versionAtStart;
                ApplyButton.IsEnabled = true;
            }

            VersionStartedText.Text = $"initial: {projVersion}";
            VersionCurrentText.Text = $"current: {projVersion}";
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

        private void VersionIncriment(object sender, RoutedEventArgs e)
        {
            projVersion.Incriment();
            VersionCurrentText.Text = $"current: {projVersion}";
            ApplyButton.IsEnabled = true;
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
                thisProjVersion.version = projVersion.version;
                thisProjVersion.Save();
            }

            ApplyButton.IsEnabled = false;
        }

        private void BuildRelease(object sender, RoutedEventArgs e)
        {

        }
    }
}
