namespace WinForms_Demo.Demos;

partial class ScrollViewerDemo
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
        panel1 = new Panel();
        tableLayoutPanel1 = new TableLayoutPanel();
        formsPlot1 = new ScottPlot.WinForms.FormsPlot();
        formsPlot2 = new ScottPlot.WinForms.FormsPlot();
        formsPlot3 = new ScottPlot.WinForms.FormsPlot();
        groupBox1 = new GroupBox();
        radioZoomInOut = new RadioButton();
        radioScrollUpDown = new RadioButton();
        panel1.SuspendLayout();
        tableLayoutPanel1.SuspendLayout();
        groupBox1.SuspendLayout();
        SuspendLayout();
        // 
        // panel1
        // 
        panel1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        panel1.AutoScroll = true;
        panel1.BorderStyle = BorderStyle.Fixed3D;
        panel1.Controls.Add(tableLayoutPanel1);
        panel1.Location = new Point(12, 91);
        panel1.Name = "panel1";
        panel1.Size = new Size(517, 378);
        panel1.TabIndex = 0;
        // 
        // tableLayoutPanel1
        // 
        tableLayoutPanel1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        tableLayoutPanel1.ColumnCount = 1;
        tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        tableLayoutPanel1.Controls.Add(formsPlot1, 0, 0);
        tableLayoutPanel1.Controls.Add(formsPlot2, 0, 1);
        tableLayoutPanel1.Controls.Add(formsPlot3, 0, 2);
        tableLayoutPanel1.Location = new Point(3, 3);
        tableLayoutPanel1.Name = "tableLayoutPanel1";
        tableLayoutPanel1.RowCount = 3;
        tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 33.3333321F));
        tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 33.3333321F));
        tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 33.3333321F));
        tableLayoutPanel1.Size = new Size(490, 879);
        tableLayoutPanel1.TabIndex = 0;
        // 
        // formsPlot1
        // 
        formsPlot1.DisplayScale = 1F;
        formsPlot1.Dock = DockStyle.Fill;
        formsPlot1.Location = new Point(3, 3);
        formsPlot1.Name = "formsPlot1";
        formsPlot1.Size = new Size(484, 286);
        formsPlot1.TabIndex = 0;
        // 
        // formsPlot2
        // 
        formsPlot2.DisplayScale = 1F;
        formsPlot2.Dock = DockStyle.Fill;
        formsPlot2.Location = new Point(3, 295);
        formsPlot2.Name = "formsPlot2";
        formsPlot2.Size = new Size(484, 286);
        formsPlot2.TabIndex = 1;
        // 
        // formsPlot3
        // 
        formsPlot3.DisplayScale = 1F;
        formsPlot3.Dock = DockStyle.Fill;
        formsPlot3.Location = new Point(3, 587);
        formsPlot3.Name = "formsPlot3";
        formsPlot3.Size = new Size(484, 289);
        formsPlot3.TabIndex = 2;
        // 
        // groupBox1
        // 
        groupBox1.Controls.Add(radioZoomInOut);
        groupBox1.Controls.Add(radioScrollUpDown);
        groupBox1.Location = new Point(12, 12);
        groupBox1.Name = "groupBox1";
        groupBox1.Size = new Size(140, 73);
        groupBox1.TabIndex = 1;
        groupBox1.TabStop = false;
        groupBox1.Text = "Mouse Wheel Action";
        // 
        // radioZoomInOut
        // 
        radioZoomInOut.AutoSize = true;
        radioZoomInOut.Checked = true;
        radioZoomInOut.Location = new Point(8, 47);
        radioZoomInOut.Name = "radioZoomInOut";
        radioZoomInOut.Size = new Size(93, 19);
        radioZoomInOut.TabIndex = 1;
        radioZoomInOut.TabStop = true;
        radioZoomInOut.Text = "Zoom in/out";
        radioZoomInOut.UseVisualStyleBackColor = true;
        // 
        // radioScrollUpDown
        // 
        radioScrollUpDown.AutoSize = true;
        radioScrollUpDown.Location = new Point(8, 22);
        radioScrollUpDown.Name = "radioScrollUpDown";
        radioScrollUpDown.Size = new Size(106, 19);
        radioScrollUpDown.TabIndex = 0;
        radioScrollUpDown.Text = "Scroll up/down";
        radioScrollUpDown.UseVisualStyleBackColor = true;
        // 
        // ScrollViewerDemo
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(541, 481);
        Controls.Add(groupBox1);
        Controls.Add(panel1);
        Name = "ScrollViewerDemo";
        Text = "ScrollViewerDemo";
        panel1.ResumeLayout(false);
        tableLayoutPanel1.ResumeLayout(false);
        groupBox1.ResumeLayout(false);
        groupBox1.PerformLayout();
        ResumeLayout(false);
    }

    #endregion

    private Panel panel1;
    private TableLayoutPanel tableLayoutPanel1;
    private ScottPlot.WinForms.FormsPlot formsPlot1;
    private ScottPlot.WinForms.FormsPlot formsPlot2;
    private ScottPlot.WinForms.FormsPlot formsPlot3;
    private GroupBox groupBox1;
    private RadioButton radioZoomInOut;
    private RadioButton radioScrollUpDown;
}