using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ScottPlot.Demo.WPF
{
    /// <summary>
    /// Interaction logic for DemoNavigator.xaml
    /// </summary>
    public partial class CookbookWindow : Window
    {
        public CookbookWindow()
        {
            InitializeComponent();
            LoadTreeWithDemos();
        }

        private void DemoSelected(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            TreeViewItem sel = (TreeViewItem)DemoTreeview.SelectedItem;
            if (sel is null || sel.Tag is null)
                return;
            DemoPlotControl1.LoadDemo(sel.Tag.ToString());
        }

        private void LoadTreeWithDemos()
        {
            DemoTreeview.Items.Clear();
            foreach (var dict in Cookbook.Locate.GetCategorizedRecipes())
            {
                string category = dict.Key;
                Cookbook.IRecipe[] recipes = dict.Value;

                TreeViewItem categoryNode = new TreeViewItem() { Header = category };
                DemoTreeview.Items.Add(categoryNode);

                foreach (Cookbook.IRecipe recipe in recipes)
                {
                    TreeViewItem recipeNode = new TreeViewItem() { Header = recipe.Title, Tag = recipe.ID };
                    categoryNode.Items.Add(recipeNode);
                }
            }

            DemoTreeview.Focus();
            ((TreeViewItem)DemoTreeview.Items[0]).IsExpanded = true;
            ((TreeViewItem)((TreeViewItem)DemoTreeview.Items[0]).Items[0]).IsSelected = true;
        }
    }
}
