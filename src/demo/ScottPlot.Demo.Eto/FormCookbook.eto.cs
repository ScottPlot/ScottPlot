using Eto.Forms;
using Eto.Drawing;

namespace ScottPlot.Demo.Eto
{
    partial class FormCookbook : Form
    {
        private void InitializeComponent()
        {
            this.treeView1 = new TreeGridView();
            this.groupBox1 = new GroupBox();
            this.DemoNameLabel = new Label();
            this.DescriptionTextbox = new TextArea();
            this.sourceCodeGroupbox = new GroupBox();
            this.sourceCodeTextbox = new TextArea();
            this.formsPlot1 = new ScottPlot.Eto.PlotView();
            this.gbPlot = new GroupBox();
            this.groupBox1.SuspendLayout();
            this.sourceCodeGroupbox.SuspendLayout();
            this.gbPlot.SuspendLayout();
            this.SuspendLayout();

            var layout2 = new StackLayout(DemoNameLabel, DescriptionTextbox, gbPlot, sourceCodeGroupbox) { Padding = 5 };
            this.Content = new StackLayout(groupBox1, layout2) { Orientation = global::Eto.Forms.Orientation.Horizontal, Padding = 5 };

            // 
            // treeView1
            // 
            this.treeView1.Border = BorderType.None;
            this.treeView1.Size = new Size(200, 700);
            this.treeView1.Columns.Add(new GridColumn { DataCell = new TextBoxCell(0), Width = treeView1.Width, HeaderText = "ColumnHeader", Expand = true });
            this.treeView1.ShowHeader = false;
            // 
            // groupBox1
            // 
            this.groupBox1.Content = this.treeView1;
            this.groupBox1.Size = new Size(-1, 793);
            this.groupBox1.Padding = 5;
            this.groupBox1.Text = "Demo Plots";
            // 
            // DemoNameLabel
            // 
            this.DemoNameLabel.Font = Fonts.Sans(16);
            this.DemoNameLabel.Text = "Scatter Plot Quickstart";
            // 
            // DescriptionTextbox
            // 
            this.DescriptionTextbox.BackgroundColor = SystemColors.Control;
            this.DescriptionTextbox.Size = new Size(705, 61);
            this.DescriptionTextbox.Text = "description...";
            // 
            // sourceCodeGroupbox
            // 
            this.sourceCodeGroupbox.Content = this.sourceCodeTextbox;
            this.sourceCodeGroupbox.Size = new Size(705, 255);
            this.sourceCodeGroupbox.Text = "Source Code";
            // 
            // sourceCodeTextbox
            // 
            this.sourceCodeTextbox.Font = Fonts.Sans(8);
            this.sourceCodeTextbox.Size = new Size(697, 233);
            this.sourceCodeTextbox.Wrap = false;
            // 
            // gbPlot
            // 
            this.gbPlot.Content = this.formsPlot1;
            this.gbPlot.Size = new Size(705, 432);
            this.gbPlot.Text = "Interactive Plot";
            // 
            // FormCookbook
            // 
            this.ClientSize = new Size(-1, this.groupBox1.Height + 10);
            this.Resizable = false;
            this.Title = "ScottPlot Demos (Eto.Forms)";
            this.groupBox1.ResumeLayout();
            this.sourceCodeGroupbox.ResumeLayout();
            this.gbPlot.ResumeLayout();
            this.ResumeLayout();
        }

        private TreeGridView treeView1;
        private GroupBox groupBox1;
        private Label DemoNameLabel;
        private GroupBox sourceCodeGroupbox;
        private ScottPlot.Eto.PlotView formsPlot1;
        private TextArea DescriptionTextbox;
        private TextArea sourceCodeTextbox;
        private GroupBox gbPlot;
    }
}

