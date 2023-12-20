using ScottPlotCookbook;
using ScottPlotCookbook.Recipes;
using ScottPlotCookbook.Website;

namespace WinForms_Demo.Demos;

public partial class CookbookViewer : Form, IDemoWindow
{
    public string Title => "ScottPlot Cookbook";

    public string Description => "Common ScottPlot features demonstrated " +
        "as interactive graphs displayed next to the code used to create them";

    readonly Dictionary<ICategory, IEnumerable<IRecipe>> RecipesByCategory = Query.GetRecipesByCategory();

    readonly SourceDatabase SB;

    public CookbookViewer()
    {
        InitializeComponent();
        SB = new();
    }

    private void CookbookViewer_Load(object sender, EventArgs e)
    {
        listView1.Items.Clear();
        listView1.Groups.Clear();

        foreach (string chapter in Query.GetChapterNamesInOrder())
        {
            IEnumerable<ICategory> categoriesInChapter = RecipesByCategory.Keys.Where(x => x.Chapter == chapter);
            foreach (ICategory category in categoriesInChapter)
            {
                ListViewGroup group = new()
                {
                    HeaderAlignment = HorizontalAlignment.Center,
                    Header = category.CategoryName,
                };

                listView1.Groups.Add(group);

                foreach (IRecipe recipe in RecipesByCategory[category])
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

        listView1.Items[0].Selected = true;
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
            richTextBox1.Text = "Source code not found.\nRun test suite to generate JSON file.";
        }
        else
        {
            richTextBox1.Text = recipeInfo.Value.Source;
        }
    }
}
