using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScottPlot.Cookbook.Website
{
    class RecipeCard : IPageElement
    {
        public string Markdown { get; private set; }
        public string Html { get; private set; }

        public RecipeCard(IRecipe recipe)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"<div class='col-md-4 mb-3'>");
            sb.AppendLine($"  <div class='card'>");
            sb.AppendLine($"    <div class='card-header'><strong>");
            sb.AppendLine($"      <a href='{Page.GetRecipeUrl(recipe)}'>{recipe.Title}</a>");
            sb.AppendLine($"      </strong></div>");
            sb.AppendLine($"    <div class='card-body'>");
            sb.AppendLine($"      <p class='card-text'>{recipe.Description}</p>");
            sb.AppendLine($"      <div class='text-center'>");
            sb.AppendLine($"        <a href='{Page.GetRecipeUrl(recipe)}'>");
            sb.AppendLine($"          <img src='{Page.GetThumbnailUrl(recipe)}' class='mw-100' />");
            sb.AppendLine($"        </a>");
            sb.AppendLine($"      </div>");
            sb.AppendLine($"    </div>");
            sb.AppendLine($"  </div>");
            sb.AppendLine($"</div>");

            Html = sb.ToString();

            // put a huge HTML block in markdown
            Markdown = string.Join("", Html.Split('\n').Select(x => x.Trim()));
        }
    }
}
