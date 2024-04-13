namespace WinForms_Demo.Demos;

partial class SelectPoints
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
        checkBox1 = new CheckBox();
        SuspendLayout();
        // 
        // formsPlot1
        // 
        formsPlot1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        formsPlot1.DisplayScale = 1F;
        formsPlot1.Location = new Point(12, 37);
        formsPlot1.Name = "formsPlot1";
        formsPlot1.Size = new Size(750, 439);
        formsPlot1.TabIndex = 0;
        // 
        // checkBox1
        // 
        checkBox1.AutoSize = true;
        checkBox1.Checked = true;
        checkBox1.CheckState = CheckState.Checked;
        checkBox1.Location = new Point(12, 12);
        checkBox1.Name = "checkBox1";
        checkBox1.Size = new Size(93, 19);
        checkBox1.TabIndex = 1;
        checkBox1.Text = "Select Points";
        checkBox1.UseVisualStyleBackColor = true;
        // 
        // SelectPoints
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(774, 488);
        Controls.Add(checkBox1);
        Controls.Add(formsPlot1);
        Name = "SelectPoints";
        Text = "SelectPoints";
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private ScottPlot.WinForms.FormsPlot formsPlot1;
    private CheckBox checkBox1;
}