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
        tabControl1 = new TabControl();
        Single = new TabPage();
        Double = new TabPage();
        tableLayoutPanel1 = new TableLayoutPanel();
        formsPlot2 = new ScottPlot.WinForms.FormsPlot();
        formsPlot3 = new ScottPlot.WinForms.FormsPlot();
        tabControl1.SuspendLayout();
        Single.SuspendLayout();
        Double.SuspendLayout();
        tableLayoutPanel1.SuspendLayout();
        SuspendLayout();
        // 
        // formsPlot1
        // 
        formsPlot1.DisplayScale = 1F;
        formsPlot1.Dock = DockStyle.Fill;
        formsPlot1.Location = new Point(3, 3);
        formsPlot1.Name = "formsPlot1";
        formsPlot1.Size = new Size(714, 521);
        formsPlot1.TabIndex = 0;
        // 
        // tabControl1
        // 
        tabControl1.Controls.Add(Single);
        tabControl1.Controls.Add(Double);
        tabControl1.Dock = DockStyle.Fill;
        tabControl1.Location = new Point(0, 0);
        tabControl1.Name = "tabControl1";
        tabControl1.SelectedIndex = 0;
        tabControl1.Size = new Size(728, 555);
        tabControl1.TabIndex = 1;
        // 
        // Single
        // 
        Single.Controls.Add(formsPlot1);
        Single.Location = new Point(4, 24);
        Single.Name = "Single";
        Single.Padding = new Padding(3);
        Single.Size = new Size(720, 527);
        Single.TabIndex = 0;
        Single.Text = "Single Plot";
        Single.UseVisualStyleBackColor = true;
        // 
        // Double
        // 
        Double.Controls.Add(tableLayoutPanel1);
        Double.Location = new Point(4, 24);
        Double.Name = "Double";
        Double.Padding = new Padding(3);
        Double.Size = new Size(720, 527);
        Double.TabIndex = 1;
        Double.Text = "Stacked Plots";
        Double.UseVisualStyleBackColor = true;
        // 
        // tableLayoutPanel1
        // 
        tableLayoutPanel1.ColumnCount = 1;
        tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        tableLayoutPanel1.Controls.Add(formsPlot2, 0, 0);
        tableLayoutPanel1.Controls.Add(formsPlot3, 0, 1);
        tableLayoutPanel1.Dock = DockStyle.Fill;
        tableLayoutPanel1.Location = new Point(3, 3);
        tableLayoutPanel1.Name = "tableLayoutPanel1";
        tableLayoutPanel1.RowCount = 2;
        tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
        tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
        tableLayoutPanel1.Size = new Size(714, 521);
        tableLayoutPanel1.TabIndex = 0;
        // 
        // formsPlot2
        // 
        formsPlot2.DisplayScale = 1F;
        formsPlot2.Dock = DockStyle.Fill;
        formsPlot2.Location = new Point(3, 3);
        formsPlot2.Name = "formsPlot2";
        formsPlot2.Size = new Size(708, 254);
        formsPlot2.TabIndex = 0;
        // 
        // formsPlot3
        // 
        formsPlot3.DisplayScale = 1F;
        formsPlot3.Dock = DockStyle.Fill;
        formsPlot3.Location = new Point(3, 263);
        formsPlot3.Name = "formsPlot3";
        formsPlot3.Size = new Size(708, 255);
        formsPlot3.TabIndex = 1;
        // 
        // Form1
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(728, 555);
        Controls.Add(tabControl1);
        Name = "Form1";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "ScottPlot 5 - Windows Forms Sandbox";
        tabControl1.ResumeLayout(false);
        Single.ResumeLayout(false);
        Double.ResumeLayout(false);
        tableLayoutPanel1.ResumeLayout(false);
        ResumeLayout(false);
    }

    #endregion

    private ScottPlot.WinForms.FormsPlot formsPlot1;
    private TabControl tabControl1;
    private TabPage Single;
    private TabPage Double;
    private TableLayoutPanel tableLayoutPanel1;
    private ScottPlot.WinForms.FormsPlot formsPlot2;
    private ScottPlot.WinForms.FormsPlot formsPlot3;
}
