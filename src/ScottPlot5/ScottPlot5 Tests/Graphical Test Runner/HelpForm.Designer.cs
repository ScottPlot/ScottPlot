namespace GraphicalTestRunner;

partial class HelpForm
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
        richTextBox1 = new RichTextBox();
        button1 = new Button();
        SuspendLayout();
        // 
        // richTextBox1
        // 
        richTextBox1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        richTextBox1.BackColor = SystemColors.Control;
        richTextBox1.BorderStyle = BorderStyle.None;
        richTextBox1.Font = new Font("Segoe UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
        richTextBox1.Location = new Point(12, 12);
        richTextBox1.Name = "richTextBox1";
        richTextBox1.Size = new Size(592, 271);
        richTextBox1.TabIndex = 0;
        richTextBox1.Text = "Testing 123";
        // 
        // button1
        // 
        button1.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        button1.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
        button1.Location = new Point(514, 289);
        button1.Name = "button1";
        button1.Size = new Size(90, 43);
        button1.TabIndex = 1;
        button1.Text = "Close";
        button1.UseVisualStyleBackColor = true;
        button1.Click += button1_Click;
        // 
        // HelpForm
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(616, 344);
        Controls.Add(button1);
        Controls.Add(richTextBox1);
        FormBorderStyle = FormBorderStyle.SizableToolWindow;
        Name = "HelpForm";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Help";
        ResumeLayout(false);
    }

    #endregion

    private RichTextBox richTextBox1;
    private Button button1;
}