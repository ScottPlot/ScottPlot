using System;
using System.Collections.Generic;
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

        private void LoadTreeWithDemos()
        {
            var quickstart = new TreeViewItem { Header = "Quickstart Plots" };
            TreeView1.Items.Add(quickstart);

            var plotTypesItem = new TreeViewItem { Header = "Plot Types" };
            TreeView1.Items.Add(plotTypesItem);

            Assembly asm = Assembly.LoadFrom("ScottPlot.Demo.dll");

            foreach (Type plotType in asm.GetTypes())
            {
                if (plotType.ToString().Contains("PlotTypes"))
                {
                    string plotTypeName = plotType.ToString();
                    plotTypeName = plotType.ToString().Replace("ScottPlot.Demo.PlotTypes.", "");
                    plotTypeName = plotTypeName.Split("+")[0];

                    var plotTypeItem = new TreeViewItem { Header = plotTypeName };
                    plotTypesItem.Items.Add(plotTypeItem);

                    foreach (MethodInfo plotTypeMethod in plotType.GetMethods())
                    {
                        if (plotTypeMethod.ReturnType.ToString() == "ScottPlot.Plot")
                        {
                            var plotTypeMethodItem = new TreeViewItem { Header = plotTypeMethod.Name };
                            plotTypeItem.Items.Add(plotTypeMethodItem);
                        }
                    }
                }
            }

        }
    }
}