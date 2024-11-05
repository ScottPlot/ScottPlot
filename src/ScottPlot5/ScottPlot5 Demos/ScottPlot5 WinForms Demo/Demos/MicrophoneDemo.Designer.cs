namespace WinForms_Demo.Demos;

partial class MicrophoneDemo
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
        tableLayoutPanel1 = new TableLayoutPanel();
        formsPlot2 = new ScottPlot.WinForms.FormsPlot();
        formsPlot1 = new ScottPlot.WinForms.FormsPlot();
        comboBox1 = new ComboBox();
        label1 = new Label();
        tableLayoutPanel1.SuspendLayout();
        SuspendLayout();
        // 
        // tableLayoutPanel1
        // 
        tableLayoutPanel1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        tableLayoutPanel1.ColumnCount = 1;
        tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        tableLayoutPanel1.Controls.Add(formsPlot2, 0, 1);
        tableLayoutPanel1.Controls.Add(formsPlot1, 0, 0);
        tableLayoutPanel1.Location = new Point(0, 41);
        tableLayoutPanel1.Name = "tableLayoutPanel1";
        tableLayoutPanel1.RowCount = 2;
        tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
        tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
        tableLayoutPanel1.Size = new Size(800, 556);
        tableLayoutPanel1.TabIndex = 0;
        // 
        // formsPlot2
        // 
        formsPlot2.DisplayScale = 1F;
        formsPlot2.Dock = DockStyle.Fill;
        formsPlot2.Location = new Point(3, 281);
        formsPlot2.Name = "formsPlot2";
        formsPlot2.Size = new Size(794, 272);
        formsPlot2.TabIndex = 3;
        // 
        // formsPlot1
        // 
        formsPlot1.DisplayScale = 1F;
        formsPlot1.Dock = DockStyle.Fill;
        formsPlot1.Location = new Point(3, 3);
        formsPlot1.Name = "formsPlot1";
        formsPlot1.Size = new Size(794, 272);
        formsPlot1.TabIndex = 2;
        // 
        // comboBox1
        // 
        comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
        comboBox1.FormattingEnabled = true;
        comboBox1.Location = new Point(120, 6);
        comboBox1.Name = "comboBox1";
        comboBox1.Size = new Size(196, 23);
        comboBox1.TabIndex = 1;
        // 
        // label1
        // 
        label1.AutoSize = true;
        label1.Location = new Point(12, 9);
        label1.Name = "label1";
        label1.Size = new Size(102, 15);
        label1.TabIndex = 2;
        label1.Text = "Recording Device:";
        // 
        // MicrophoneDemo
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(800, 597);
        Controls.Add(label1);
        Controls.Add(comboBox1);
        Controls.Add(tableLayoutPanel1);
        Name = "MicrophoneDemo";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "ScottPlot Microphone Demo";
        tableLayoutPanel1.ResumeLayout(false);
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private TableLayoutPanel tableLayoutPanel1;
    private ScottPlot.WinForms.FormsPlot formsPlot2;
    private ScottPlot.WinForms.FormsPlot formsPlot1;
    private ComboBox comboBox1;
    private Label label1;
}