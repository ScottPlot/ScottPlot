using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ScottPlot.Demo.WPF
{
    /// <summary>
    /// Interaction logic for DemoNavigator.xaml
    /// </summary>
    public partial class DemoNavigator : Window
    {
        public DemoNavigator()
        {
            InitializeComponent();
            LoadTreeWithDemos();
        }

        private void DemoSelected(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            var selectedDemoItem = (TreeViewItem)DemoTreeview.SelectedItem;
            if (selectedDemoItem.Tag != null)
            {
                DemoPlotControl1.Visibility = Visibility.Visible;
                AboutControl.Visibility = Visibility.Hidden;
                DemoPlotControl1.LoadDemo(selectedDemoItem.Tag.ToString());
            }
            else
            {
                DemoPlotControl1.Visibility = Visibility.Hidden;
                AboutControl.Visibility = Visibility.Visible;
            }
        }

        private void LoadTreeWithDemos()
        {
            IPlotDemo[] plots = Reflection.GetPlots();
            IEnumerable<string> majorCategories = plots.Select(x => x.categoryMajor).Distinct();
            foreach (string majorCategory in majorCategories)
            {
                var majorTreeItem = new TreeViewItem() { Header = majorCategory, IsExpanded = true };
                DemoTreeview.Items.Add(majorTreeItem);

                IEnumerable<string> minorCategories = plots.Where(x => x.categoryMajor == majorCategory).Select(x => x.categoryMinor).Distinct();
                foreach (string minorCategory in minorCategories)
                {
                    var minorTreeItem = new TreeViewItem() { Header = minorCategory };
                    if (majorCategory == "PlotTypes" && minorCategory == "Scatter")
                        minorTreeItem.IsExpanded = true;
                    majorTreeItem.Items.Add(minorTreeItem);

                    IEnumerable<IPlotDemo> categoryPlots = plots.Where(x => x.categoryMajor == majorCategory && x.categoryMinor == minorCategory);
                    foreach (IPlotDemo demoPlot in categoryPlots)
                    {
                        var classNameTreeItem = new TreeViewItem() { Header = demoPlot.name, Tag = demoPlot.classPath };
                        if (demoPlot.classPath == "ScottPlot.Demo.PlotTypes.Scatter+Quickstart")
                            classNameTreeItem.IsSelected = true;
                        minorTreeItem.Items.Add(classNameTreeItem);
                    }
                }
            }

            DemoTreeview.Focus();
        }
    }
}