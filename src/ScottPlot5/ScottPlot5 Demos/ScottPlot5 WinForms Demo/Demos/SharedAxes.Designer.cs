namespace WinForms_Demo.Demos;

partial class SharedAxes
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
        formsPlot1 = new ScottPlot.WinForms.FormsPlot();
        formsPlot2 = new ScottPlot.WinForms.FormsPlot();
        checkShareX = new CheckBox();
        checkShareY = new CheckBox();
        tableLayoutPanel1.SuspendLayout();
        SuspendLayout();
        // 
        // tableLayoutPanel1
        // 
        tableLayoutPanel1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        tableLayoutPanel1.ColumnCount = 1;
        tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        tableLayoutPanel1.Controls.Add(formsPlot1, 0, 0);
        tableLayoutPanel1.Controls.Add(formsPlot2, 0, 1);
        tableLayoutPanel1.Location = new Point(0, 37);
        tableLayoutPanel1.Name = "tableLayoutPanel1";
        tableLayoutPanel1.RowCount = 2;
        tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
        tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
        tableLayoutPanel1.Size = new Size(800, 616);
        tableLayoutPanel1.TabIndex = 0;
        // 
        // formsPlot1
        // 
        formsPlot1.DisplayScale = 1F;
        formsPlot1.Dock = DockStyle.Fill;
        formsPlot1.Location = new Point(3, 3);
        formsPlot1.Name = "formsPlot1";
        formsPlot1.Size = new Size(794, 302);
        formsPlot1.TabIndex = 0;
        // 
        // formsPlot2
        // 
        formsPlot2.DisplayScale = 1F;
        formsPlot2.Dock = DockStyle.Fill;
        formsPlot2.Location = new Point(3, 311);
        formsPlot2.Name = "formsPlot2";
        formsPlot2.Size = new Size(794, 302);
        formsPlot2.TabIndex = 1;
        // 
        // checkShareX
        // 
        checkShareX.AutoSize = true;
        checkShareX.Checked = true;
        checkShareX.CheckState = CheckState.Checked;
        checkShareX.Location = new Point(12, 12);
        checkShareX.Name = "checkShareX";
        checkShareX.Size = new Size(65, 19);
        checkShareX.TabIndex = 1;
        checkShareX.Text = "Share X";
        checkShareX.UseVisualStyleBackColor = true;
        // 
        // checkShareY
        // 
        checkShareY.AutoSize = true;
        checkShareY.Checked = true;
        checkShareY.CheckState = CheckState.Checked;
        checkShareY.Location = new Point(83, 12);
        checkShareY.Name = "checkShareY";
        checkShareY.Size = new Size(65, 19);
        checkShareY.TabIndex = 2;
        checkShareY.Text = "Share Y";
        checkShareY.UseVisualStyleBackColor = true;
        // 
        // SharedAxes
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(800, 653);
        Controls.Add(checkShareY);
        Controls.Add(checkShareX);
        Controls.Add(tableLayoutPanel1);
        Name = "SharedAxes";
        Text = "Shared Axes";
        tableLayoutPanel1.ResumeLayout(false);
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private TableLayoutPanel tableLayoutPanel1;
    private ScottPlot.WinForms.FormsPlot formsPlot1;
    private ScottPlot.WinForms.FormsPlot formsPlot2;
    private CheckBox checkShareX;
    private CheckBox checkShareY;
}