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
        chk_showLegend = new System.Windows.Forms.CheckBox();
        SuspendLayout();
        // 
        // formsPlot1
        // 
        formsPlot1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
        formsPlot1.Location = new System.Drawing.Point(15, 97);
        formsPlot1.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
        formsPlot1.Name = "formsPlot1";
        formsPlot1.Size = new System.Drawing.Size(817, 468);
        formsPlot1.TabIndex = 0;
        // 
        // cbEnableViewManagement
        // 
        cbEnableViewManagement.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
        cbEnableViewManagement.AutoSize = true;
        cbEnableViewManagement.Checked = true;
        cbEnableViewManagement.CheckState = System.Windows.Forms.CheckState.Checked;
        cbEnableViewManagement.Location = new System.Drawing.Point(673, 11);
        cbEnableViewManagement.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
        cbEnableViewManagement.Name = "cbEnableViewManagement";
        cbEnableViewManagement.Size = new System.Drawing.Size(159, 24);
        cbEnableViewManagement.TabIndex = 3;
        cbEnableViewManagement.Text = "Manage Axis Limits";
        cbEnableViewManagement.UseVisualStyleBackColor = true;
        cbEnableViewManagement.CheckedChanged += cbView_CheckedChanged;
        // 
        // btnFull
        // 
        btnFull.Location = new System.Drawing.Point(14, 36);
        btnFull.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
        btnFull.Name = "btnFull";
        btnFull.Size = new System.Drawing.Size(86, 53);
        btnFull.TabIndex = 4;
        btnFull.Text = "Full";
        btnFull.UseVisualStyleBackColor = true;
        btnFull.Click += btnFull_Click;
        // 
        // label1
        // 
        label1.AutoSize = true;
        label1.Location = new System.Drawing.Point(14, 12);
        label1.Name = "label1";
        label1.Size = new System.Drawing.Size(424, 20);
        label1.TabIndex = 5;
        label1.Text = "The DataLogger stores and displays infinitely growing datasets";
        // 
        // btnClear
        // 
        btnClear.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
        btnClear.Location = new System.Drawing.Point(746, 36);
        btnClear.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
        btnClear.Name = "btnClear";
        btnClear.Size = new System.Drawing.Size(86, 53);
        btnClear.TabIndex = 6;
        btnClear.Text = "Clear";
        btnClear.UseVisualStyleBackColor = true;
        btnClear.Click += btnClear_Click;
        // 
        // btnJump
        // 
        btnJump.Location = new System.Drawing.Point(106, 36);
        btnJump.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
        btnJump.Name = "btnJump";
        btnJump.Size = new System.Drawing.Size(86, 53);
        btnJump.TabIndex = 7;
        btnJump.Text = "Jump";
        btnJump.UseVisualStyleBackColor = true;
        btnJump.Click += btnJump_Click;
        // 
        // btnSlide
        // 
        btnSlide.Location = new System.Drawing.Point(199, 36);
        btnSlide.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
        btnSlide.Name = "btnSlide";
        btnSlide.Size = new System.Drawing.Size(86, 53);
        btnSlide.TabIndex = 8;
        btnSlide.Text = "Slide";
        btnSlide.UseVisualStyleBackColor = true;
        btnSlide.Click += btnSlide_Click;
        // 
        // chk_showLegend
        // 
        chk_showLegend.AutoSize = true;
        chk_showLegend.Location = new System.Drawing.Point(337, 51);
        chk_showLegend.Name = "chk_showLegend";
        chk_showLegend.Size = new System.Drawing.Size(117, 24);
        chk_showLegend.TabIndex = 9;
        chk_showLegend.Text = "Show legend";
        chk_showLegend.UseVisualStyleBackColor = true;
        chk_showLegend.CheckedChanged += chk_showLegend_CheckedChanged;
        // 
        // DataLogger
        // 
        AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
        AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        ClientSize = new System.Drawing.Size(847, 581);
        Controls.Add(chk_showLegend);
        Controls.Add(btnSlide);
        Controls.Add(btnJump);
        Controls.Add(btnClear);
        Controls.Add(label1);
        Controls.Add(btnFull);
        Controls.Add(cbEnableViewManagement);
        Controls.Add(formsPlot1);
        Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
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
    private System.Windows.Forms.CheckBox chk_showLegend;
}