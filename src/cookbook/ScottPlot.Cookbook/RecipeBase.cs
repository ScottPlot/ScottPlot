using System;
using System.Collections.Generic;
using System.Text;

namespace ScottPlot.Cookbook
{
    public abstract class Recipe
    {
        // TODO: test to ensure all nodes with children have Introduction child
        // TODO: test to ensure no duplicate IDs

        /// <summary>
        /// Hierarchical node names separated by underscores.
        /// Nodes are sorted lexicographically, so prepend names with numbers to define a specific order.
        /// </summary>
        public abstract string ID { get; }
        public abstract string Title { get; }
        public abstract string Description { get; }
        public abstract void ExecuteRecipe(Plot plt);
    }
}
