using System;
using System.Collections.Generic;

using Avalonia.Controls;

namespace ScottPlot.Demo.Avalonia
{
    public partial class CookbookControl : UserControl
    {
        private readonly Dictionary<string, Cookbook.RecipeSource> Recipes;

        public CookbookControl()
        {
            this.InitializeComponent();
            Recipes = Cookbook.RecipeJson.GetRecipes();
            this.AvaPlot1.Configuration.WarnIfRenderNotCalledManually = false;
        }

        private void AvaPlot1_Rendered(object sender, EventArgs e)
        {
            if (this.AvaPlot1.IsVisible)
                this.BenchmarkLabel.Text = this.AvaPlot1.Plot.GetSettings(false).BenchmarkMessage.Message;
            else
                this.BenchmarkLabel.Text = "This plot is a non-interactive Bitmap";
        }

        public void LoadDemo(string id)
        {
            Cookbook.IRecipe recipe = Cookbook.Locate.GetRecipe(id);

            this.DemoNameLabel.Text = recipe.Title;
            this.SourceCodeLabel.Text = "Source Code";
            this.DescriptionTextbox.Text = recipe.Description;
            string source = Recipes is null ? Cookbook.RecipeJson.NotFoundMessage : Recipes[id].Code;
            this.SourceTextBox.Text = source.Replace("\n", Environment.NewLine);

            this.AvaPlot1.Reset();
            this.imagePlot.IsVisible = false;
            this.AvaPlot1.IsVisible = true;
            recipe.ExecuteRecipe(this.AvaPlot1.Plot);
            this.AvaPlot1.Refresh();
        }
    }
}
