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
                var demoPlot = Reflection.GetPlot(selectedDemoItem.Tag.ToString());

                SelectedDemoLabel.Content = selectedDemoItem.Tag;
                DescriptionTextbox.Text = demoPlot.description;
                //https://github.com/swharden/ScottPlot/blob/master/src/ScottPlot.Demo/PlotTypes/BoxAndWhisker.cs

                wpfPlot1.plt.GetPlottables().Clear();
                wpfPlot1.plt.Clear();
                wpfPlot1.plt.XTicks();
                demoPlot.Render(wpfPlot1.plt);
                wpfPlot1.Render();
            }
        }

        private void LoadTreeWithDemos()
        {
            // PLOT GENERAL

            var generalTreeItem = new TreeViewItem() { Header = "General Plots", IsExpanded = true };
            DemoTreeview.Items.Add(generalTreeItem);

            string generalPrefix = "ScottPlot.Demo.General.";
            foreach (string demoName in Demo.Reflection.GetDemoPlots(generalPrefix))
            {
                IPlotDemo plotDemo = Reflection.GetPlot(demoName);
                generalTreeItem.Items.Add(new TreeViewItem() { Header = plotDemo.name, ToolTip = plotDemo.description, Tag = demoName.ToString() });
            }

            var plotTypesTreeItem = new TreeViewItem() { Header = "Plot Types", IsExpanded = true };
            DemoTreeview.Items.Add(plotTypesTreeItem);

            // PLOT TYPES

            string plotTypsPrefix = "ScottPlot.Demo.PlotTypes.";
            foreach (string demoName in Demo.Reflection.GetDemoPlots(plotTypsPrefix))
            {
                IPlotDemo plotDemo = Reflection.GetPlot(demoName);
                plotTypesTreeItem.Items.Add(new TreeViewItem() { Header = plotDemo.name, ToolTip = plotDemo.description, Tag = demoName.ToString() });
            }

            // EXPERIMENTAL

            var experimentalTreeItem = new TreeViewItem() { Header = "Experimental", IsExpanded = true };
            DemoTreeview.Items.Add(experimentalTreeItem);
        }
    }
}