namespace WinForms_Demo.Demos;

partial class CookbookViewer
{
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        ListViewGroup listViewGroup1 = new ListViewGroup("Group1", HorizontalAlignment.Left);
        ListViewGroup listViewGroup2 = new ListViewGroup("Group2", HorizontalAlignment.Center);
        ListViewGroup listViewGroup3 = new ListViewGroup("Group3", HorizontalAlignment.Left);
        ListViewItem listViewItem1 = new ListViewItem("item1");
        ListViewItem listViewItem2 = new ListViewItem("item2");
        ListViewItem listViewItem3 = new ListViewItem("item3");
        ListViewItem listViewItem4 = new ListViewItem("asdf");
        ListViewItem listViewItem5 = new ListViewItem("qwer");
        ListViewItem listViewItem6 = new ListViewItem("zxcv");
        ListViewItem listViewItem7 = new ListViewItem("dfgh");
        listView1 = new ListView();
        tableLayoutPanel1 = new TableLayoutPanel();
        groupBox1 = new GroupBox();
        richTextBox1 = new RichTextBox();
        formsPlot1 = new ScottPlot.WinForms.FormsPlot();
        tableLayoutPanel1.SuspendLayout();
        groupBox1.SuspendLayout();
        SuspendLayout();
        // 
        // listView1
        // 
        listView1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
        listView1.FullRowSelect = true;
        listViewGroup1.Header = "Group1";
        listViewGroup1.Name = "listViewGroup1";
        listViewGroup1.Subtitle = "asdf";
        listViewGroup2.Footer = "qwerqwer";
        listViewGroup2.Header = "Group2";
        listViewGroup2.HeaderAlignment = HorizontalAlignment.Center;
        listViewGroup2.Name = "listViewGroup2";
        listViewGroup2.Subtitle = "asdfasdf";
        listViewGroup3.Header = "Group3";
        listViewGroup3.Name = "listViewGroup3";
        listViewGroup3.Subtitle = "asdf";
        listView1.Groups.AddRange(new ListViewGroup[] { listViewGroup1, listViewGroup2, listViewGroup3 });
        listViewItem1.Group = listViewGroup2;
        listViewItem5.Group = listViewGroup3;
        listView1.Items.AddRange(new ListViewItem[] { listViewItem1, listViewItem2, listViewItem3, listViewItem4, listViewItem5, listViewItem6, listViewItem7 });
        listView1.Location = new Point(12, 12);
        listView1.MultiSelect = false;
        listView1.Name = "listView1";
        listView1.Size = new Size(192, 426);
        listView1.TabIndex = 1;
        listView1.UseCompatibleStateImageBehavior = false;
        listView1.View = View.SmallIcon;
        listView1.SelectedIndexChanged += listView1_SelectedIndexChanged;
        // 
        // tableLayoutPanel1
        // 
        tableLayoutPanel1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        tableLayoutPanel1.ColumnCount = 1;
        tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        tableLayoutPanel1.Controls.Add(groupBox1, 0, 1);
        tableLayoutPanel1.Controls.Add(formsPlot1, 0, 0);
        tableLayoutPanel1.Location = new Point(210, 12);
        tableLayoutPanel1.Name = "tableLayoutPanel1";
        tableLayoutPanel1.RowCount = 2;
        tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 70F));
        tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 30F));
        tableLayoutPanel1.Size = new Size(578, 426);
        tableLayoutPanel1.TabIndex = 2;
        // 
        // groupBox1
        // 
        groupBox1.Controls.Add(richTextBox1);
        groupBox1.Dock = DockStyle.Fill;
        groupBox1.Location = new Point(3, 301);
        groupBox1.Name = "groupBox1";
        groupBox1.Size = new Size(572, 122);
        groupBox1.TabIndex = 0;
        groupBox1.TabStop = false;
        groupBox1.Text = "Source Code";
        // 
        // richTextBox1
        // 
        richTextBox1.BackColor = SystemColors.Control;
        richTextBox1.BorderStyle = BorderStyle.None;
        richTextBox1.Dock = DockStyle.Fill;
        richTextBox1.Font = new Font("Consolas", 9F, FontStyle.Regular, GraphicsUnit.Point);
        richTextBox1.Location = new Point(3, 19);
        richTextBox1.Name = "richTextBox1";
        richTextBox1.Size = new Size(566, 100);
        richTextBox1.TabIndex = 0;
        richTextBox1.Text = "ScottPlot.Plot = new();";
        // 
        // formsPlot1
        // 
        formsPlot1.DisplayScale = 1F;
        formsPlot1.Dock = DockStyle.Fill;
        formsPlot1.Location = new Point(3, 3);
        formsPlot1.Name = "formsPlot1";
        formsPlot1.Size = new Size(572, 292);
        formsPlot1.TabIndex = 1;
        // 
        // CookbookViewer
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(800, 450);
        Controls.Add(tableLayoutPanel1);
        Controls.Add(listView1);
        Name = "CookbookViewer";
        Text = "ScottPlot Cookbook - Windows Forms";
        Load += CookbookViewer_Load;
        tableLayoutPanel1.ResumeLayout(false);
        groupBox1.ResumeLayout(false);
        ResumeLayout(false);
    }

    #endregion

    private ListView listView1;
    private TableLayoutPanel tableLayoutPanel1;
    private GroupBox groupBox1;
    private RichTextBox richTextBox1;
    private ScottPlot.WinForms.FormsPlot formsPlot1;
}