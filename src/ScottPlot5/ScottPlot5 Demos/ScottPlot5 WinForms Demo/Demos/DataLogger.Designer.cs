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
        cbManageLimits = new CheckBox();
        formsPlot1 = new ScottPlot.WinForms.FormsPlot();
        btnSlide = new Button();
        chkRightAxe = new CheckBox();
        SuspendLayout();
        // 
        // btnJump
        // 
        btnJump.Location = new Point(121, 16);
        btnJump.Margin = new Padding(3, 4, 3, 4);
        btnJump.Name = "btnJump";
        btnJump.Size = new Size(101, 45);
        btnJump.TabIndex = 9;
        btnJump.Text = "Jump";
        btnJump.UseVisualStyleBackColor = true;
        // 
        // btnFull
        // 
        btnFull.Location = new Point(14, 16);
        btnFull.Margin = new Padding(3, 4, 3, 4);
        btnFull.Name = "btnFull";
        btnFull.Size = new Size(101, 45);
        btnFull.TabIndex = 8;
        btnFull.Text = "Full";
        btnFull.UseVisualStyleBackColor = true;
        // 
        // cbManageLimits
        // 
        cbManageLimits.AutoSize = true;
        cbManageLimits.Checked = true;
        cbManageLimits.CheckState = CheckState.Checked;
        cbManageLimits.Location = new Point(350, 28);
        cbManageLimits.Margin = new Padding(3, 4, 3, 4);
        cbManageLimits.Name = "cbManageLimits";
        cbManageLimits.Size = new Size(159, 24);
        cbManageLimits.TabIndex = 7;
        cbManageLimits.Text = "Manage Axis Limits";
        cbManageLimits.UseVisualStyleBackColor = true;
        // 
        // formsPlot1
        // 
        formsPlot1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        formsPlot1.DisplayScale = 1F;
        formsPlot1.Location = new Point(14, 69);
        formsPlot1.Margin = new Padding(3, 4, 3, 4);
        formsPlot1.Name = "formsPlot1";
        formsPlot1.Size = new Size(783, 460);
        formsPlot1.TabIndex = 6;
        // 
        // btnSlide
        // 
        btnSlide.Location = new Point(229, 16);
        btnSlide.Margin = new Padding(3, 4, 3, 4);
        btnSlide.Name = "btnSlide";
        btnSlide.Size = new Size(101, 45);
        btnSlide.TabIndex = 10;
        btnSlide.Text = "Slide";
        btnSlide.UseVisualStyleBackColor = true;
        // 
        // chkRightAxe
        // 
        chkRightAxe.AutoSize = true;
        chkRightAxe.Location = new Point(550, 28);
        chkRightAxe.Margin = new Padding(3, 4, 3, 4);
        chkRightAxe.Name = "chkRightAxe";
        chkRightAxe.Size = new Size(119, 24);
        chkRightAxe.TabIndex = 11;
        chkRightAxe.Text = "Use right axis";
        chkRightAxe.UseVisualStyleBackColor = true;
        chkRightAxe.CheckedChanged += chkRightAxe_CheckedChanged;
        // 
        // DataLogger
        // 
        AutoScaleDimensions = new SizeF(8F, 20F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(810, 545);
        Controls.Add(chkRightAxe);
        Controls.Add(btnSlide);
        Controls.Add(btnJump);
        Controls.Add(btnFull);
        Controls.Add(cbManageLimits);
        Controls.Add(formsPlot1);
        Margin = new Padding(3, 4, 3, 4);
        Name = "DataLogger";
        Text = "DataLogger";
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private Button btnJump;
    private Button btnFull;
    private CheckBox cbManageLimits;
    private ScottPlot.WinForms.FormsPlot formsPlot1;
    private Button btnSlide;
    private CheckBox chkRightAxe;
}