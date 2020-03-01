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
        string[] demoPlotNames = Reflection.GetDemoPlots();
        string outputFolder = System.IO.Path.GetFullPath("./output");
        string imageFolder = System.IO.Path.GetFullPath("./output/images");

        public CookbookGeneratorWindow()
        {
            InitializeComponent();
            ProgressBar1.Value = 0;
            ProgressLabel.Content = $"ready to generate {demoPlotNames.Length} cookbook plots...";
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

        private void GenerateClicked(object sender, RoutedEventArgs e)
        {
            BackgroundWorker worker = new BackgroundWorker();
            worker.WorkerReportsProgress = true;
            worker.DoWork += Worker_DoWork;
            worker.ProgressChanged += Worker_ProgressChanged;
            worker.RunWorkerCompleted += Worker_RunWorkerCompleted;

            GenerateButton.IsEnabled = false;
            worker.RunWorkerAsync();
            GenerateButton.IsEnabled = true;
        }

        private void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            LoadImages();
        }

        BitmapImage[] images;
        private void LoadImages()
        {
            string[] imagePaths = System.IO.Directory.GetFiles(imageFolder, "*.png");
            
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
            for (int i = 0; i < demoPlotNames.Length; i++)
            {
                IPlotDemo plotDemo = Reflection.GetPlot(demoPlotNames[i]);
                (sender as BackgroundWorker).ReportProgress(i * 100 / demoPlotNames.Length, plotDemo.name);
                string imageFilePath = $"{imageFolder}/{plotDemo.name}.png";

                var plt = new Plot(600, 400);
                plotDemo.Render(plt);
                plt.SaveFig(imageFilePath);
            }

            (sender as BackgroundWorker).ReportProgress(100, $"Completed generating {demoPlotNames.Length} plots");
        }

        private void Worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            ProgressBar1.Value = e.ProgressPercentage;
            ProgressLabel.Content = e.UserState;
        }

        private void OpenClicked(object sender, RoutedEventArgs e)
        {
            Process.Start("explorer.exe", outputFolder);
        }

        private void ImageSliderChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (images != null)
                PlotImage.Source = images[(int)ImageSlider.Value];
        }
    }
}
