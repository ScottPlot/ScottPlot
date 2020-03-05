using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Linq;
using System.Diagnostics;

namespace ScottPlot.Demo.WinForms
{
    public partial class FormCookbook : Form
    {
        public FormCookbook()
        {
            InitializeComponent();
            LoadTreeWithDemos();
            treeView1.SelectedNode = treeView1.Nodes[0].Nodes[0];
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void LoadTreeWithDemos()
        {
            // TODO: make this tree in our own class and use binding to display it

            // GENERAL
            var generalTreeItem = new TreeNode("General Plots");
            treeView1.Nodes.Add(generalTreeItem);
            generalTreeItem.Expand();
            foreach (IPlotDemo plotDemo in Demo.Reflection.GetPlots("ScottPlot.Demo.General."))
            {
                generalTreeItem.Nodes.Add(new TreeNode { Text = plotDemo.name, ToolTipText = plotDemo.description, Tag = plotDemo.GetType().ToString() });
            }

            // PLOT TYPES
            var plotTypesTreeItem = new TreeNode("Plot Types");
            treeView1.Nodes.Add(plotTypesTreeItem);
            foreach (IPlotDemo plotDemo in Demo.Reflection.GetPlots("ScottPlot.Demo.PlotTypes."))
            {
                plotTypesTreeItem.Nodes.Add(new TreeNode { Text = plotDemo.name, ToolTipText = plotDemo.description, Tag = plotDemo.GetType().ToString() });
            }

            // STYLE
            var styleTreeItem = new TreeNode("Custom Plot Styles");
            treeView1.Nodes.Add(styleTreeItem);
            foreach (IPlotDemo plotDemo in Demo.Reflection.GetPlots("ScottPlot.Demo.Style."))
            {
                styleTreeItem.Nodes.Add(new TreeNode { Text = plotDemo.name, ToolTipText = plotDemo.description, Tag = plotDemo.GetType().ToString() });
            }

            // EXPERIMENTAL
            var experimentalTreeItem = new TreeNode("Experimental Plots");
            treeView1.Nodes.Add(experimentalTreeItem);
            if (Debugger.IsAttached)
                experimentalTreeItem.Expand();
            foreach (IPlotDemo plotDemo in Demo.Reflection.GetPlots("ScottPlot.Demo.Experimental."))
            {
                experimentalTreeItem.Nodes.Add(new TreeNode { Text = plotDemo.name, ToolTipText = plotDemo.description, Tag = plotDemo.GetType().ToString() });
            }
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (treeView1.SelectedNode?.Tag is null)
                return;

            string tag = treeView1.SelectedNode.Tag.ToString();
            if (tag != null)
                LoadDemo(tag);
        }

        private void LoadDemo(string objectPath)
        {
            var demoPlot = Reflection.GetPlot(objectPath);

            DemoNameLabel.Text = demoPlot.name;
            sourceCodeGroupbox.Text = $"SourceCode: {demoPlot.classPath.Replace("+",".")}";
            DescriptionTextbox.Text = (demoPlot.description is null) ? "no descriton provided..." : demoPlot.description;
            sourceCodeTextbox.Text = demoPlot.GetSourceCode("../../../../src/ScottPlot.Demo/");

            formsPlot1.Reset();
            demoPlot.Render(formsPlot1.plt);
            formsPlot1.Render();
        }

        private void formsPlot1_Rendered(object sender, EventArgs e)
        {
            //PerformanceLabel.Text = formsPlot1.plt.GetSettings(false).benchmark.ToString();
        }
    }
}
