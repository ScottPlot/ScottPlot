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
            wpfPlot1.Rendered += WpfPlot1_Rendered;
            LoadTreeWithDemos();
            LoadDemo("ScottPlot.Demo.General.Plots+SinAndCos");
        }

        private void WpfPlot1_Rendered(object sender, EventArgs e)
        {
            BenchmarkLabel.Content = wpfPlot1.plt.GetSettings(false).benchmark.ToString();
        }

        private void DemoSelected(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            var selectedDemoItem = (TreeViewItem)DemoTreeview.SelectedItem;
            if (selectedDemoItem.Tag != null)
            {
                wpfPlot1.Visibility = Visibility.Visible;
                LoadDemo(selectedDemoItem.Tag.ToString());
            }
            else
            {
                wpfPlot1.Visibility = Visibility.Hidden;
            }
        }

        private void LoadDemo(string objectPath)
        {
            Debug.WriteLine($"Loading demo: {objectPath}");
            string fileName = "/src/" + objectPath.Split('+')[0].Replace(".", "/") + ".cs";
            string methodName = objectPath.Split('+')[1];
            var demoPlot = Reflection.GetPlot(objectPath);

            DemoNameLabel.Content = demoPlot.name;
            DemoFileLabel.Content = $"{fileName} ({methodName})";
            DescriptionTextbox.Text = (demoPlot.description is null) ? "no descriton provided..." : demoPlot.description;

            wpfPlot1.plt.GetPlottables().Clear();
            wpfPlot1.plt.Clear();
            wpfPlot1.plt.XTicks();
            demoPlot.Render(wpfPlot1.plt);
            wpfPlot1.Render();
        }


        private void LoadTreeWithDemos()
        {
            // TODO: make this tree in our own class and use binding to display it

            // GENERAL
            var generalTreeItem = new TreeViewItem() { Header = "General Plots", IsExpanded = true };
            DemoTreeview.Items.Add(generalTreeItem);
            foreach (string demoName in Demo.Reflection.GetDemoPlots("ScottPlot.Demo.General."))
            {
                IPlotDemo plotDemo = Reflection.GetPlot(demoName);
                generalTreeItem.Items.Add(new TreeViewItem() { Header = plotDemo.name, ToolTip = plotDemo.description, Tag = demoName.ToString() });
            }

            // PLOT TYPES
            var plotTypesTreeItem = new TreeViewItem() { Header = "Plot Types", IsExpanded = true };
            DemoTreeview.Items.Add(plotTypesTreeItem);
            foreach (string demoName in Demo.Reflection.GetDemoPlots("ScottPlot.Demo.PlotTypes."))
            {
                IPlotDemo plotDemo = Reflection.GetPlot(demoName);
                plotTypesTreeItem.Items.Add(new TreeViewItem() { Header = plotDemo.name, ToolTip = plotDemo.description, Tag = demoName.ToString() });
            }

            // STYLE
            var styleTreeItem = new TreeViewItem() { Header = "Custom Plot Styles", IsExpanded = false };
            DemoTreeview.Items.Add(styleTreeItem);
            foreach (string demoName in Demo.Reflection.GetDemoPlots("ScottPlot.Demo.Style."))
            {
                IPlotDemo plotDemo = Reflection.GetPlot(demoName);
                styleTreeItem.Items.Add(new TreeViewItem() { Header = plotDemo.name, ToolTip = plotDemo.description, Tag = demoName.ToString() });
            }

            // EXPERIMENTAL
            var experimentalTreeItem = new TreeViewItem() { Header = "Experimental Plots", IsExpanded = true };
            DemoTreeview.Items.Add(experimentalTreeItem);
            foreach (string demoName in Demo.Reflection.GetDemoPlots("ScottPlot.Demo.Experimental."))
            {
                IPlotDemo plotDemo = Reflection.GetPlot(demoName);
                experimentalTreeItem.Items.Add(new TreeViewItem() { Header = plotDemo.name, ToolTip = plotDemo.description, Tag = demoName.ToString() });
            }
        }
    }
}