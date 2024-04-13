namespace WinForms_Demo.Demos;

partial class PersistingPlot
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
        button1 = new Button();
        richTextBox1 = new RichTextBox();
        SuspendLayout();
        // 
        // button1
        // 
        button1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
        button1.Font = new Font("Segoe UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point);
        button1.Location = new Point(12, 12);
        button1.Name = "button1";
        button1.Size = new Size(101, 70);
        button1.TabIndex = 0;
        button1.Text = "Launch";
        button1.UseVisualStyleBackColor = true;
        // 
        // richTextBox1
        // 
        richTextBox1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        richTextBox1.BackColor = SystemColors.Control;
        richTextBox1.BorderStyle = BorderStyle.None;
        richTextBox1.Font = new Font("Segoe UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point);
        richTextBox1.Location = new Point(119, 12);
        richTextBox1.Name = "richTextBox1";
        richTextBox1.Size = new Size(318, 70);
        richTextBox1.TabIndex = 2;
        richTextBox1.Text = "Plot manipulations in another Form persist through Close() events.";
        // 
        // PersistingPlot
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(449, 94);
        Controls.Add(richTextBox1);
        Controls.Add(button1);
        Name = "PersistingPlot";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Persisting Plot Demo";
        ResumeLayout(false);
    }

    #endregion

    private Button button1;
    private RichTextBox richTextBox1;
}