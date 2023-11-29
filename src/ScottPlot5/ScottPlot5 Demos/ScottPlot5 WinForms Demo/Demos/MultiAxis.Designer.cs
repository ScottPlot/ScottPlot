namespace WinForms_Demo.Demos;

partial class MultiAxis
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
        btnRandomize = new Button();
        btnManualScale = new Button();
        btnAutoScale = new Button();
        formsPlot1 = new ScottPlot.WinForms.FormsPlot();
        btnAutoScaleTight = new Button();
        btnAutoScaleWithPadding = new Button();
        SuspendLayout();
        // 
        // btnRandomize
        // 
        btnRandomize.Location = new Point(12, 12);
        btnRandomize.Name = "btnRandomize";
        btnRandomize.Size = new Size(75, 23);
        btnRandomize.TabIndex = 0;
        btnRandomize.Text = "Randomize";
        btnRandomize.UseVisualStyleBackColor = true;
        btnRandomize.Click += btnRandomize_Click;
        // 
        // btnManualScale
        // 
        btnManualScale.Location = new Point(93, 12);
        btnManualScale.Name = "btnManualScale";
        btnManualScale.Size = new Size(101, 23);
        btnManualScale.TabIndex = 1;
        btnManualScale.Text = "Manual Scale";
        btnManualScale.UseVisualStyleBackColor = true;
        btnManualScale.Click += btnManualScale_Click;
        // 
        // btnAutoScale
        // 
        btnAutoScale.Location = new Point(200, 12);
        btnAutoScale.Name = "btnAutoScale";
        btnAutoScale.Size = new Size(75, 23);
        btnAutoScale.TabIndex = 2;
        btnAutoScale.Text = "AutoScale";
        btnAutoScale.UseVisualStyleBackColor = true;
        btnAutoScale.Click += btnAutoScale_Click;
        // 
        // formsPlot1
        // 
        formsPlot1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        formsPlot1.DisplayScale = 1F;
        formsPlot1.Location = new Point(12, 41);
        formsPlot1.Name = "formsPlot1";
        formsPlot1.Size = new Size(776, 397);
        formsPlot1.TabIndex = 3;
        // 
        // btnAutoScaleTight
        // 
        btnAutoScaleTight.Location = new Point(281, 12);
        btnAutoScaleTight.Name = "btnAutoScaleTight";
        btnAutoScaleTight.Size = new Size(110, 23);
        btnAutoScaleTight.TabIndex = 4;
        btnAutoScaleTight.Text = "AutoScale Tight";
        btnAutoScaleTight.UseVisualStyleBackColor = true;
        btnAutoScaleTight.Click += btnAutoScaleTight_Click;
        // 
        // btnAutoScaleWithPadding
        // 
        btnAutoScaleWithPadding.Location = new Point(397, 12);
        btnAutoScaleWithPadding.Name = "btnAutoScaleWithPadding";
        btnAutoScaleWithPadding.Size = new Size(158, 23);
        btnAutoScaleWithPadding.TabIndex = 5;
        btnAutoScaleWithPadding.Text = "AutoScale with Padding";
        btnAutoScaleWithPadding.UseVisualStyleBackColor = true;
        btnAutoScaleWithPadding.Click += btnAutoScaleWithPadding_Click;
        // 
        // MultiAxis
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(800, 450);
        Controls.Add(btnAutoScaleWithPadding);
        Controls.Add(btnAutoScaleTight);
        Controls.Add(formsPlot1);
        Controls.Add(btnAutoScale);
        Controls.Add(btnManualScale);
        Controls.Add(btnRandomize);
        Name = "MultiAxis";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "MultiAxis";
        ResumeLayout(false);
    }

    #endregion

    private Button btnRandomize;
    private Button btnManualScale;
    private Button btnAutoScale;
    private ScottPlot.WinForms.FormsPlot formsPlot1;
    private Button btnAutoScaleTight;
    private Button btnAutoScaleWithPadding;
}