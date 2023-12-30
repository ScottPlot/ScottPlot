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
        rtbCode = new RichTextBox();
        formsPlot1 = new ScottPlot.WinForms.FormsPlot();
        groupBox2 = new GroupBox();
        tbFilter = new TextBox();
        tableLayoutPanel1.SuspendLayout();
        groupBox1.SuspendLayout();
        groupBox2.SuspendLayout();
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
        listView1.Size = new Size(192, 568);
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
        tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 63.636364F));
        tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 36.363636F));
        tableLayoutPanel1.Size = new Size(663, 627);
        tableLayoutPanel1.TabIndex = 2;
        // 
        // groupBox1
        // 
        groupBox1.Controls.Add(rtbCode);
        groupBox1.Dock = DockStyle.Fill;
        groupBox1.Location = new Point(3, 402);
        groupBox1.Name = "groupBox1";
        groupBox1.Size = new Size(657, 222);
        groupBox1.TabIndex = 0;
        groupBox1.TabStop = false;
        groupBox1.Text = "Source Code";
        // 
        // richTextBox1
        // 
        rtbCode.BackColor = SystemColors.Control;
        rtbCode.BorderStyle = BorderStyle.None;
        rtbCode.Dock = DockStyle.Fill;
        rtbCode.Font = new Font("Consolas", 9F, FontStyle.Regular, GraphicsUnit.Point);
        rtbCode.Location = new Point(3, 19);
        rtbCode.Name = "richTextBox1";
        rtbCode.Size = new Size(651, 200);
        rtbCode.TabIndex = 0;
        rtbCode.Text = "ScottPlot.Plot = new();";
        // 
        // formsPlot1
        // 
        formsPlot1.DisplayScale = 1F;
        formsPlot1.Dock = DockStyle.Fill;
        formsPlot1.Location = new Point(3, 3);
        formsPlot1.Name = "formsPlot1";
        formsPlot1.Size = new Size(657, 393);
        formsPlot1.TabIndex = 1;
        // 
        // groupBox2
        // 
        groupBox2.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        groupBox2.Controls.Add(tbFilter);
        groupBox2.Location = new Point(12, 586);
        groupBox2.Name = "groupBox2";
        groupBox2.Size = new Size(192, 53);
        groupBox2.TabIndex = 3;
        groupBox2.TabStop = false;
        groupBox2.Text = "Filter";
        // 
        // tbFilter
        // 
        tbFilter.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        tbFilter.Location = new Point(6, 22);
        tbFilter.Name = "tbFilter";
        tbFilter.Size = new Size(180, 23);
        tbFilter.TabIndex = 0;
        tbFilter.TextChanged += tbFilter_TextChanged;
        // 
        // CookbookViewer
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(885, 651);
        Controls.Add(groupBox2);
        Controls.Add(tableLayoutPanel1);
        Controls.Add(listView1);
        Name = "CookbookViewer";
        Text = "ScottPlot Cookbook - Windows Forms";
        Load += CookbookViewer_Load;
        tableLayoutPanel1.ResumeLayout(false);
        groupBox1.ResumeLayout(false);
        groupBox2.ResumeLayout(false);
        groupBox2.PerformLayout();
        ResumeLayout(false);
    }

    #endregion

    private ListView listView1;
    private TableLayoutPanel tableLayoutPanel1;
    private GroupBox groupBox1;
    private RichTextBox rtbCode;
    private ScottPlot.WinForms.FormsPlot formsPlot1;
    private GroupBox groupBox2;
    private TextBox tbFilter;
}