namespace WinForms_Demo;

partial class DemoWindowInfo
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

    #region Component Designer generated code

    /// <summary> 
    /// Required method for Designer support - do not modify 
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        groupBox1 = new GroupBox();
        label1 = new Label();
        richTextBox1 = new RichTextBox();
        button1 = new Button();
        groupBox1.SuspendLayout();
        SuspendLayout();
        // 
        // groupBox1
        // 
        groupBox1.Controls.Add(label1);
        groupBox1.Controls.Add(richTextBox1);
        groupBox1.Controls.Add(button1);
        groupBox1.Dock = DockStyle.Fill;
        groupBox1.Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold, GraphicsUnit.Point);
        groupBox1.Location = new Point(0, 0);
        groupBox1.Name = "groupBox1";
        groupBox1.Size = new Size(483, 83);
        groupBox1.TabIndex = 0;
        groupBox1.TabStop = false;
        groupBox1.Text = "Title";
        // 
        // label1
        // 
        label1.AutoSize = true;
        label1.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Regular, GraphicsUnit.Point);
        label1.ForeColor = SystemColors.ControlDark;
        label1.Location = new Point(6, 64);
        label1.Name = "label1";
        label1.Size = new Size(73, 15);
        label1.TabIndex = 9;
        label1.Text = "View Source";
        // 
        // richTextBox1
        // 
        richTextBox1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        richTextBox1.BackColor = SystemColors.Control;
        richTextBox1.BorderStyle = BorderStyle.None;
        richTextBox1.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
        richTextBox1.Location = new Point(85, 22);
        richTextBox1.Name = "richTextBox1";
        richTextBox1.Size = new Size(392, 55);
        richTextBox1.TabIndex = 8;
        richTextBox1.Text = "description";
        // 
        // button1
        // 
        button1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
        button1.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
        button1.Location = new Point(6, 22);
        button1.Name = "button1";
        button1.Size = new Size(73, 39);
        button1.TabIndex = 6;
        button1.Text = "Launch";
        button1.UseVisualStyleBackColor = true;
        // 
        // DemoWindowInfo
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        Controls.Add(groupBox1);
        Name = "DemoWindowInfo";
        Size = new Size(483, 83);
        groupBox1.ResumeLayout(false);
        groupBox1.PerformLayout();
        ResumeLayout(false);
    }

    #endregion

    private GroupBox groupBox1;
    private RichTextBox richTextBox1;
    private Button button1;
    private Label label1;
}
