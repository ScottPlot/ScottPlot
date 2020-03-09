using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace ScottPlot.Demo.WPF
{
    /// <summary>
    /// Interaction logic for CookbookGeneratorWindow.xaml
    /// </summary>
    public partial class CookbookGeneratorWindow : Window
    {
        Stopwatch stopwatch = Stopwatch.StartNew();
        Cookbook.Chef chef = new Cookbook.Chef("../../../../../src/ScottPlot.Demo/");
        IPlotDemo[] recipes = Reflection.GetPlots();

        public CookbookGeneratorWindow()
        {
            InitializeComponent();
            ProgressBar1.Value = 0;
        }

        private void GenerateClicked(object sender, RoutedEventArgs e)
        {
            BackgroundWorker worker = new BackgroundWorker();
            worker.WorkerReportsProgress = true;
            worker.DoWork += Worker_DoWork;
            worker.ProgressChanged += Worker_ProgressChanged;
            worker.RunWorkerCompleted += Worker_RunWorkerCompleted;

            GenerateButton.IsEnabled = false;
            OpenFolderButton.IsEnabled = false;
            messageTextBox.Clear();
            stopwatch.Restart();
            worker.RunWorkerAsync();
        }

        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            (sender as BackgroundWorker).ReportProgress(0, "preparing output folders...");
            chef.ClearFolders();

            for (int i = 0; i < recipes.Length; i++)
            {
                var recipe = recipes[i];
                int progressPercent = (int)(i * 100.0 / recipes.Length);
                string message = $"creating {recipe.categoryMajor}.{recipe.categoryMinor}.{recipe.categoryClass}...";
                (sender as BackgroundWorker).ReportProgress(progressPercent, message);
                chef.CreateImage(recipe);
            }

            (sender as BackgroundWorker).ReportProgress(100, "generating reports...");
            chef.MakeReports();
        }

        private void Worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            ProgressBar1.Value = e.ProgressPercentage;
            messageTextBox.AppendText(e.UserState + "\r\n");
            messageTextBox.ScrollToEnd();
        }

        private void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            stopwatch.Stop();
            double elapsedSec = (double)stopwatch.ElapsedTicks / Stopwatch.Frequency;
            messageTextBox.AppendText($"Genereated {recipes.Length} cookbook images in {elapsedSec:0.00} sec");
            GenerateButton.IsEnabled = true;
            OpenFolderButton.IsEnabled = true;
        }

        private void OpenClicked(object sender, RoutedEventArgs e)
        {
            Process.Start("explorer.exe", chef.outputFolder);
        }
    }
}
