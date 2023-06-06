namespace ScottPlot.Demo.WinForms.WinFormsDemos;

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
        formsPlot1 = new FormsPlot();
        cbEnableViewManagement = new System.Windows.Forms.CheckBox();
        btnFull = new System.Windows.Forms.Button();
        label1 = new System.Windows.Forms.Label();
        btnClear = new System.Windows.Forms.Button();
        btnJump = new System.Windows.Forms.Button();
        btnSlide = new System.Windows.Forms.Button();
        SuspendLayout();
        // 
        // formsPlot1
        // 
        formsPlot1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
        formsPlot1.Location = new System.Drawing.Point(13, 73);
        formsPlot1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
        formsPlot1.Name = "formsPlot1";
        formsPlot1.Size = new System.Drawing.Size(715, 351);
        formsPlot1.TabIndex = 0;
        // 
        // cbEnableViewManagement
        // 
        cbEnableViewManagement.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
        cbEnableViewManagement.AutoSize = true;
        cbEnableViewManagement.Checked = true;
        cbEnableViewManagement.CheckState = System.Windows.Forms.CheckState.Checked;
        cbEnableViewManagement.Location = new System.Drawing.Point(599, 8);
        cbEnableViewManagement.Name = "cbEnableViewManagement";
        cbEnableViewManagement.Size = new System.Drawing.Size(129, 19);
        cbEnableViewManagement.TabIndex = 3;
        cbEnableViewManagement.Text = "Manage Axis Limits";
        cbEnableViewManagement.UseVisualStyleBackColor = true;
        cbEnableViewManagement.CheckedChanged += cbView_CheckedChanged;
        // 
        // btnFull
        // 
        btnFull.Location = new System.Drawing.Point(12, 27);
        btnFull.Name = "btnFull";
        btnFull.Size = new System.Drawing.Size(75, 40);
        btnFull.TabIndex = 4;
        btnFull.Text = "Full";
        btnFull.UseVisualStyleBackColor = true;
        btnFull.Click += btnFull_Click;
        // 
        // label1
        // 
        label1.AutoSize = true;
        label1.Location = new System.Drawing.Point(12, 9);
        label1.Name = "label1";
        label1.Size = new System.Drawing.Size(334, 15);
        label1.TabIndex = 5;
        label1.Text = "The DataLogger stores and displays infinitely growing datasets";
        // 
        // btnClear
        // 
        btnClear.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
        btnClear.Location = new System.Drawing.Point(653, 27);
        btnClear.Name = "btnClear";
        btnClear.Size = new System.Drawing.Size(75, 40);
        btnClear.TabIndex = 6;
        btnClear.Text = "Clear";
        btnClear.UseVisualStyleBackColor = true;
        btnClear.Click += btnClear_Click;
        // 
        // btnJump
        // 
        btnJump.Location = new System.Drawing.Point(93, 27);
        btnJump.Name = "btnJump";
        btnJump.Size = new System.Drawing.Size(75, 40);
        btnJump.TabIndex = 7;
        btnJump.Text = "Jump";
        btnJump.UseVisualStyleBackColor = true;
        btnJump.Click += btnJump_Click;
        // 
        // btnSlide
        // 
        btnSlide.Location = new System.Drawing.Point(174, 27);
        btnSlide.Name = "btnSlide";
        btnSlide.Size = new System.Drawing.Size(75, 40);
        btnSlide.TabIndex = 8;
        btnSlide.Text = "Slide";
        btnSlide.UseVisualStyleBackColor = true;
        btnSlide.Click += btnSlide_Click;
        // 
        // DataLogger
        // 
        AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
        AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        ClientSize = new System.Drawing.Size(741, 436);
        Controls.Add(btnSlide);
        Controls.Add(btnJump);
        Controls.Add(btnClear);
        Controls.Add(label1);
        Controls.Add(btnFull);
        Controls.Add(cbEnableViewManagement);
        Controls.Add(formsPlot1);
        Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
        Name = "DataLogger";
        StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
        Text = "DataLogger Demo";
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private FormsPlot formsPlot1;
    private System.Windows.Forms.CheckBox cbEnableViewManagement;
    private System.Windows.Forms.Button btnFull;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Button btnClear;
    private System.Windows.Forms.Button btnJump;
    private System.Windows.Forms.Button btnSlide;
}