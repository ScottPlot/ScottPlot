namespace WinForms_Demo.Demos;

partial class AxisRules
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
        btnBoundaryMin = new Button();
        btnBoundaryMax = new Button();
        btnScalePreserveX = new Button();
        btnScalePreserveY = new Button();
        btnScaleZoom = new Button();
        groupBox1 = new GroupBox();
        groupBox2 = new GroupBox();
        btnReset = new Button();
        groupBox3 = new GroupBox();
        btnSpanMin = new Button();
        btnSpanMax = new Button();
        groupBox4 = new GroupBox();
        btnLockHorizontal = new Button();
        btnLockVertical = new Button();
        cbInvertX = new CheckBox();
        cbInvertY = new CheckBox();
        groupBox1.SuspendLayout();
        groupBox2.SuspendLayout();
        groupBox3.SuspendLayout();
        groupBox4.SuspendLayout();
        SuspendLayout();
        // 
        // formsPlot1
        // 
        formsPlot1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        formsPlot1.DisplayScale = 1F;
        formsPlot1.Location = new Point(12, 93);
        formsPlot1.Name = "formsPlot1";
        formsPlot1.Size = new Size(1180, 521);
        formsPlot1.TabIndex = 0;
        // 
        // btnBoundaryMin
        // 
        btnBoundaryMin.Location = new Point(6, 22);
        btnBoundaryMin.Name = "btnBoundaryMin";
        btnBoundaryMin.Size = new Size(90, 43);
        btnBoundaryMin.TabIndex = 1;
        btnBoundaryMin.Text = "Minimum";
        btnBoundaryMin.UseVisualStyleBackColor = true;
        btnBoundaryMin.Click += btnBoundaryMin_Click;
        // 
        // btnBoundaryMax
        // 
        btnBoundaryMax.Location = new Point(102, 22);
        btnBoundaryMax.Name = "btnBoundaryMax";
        btnBoundaryMax.Size = new Size(90, 43);
        btnBoundaryMax.TabIndex = 2;
        btnBoundaryMax.Text = "Maximum";
        btnBoundaryMax.UseVisualStyleBackColor = true;
        btnBoundaryMax.Click += btnBoundaryMax_Click;
        // 
        // btnScalePreserveX
        // 
        btnScalePreserveX.Location = new Point(6, 22);
        btnScalePreserveX.Name = "btnScalePreserveX";
        btnScalePreserveX.Size = new Size(96, 43);
        btnScalePreserveX.TabIndex = 3;
        btnScalePreserveX.Text = "Preserve X";
        btnScalePreserveX.UseVisualStyleBackColor = true;
        btnScalePreserveX.Click += btnScalePreserveX_Click;
        // 
        // btnScalePreserveY
        // 
        btnScalePreserveY.Location = new Point(108, 22);
        btnScalePreserveY.Name = "btnScalePreserveY";
        btnScalePreserveY.Size = new Size(96, 43);
        btnScalePreserveY.TabIndex = 4;
        btnScalePreserveY.Text = "Preserve Y";
        btnScalePreserveY.UseVisualStyleBackColor = true;
        btnScalePreserveY.Click += btnScalePreserveY_Click;
        // 
        // btnScaleZoom
        // 
        btnScaleZoom.Location = new Point(210, 22);
        btnScaleZoom.Name = "btnScaleZoom";
        btnScaleZoom.Size = new Size(96, 43);
        btnScaleZoom.TabIndex = 5;
        btnScaleZoom.Text = "Zoom Out";
        btnScaleZoom.UseVisualStyleBackColor = true;
        btnScaleZoom.Click += btnScaleZoom_Click;
        // 
        // groupBox1
        // 
        groupBox1.Controls.Add(btnBoundaryMin);
        groupBox1.Controls.Add(btnBoundaryMax);
        groupBox1.Location = new Point(12, 12);
        groupBox1.Name = "groupBox1";
        groupBox1.Size = new Size(200, 75);
        groupBox1.TabIndex = 8;
        groupBox1.TabStop = false;
        groupBox1.Text = "Boundary";
        // 
        // groupBox2
        // 
        groupBox2.Controls.Add(btnScalePreserveX);
        groupBox2.Controls.Add(btnScalePreserveY);
        groupBox2.Controls.Add(btnScaleZoom);
        groupBox2.Location = new Point(218, 12);
        groupBox2.Name = "groupBox2";
        groupBox2.Size = new Size(317, 75);
        groupBox2.TabIndex = 9;
        groupBox2.TabStop = false;
        groupBox2.Text = "Square Scaling";
        // 
        // btnReset
        // 
        btnReset.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnReset.Location = new Point(1074, 34);
        btnReset.Name = "btnReset";
        btnReset.Size = new Size(96, 43);
        btnReset.TabIndex = 10;
        btnReset.Text = "Reset";
        btnReset.UseVisualStyleBackColor = true;
        btnReset.Click += btnReset_Click;
        // 
        // groupBox3
        // 
        groupBox3.Controls.Add(btnSpanMin);
        groupBox3.Controls.Add(btnSpanMax);
        groupBox3.Location = new Point(541, 12);
        groupBox3.Name = "groupBox3";
        groupBox3.Size = new Size(214, 75);
        groupBox3.TabIndex = 11;
        groupBox3.TabStop = false;
        groupBox3.Text = "Span Limit";
        // 
        // btnSpanMin
        // 
        btnSpanMin.Location = new Point(6, 22);
        btnSpanMin.Name = "btnSpanMin";
        btnSpanMin.Size = new Size(96, 43);
        btnSpanMin.TabIndex = 3;
        btnSpanMin.Text = "Minimum";
        btnSpanMin.UseVisualStyleBackColor = true;
        btnSpanMin.Click += btnSpanMin_Click;
        // 
        // btnSpanMax
        // 
        btnSpanMax.Location = new Point(108, 22);
        btnSpanMax.Name = "btnSpanMax";
        btnSpanMax.Size = new Size(96, 43);
        btnSpanMax.TabIndex = 4;
        btnSpanMax.Text = "Maximum";
        btnSpanMax.UseVisualStyleBackColor = true;
        btnSpanMax.Click += btnSpanMax_Click;
        // 
        // groupBox4
        // 
        groupBox4.Controls.Add(btnLockHorizontal);
        groupBox4.Controls.Add(btnLockVertical);
        groupBox4.Location = new Point(761, 12);
        groupBox4.Name = "groupBox4";
        groupBox4.Size = new Size(214, 75);
        groupBox4.TabIndex = 12;
        groupBox4.TabStop = false;
        groupBox4.Text = "Axis Lock";
        // 
        // btnLockHorizontal
        // 
        btnLockHorizontal.Location = new Point(6, 22);
        btnLockHorizontal.Name = "btnLockHorizontal";
        btnLockHorizontal.Size = new Size(96, 43);
        btnLockHorizontal.TabIndex = 3;
        btnLockHorizontal.Text = "Horizontal";
        btnLockHorizontal.UseVisualStyleBackColor = true;
        btnLockHorizontal.Click += btnLockHorizontal_Click;
        // 
        // btnLockVertical
        // 
        btnLockVertical.Location = new Point(108, 22);
        btnLockVertical.Name = "btnLockVertical";
        btnLockVertical.Size = new Size(96, 43);
        btnLockVertical.TabIndex = 4;
        btnLockVertical.Text = "Vertical";
        btnLockVertical.UseVisualStyleBackColor = true;
        btnLockVertical.Click += btnLockVertical_Click;
        // 
        // cbInvertX
        // 
        cbInvertX.AutoSize = true;
        cbInvertX.Location = new Point(981, 34);
        cbInvertX.Name = "cbInvertX";
        cbInvertX.Size = new Size(79, 19);
        cbInvertX.TabIndex = 13;
        cbInvertX.Text = "Inverted X";
        cbInvertX.UseVisualStyleBackColor = true;
        // 
        // cbInvertY
        // 
        cbInvertY.AutoSize = true;
        cbInvertY.Location = new Point(981, 59);
        cbInvertY.Name = "cbInvertY";
        cbInvertY.Size = new Size(79, 19);
        cbInvertY.TabIndex = 14;
        cbInvertY.Text = "Inverted Y";
        cbInvertY.UseVisualStyleBackColor = true;
        // 
        // AxisRules
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(1204, 626);
        Controls.Add(cbInvertY);
        Controls.Add(cbInvertX);
        Controls.Add(groupBox4);
        Controls.Add(groupBox3);
        Controls.Add(btnReset);
        Controls.Add(groupBox2);
        Controls.Add(groupBox1);
        Controls.Add(formsPlot1);
        Name = "AxisRules";
        Text = "AxisRules";
        groupBox1.ResumeLayout(false);
        groupBox2.ResumeLayout(false);
        groupBox3.ResumeLayout(false);
        groupBox4.ResumeLayout(false);
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private ScottPlot.WinForms.FormsPlot formsPlot1;
    private Button btnBoundaryMin;
    private Button btnBoundaryMax;
    private Button btnScalePreserveX;
    private Button btnScalePreserveY;
    private Button btnScaleZoom;
    private GroupBox groupBox1;
    private GroupBox groupBox2;
    private Button btnReset;
    private GroupBox groupBox3;
    private Button btnSpanMin;
    private Button btnSpanMax;
    private GroupBox groupBox4;
    private Button btnLockHorizontal;
    private Button btnLockVertical;
    private CheckBox cbInvertX;
    private CheckBox cbInvertY;
}