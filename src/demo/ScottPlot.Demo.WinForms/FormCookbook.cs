using System;
using System.Windows.Forms;

namespace ScottPlot.Demo.WinForms
{
    public partial class FormCookbook : Form
    {
        public FormCookbook()
        {
            InitializeComponent();
            LoadTreeWithDemosNew();
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            treeView1.HideSelection = false;
        }

        private void Form1_Load(object sender, EventArgs e) { }

        private void LoadTreeWithDemosNew()
        {
            treeView1.Nodes.Clear();
            foreach (var dict in Cookbook.Locate.GetCategorizedRecipes())
            {
                string category = dict.Key;
                Cookbook.IRecipe[] recipes = dict.Value;

                TreeNode categoryNode = new TreeNode(category);
                treeView1.Nodes.Add(categoryNode);

                foreach (Cookbook.IRecipe recipe in recipes)
                {
                    TreeNode recipeNode = new TreeNode(recipe.Title) { Tag = recipe.ID };
                    categoryNode.Nodes.Add(recipeNode);
                }
            }

            // expand and select the first example
            treeView1.Nodes[0].Expand();
            treeView1.SelectedNode = treeView1.Nodes[0].Nodes[0];
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            TreeNode selectedNode = treeView1.SelectedNode;
            if (selectedNode is null || selectedNode.Tag is null)
                return;
            LoadDemo(selectedNode.Tag.ToString());
        }

        private void LoadDemo(string id)
        {
            if (id is null)
                return;

            var recipe = Cookbook.Locate.GetRecipe(id);
            DemoNameLabel.Text = recipe.Title;
            DescriptionTextbox.Text = recipe.Description;
            sourceCodeTextbox.Text = Cookbook.Locate.RecipeSourceCode(id);

            formsPlot1.Reset();
            recipe.ExecuteRecipe(formsPlot1.Plot);
        }
    }
}
