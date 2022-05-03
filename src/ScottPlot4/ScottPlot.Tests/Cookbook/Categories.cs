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

                Assert.IsNotNull(category.Name, category.ToString());
                Assert.IsNotEmpty(category.Name, category.ToString());
                Assert.AreEqual(category.Name.Trim(), category.Name, category.ToString());

                Assert.IsNotNull(category.Description, category.ToString());
                Assert.IsNotEmpty(category.Description, category.ToString());
                Assert.AreEqual(category.Description.Trim(), category.Description, category.ToString());

                // TODO: this should be opposite
                Assert.That(!category.Description.EndsWith("."), category.ToString());
            }
        }
    }
}
