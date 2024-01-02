namespace WinForms_Demo.Demos;

partial class OpenGL
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
        formsPlotgl1 = new ScottPlot.WinForms.FormsPlotGL();
        cbPointCount = new ComboBox();
        label1 = new Label();
        cbDataType = new ComboBox();
        label2 = new Label();
        label3 = new Label();
        cbPlotType = new ComboBox();
        tableLayoutPanel1.SuspendLayout();
        SuspendLayout();
        // 
        // tableLayoutPanel1
        // 
        tableLayoutPanel1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        tableLayoutPanel1.ColumnCount = 2;
        tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        tableLayoutPanel1.Controls.Add(formsPlot1, 0, 0);
        tableLayoutPanel1.Controls.Add(formsPlotgl1, 1, 0);
        tableLayoutPanel1.Location = new Point(12, 41);
        tableLayoutPanel1.Name = "tableLayoutPanel1";
        tableLayoutPanel1.RowCount = 1;
        tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
        tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
        tableLayoutPanel1.Size = new Size(1360, 507);
        tableLayoutPanel1.TabIndex = 0;
        // 
        // formsPlot1
        // 
        formsPlot1.DisplayScale = 1F;
        formsPlot1.Dock = DockStyle.Fill;
        formsPlot1.Location = new Point(3, 3);
        formsPlot1.Name = "formsPlot1";
        formsPlot1.Size = new Size(674, 501);
        formsPlot1.TabIndex = 0;
        // 
        // formsPlotgl1
        // 
        formsPlotgl1.DisplayScale = 1F;
        formsPlotgl1.Dock = DockStyle.Fill;
        formsPlotgl1.Location = new Point(683, 3);
        formsPlotgl1.Name = "formsPlotgl1";
        formsPlotgl1.Size = new Size(674, 501);
        formsPlotgl1.TabIndex = 1;
        // 
        // cbPointCount
        // 
        cbPointCount.FormattingEnabled = true;
        cbPointCount.Location = new Point(264, 12);
        cbPointCount.Name = "cbPointCount";
        cbPointCount.Size = new Size(121, 23);
        cbPointCount.TabIndex = 3;
        // 
        // label1
        // 
        label1.AutoSize = true;
        label1.Location = new Point(215, 15);
        label1.Name = "label1";
        label1.Size = new Size(43, 15);
        label1.TabIndex = 4;
        label1.Text = "Points:";
        // 
        // cbDataType
        // 
        cbDataType.FormattingEnabled = true;
        cbDataType.Location = new Point(49, 12);
        cbDataType.Name = "cbDataType";
        cbDataType.Size = new Size(121, 23);
        cbDataType.TabIndex = 5;
        // 
        // label2
        // 
        label2.AutoSize = true;
        label2.Location = new Point(9, 15);
        label2.Name = "label2";
        label2.Size = new Size(34, 15);
        label2.TabIndex = 6;
        label2.Text = "Data:";
        // 
        // label3
        // 
        label3.AutoSize = true;
        label3.Location = new Point(449, 15);
        label3.Name = "label3";
        label3.Size = new Size(34, 15);
        label3.TabIndex = 7;
        label3.Text = "Type:";
        // 
        // cbPlotType
        // 
        cbPlotType.FormattingEnabled = true;
        cbPlotType.Location = new Point(489, 12);
        cbPlotType.Name = "cbPlotType";
        cbPlotType.Size = new Size(121, 23);
        cbPlotType.TabIndex = 8;
        // 
        // OpenGLPerformance
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(1384, 560);
        Controls.Add(cbPlotType);
        Controls.Add(label3);
        Controls.Add(label2);
        Controls.Add(cbDataType);
        Controls.Add(label1);
        Controls.Add(cbPointCount);
        Controls.Add(tableLayoutPanel1);
        Name = "OpenGLPerformance";
        Text = "OpenGLPerformance";
        tableLayoutPanel1.ResumeLayout(false);
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private TableLayoutPanel tableLayoutPanel1;
    private ScottPlot.WinForms.FormsPlot formsPlot1;
    private ScottPlot.WinForms.FormsPlotGL formsPlotgl1;
    private ComboBox cbPointCount;
    private Label label1;
    private ComboBox cbDataType;
    private Label label2;
    private Label label3;
    private ComboBox cbPlotType;
}