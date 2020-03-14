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
    public partial class CookbookWindow : Window
    {
        public CookbookWindow()
        {
            InitializeComponent();
            LoadTreeWithDemos();
        }

        private void DemoSelected(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            var selectedDemoItem = (DemoNodeItem)DemoTreeview.SelectedItem;
            if (selectedDemoItem.Tag != null)
            {
                DemoPlotControl1.Visibility = Visibility.Visible;
                AboutControl.Visibility = Visibility.Hidden;
                DemoPlotControl1.LoadDemo(selectedDemoItem.Tag);
            }
            else
            {
                DemoPlotControl1.Visibility = Visibility.Hidden;
                AboutControl.Visibility = Visibility.Visible;
            }
        }

        private void LoadTreeWithDemos()
        {
            IPlotDemo[] plots = Reflection.GetPlotsInOrder();

            var demoNodeItems = plots
                .GroupBy(x => x.categoryMajor)
                .Select(majorCategory =>
                    new DemoNodeItem
                    {
                        Header = majorCategory.Key,
                        IsExpanded = true,
                        Items = majorCategory
                            .GroupBy(x => x.categoryMinor)
                            .Select(minorCategory =>
                                new DemoNodeItem
                                {
                                    Header = minorCategory.Key,
                                    IsExpanded = false,
                                    Items = minorCategory
                                        .Select(demoPlot =>
                                                    new DemoNodeItem
                                                    {
                                                        Header = demoPlot.name,
                                                        Tag = demoPlot.classPath.ToString()
                                                    })
                                        .ToList()
                                })
                            .ToList()
                    })
                .ToList();

            // expand and select a default node/demo
            demoNodeItems[0].Items[0].IsExpanded = true;
            demoNodeItems[0].Items[0].Items[0].IsSelected = true;

            DemoTreeview.ItemsSource = demoNodeItems;
            DemoTreeview.Focus();
        }
    }
}