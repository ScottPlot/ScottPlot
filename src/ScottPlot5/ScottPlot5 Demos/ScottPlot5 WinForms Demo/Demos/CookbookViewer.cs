using ScottPlotCookbook;
using ScottPlotCookbook.Recipes;

namespace WinForms_Demo.Demos;

public partial class CookbookViewer : Form, IDemoWindow
{
    public string Title => "ScottPlot Cookbook";

    public string Description => "Common ScottPlot features demonstrated " +
        "as interactive graphs displayed next to the code used to create them";

    public CookbookViewer()
    {
        InitializeComponent();
    }

    private void CookbookViewer_Load(object sender, EventArgs e)
    {
        listView1.Items.Clear();
        listView1.Groups.Clear();

        foreach (Chapter chapter in Cookbook.GetChapters())
        {
            foreach (RecipePageBase recipePage in Cookbook.GetPagesInChapter(chapter))
            {
                ListViewGroup group = new()
                {
                    HeaderAlignment = HorizontalAlignment.Center,
                    Header = recipePage.PageDetails.PageName,
                };

                listView1.Groups.Add(group);

                foreach (RecipeBase recipe in recipePage.GetRecipes())
                {

                    ListViewItem item = new()
                    {
                        Text = recipe.Name,
                        Tag = recipe,
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

        RecipeBase recipe = (RecipeBase)listView1.SelectedItems[0].Tag;
        formsPlot1.Reset();
        recipe.Execute(formsPlot1.Plot);
        formsPlot1.Refresh();
    }
}
