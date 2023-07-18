using System.Linq;
using System.Text;

namespace ScottPlot.Cookbook.Experimental;

public static class MenuBuilder
{
    public static string GetHtml(IRecipe[] recipes)
    {
        StringBuilder sb = new();


        foreach (string category in recipes.Select(x => x.Category.Name).Distinct())
        {
            IRecipe[] categoryRecipes = recipes.Where(x => x.Category.Name == category).ToArray();
            string categoryHtml = GetHtmlMenuGroup(categoryRecipes);
            sb.AppendLine(categoryHtml);
        }

        // this is the whole navbar
        string body = $@"
                <nav class='navbar navbar-expand-md'>
                    <div class='container-fluid'>

                        <!-- hamburger -->
                        <button class='navbar-toggler' type='button' data-bs-toggle='collapse' data-bs-target='#navbarNav'
                            aria-controls='navbarNav' aria-expanded='false' aria-label='Toggle navigation'>
                            <span class='navbar-toggler-icon'></span>
                        </button>

                        <div class='collapse navbar-collapse mt-2' id='navbarNav'>
                            <div class='flex-shrink-0 bg-white' style='width: 280px;'>
                                <ul class='list-unstyled ps-0'>
                                    {sb}
                                </ul>
                            </div>
                        </div>
                    </div>
                </nav>";

        return GetBootstrapHtml(body);
    }

    private static string GetHtmlMenuGroup(IRecipe[] recipes, bool collapsed = true)
    {
        ICategory category = recipes.First().Category;

        StringBuilder recipeListHtml = new();
        foreach (IRecipe recipe in recipes)
        {
            // this is a single recipe link (usually collapsed inside a dropdown)
            recipeListHtml.AppendLine($"<li><a href='#{recipe.ID}'" +
                $" class='link-dark d-inline-flex text-decoration-none rounded'>{recipe.Title}</a></li>");
        }

        // this is a single collapsible dropdown
        string show = collapsed ? "" : "show";
        string coll = collapsed ? "collapsed" : "";
        string expanded = collapsed ? "false" : "true";

        return @$"
                <li class='mb-1'>
                    <button class='btn btn-toggle d-inline-flex align-items-center rounded border-0 {coll}'
                        data-bs-toggle='collapse' data-bs-target='#{category.Folder}-collapse' aria-expanded='{expanded}'>
                        {category.Name}
                    </button>
                    <div class='collapse {show}' id='{category.Folder}-collapse'>
                        <ul class='btn-toggle-nav list-unstyled fw-normal pb-1 small'>
                            {recipeListHtml}
                        </ul>
                    </div>
                </li>";
    }

    private static string GetBootstrapHtml(string body)
    {
        return $@"
                <!doctype html>
                <html lang='en'>

                <head>
                    <meta charset='utf-8'>
                    <meta name='viewport' content='width=device-width, initial-scale=1'>
                    <link href='https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css' rel='stylesheet'
                        integrity='sha384-9ndCyUaIbzAi2FUVXJi0CjmCapSmO7SnpJef0486qhLnuZ2cdeRhO02iuK6FUUVM' crossorigin='anonymous'>
                    <style>
                    {GetHtmlMenuGroupCSS()}
                    </style>
                </head>

                <body>
                    {body}
                    <script src='https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js'
                        integrity='sha384-geWF76RCwLtnZ8qwWowPQNguL3RmwHVBC9FhGdlKrxdiJJigb/j/68SIy3Te4Bkz'
                        crossorigin='anonymous'></script>
                </body>

                </html>";
    }

    private static string GetHtmlMenuGroupCSS()
    {
        return @"
                body {
                    min-height: 100vh;
                    min-height: -webkit-fill-available;
                }

                html {
                    height: -webkit-fill-available;
                }

                main {
                    height: 100vh;
                    height: -webkit-fill-available;
                    max-height: 100vh;
                    overflow-x: auto;
                    overflow-y: hidden;
                }

                .dropdown-toggle {
                    outline: 0;
                }

                .btn-toggle {
                    padding: .25rem .5rem;
                    font-weight: 600;
                    color: rgba(0, 0, 0, .65);
                    background-color: transparent;
                }

                .btn-toggle:hover,
                .btn-toggle:focus {
                    color: rgba(0, 0, 0, .85);
                    background-color: #d2f4ea;
                }

                .btn-toggle::before {
                    width: 1.25em;
                    line-height: 0;
                    content: url(""data:image/svg+xml,%3csvg xmlns='http://www.w3.org/2000/svg' width='16' height='16' viewBox='0 0 16 16'%3e%3cpath fill='none' stroke='rgba%280,0,0,.5%29' stroke-linecap='round' stroke-linejoin='round' stroke-width='2' d='M5 14l6-6-6-6'/%3e%3c/svg%3e"");
                    transition: transform .35s ease;
                    transform-origin: .5em 50%;
                }

                .btn-toggle[aria-expanded='true'] {
                    color: rgba(0, 0, 0, .85);
                }

                .btn-toggle[aria-expanded='true']::before {
                    transform: rotate(90deg);
                }

                .btn-toggle-nav a {
                    padding: .1875rem .5rem;
                    margin-top: .125rem;
                    margin-left: 1.25rem;
                }

                .btn-toggle-nav a:hover,
                .btn-toggle-nav a:focus {
                    background-color: #d2f4ea;
                }

                .scrollarea {
                    overflow-y: auto;
                }";
    }
}
