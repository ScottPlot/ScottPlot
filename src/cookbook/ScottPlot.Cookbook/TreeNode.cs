using System;
using System.Collections.Generic;
using System.Text;

namespace ScottPlot.Cookbook
{
    public class TreeNode
    {
        public string Title { get; set; }
        public string ID { get; set; }
        public bool IsExpanded { get; set; }
        public bool IsSelected { get; set; }
        public List<TreeNode> Items { get; set; } = new List<TreeNode>();

        /// <summary>
        /// Create a category-level node
        /// </summary>
        public TreeNode(string category)
        {
            Title = category;
        }

        /// <summary>
        /// Create a node describing a single recipe
        /// </summary>
        public TreeNode(IRecipe recipe)
        {
            Title = recipe.Title;
            ID = recipe.ID;
        }
    }
}
