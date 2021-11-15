using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media.Imaging;
using System;
using System.Collections.Generic;

namespace ScottPlot.Demo.Avalonia
{
    public class CookbookControl : UserControl
    {
        readonly Dictionary<string, Cookbook.RecipeSource> Recipes;

        public CookbookControl()
        {
            this.InitializeComponent();
            Recipes = Cookbook.RecipeJson.GetRecipes();
            this.Find<ScottPlot.Avalonia.AvaPlot>("AvaPlot1").Configuration.WarnIfRenderNotCalledManually = false;
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
                BenchmarkLabel.Text = AvaPlot1.Plot.GetSettings(false).BenchmarkMessage.Message;
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

        public void LoadDemo(string id)
        {
            Cookbook.IRecipe recipe = Cookbook.Locate.GetRecipe(id);
            var avaPlot1 = this.Find<ScottPlot.Avalonia.AvaPlot>("AvaPlot1");
            var imagePlot1 = this.Find<Image>("imagePlot");

            this.Find<TextBlock>("DemoNameLabel").Text = recipe.Title;
            this.Find<TextBlock>("SourceCodeLabel").Text = "Source Code";
            this.Find<TextBox>("DescriptionTextbox").Text = recipe.Description;
            string source = Recipes is null ? Cookbook.RecipeJson.NotFoundMessage : Recipes[id].Code;
            this.Find<TextBox>("SourceTextBox").Text = source.Replace("\n", Environment.NewLine);

            avaPlot1.Reset();
            imagePlot1.IsVisible = false;
            avaPlot1.IsVisible = true;
            recipe.ExecuteRecipe(avaPlot1.Plot);
            avaPlot1.Refresh();
        }
    }
}
