using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ScottPlot.Cookbook
{
    public static class Category
    {
        public static ICategory[] GetCategories()
        {
            return Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(x => x.GetInterfaces().Contains(typeof(ICategory)))
                .Select(x => (ICategory)Activator.CreateInstance(x))
                .ToArray();
        }
    }
}
