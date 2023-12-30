using ScottPlotCookbook;
using ScottPlotCookbook.Website;
using System.Linq;

namespace WinForms_Demo.Demos;

public partial class CookbookViewer : Form, IDemoWindow
{
    public string Title => "ScottPlot Cookbook";

    public string Description => "Common ScottPlot features demonstrated " +
        "as interactive graphs displayed next to the code used to create them";

    readonly Dictionary<ICategory, IEnumerable<IRecipe>> RecipesByCategory = Query.GetRecipesByCategory();

    readonly SourceDatabase SB = new();

    public CookbookViewer()
    {
        InitializeComponent();
    }

    private void CookbookViewer_Load(object sender, EventArgs e)
    {
        UpdateRecipeList();
        listView1.Items[0].Selected = true;
        tbFilter.Select();
    }

    private void UpdateRecipeList(string match = "")
    {
        listView1.Items.Clear();
        listView1.Groups.Clear();

        foreach (string chapter in Query.GetChapterNamesInOrder())
        {
            IEnumerable<ICategory> categoriesInChapter = RecipesByCategory.Keys.Where(x => x.Chapter == chapter);
            foreach (ICategory category in categoriesInChapter)
            {
                List<IRecipe> matchingRecipes = new();
                foreach (IRecipe recipe in RecipesByCategory[category])
                {
                    if (!string.IsNullOrEmpty(match))
                    {
                        bool matches =
                            recipe.Name.Contains(match, StringComparison.InvariantCultureIgnoreCase) ||
                            recipe.Description.Contains(match, StringComparison.InvariantCultureIgnoreCase);

                        if (matches == false)
                            continue;
                    }

                    matchingRecipes.Add(recipe);
                }

                if (!matchingRecipes.Any())
                    continue;

                ListViewGroup group = new()
                {
                    HeaderAlignment = HorizontalAlignment.Center,
                    Header = category.CategoryName,
                };

                listView1.Groups.Add(group);

                foreach (IRecipe recipe in matchingRecipes)
                {
                    ListViewItem item = new()
                    {
                        Text = recipe.Name,
                        Group = group,
                    };

                    listView1.Items.Add(item);
                }
            }
        }
    }

    private void listView1_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (listView1.SelectedItems.Count == 0)
            return;

        IRecipe selectedRecipe = RecipesByCategory
            .SelectMany(x => x.Value)
            .Where(x => x.Name == listView1.SelectedItems[0].Text)
            .Single();

        formsPlot1.Reset();
        selectedRecipe.Execute(formsPlot1.Plot);
        formsPlot1.Refresh();

        RecipeInfo? recipeInfo = SB.GetInfo(selectedRecipe);

        if (recipeInfo is null)
        {
            rtbCode.Text = "Source code not found.\nRun test suite to generate JSON file.";
        }
        else
        {
            rtbCode.Text = recipeInfo.Value.Source;
        }
    }

    private void tbFilter_TextChanged(object sender, EventArgs e)
    {
        UpdateRecipeList(tbFilter.Text);
    }
}
