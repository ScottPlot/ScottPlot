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
        button1 = new Button();
        SuspendLayout();
        // 
        // formsPlot1
        // 
        formsPlot1.DisplayScale = 1F;
        formsPlot1.Location = new Point(61, 84);
        formsPlot1.Name = "formsPlot1";
        formsPlot1.Size = new Size(544, 330);
        formsPlot1.TabIndex = 0;
        // 
        // button1
        // 
        button1.Location = new Point(23, 27);
        button1.Name = "button1";
        button1.Size = new Size(75, 23);
        button1.TabIndex = 1;
        button1.Text = "button1";
        button1.UseVisualStyleBackColor = true;
        // 
        // Form1
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(669, 415);
        Controls.Add(button1);
        Controls.Add(formsPlot1);
        Margin = new Padding(4, 3, 4, 3);
        Name = "Form1";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "ScottPlot 5 - Windows Forms Sandbox";
        ResumeLayout(false);
    }

    #endregion

    private ScottPlot.WinForms.FormsPlot formsPlot1;
    private Button button1;
}
