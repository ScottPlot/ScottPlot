using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ScottPlot.Demo.WinForms
{
    public partial class FormCookbook : Form
    {
        public FormCookbook()
        {
            InitializeComponent();
            pictureBox1.Dock = DockStyle.Fill;
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

            var demoPlot = Cookbook.Locate.GetRecipe(id);

            DemoNameLabel.Text = demoPlot.Title;
            DescriptionTextbox.Text = demoPlot.Description;
            sourceCodeTextbox.Text = Cookbook.Locate.RecipeSourceCode(id);

            formsPlot1.Reset();

            if (demoPlot is IRecipeNonInteractive bmpPlot)
            {
                formsPlot1.Visible = false;
                pictureBox1.Visible = true;
                pictureBox1.Image = bmpPlot.Render(800, 600);
            }
            else
            {
                formsPlot1.Visible = true;
                pictureBox1.Visible = false;

                demoPlot.ExecuteRecipe(formsPlot1.plt);
                formsPlot1.Render();
            }
        }
    }
}
