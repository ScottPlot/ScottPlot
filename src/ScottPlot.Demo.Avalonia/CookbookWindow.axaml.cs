using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace ScottPlot.Demo.Avalonia
{
    public class CookbookWindow : Window
    {
        public CookbookWindow()
        {
            this.InitializeComponent();
            LoadTreeWithDemos();
#if DEBUG
            this.AttachDevTools();
#endif

            var demoTreeview = this.Find<TreeView>("DemoTreeview");
            demoTreeview.SelectionChanged += DemoSelected;
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private void DemoSelected(object sender, SelectionChangedEventArgs e)
        {
            TreeView DemoTreeview = this.Find<TreeView>("DemoTreeview");
            CookbookControl DemoPlotControl1 = this.Find<CookbookControl>("DemoPlotControl1");
            var AboutControl1 = this.Find<AboutControl>("AboutControl1");


            DemoNodeItem selectedDemoItem = (DemoNodeItem)DemoTreeview.SelectedItem;
            if (selectedDemoItem.Tag != null)
            {
                DemoPlotControl1.IsVisible = true;
                AboutControl1.IsVisible = false;
                DemoPlotControl1.LoadDemo(selectedDemoItem.Tag);
            }
            else
            {
                DemoPlotControl1.IsVisible = false;
                AboutControl1.IsVisible = true;
            }
        }

        private void LoadTreeWithDemos()
        {
            TreeView DemoTreeview = this.Find<TreeView>("DemoTreeview");
            DemoTreeview.Items = Reflection.GetPlotNodeItems();
            DemoTreeview.Focus();
        }
    }
}
