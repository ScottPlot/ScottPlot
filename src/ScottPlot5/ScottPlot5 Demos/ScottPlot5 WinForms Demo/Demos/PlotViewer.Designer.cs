namespace WinForms_Demo.Demos;

partial class PlotViewer
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
        richTextBox1 = new RichTextBox();
        richTextBox2 = new RichTextBox();
        numericUpDown1 = new NumericUpDown();
        label1 = new Label();
        ((System.ComponentModel.ISupportInitialize)numericUpDown1).BeginInit();
        SuspendLayout();
        // 
        // button1
        // 
        button1.Font = new Font("Segoe UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point);
        button1.Location = new Point(303, 20);
        button1.Name = "button1";
        button1.Size = new Size(88, 47);
        button1.TabIndex = 0;
        button1.Text = "Launch";
        button1.UseVisualStyleBackColor = true;
        // 
        // richTextBox1
        // 
        richTextBox1.BackColor = SystemColors.Control;
        richTextBox1.BorderStyle = BorderStyle.None;
        richTextBox1.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
        richTextBox1.Location = new Point(32, 87);
        richTextBox1.Name = "richTextBox1";
        richTextBox1.Size = new Size(359, 68);
        richTextBox1.TabIndex = 3;
        richTextBox1.Text = "When the Launch button is pressed, a Plot is generated programmatically and displayed in a FormsPlotViewer.";
        // 
        // richTextBox2
        // 
        richTextBox2.BackColor = SystemColors.Control;
        richTextBox2.BorderStyle = BorderStyle.None;
        richTextBox2.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
        richTextBox2.Location = new Point(32, 170);
        richTextBox2.Name = "richTextBox2";
        richTextBox2.Size = new Size(359, 68);
        richTextBox2.TabIndex = 4;
        richTextBox2.Text = "This strategy can be used to launch mouse-interactive plots from console applications if the ScottPlot.WinForms package is included.";
        // 
        // numericUpDown1
        // 
        numericUpDown1.Font = new Font("Segoe UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point);
        numericUpDown1.Location = new Point(197, 29);
        numericUpDown1.Name = "numericUpDown1";
        numericUpDown1.Size = new Size(81, 33);
        numericUpDown1.TabIndex = 1;
        numericUpDown1.Value = new decimal(new int[] { 3, 0, 0, 0 });
        // 
        // label1
        // 
        label1.AutoSize = true;
        label1.Font = new Font("Segoe UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point);
        label1.Location = new Point(32, 31);
        label1.Name = "label1";
        label1.Size = new Size(153, 25);
        label1.TabIndex = 2;
        label1.Text = "Number of Plots:";
        // 
        // PlotViewer
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(451, 256);
        Controls.Add(richTextBox2);
        Controls.Add(richTextBox1);
        Controls.Add(label1);
        Controls.Add(numericUpDown1);
        Controls.Add(button1);
        Name = "PlotViewer";
        Text = "PlotViewer Demo";
        ((System.ComponentModel.ISupportInitialize)numericUpDown1).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private Button button1;
    private RichTextBox richTextBox1;
    private RichTextBox richTextBox2;
    private NumericUpDown numericUpDown1;
    private Label label1;
}