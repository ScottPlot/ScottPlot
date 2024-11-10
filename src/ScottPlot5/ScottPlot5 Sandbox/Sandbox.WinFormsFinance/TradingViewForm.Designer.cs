namespace Sandbox.WinFormsFinance;

partial class TradingViewForm
{
    /// <summary>
    ///  Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    ///  Clean up any resources being used.
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
    ///  Required method for Designer support - do not modify
    ///  the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        formsPlot1 = new ScottPlot.WinForms.FormsPlot();
        buttonClearAll = new Button();
        button1 = new Button();
        checkBoxLockScale = new CheckBox();
        SuspendLayout();
        // 
        // formsPlot1
        // 
        formsPlot1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        formsPlot1.DisplayScale = 1F;
        formsPlot1.Location = new Point(93, 12);
        formsPlot1.Name = "formsPlot1";
        formsPlot1.Size = new Size(979, 737);
        formsPlot1.TabIndex = 7;
        // 
        // buttonClearAll
        // 
        buttonClearAll.Location = new Point(12, 49);
        buttonClearAll.Name = "buttonClearAll";
        buttonClearAll.Size = new Size(75, 31);
        buttonClearAll.TabIndex = 12;
        buttonClearAll.Text = "Clear";
        buttonClearAll.UseVisualStyleBackColor = true;
        // 
        // button1
        // 
        button1.Location = new Point(12, 12);
        button1.Name = "button1";
        button1.Size = new Size(75, 31);
        button1.TabIndex = 13;
        button1.Text = "Line";
        button1.UseVisualStyleBackColor = true;
        // 
        // checkBoxLockScale
        // 
        checkBoxLockScale.Appearance = Appearance.Button;
        checkBoxLockScale.AutoSize = true;
        checkBoxLockScale.Location = new Point(14, 120);
        checkBoxLockScale.Name = "checkBoxLockScale";
        checkBoxLockScale.Size = new Size(72, 25);
        checkBoxLockScale.TabIndex = 14;
        checkBoxLockScale.Text = "Lock Scale";
        checkBoxLockScale.UseVisualStyleBackColor = true;
        // 
        // TradingViewForm
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(1084, 761);
        Controls.Add(checkBoxLockScale);
        Controls.Add(button1);
        Controls.Add(buttonClearAll);
        Controls.Add(formsPlot1);
        Name = "TradingViewForm";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "ScottPlot Financial Charting Sandbox (TradingView)";
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion
    private ScottPlot.WinForms.FormsPlot formsPlot1;
    private Button buttonClearAll;
    private Button button1;
    private CheckBox checkBoxLockScale;
}
