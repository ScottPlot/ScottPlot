using Avalonia.Controls;

namespace ScottPlot.Demo.Avalonia
{
    public partial class CookbookWindow : Window
    {
        public CookbookWindow()
        {
            this.InitializeComponent();
            LoadTreeWithDemos();

            this.DemoTreeview.SelectionChanged += DemoSelected;
        }

        private void DemoSelected(object sender, SelectionChangedEventArgs e)
        {
            TreeView DemoTreeview = (TreeView)sender;

            Cookbook.TreeNode selectedDemoItem = null;
            if (DemoTreeview.SelectedItems.Count > 0)
            {
                selectedDemoItem = (Cookbook.TreeNode)DemoTreeview.SelectedItems[0];
            }
            if (selectedDemoItem?.ID is not null)
            {
                DemoPlotControl1.IsVisible = true;
                AboutControl1.IsVisible = false;
                DemoPlotControl1.LoadDemo(selectedDemoItem.ID);
            }
            else
            {
                DemoPlotControl1.IsVisible = false;
                AboutControl1.IsVisible = true;
            }
        }

        private void LoadTreeWithDemos()
        {
            var demos = Cookbook.Tree.GetRecipes();
            DemoTreeview.ItemsSource = demos;

            DemoPlotControl1.IsVisible = true;
            AboutControl1.IsVisible = false;
            DemoPlotControl1.LoadDemo(demos[0].Items[0].ID);
        }
    }
}
