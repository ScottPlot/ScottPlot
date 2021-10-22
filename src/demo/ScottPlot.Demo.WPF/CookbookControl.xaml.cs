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
        readonly Dictionary<string, Cookbook.RecipeSource> Recipes;

        public CookbookControl()
        {
            InitializeComponent();
            Recipes = Cookbook.RecipeJson.GetRecipes();
            wpfPlot1.Configuration.WarnIfRenderNotCalledManually = false;
        }

        public void LoadDemo(string id)
        {
            var recipe = Cookbook.Locate.GetRecipe(id);
            string source = Cookbook.RecipeJson.NotFoundMessage;
            if ((Recipes is object) && Recipes.ContainsKey(id))
                source = Recipes[id].Code;

            DemoNameLabel.Content = recipe.Title;
            SourceCodeLabel.Content = recipe.ID;
            DescriptionTextbox.Text = recipe.Description;
            SourceTextBox.Text = source.Replace("\n", Environment.NewLine);

            wpfPlot1.Reset();
            recipe.ExecuteRecipe(wpfPlot1.Plot);
            wpfPlot1.Refresh();
        }
    }
}
