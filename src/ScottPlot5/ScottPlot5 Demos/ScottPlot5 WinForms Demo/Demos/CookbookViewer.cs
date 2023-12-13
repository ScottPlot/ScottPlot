using ScottPlotCookbook;
using ScottPlotCookbook.Recipes;
using ScottPlotCookbook.Website;

namespace WinForms_Demo.Demos;

public partial class CookbookViewer : Form, IDemoWindow
{
    public string Title => "ScottPlot Cookbook";

    public string Description => "Common ScottPlot features demonstrated " +
        "as interactive graphs displayed next to the code used to create them";

    IEnumerable<IRecipe> InstantiatedRecipes = Query.GetInstantiatedRecipes();
    Dictionary<ICategory, IEnumerable<RecipeInfo>> RecipeInfoByCategory = Query.GetWebRecipesByCategory();

    public CookbookViewer()
    {
        InitializeComponent();
    }

    private void CookbookViewer_Load(object sender, EventArgs e)
    {
        listView1.Items.Clear();
        listView1.Groups.Clear();

        foreach (string chapter in Query.GetChapterNamesInOrder())
        {
            IEnumerable<ICategory> categories = RecipeInfoByCategory.Keys.Where(x => x.Chapter == chapter);
            foreach (ICategory category in categories)
            {
                ListViewGroup group = new()
                {
                    HeaderAlignment = HorizontalAlignment.Center,
                    Header = category.CategoryName,
                };

                listView1.Groups.Add(group);

                foreach (RecipeInfo recipe in RecipeInfoByCategory[category])
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

        IRecipe recipe = InstantiatedRecipes.Where(x => x.Name == listView1.SelectedItems[0].Text).Single();

        formsPlot1.Reset();
        recipe.Execute(formsPlot1.Plot);
        formsPlot1.Refresh();

        RecipeInfo recipeInfo = RecipeInfoByCategory.Values.SelectMany(x => x).Where(x => x.Name == recipe.Name).Single();
        richTextBox1.Text = recipeInfo.Source;
    }
}
