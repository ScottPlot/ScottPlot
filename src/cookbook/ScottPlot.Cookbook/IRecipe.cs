using System;
using System.Collections.Generic;
using System.Text;

namespace ScottPlot.Cookbook
{
    public interface IRecipe
    {
        // TODO: test to ensure all nodes with children have Introduction child
        // TODO: test to ensure no duplicate IDs

        /// <summary>
        /// Hierarchical node names separated by underscores.
        /// Nodes are sorted lexicographically, so prepend names with numbers to define a specific order.
        /// </summary>
        string ID { get; }
        string Title { get; }
        string Description { get; }
        void ExecuteRecipe(Plot plt);
    }
}
