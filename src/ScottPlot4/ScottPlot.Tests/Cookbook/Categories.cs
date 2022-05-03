using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ScottPlot.Tests.Cookbook
{
    internal class Categories
    {
        [Test]
        public void Test_Categories_HaveValidText()
        {
            foreach (ScottPlot.Cookbook.ICategory category in ScottPlot.Cookbook.Category.GetCategories())
            {
                Console.WriteLine(category);

                Assert.IsNotNull(category.Name);
                Assert.IsNotEmpty(category.Name);
                Assert.AreEqual(category.Name.Trim(), category.Name);

                Assert.IsNotNull(category.Description);
                Assert.IsNotEmpty(category.Description);
                Assert.AreEqual(category.Description.Trim(), category.Description);

                // TODO: this should be opposite
                Assert.That(!category.Description.EndsWith("."));
            }
        }
    }
}
