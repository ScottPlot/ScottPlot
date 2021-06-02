using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScottPlot.Cookbook.Website
{
    class RecipeCard : IPageElement
    {
        public string Markdown { get; private set; }

        public RecipeCard(IRecipe recipe)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"<div class='col-md-4 mb-3'>");
            sb.AppendLine($"  <div class='card'>");
            sb.AppendLine($"    <div class='card-header'><strong>");
            sb.AppendLine($"      <a href='{GetRecipeUrl(recipe)}'>{recipe.Title}</a>");
            sb.AppendLine($"      </strong></div>");
            sb.AppendLine($"    <div class='card-body'>");
            sb.AppendLine($"      <p class='card-text'>{recipe.Description}</p>");
            sb.AppendLine($"      <div class='text-center'>");
            sb.AppendLine($"        <a href='{GetRecipeUrl(recipe)}'>");
            sb.AppendLine($"          <img src='{GetThumbnailUrl(recipe)}' class='mw-100' />");
            sb.AppendLine($"        </a>");
            sb.AppendLine($"      </div>");
            sb.AppendLine($"    </div>");
            sb.AppendLine($"  </div>");
            sb.AppendLine($"</div>");

            // put a huge HTML block in markdown
            Markdown = string.Join("", sb.ToString().Split('\n').Select(x => x.Trim()));
        }
        private static string GetRecipeUrl(IRecipe recipe) =>
            $"category/{Page.Sanitize(recipe.Category)}/#{Page.Sanitize(recipe.Title)}";

        private static string GetThumbnailUrl(IRecipe recipe) =>
            $"images/{Page.Sanitize(recipe.ID)}_thumb.jpg";
    }
}
