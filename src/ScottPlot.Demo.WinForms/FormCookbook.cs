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
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void LoadTreeWithDemos()
        {
            IPlotDemo[] plots = Reflection.GetPlotsInOrder();
            IEnumerable<string> majorCategories = plots.Select(x => x.categoryMajor).Distinct();

            foreach (string majorCategory in majorCategories)
            {
                var majorNode = new TreeNode(majorCategory);
                treeView1.Nodes.Add(majorNode);

                IEnumerable<string> minorCategories = plots.Where(x => x.categoryMajor == majorCategory).Select(x => x.categoryMinor).Distinct();
                foreach (string minorCategory in minorCategories)
                {
                    var minorNode = new TreeNode(minorCategory);
                    majorNode.Nodes.Add(minorNode);

                    IEnumerable<IPlotDemo> categoryPlots = plots.Where(x => x.categoryMajor == majorCategory && x.categoryMinor == minorCategory);
                    foreach (IPlotDemo demoPlot in categoryPlots)
                    {
                        var classNode = new TreeNode(demoPlot.name);
                        classNode.Tag = demoPlot.classPath.ToString();
                        minorNode.Nodes.Add(classNode);
                    }
                }
            }

            treeView1.SelectedNode = treeView1.Nodes[0].Nodes[0].Nodes[0];
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
