using System;
using System.Collections.Generic;
using System.Text;

namespace ScottPlot.Cookbook
{
    public static class Tree
    {
        public static List<TreeNode> GetRecipes()
        {
            var categoryNodes = new List<TreeNode>();

            foreach (var dict in Locate.GetCategorizedRecipes())
            {
                string category = dict.Key;
                IRecipe[] recipes = dict.Value;

                var categoryNode = new TreeNode(category);
                categoryNodes.Add(categoryNode);

                foreach (IRecipe recipe in recipes)
                    categoryNode.Items.Add(new TreeNode(recipe));
            }

            return categoryNodes;
        }
    }
}
