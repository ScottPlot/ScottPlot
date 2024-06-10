namespace WinForms_Demo.Demos;

partial class ShowValueOnHover
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
        rbNearestXY = new RadioButton();
        radioButton1 = new RadioButton();
        SuspendLayout();
        // 
        // formsPlot1
        // 
        formsPlot1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        formsPlot1.DisplayScale = 1F;
        formsPlot1.Location = new Point(0, 37);
        formsPlot1.Name = "formsPlot1";
        formsPlot1.Size = new Size(800, 413);
        formsPlot1.TabIndex = 1;
        // 
        // rbNearestXY
        // 
        rbNearestXY.AutoSize = true;
        rbNearestXY.Checked = true;
        rbNearestXY.Location = new Point(12, 12);
        rbNearestXY.Name = "rbNearestXY";
        rbNearestXY.Size = new Size(87, 19);
        rbNearestXY.TabIndex = 2;
        rbNearestXY.TabStop = true;
        rbNearestXY.Text = "Nearest X/Y";
        rbNearestXY.UseVisualStyleBackColor = true;
        // 
        // radioButton1
        // 
        radioButton1.AutoSize = true;
        radioButton1.Location = new Point(105, 12);
        radioButton1.Name = "radioButton1";
        radioButton1.Size = new Size(103, 19);
        radioButton1.TabIndex = 3;
        radioButton1.TabStop = true;
        radioButton1.Text = "Nearest X Only";
        radioButton1.UseVisualStyleBackColor = true;
        // 
        // ShowValueOnHover
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(800, 450);
        Controls.Add(radioButton1);
        Controls.Add(rbNearestXY);
        Controls.Add(formsPlot1);
        Name = "ShowValueOnHover";
        Text = "ShowValueOnHover";
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private ScottPlot.WinForms.FormsPlot formsPlot1;
    private RadioButton rbNearestXY;
    private RadioButton radioButton1;
}