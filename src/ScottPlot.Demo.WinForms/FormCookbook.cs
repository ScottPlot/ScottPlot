using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ScottPlot.Demo.WinForms
{
    public partial class FormCookbook : Form
    {
        public FormCookbook()
        {
            InitializeComponent();
            Icon = new Icon("icon.ico");
            pictureBox1.Dock = DockStyle.Fill;
            LoadTreeWithDemos();
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void LoadTreeWithDemos()
        {
            foreach (DemoNodeItem majorItem in Reflection.GetPlotNodeItems())
            {
                var majorNode = new TreeNode(majorItem.Header);
                treeView1.Nodes.Add(majorNode);
                foreach (DemoNodeItem minorItem in majorItem.Items)
                {
                    var minorNode = new TreeNode(minorItem.Header);
                    majorNode.Nodes.Add(minorNode);
                    foreach (DemoNodeItem plotItem in minorItem.Items)
                    {
                        var plotNode = new TreeNode(plotItem.Header);
                        plotNode.Tag = plotItem.Tag;
                        minorNode.Nodes.Add(plotNode);
                    }
                }
            }
            treeView1.Nodes[1].Expand();
            treeView1.Nodes[2].Expand();
            treeView1.SelectedNode = treeView1.Nodes[0].Nodes[0].Nodes[0];
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            string tag = treeView1.SelectedNode?.Tag?.ToString();
            if (tag != null)
                LoadDemo(tag);
        }

        private void LoadDemo(string objectPath)
        {
            var demoPlot = Reflection.GetPlot(objectPath);

            DemoNameLabel.Text = demoPlot.name;
            sourceCodeGroupbox.Text = demoPlot.classPath.Replace("+", ".");
            DescriptionTextbox.Text = (demoPlot.description is null) ? "no descriton provided..." : demoPlot.description;
            sourceCodeTextbox.Text = demoPlot.GetSourceCode("../../../../src/ScottPlot.Demo/");

            formsPlot1.Reset();

            if (demoPlot is IBitmapDemo bmpPlot)
            {
                formsPlot1.Visible = false;
                pictureBox1.Visible = true;
                pictureBox1.Image = bmpPlot.Render(800, 600);
                formsPlot1_Rendered(null, null);
            }
            else
            {
                formsPlot1.Visible = true;
                pictureBox1.Visible = false;

                demoPlot.Render(formsPlot1.plt);
                formsPlot1.Render();
            }

        }

        private void formsPlot1_Rendered(object sender, EventArgs e)
        {
            if (formsPlot1.Visible)
                tbBenchmark.Text = formsPlot1.plt.GetSettings(false).benchmark.ToString();
            else
                tbBenchmark.Text = "This plot is a non-interactive Bitmap";
        }
    }
}
