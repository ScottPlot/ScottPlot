namespace WinForms_Demo.Demos;

partial class ReportViewer
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
        formsPlot1 = new ScottPlot.WinForms.FormsPlot();
        listBox1 = new ListBox();
        groupBox1 = new GroupBox();
        groupBox2 = new GroupBox();
        richTextBox1 = new RichTextBox();
        groupBox1.SuspendLayout();
        groupBox2.SuspendLayout();
        SuspendLayout();
        // 
        // formsPlot1
        // 
        formsPlot1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        formsPlot1.DisplayScale = 1F;
        formsPlot1.Location = new Point(12, 102);
        formsPlot1.Name = "formsPlot1";
        formsPlot1.Size = new Size(696, 391);
        formsPlot1.TabIndex = 1;
        // 
        // listBox1
        // 
        listBox1.Dock = DockStyle.Fill;
        listBox1.FormattingEnabled = true;
        listBox1.ItemHeight = 15;
        listBox1.Location = new Point(3, 19);
        listBox1.Name = "listBox1";
        listBox1.Size = new Size(205, 62);
        listBox1.TabIndex = 2;
        // 
        // groupBox1
        // 
        groupBox1.Controls.Add(listBox1);
        groupBox1.Location = new Point(12, 12);
        groupBox1.Name = "groupBox1";
        groupBox1.Size = new Size(211, 84);
        groupBox1.TabIndex = 3;
        groupBox1.TabStop = false;
        groupBox1.Text = "Figures";
        // 
        // groupBox2
        // 
        groupBox2.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        groupBox2.Controls.Add(richTextBox1);
        groupBox2.Location = new Point(229, 12);
        groupBox2.Name = "groupBox2";
        groupBox2.Padding = new Padding(6);
        groupBox2.Size = new Size(479, 84);
        groupBox2.TabIndex = 4;
        groupBox2.TabStop = false;
        groupBox2.Text = "Description";
        // 
        // richTextBox1
        // 
        richTextBox1.BackColor = SystemColors.Control;
        richTextBox1.BorderStyle = BorderStyle.None;
        richTextBox1.Dock = DockStyle.Fill;
        richTextBox1.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
        richTextBox1.Location = new Point(6, 22);
        richTextBox1.Name = "richTextBox1";
        richTextBox1.Size = new Size(467, 56);
        richTextBox1.TabIndex = 0;
        richTextBox1.Text = "Test 123";
        // 
        // ReportViewer
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(720, 505);
        Controls.Add(groupBox2);
        Controls.Add(groupBox1);
        Controls.Add(formsPlot1);
        Name = "ReportViewer";
        Text = "Report Viewer";
        groupBox1.ResumeLayout(false);
        groupBox2.ResumeLayout(false);
        ResumeLayout(false);
    }

    #endregion

    private ScottPlot.WinForms.FormsPlot formsPlot1;
    private ListBox listBox1;
    private GroupBox groupBox1;
    private GroupBox groupBox2;
    private RichTextBox richTextBox1;
}