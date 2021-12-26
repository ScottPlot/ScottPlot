using System;
using System.Collections.Generic;
using Eto.Forms;

namespace ScottPlot.Demo.Eto
{
    public partial class FormCookbook : Form
    {
        readonly Dictionary<string, Cookbook.RecipeSource> Recipes;

        public FormCookbook()
        {
            InitializeComponent();
            Recipes = Cookbook.RecipeJson.GetRecipes();
            formsPlot1.Configuration.WarnIfRenderNotCalledManually = false;

            this.treeView1.SelectedItemChanged += TreeView1_SelectedItemChanged;

            LoadTreeWithDemosNew();
            // todo
            //SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            //treeView.HideSelection = false;
        }

        private void LoadTreeWithDemosNew()
        {
            treeView1.Nodes().Clear();
            foreach (var dict in Cookbook.Locate.GetCategorizedRecipes())
            {
                string category = dict.Key;
                Cookbook.IRecipe[] recipes = dict.Value;

                TreeGridItem categoryNode = new TreeGridItem(category);
                treeView1.Nodes().Add(categoryNode);

                foreach (Cookbook.IRecipe recipe in recipes)
                {
                    TreeGridItem recipeNode = new TreeGridItem(recipe.Title) { Tag = recipe.ID };
                    categoryNode.Children.Add(recipeNode);
                }
            }

            // expand and select the first example
            treeView1.Nodes()[0].Expanded = true;
            treeView1.SelectedItem = (treeView1.Nodes()[0] as TreeGridItem).Children[0];
        }
        private Dictionary<ITreeGridItem, Cookbook.IRecipe> _recipies = new System.Collections.Generic.Dictionary<ITreeGridItem, Cookbook.IRecipe>();
        private void TreeView1_SelectedItemChanged(object sender, EventArgs e)
        {
            TreeGridItem selectedNode = treeView1.SelectedItem as TreeGridItem;
            if (selectedNode is null || selectedNode.Tag is null)
                return;
            LoadDemo(selectedNode.Tag.ToString());
        }

        private void LoadDemo(string id)
        {
            if (id is null)
                return;

            var recipe = Cookbook.Locate.GetRecipe(id);

            string source = Cookbook.RecipeJson.NotFoundMessage;
            if (Recipes != null)
            {
                if (Recipes.ContainsKey(id))
                    source = Recipes[id].Code;
            }

            DemoNameLabel.Text = recipe.Title;
            DescriptionTextbox.Text = recipe.Description;
            sourceCodeTextbox.Text = source.Replace("\n", Environment.NewLine);

            formsPlot1.Reset();
            recipe.ExecuteRecipe(formsPlot1.Plot);
            formsPlot1.Refresh();
        }
    }
}
