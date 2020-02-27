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
            SelectedDemoLabel.Content = selectedDemoItem.Tag;
            //Debug.WriteLine($"Selected: {selectedDemoItem.Tag}");
            //https://github.com/swharden/ScottPlot/blob/master/src/ScottPlot.Demo/PlotTypes/BoxAndWhisker.cs

            var demoPlot = Reflection.GetPlot(selectedDemoItem.Tag.ToString());
            wpfPlot1.plt.GetPlottables().Clear();
            wpfPlot1.plt.Clear();
            demoPlot.Render(wpfPlot1.plt);
            wpfPlot1.Render();
        }

        private void LoadTreeWithDemos()
        {
            foreach (var demoPlot in Demo.Reflection.GetDemoPlots())
            {
                string shortName = demoPlot.Replace("ScottPlot.Demo.PlotTypes.", "");
                DemoTreeview.Items.Add(new TreeViewItem() { Header = shortName, Tag = demoPlot.ToString() });
            }
        }
    }
}