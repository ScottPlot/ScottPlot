namespace WinForms_Demo.Demos;

partial class CustomMouseActions
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
        btnDefault = new Button();
        btnDisable = new Button();
        btnCustom = new Button();
        richTextBox1 = new RichTextBox();
        SuspendLayout();
        // 
        // formsPlot1
        // 
        formsPlot1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        formsPlot1.DisplayScale = 1F;
        formsPlot1.Location = new Point(12, 59);
        formsPlot1.Name = "formsPlot1";
        formsPlot1.Size = new Size(776, 379);
        formsPlot1.TabIndex = 0;
        // 
        // btnDefault
        // 
        btnDefault.Location = new Point(12, 12);
        btnDefault.Name = "btnDefault";
        btnDefault.Size = new Size(82, 41);
        btnDefault.TabIndex = 1;
        btnDefault.Text = "Default";
        btnDefault.UseVisualStyleBackColor = true;
        btnDefault.Click += btnDefault_Click;
        // 
        // btnDisable
        // 
        btnDisable.Location = new Point(100, 12);
        btnDisable.Name = "btnDisable";
        btnDisable.Size = new Size(82, 41);
        btnDisable.TabIndex = 2;
        btnDisable.Text = "Disable";
        btnDisable.UseVisualStyleBackColor = true;
        btnDisable.Click += btnDisable_Click;
        // 
        // btnCustom
        // 
        btnCustom.Location = new Point(188, 12);
        btnCustom.Name = "btnCustom";
        btnCustom.Size = new Size(82, 41);
        btnCustom.TabIndex = 3;
        btnCustom.Text = "Custom";
        btnCustom.UseVisualStyleBackColor = true;
        btnCustom.Click += btnCustom_Click;
        // 
        // richTextBox1
        // 
        richTextBox1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        richTextBox1.BackColor = SystemColors.Control;
        richTextBox1.BorderStyle = BorderStyle.None;
        richTextBox1.Location = new Point(276, 12);
        richTextBox1.Name = "richTextBox1";
        richTextBox1.Size = new Size(512, 41);
        richTextBox1.TabIndex = 4;
        richTextBox1.Text = "";
        // 
        // CustomMouseActions
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(800, 450);
        Controls.Add(richTextBox1);
        Controls.Add(btnCustom);
        Controls.Add(btnDisable);
        Controls.Add(btnDefault);
        Controls.Add(formsPlot1);
        Name = "CustomMouseActions";
        Text = "Custom Mouse Actions";
        ResumeLayout(false);
    }

    #endregion

    private ScottPlot.WinForms.FormsPlot formsPlot1;
    private Button btnDefault;
    private Button btnDisable;
    private Button btnCustom;
    private RichTextBox richTextBox1;
}