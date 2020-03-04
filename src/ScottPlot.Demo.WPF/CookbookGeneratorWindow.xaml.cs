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
        IPlotDemo[] demoPlots = Reflection.GetPlots();
        string outputFolder = System.IO.Path.GetFullPath("./output");
        string imageFolder = System.IO.Path.GetFullPath("./output/images");

        public CookbookGeneratorWindow()
        {
            InitializeComponent();
            ProgressBar1.Value = 0;
            ProgressLabel.Content = $"ready to generate {demoPlots.Length} cookbook plots...";
            ResetOutputFolder();
        }

        private void ResetOutputFolder()
        {
            if (System.IO.Directory.Exists(outputFolder))
            {
                Debug.WriteLine($"Deleting folder: {outputFolder}");
                System.IO.Directory.Delete(outputFolder, recursive: true);
            }

            Debug.WriteLine($"Creating folder: {outputFolder}");
            System.IO.Directory.CreateDirectory(outputFolder);

            Debug.WriteLine($"Creating folder: {imageFolder}");
            System.IO.Directory.CreateDirectory(imageFolder);
        }

        Stopwatch stopwatch = Stopwatch.StartNew();
        private void GenerateClicked(object sender, RoutedEventArgs e)
        {
            BackgroundWorker worker = new BackgroundWorker();
            worker.WorkerReportsProgress = true;
            worker.DoWork += Worker_DoWork;
            worker.ProgressChanged += Worker_ProgressChanged;
            worker.RunWorkerCompleted += Worker_RunWorkerCompleted;

            GenerateButton.IsEnabled = false;
            stopwatch.Restart();
            worker.RunWorkerAsync();
        }

        BitmapImage[] images;
        string[] imagePaths;
        private void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            imagePaths = System.IO.Directory.GetFiles(imageFolder, "*.png");
            images = new BitmapImage[imagePaths.Length];

            for (int i = 0; i < imagePaths.Length; i++)
            {
                BitmapImage b = new BitmapImage();
                b.BeginInit();
                b.UriSource = new Uri(imagePaths[i]);
                b.EndInit();
                images[i] = b;
            }

            ImageSlider.Maximum = images.Length - 1;
            ImageSlider.Value = 0;
            ImageSliderChanged(null, null);
        }

        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            for (int i = 0; i < demoPlots.Length; i++)
            {
                IPlotDemo demoPlot = demoPlots[i];
                string imageFilePath = $"{imageFolder}/{demoPlot.id}.png";

                var plt = new Plot(600, 400);
                demoPlot.Render(plt);
                plt.SaveFig(imageFilePath);
                Debug.WriteLine($"Saved: {imageFilePath}");

                (sender as BackgroundWorker).ReportProgress(i * 100 / demoPlots.Length, i);
            }

            (sender as BackgroundWorker).ReportProgress(100, -1);
        }

        private void Worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            Debug.WriteLine($"Progress: {e.ProgressPercentage}");

            int demoPlotIndex = (int)e.UserState;
            ProgressBar1.Value = e.ProgressPercentage;

            if (demoPlotIndex >= 0)
            {
                IPlotDemo demoPlot = demoPlots[demoPlotIndex];
                ProgressLabel.Content = demoPlot.name;
            }
            else
            {
                stopwatch.Stop();
                double elapsedSec = (double)stopwatch.ElapsedTicks / Stopwatch.Frequency;
                ProgressLabel.Content = $"Generated {demoPlots.Length} cookbook plots in {elapsedSec:0.000} seconds";
                ImageSlider.Visibility = Visibility.Visible;
            }
        }

        private void OpenClicked(object sender, RoutedEventArgs e)
        {
            Process.Start("explorer.exe", outputFolder);
        }

        private void ImageSliderChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (images != null)
            {
                int imageIndex = (int)ImageSlider.Value;
                PlotImage.Source = images[imageIndex];
                ProgressLabel.Content = System.IO.Path.GetFileName(imagePaths[imageIndex]);
            }
        }

    }
}
