using Eto.Forms;
using Eto.Drawing;

namespace ScottPlot.Demo.Eto
{
    partial class FormCookbook : Form
    {
        private void InitializeComponent()
        {
            //this.components = new System.ComponentModel.Container();
            //System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormCookbook));
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
            //this.treeView1.BackgroundColor = SystemColors.Control;
            this.treeView1.Border = BorderType.None;
            //this.treeView1.Dock = System.Windows.Forms.DockStyle.Fill;
            //this.treeView1.Location = new System.Drawing.Point(4, 19);
            //this.treeView1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            //this.treeView1.Name = "treeView1";
            this.treeView1.Size = new Size(200, 700);
            this.treeView1.Columns.Add(new GridColumn { DataCell = new TextBoxCell(0), Width = treeView1.Width, HeaderText = "ColumnHeader", Expand = true });
            this.treeView1.ShowHeader = false;
            //this.treeView1.AllowEmptySelection = false;
            this.treeView1.TabIndex = 0;
            this.treeView1.SelectedItemChanged += TreeView1_SelectedItemChanged;
            // 
            // groupBox1
            // 
            //this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            //| System.Windows.Forms.AnchorStyles.Left)));
            var scrollable = new Scrollable { Content = this.treeView1 };
            this.groupBox1.Content = this.treeView1; // Content this.treeView1; // .Controls.Add(this.treeView1);
            //this.groupBox1.Location = new System.Drawing.Point(14, 14);
            //this.groupBox1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            //this.groupBox1.Name = "groupBox1";
            //this.groupBox1.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.groupBox1.Size = new Size(-1, 793);
            //this.groupBox1.Size = new Size(350, 793);

            this.groupBox1.Padding = 5;
            this.groupBox1.TabIndex = 1;
            //this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Demo Plots";
            // 
            // DemoNameLabel
            // 
            //this.DemoNameLabel.AutoSize = true;
            this.DemoNameLabel.Font = Fonts.Sans(16);// new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            //this.DemoNameLabel.Location = new System.Drawing.Point(365, 14);
            //this.DemoNameLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            //this.DemoNameLabel.Name = "DemoNameLabel";
            //this.DemoNameLabel.Size = new Size(226, 25);
            this.DemoNameLabel.TabIndex = 2;
            this.DemoNameLabel.Text = "Scatter Plot Quickstart";
            // 
            // DescriptionTextbox
            // 
            //this.DescriptionTextbox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            //| System.Windows.Forms.AnchorStyles.Right)));
            this.DescriptionTextbox.BackgroundColor = SystemColors.Control;
            //this.DescriptionTextbox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            //this.DescriptionTextbox.Location = new System.Drawing.Point(371, 46);
            //this.DescriptionTextbox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            //this.DescriptionTextbox.Multiline = true;
            //this.DescriptionTextbox.Name = "DescriptionTextbox";
            //this.DescriptionTextbox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.DescriptionTextbox.Size = new Size(705, 61);
            this.DescriptionTextbox.TabIndex = 0;
            this.DescriptionTextbox.Text = "description...";
            // 
            // sourceCodeGroupbox
            // 
            //this.sourceCodeGroupbox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            //| System.Windows.Forms.AnchorStyles.Right)));
            this.sourceCodeGroupbox.Content = this.sourceCodeTextbox; // .Controls.Add(this.sourceCodeTextbox);
            //this.sourceCodeGroupbox.Location = new System.Drawing.Point(371, 552);
            //this.sourceCodeGroupbox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            //this.sourceCodeGroupbox.Name = "sourceCodeGroupbox";
            //this.sourceCodeGroupbox.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.sourceCodeGroupbox.Size = new Size(705, 255);
            this.sourceCodeGroupbox.TabIndex = 6;
            //this.sourceCodeGroupbox.TabStop = false;
            this.sourceCodeGroupbox.Text = "Source Code";
            // 
            // sourceCodeTextbox
            // 
            //this.sourceCodeTextbox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sourceCodeTextbox.Font = Fonts.Sans(8);// new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            //this.sourceCodeTextbox.Location = new System.Drawing.Point(4, 19);
            //this.sourceCodeTextbox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            //this.sourceCodeTextbox.Multiline = true;
            //this.sourceCodeTextbox.Name = "sourceCodeTextbox";
            //this.sourceCodeTextbox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.sourceCodeTextbox.Size = new Size(697, 233);
            this.sourceCodeTextbox.TabIndex = 0;
            this.sourceCodeTextbox.Wrap = false;
            // 
            // formsPlot1
            // 
            //this.formsPlot1.Dock = System.Windows.Forms.DockStyle.Fill;
            //this.formsPlot1.Location = new System.Drawing.Point(4, 19);
            //this.formsPlot1.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            //this.formsPlot1.Name = "formsPlot1";
            //this.formsPlot1.Size = new Size(693, 410);
            this.formsPlot1.TabIndex = 0;
            // 
            // gbPlot
            // 
            //this.gbPlot.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            //| System.Windows.Forms.AnchorStyles.Left) 
            //| System.Windows.Forms.AnchorStyles.Right)));
            this.gbPlot.Content = this.formsPlot1; // .Controls.Add(this.formsPlot1);
            //this.gbPlot.Location = new System.Drawing.Point(374, 114);
            //this.gbPlot.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            //this.gbPlot.Name = "gbPlot";
            //this.gbPlot.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.gbPlot.Size = new Size(705, 432);
            this.gbPlot.TabIndex = 7;
            //this.gbPlot.TabStop = false;
            this.gbPlot.Text = "Interactive Plot";
            // 
            // FormCookbook
            // 
            //this.AutoScaleDimensions = new SizeF(7F, 15F);
            //this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new Size(-1, this.groupBox1.Height + 10);
            this.Resizable = false;
            //this.Controls.Add(this.gbPlot);
            //this.Controls.Add(this.DescriptionTextbox);
            //this.Controls.Add(this.sourceCodeGroupbox);
            //this.Controls.Add(this.DemoNameLabel);
            //this.Controls.Add(this.groupBox1);
            //this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            //this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            //this.Name = "FormCookbook";
            //this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Title = "ScottPlot Demos (Eto.Forms)";
            this.groupBox1.ResumeLayout();
            this.sourceCodeGroupbox.ResumeLayout();
            //this.sourceCodeGroupbox.PerformLayout();
            this.gbPlot.ResumeLayout();
            this.ResumeLayout();
            //this.PerformLayout();
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

