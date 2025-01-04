namespace WinForms_Demo.Demos;

partial class MultiplotAlignment
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
        btnFixed = new Button();
        SuspendLayout();
        // 
        // formsPlot1
        // 
        formsPlot1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        formsPlot1.DisplayScale = 1F;
        formsPlot1.Location = new Point(12, 49);
        formsPlot1.Name = "formsPlot1";
        formsPlot1.Size = new Size(776, 389);
        formsPlot1.TabIndex = 6;
        // 
        // btnDefault
        // 
        btnDefault.Location = new Point(12, 12);
        btnDefault.Name = "btnDefault";
        btnDefault.Size = new Size(99, 31);
        btnDefault.TabIndex = 7;
        btnDefault.Text = "Default";
        btnDefault.UseVisualStyleBackColor = true;
        // 
        // btnFixed
        // 
        btnFixed.Location = new Point(117, 12);
        btnFixed.Name = "btnFixed";
        btnFixed.Size = new Size(99, 31);
        btnFixed.TabIndex = 8;
        btnFixed.Text = "Fixed Padding";
        btnFixed.UseVisualStyleBackColor = true;
        // 
        // MultiplotAlignment
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(800, 450);
        Controls.Add(btnFixed);
        Controls.Add(btnDefault);
        Controls.Add(formsPlot1);
        Name = "MultiplotAlignment";
        Text = "MultiplotAlignment";
        ResumeLayout(false);
    }

    #endregion

    private ScottPlot.WinForms.FormsPlot formsPlot1;
    private Button btnDefault;
    private Button btnFixed;
}