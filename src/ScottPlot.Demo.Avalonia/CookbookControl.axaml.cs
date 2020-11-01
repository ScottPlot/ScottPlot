using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media.Imaging;
using System;

namespace ScottPlot.Demo.Avalonia
{
    public class CookbookControl : UserControl
    {
        string sourceCodeFolder = Reflection.FindDemoSourceFolder();

        public CookbookControl()
        {
            this.InitializeComponent();

            this.Find<ScottPlot.Avalonia.AvaPlot>("AvaPlot1").Rendered += AvaPlot1_Rendered;
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private void AvaPlot1_Rendered(object sender, EventArgs e)
        {
            ScottPlot.Avalonia.AvaPlot AvaPlot1 = this.Find<ScottPlot.Avalonia.AvaPlot>("AvaPlot1");
            TextBlock BenchmarkLabel = this.Find<TextBlock>("BenchmarkLabel");

            if (AvaPlot1.IsVisible)
                BenchmarkLabel.Text = AvaPlot1.plt.GetSettings(false).Benchmark.ToString();
            else
                BenchmarkLabel.Text = "This plot is a non-interactive Bitmap";
        }

        private Bitmap BmpImageFromBmp(System.Drawing.Bitmap bmp)
        {
            using (var memory = new System.IO.MemoryStream())
            {
                bmp.Save(memory, System.Drawing.Imaging.ImageFormat.Png);
                memory.Position = 0;

                var bitmapImage = new global::Avalonia.Media.Imaging.Bitmap(memory);

                return bitmapImage;
            }
        }

        public void LoadDemo(string objectPath)
        {
            var demoPlot = Reflection.GetPlot(objectPath);
            var avaPlot1 = this.Find<ScottPlot.Avalonia.AvaPlot>("AvaPlot1");
            var imagePlot1 = this.Find<Image>("imagePlot");

            this.Find<TextBlock>("DemoNameLabel").Text = demoPlot.name;
            this.Find<TextBlock>("SourceCodeLabel").Text = $"{demoPlot.sourceFile} ({demoPlot.categoryClass})";
            this.Find<TextBox>("DescriptionTextbox").Text = (demoPlot.description is null) ? "no descriton provided..." : demoPlot.description;
            string sourceCode = demoPlot.GetSourceCode(sourceCodeFolder);

            avaPlot1.Reset();

            if (demoPlot is IBitmapDemo bmpPlot)
            {
                imagePlot1.IsVisible = true;
                avaPlot1.IsVisible = false;
                imagePlot1.Source = BmpImageFromBmp(bmpPlot.Render(600, 400));
                AvaPlot1_Rendered(null, null);
            }
            else
            {
                imagePlot1.IsVisible = false;
                avaPlot1.IsVisible = true;

                demoPlot.Render(avaPlot1.plt);
                avaPlot1.Render();
            }

            this.Find<TextBox>("SourceTextBox").Text = sourceCode;
        }
    }
}
