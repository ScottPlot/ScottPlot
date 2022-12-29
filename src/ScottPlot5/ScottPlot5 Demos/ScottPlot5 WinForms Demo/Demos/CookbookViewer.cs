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

    }
}
