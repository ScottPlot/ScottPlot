using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ScottPlot.Demo.WPF
{
    /// <summary>
    /// Interaction logic for DemoPlotControl.xaml
    /// </summary>
    public partial class CookbookControl : UserControl
    {
        string sourceCodeFolder = Reflection.FindDemoSourceFolder();

        public CookbookControl()
        {
            InitializeComponent();

            if (sourceCodeFolder is null)
                throw new ArgumentException("cannot locate source code");
        }

        private void WpfPlot1_Rendered(object sender, EventArgs e)
        {
            if (wpfPlot1.Visibility == Visibility.Visible)
                BenchmarkLabel.Content = wpfPlot1.plt.GetSettings(false).benchmark.ToString();
            else
                BenchmarkLabel.Content = "This plot is a non-interactive Bitmap";
        }

        private BitmapImage BmpImageFromBmp(System.Drawing.Bitmap bmp)
        {
            System.IO.MemoryStream stream = new System.IO.MemoryStream();
            bmp.Save(stream, System.Drawing.Imaging.ImageFormat.Png); // use PNG to support transparency
            BitmapImage bmpImage = new BitmapImage();
            bmpImage.BeginInit();
            stream.Seek(0, System.IO.SeekOrigin.Begin);
            bmpImage.StreamSource = stream;
            bmpImage.EndInit();
            return bmpImage;
        }

        public void LoadDemo(string objectPath)
        {
            var demoPlot = Reflection.GetPlot(objectPath);

            DemoNameLabel.Content = demoPlot.name;
            SourceCodeLabel.Content = $"{demoPlot.sourceFile} ({demoPlot.categoryClass})";
            DescriptionTextbox.Text = (demoPlot.description is null) ? "no descriton provided..." : demoPlot.description;
            string sourceCode = demoPlot.GetSourceCode(sourceCodeFolder);

            wpfPlot1.Reset();

            if (demoPlot is IBitmapDemo bmpPlot)
            {
                imagePlot.Visibility = Visibility.Visible;
                wpfPlot1.Visibility = Visibility.Hidden;
                imagePlot.Source = BmpImageFromBmp(bmpPlot.Render(600, 400));
                WpfPlot1_Rendered(null, null);
            }
            else
            {
                imagePlot.Visibility = Visibility.Hidden;
                wpfPlot1.Visibility = Visibility.Visible;

                demoPlot.Render(wpfPlot1.plt);
                wpfPlot1.Render();
            }

            SourceTextBox.Text = sourceCode;
        }
    }
}
