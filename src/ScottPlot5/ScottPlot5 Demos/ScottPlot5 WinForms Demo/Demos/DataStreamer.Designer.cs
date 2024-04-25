namespace WinForms_Demo.Demos;

partial class DataStreamer
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
        btnWipeRight = new Button();
        btnScrollLeft = new Button();
        rbManage = new RadioButton();
        rbContinuous = new RadioButton();
        btnMark = new Button();
        SuspendLayout();
        // 
        // formsPlot1
        // 
        formsPlot1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        formsPlot1.DisplayScale = 1F;
        formsPlot1.Location = new Point(12, 52);
        formsPlot1.Name = "formsPlot1";
        formsPlot1.Size = new Size(776, 386);
        formsPlot1.TabIndex = 0;
        // 
        // btnWipeRight
        // 
        btnWipeRight.Location = new Point(12, 12);
        btnWipeRight.Name = "btnWipeRight";
        btnWipeRight.Size = new Size(88, 34);
        btnWipeRight.TabIndex = 2;
        btnWipeRight.Text = "Wipe";
        btnWipeRight.UseVisualStyleBackColor = true;
        // 
        // btnScrollLeft
        // 
        btnScrollLeft.Location = new Point(106, 12);
        btnScrollLeft.Name = "btnScrollLeft";
        btnScrollLeft.Size = new Size(88, 34);
        btnScrollLeft.TabIndex = 5;
        btnScrollLeft.Text = "Scroll";
        btnScrollLeft.UseVisualStyleBackColor = true;
        // 
        // rbManage
        // 
        rbManage.AutoSize = true;
        rbManage.Checked = true;
        rbManage.Location = new Point(221, 20);
        rbManage.Name = "rbManage";
        rbManage.Size = new Size(196, 19);
        rbManage.TabIndex = 7;
        rbManage.TabStop = true;
        rbManage.Text = "DataStreamer.ManageAxisLimits";
        rbManage.UseVisualStyleBackColor = true;
        // 
        // rbContinuous
        // 
        rbContinuous.AutoSize = true;
        rbContinuous.Location = new Point(450, 20);
        rbContinuous.Name = "rbContinuous";
        rbContinuous.Size = new Size(173, 19);
        rbContinuous.TabIndex = 8;
        rbContinuous.Text = "Axis.ContinuouslyAutoscale";
        rbContinuous.UseVisualStyleBackColor = true;
        // 
        // btnMark
        // 
        btnMark.Location = new Point(688, 12);
        btnMark.Name = "btnMark";
        btnMark.Size = new Size(88, 34);
        btnMark.TabIndex = 9;
        btnMark.Text = "Mark Position";
        btnMark.UseVisualStyleBackColor = true;
        // 
        // DataStreamer
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(800, 450);
        Controls.Add(btnMark);
        Controls.Add(rbContinuous);
        Controls.Add(rbManage);
        Controls.Add(btnScrollLeft);
        Controls.Add(btnWipeRight);
        Controls.Add(formsPlot1);
        Name = "DataStreamer";
        Text = "DataStreamer";
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private ScottPlot.WinForms.FormsPlot formsPlot1;
    private Button btnWipeRight;
    private Button btnScrollLeft;
    private RadioButton rbManage;
    private RadioButton rbContinuous;
    private Button btnMark;
}