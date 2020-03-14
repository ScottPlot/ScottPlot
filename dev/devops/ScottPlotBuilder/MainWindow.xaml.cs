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

        #region version

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
                worker.DoWork += NuGetWorker_DoWork;
                worker.RunWorkerCompleted += NuGetWorker_Completed;
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
        private void NuGetWorker_DoWork(object sender, DoWorkEventArgs e)
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

        private void NuGetWorker_Completed(object sender, RunWorkerCompletedEventArgs e)
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

        #endregion

        private void RunScript(string fileName)
        {
            string buildScriptPath = System.IO.Path.GetFullPath($"../../../scripts/{fileName}");

            if (!File.Exists(buildScriptPath))
                throw new InvalidOperationException("Cannot find build script: " + buildScriptPath);

            ProcessStartInfo processInfo = new ProcessStartInfo(buildScriptPath)
            {
                CreateNoWindow = false,
                UseShellExecute = false,
                WorkingDirectory = System.IO.Path.GetDirectoryName(buildScriptPath)
            };

            Process process = Process.Start(processInfo);
            process.WaitForExit();
            process.Close();
        }

        private void NuGetBuild(object sender, RoutedEventArgs e)
        {
            RunScript("clean-build.bat");
        }

        private void NuGetUpload(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("select OK to upload NuGet packages", "NuGet Upload", MessageBoxButton.OKCancel, MessageBoxImage.Exclamation);
            if (result == MessageBoxResult.OK)
                RunScript("nuget-upload.bat");
        }

        private void GenerateCookbook(object sender, RoutedEventArgs e)
        {
            MessageTextbox.Clear();
            MessageTextbox.AppendText($"Generating cookbook version {ScottPlot.Tools.GetVersionString()}" + Environment.NewLine);

            BackgroundWorker worker = new BackgroundWorker();
            worker.WorkerReportsProgress = true;
            worker.DoWork += CookbookWorker_DoWork;
            worker.ProgressChanged += CookbookWorker_ProgressChanged;
            worker.RunWorkerCompleted += CookbookWorker_RunWorkerCompleted; ;
            worker.RunWorkerAsync();
        }

        private void CookbookWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            string outputFolder = System.IO.Path.GetFullPath($"./{ScottPlot.Tools.GetVersionString()}");

            var reportGeneratpr = new ScottPlot.Demo.ReportGenerator(outputFolder: outputFolder);

            (sender as BackgroundWorker).ReportProgress(0, $"preparing folders");
            reportGeneratpr.ClearFolders();
            foreach (var recipe in ScottPlot.Demo.Reflection.GetPlots())
            {

                (sender as BackgroundWorker).ReportProgress(0, recipe.id);
                reportGeneratpr.CreateImage(recipe);
            }

            (sender as BackgroundWorker).ReportProgress(0, $"creating reports");
            reportGeneratpr.MakeReports();
        }

        private void CookbookWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            Debug.WriteLine(e.UserState);
            MessageTextbox.AppendText(e.UserState.ToString() + Environment.NewLine);
            MessageTextbox.ScrollToEnd();
        }

        private void CookbookWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            // you may access GUI components from this thread
            MessageTextbox.AppendText($"Cookbook generation complete!");
        }

        private void LaunchCookbook(object sender, RoutedEventArgs e)
        {
            string outputFolder = System.IO.Path.GetFullPath($"./{ScottPlot.Tools.GetVersionString()}");
            if (System.IO.Directory.Exists(outputFolder))
                System.Diagnostics.Process.Start("explorer.exe", outputFolder);
            else
                MessageBox.Show($"folder does not exist: {outputFolder}");
        }

        private void UploadCookbook(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("select OK to upload cookbook", "Cookbook Upload", MessageBoxButton.OKCancel, MessageBoxImage.Exclamation);
            if (result == MessageBoxResult.OK)
                RunScript("cookbook-upload.bat");
        }
    }
}
