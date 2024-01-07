namespace WinForms_Demo.Demos;

partial class CustomMenu
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
        btnDefault = new Button();
        btnCustom = new Button();
        formsPlot1 = new ScottPlot.WinForms.FormsPlot();
        SuspendLayout();
        // 
        // btnDefault
        // 
        btnDefault.Location = new Point(12, 12);
        btnDefault.Name = "btnDefault";
        btnDefault.Size = new Size(75, 43);
        btnDefault.TabIndex = 0;
        btnDefault.Text = "Default";
        btnDefault.UseVisualStyleBackColor = true;
        btnDefault.Click += btnDefault_Click;
        // 
        // btnCustom
        // 
        btnCustom.Location = new Point(93, 12);
        btnCustom.Name = "btnCustom";
        btnCustom.Size = new Size(75, 43);
        btnCustom.TabIndex = 1;
        btnCustom.Text = "Custom";
        btnCustom.UseVisualStyleBackColor = true;
        btnCustom.Click += btnCustom_Click;
        // 
        // formsPlot1
        // 
        formsPlot1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        formsPlot1.DisplayScale = 1F;
        formsPlot1.Location = new Point(12, 61);
        formsPlot1.Name = "formsPlot1";
        formsPlot1.Size = new Size(776, 377);
        formsPlot1.TabIndex = 2;
        // 
        // CustomMenu
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(800, 450);
        Controls.Add(formsPlot1);
        Controls.Add(btnCustom);
        Controls.Add(btnDefault);
        Name = "CustomMenu";
        Text = "CustomMenu";
        ResumeLayout(false);
    }

    #endregion

    private Button btnDefault;
    private Button btnCustom;
    private ScottPlot.WinForms.FormsPlot formsPlot1;
}