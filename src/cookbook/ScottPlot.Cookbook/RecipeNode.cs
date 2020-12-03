using System;
using System.Collections.Generic;
using System.Text;

namespace ScottPlot.Cookbook
{
    class RecipeNode
    {
        public string Header { get; set; }
        public bool IsExpanded { get; set; }
        public bool IsSelected { get; set; }
        public string Tag { get; set; }
        public List<RecipeNode> Items { get; set; }
    }
}
