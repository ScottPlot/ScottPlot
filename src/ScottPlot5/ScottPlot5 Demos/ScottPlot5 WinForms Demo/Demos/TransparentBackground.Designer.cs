namespace WinForms_Demo.Demos;

partial class TransparentBackground
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
        button1 = new Button();
        button2 = new Button();
        button3 = new Button();
        textBox1 = new TextBox();
        formsPlot1 = new ScottPlot.WinForms.FormsPlot();
        SuspendLayout();
        // 
        // button1
        // 
        button1.Location = new Point(12, 12);
        button1.Name = "button1";
        button1.Size = new Size(75, 32);
        button1.TabIndex = 0;
        button1.Text = "button1";
        button1.UseVisualStyleBackColor = true;
        // 
        // button2
        // 
        button2.Location = new Point(93, 12);
        button2.Name = "button2";
        button2.Size = new Size(75, 32);
        button2.TabIndex = 1;
        button2.Text = "button2";
        button2.UseVisualStyleBackColor = true;
        // 
        // button3
        // 
        button3.Location = new Point(174, 12);
        button3.Name = "button3";
        button3.Size = new Size(75, 32);
        button3.TabIndex = 2;
        button3.Text = "button3";
        button3.UseVisualStyleBackColor = true;
        // 
        // textBox1
        // 
        textBox1.Location = new Point(255, 18);
        textBox1.Name = "textBox1";
        textBox1.Size = new Size(224, 23);
        textBox1.TabIndex = 3;
        textBox1.Text = "Hello, World";
        // 
        // formsPlot1
        // 
        formsPlot1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        formsPlot1.DisplayScale = 1F;
        formsPlot1.Location = new Point(12, 50);
        formsPlot1.Name = "formsPlot1";
        formsPlot1.Size = new Size(776, 388);
        formsPlot1.TabIndex = 4;
        // 
        // TransparentBackground
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(800, 450);
        Controls.Add(formsPlot1);
        Controls.Add(textBox1);
        Controls.Add(button3);
        Controls.Add(button2);
        Controls.Add(button1);
        Name = "TransparentBackground";
        Text = "TransparentBackground";
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private Button button1;
    private Button button2;
    private Button button3;
    private TextBox textBox1;
    private ScottPlot.WinForms.FormsPlot formsPlot1;
}