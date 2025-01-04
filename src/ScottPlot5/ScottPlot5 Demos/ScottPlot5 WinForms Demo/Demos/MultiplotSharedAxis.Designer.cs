namespace WinForms_Demo.Demos;

partial class MultiplotSharedAxis
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
        btnShareNone = new Button();
        btnShareX = new Button();
        btnShareY = new Button();
        btnShareXY = new Button();
        SuspendLayout();
        // 
        // formsPlot1
        // 
        formsPlot1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        formsPlot1.DisplayScale = 1F;
        formsPlot1.Location = new Point(6, 49);
        formsPlot1.Name = "formsPlot1";
        formsPlot1.Size = new Size(794, 559);
        formsPlot1.TabIndex = 5;
        // 
        // btnShareNone
        // 
        btnShareNone.Location = new Point(12, 12);
        btnShareNone.Name = "btnShareNone";
        btnShareNone.Size = new Size(99, 31);
        btnShareNone.TabIndex = 6;
        btnShareNone.Text = "Share None";
        btnShareNone.UseVisualStyleBackColor = true;
        // 
        // btnShareX
        // 
        btnShareX.Location = new Point(117, 12);
        btnShareX.Name = "btnShareX";
        btnShareX.Size = new Size(99, 31);
        btnShareX.TabIndex = 7;
        btnShareX.Text = "Share X";
        btnShareX.UseVisualStyleBackColor = true;
        // 
        // btnShareY
        // 
        btnShareY.Location = new Point(222, 12);
        btnShareY.Name = "btnShareY";
        btnShareY.Size = new Size(99, 31);
        btnShareY.TabIndex = 8;
        btnShareY.Text = "Share Y";
        btnShareY.UseVisualStyleBackColor = true;
        // 
        // btnShareXY
        // 
        btnShareXY.Location = new Point(327, 12);
        btnShareXY.Name = "btnShareXY";
        btnShareXY.Size = new Size(99, 31);
        btnShareXY.TabIndex = 9;
        btnShareXY.Text = "Share X and Y";
        btnShareXY.UseVisualStyleBackColor = true;
        // 
        // MultiplotSharedAxis
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(800, 608);
        Controls.Add(btnShareXY);
        Controls.Add(btnShareY);
        Controls.Add(btnShareX);
        Controls.Add(btnShareNone);
        Controls.Add(formsPlot1);
        Name = "MultiplotSharedAxis";
        Text = "MultiplotSharedAxis";
        ResumeLayout(false);
    }

    #endregion

    private ScottPlot.WinForms.FormsPlot formsPlot1;
    private Button btnShareNone;
    private Button btnShareX;
    private Button btnShareY;
    private Button btnShareXY;
}