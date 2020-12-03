using System;
using System.Collections.Generic;
using System.Text;

namespace ScottPlot.Cookbook.Site
{
    /// <summary>
    /// This module builds a master index with links to all individual cookbook pages
    /// </summary>
    public class Index
    {
        string imageExtension = ".png";
        string urlExtension = ".html";

        public readonly StringBuilder SB = new StringBuilder();

        public void AddDiv(string html)
        {
            SB.AppendLine($"<div style='margin-top: 20px;'>{html}</div>");
        }

        public void AddHTML(string html)
        {
            SB.AppendLine($"<div style='margin-top: 20px;'>{html}</div>");
        }

        public void AddRecipeGroup(string groupName, IRecipe[] recipes)
        {
            // section header 
            string titleID = SiteGenerator.Sanitize(groupName);
            SB.AppendLine($"<div style='margin-top: 20px; font-size: 200%;'><a href='{titleID}{urlExtension}'>{groupName}</a></div>");

            // bullet list of links to recipes
            SB.AppendLine("<ul>");
            foreach (IRecipe recipe in recipes)
            {
                string pageUrl = $"{titleID}{urlExtension}#{recipe.ID}";
                SB.AppendLine($"<li><a href='{pageUrl}'>{recipe.Title}</a> - {recipe.Description}</li>");
            }
            SB.AppendLine("</ul>");

            // TODO: thumbnails
            foreach (IRecipe recipe in recipes)
            {
                string pageUrl = $"{titleID}{urlExtension}#{recipe.ID}";
                string imageUrl = $"source/{recipe.ID}{imageExtension}";
                SB.AppendLine($"<a href='{pageUrl}'><img src='{imageUrl}' style='height: 150px;'/></a>");
            }
        }

        public void SaveAs(string filePath)
        {
            filePath = System.IO.Path.GetFullPath(filePath);
            System.IO.File.WriteAllText(filePath, SB.ToString());
            Console.WriteLine($"Saved master index: {filePath}");
        }
    }
}
