namespace WinForms_Demo.Demos;

partial class DataLogger
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
        btnJump = new Button();
        btnFull = new Button();
        chkManageLimits = new CheckBox();
        formsPlot1 = new ScottPlot.WinForms.FormsPlot();
        btnSlide = new Button();
        chkRightAxis = new CheckBox();
        SuspendLayout();
        // 
        // btnJump
        // 
        btnJump.Location = new Point(106, 12);
        btnJump.Name = "btnJump";
        btnJump.Size = new Size(88, 34);
        btnJump.TabIndex = 9;
        btnJump.Text = "Jump";
        btnJump.UseVisualStyleBackColor = true;
        // 
        // btnFull
        // 
        btnFull.Location = new Point(12, 12);
        btnFull.Name = "btnFull";
        btnFull.Size = new Size(88, 34);
        btnFull.TabIndex = 8;
        btnFull.Text = "Full";
        btnFull.UseVisualStyleBackColor = true;
        // 
        // cbManageLimits
        // 
        chkManageLimits.AutoSize = true;
        chkManageLimits.Checked = true;
        chkManageLimits.CheckState = CheckState.Checked;
        chkManageLimits.Location = new Point(306, 21);
        chkManageLimits.Name = "cbManageLimits";
        chkManageLimits.Size = new Size(129, 19);
        chkManageLimits.TabIndex = 7;
        chkManageLimits.Text = "Manage Axis Limits";
        chkManageLimits.UseVisualStyleBackColor = true;
        // 
        // formsPlot1
        // 
        formsPlot1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        formsPlot1.DisplayScale = 1F;
        formsPlot1.Location = new Point(12, 52);
        formsPlot1.Name = "formsPlot1";
        formsPlot1.Size = new Size(685, 345);
        formsPlot1.TabIndex = 6;
        // 
        // btnSlide
        // 
        btnSlide.Location = new Point(200, 12);
        btnSlide.Name = "btnSlide";
        btnSlide.Size = new Size(88, 34);
        btnSlide.TabIndex = 10;
        btnSlide.Text = "Slide";
        btnSlide.UseVisualStyleBackColor = true;
        // 
        // chkRightAxis
        // 
        chkRightAxis.AutoSize = true;
        chkRightAxis.Location = new Point(454, 21);
        chkRightAxis.Name = "chkRightAxis";
        chkRightAxis.Size = new Size(79, 19);
        chkRightAxis.TabIndex = 11;
        chkRightAxis.Text = "Right Axis";
        chkRightAxis.UseVisualStyleBackColor = true;
        // 
        // DataLogger
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(709, 409);
        Controls.Add(chkRightAxis);
        Controls.Add(btnSlide);
        Controls.Add(btnJump);
        Controls.Add(btnFull);
        Controls.Add(chkManageLimits);
        Controls.Add(formsPlot1);
        Name = "DataLogger";
        Text = "DataLogger";
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private Button btnJump;
    private Button btnFull;
    private CheckBox chkManageLimits;
    private ScottPlot.WinForms.FormsPlot formsPlot1;
    private Button btnSlide;
    private CheckBox chkRightAxis;
}