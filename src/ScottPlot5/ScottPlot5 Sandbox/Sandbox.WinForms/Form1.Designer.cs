namespace Sandbox.WinForms;

partial class Form1
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
        cbManual = new CheckBox();
        cbInterior = new CheckBox();
        cbRotated = new CheckBox();
        SuspendLayout();
        // 
        // formsPlot1
        // 
        formsPlot1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        formsPlot1.DisplayScale = 1F;
        formsPlot1.Location = new Point(12, 37);
        formsPlot1.Name = "formsPlot1";
        formsPlot1.Size = new Size(776, 401);
        formsPlot1.TabIndex = 0;
        // 
        // cbManual
        // 
        cbManual.AutoSize = true;
        cbManual.Location = new Point(12, 12);
        cbManual.Name = "cbManual";
        cbManual.Size = new Size(108, 19);
        cbManual.TabIndex = 1;
        cbManual.Text = "Manual Isolines";
        cbManual.UseVisualStyleBackColor = true;
        // 
        // cbInterior
        // 
        cbInterior.AutoSize = true;
        cbInterior.Location = new Point(126, 12);
        cbInterior.Name = "cbInterior";
        cbInterior.Size = new Size(124, 19);
        cbInterior.TabIndex = 2;
        cbInterior.Text = "Interior Tick Labels";
        cbInterior.UseVisualStyleBackColor = true;
        // 
        // cbRotated
        // 
        cbRotated.AutoSize = true;
        cbRotated.Location = new Point(256, 12);
        cbRotated.Name = "cbRotated";
        cbRotated.Size = new Size(127, 19);
        cbRotated.TabIndex = 3;
        cbRotated.Text = "Rotated Tick Labels";
        cbRotated.UseVisualStyleBackColor = true;
        // 
        // Form1
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(800, 450);
        Controls.Add(cbRotated);
        Controls.Add(cbInterior);
        Controls.Add(cbManual);
        Controls.Add(formsPlot1);
        Name = "Form1";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "ScottPlot 5 - Windows Forms Sandbox";
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private ScottPlot.WinForms.FormsPlot formsPlot1;
    private CheckBox cbManual;
    private CheckBox cbInterior;
    private CheckBox cbRotated;
}
