namespace WinForms_Demo.Demos;

partial class LabelDemo
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
        tbSize = new TrackBar();
        tableLayoutPanel1 = new TableLayoutPanel();
        label5 = new Label();
        tbPadding = new TrackBar();
        tbRotation = new TrackBar();
        label3 = new Label();
        tbAlignment = new TrackBar();
        label2 = new Label();
        label1 = new Label();
        pictureBox1 = new PictureBox();
        ((System.ComponentModel.ISupportInitialize)tbSize).BeginInit();
        tableLayoutPanel1.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)tbPadding).BeginInit();
        ((System.ComponentModel.ISupportInitialize)tbRotation).BeginInit();
        ((System.ComponentModel.ISupportInitialize)tbAlignment).BeginInit();
        ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
        SuspendLayout();
        // 
        // tbSize
        // 
        tbSize.AutoSize = false;
        tbSize.Dock = DockStyle.Fill;
        tbSize.Location = new Point(103, 3);
        tbSize.Maximum = 96;
        tbSize.Minimum = 4;
        tbSize.Name = "tbSize";
        tbSize.Size = new Size(473, 44);
        tbSize.TabIndex = 1;
        tbSize.Value = 24;
        // 
        // tableLayoutPanel1
        // 
        tableLayoutPanel1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        tableLayoutPanel1.ColumnCount = 2;
        tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 100F));
        tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        tableLayoutPanel1.Controls.Add(label5, 0, 3);
        tableLayoutPanel1.Controls.Add(tbPadding, 0, 3);
        tableLayoutPanel1.Controls.Add(tbRotation, 1, 2);
        tableLayoutPanel1.Controls.Add(label3, 0, 2);
        tableLayoutPanel1.Controls.Add(tbAlignment, 1, 1);
        tableLayoutPanel1.Controls.Add(label2, 0, 1);
        tableLayoutPanel1.Controls.Add(label1, 0, 0);
        tableLayoutPanel1.Controls.Add(tbSize, 1, 0);
        tableLayoutPanel1.Location = new Point(12, 12);
        tableLayoutPanel1.Name = "tableLayoutPanel1";
        tableLayoutPanel1.RowCount = 4;
        tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
        tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
        tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
        tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
        tableLayoutPanel1.Size = new Size(579, 198);
        tableLayoutPanel1.TabIndex = 4;
        // 
        // label5
        // 
        label5.Dock = DockStyle.Fill;
        label5.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
        label5.Location = new Point(3, 150);
        label5.Name = "label5";
        label5.Size = new Size(94, 50);
        label5.TabIndex = 10;
        label5.Text = "Padding";
        label5.TextAlign = ContentAlignment.TopCenter;
        // 
        // tbPadding
        // 
        tbPadding.AutoSize = false;
        tbPadding.Dock = DockStyle.Fill;
        tbPadding.Location = new Point(103, 153);
        tbPadding.Maximum = 25;
        tbPadding.Name = "tbPadding";
        tbPadding.Size = new Size(473, 44);
        tbPadding.TabIndex = 9;
        // 
        // tbRotation
        // 
        tbRotation.AutoSize = false;
        tbRotation.Dock = DockStyle.Fill;
        tbRotation.Location = new Point(103, 103);
        tbRotation.Maximum = 360;
        tbRotation.Name = "tbRotation";
        tbRotation.Size = new Size(473, 44);
        tbRotation.TabIndex = 5;
        // 
        // label3
        // 
        label3.Dock = DockStyle.Fill;
        label3.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
        label3.Location = new Point(3, 100);
        label3.Name = "label3";
        label3.Size = new Size(94, 50);
        label3.TabIndex = 4;
        label3.Text = "Rotation";
        label3.TextAlign = ContentAlignment.TopCenter;
        // 
        // tbAlignment
        // 
        tbAlignment.AutoSize = false;
        tbAlignment.Dock = DockStyle.Fill;
        tbAlignment.Location = new Point(103, 53);
        tbAlignment.Maximum = 8;
        tbAlignment.Name = "tbAlignment";
        tbAlignment.Size = new Size(473, 44);
        tbAlignment.TabIndex = 3;
        // 
        // label2
        // 
        label2.Dock = DockStyle.Fill;
        label2.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
        label2.Location = new Point(3, 50);
        label2.Name = "label2";
        label2.Size = new Size(94, 50);
        label2.TabIndex = 2;
        label2.Text = "Alignment";
        label2.TextAlign = ContentAlignment.TopCenter;
        // 
        // label1
        // 
        label1.Dock = DockStyle.Fill;
        label1.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
        label1.Location = new Point(3, 0);
        label1.Name = "label1";
        label1.Size = new Size(94, 50);
        label1.TabIndex = 0;
        label1.Text = "Size";
        label1.TextAlign = ContentAlignment.TopCenter;
        // 
        // pictureBox1
        // 
        pictureBox1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        pictureBox1.BackColor = SystemColors.ControlDark;
        pictureBox1.Location = new Point(9, 216);
        pictureBox1.Name = "pictureBox1";
        pictureBox1.Size = new Size(582, 404);
        pictureBox1.TabIndex = 5;
        pictureBox1.TabStop = false;
        // 
        // LabelDemo
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(603, 632);
        Controls.Add(pictureBox1);
        Controls.Add(tableLayoutPanel1);
        Name = "LabelDemo";
        Text = "LabelDemo";
        ((System.ComponentModel.ISupportInitialize)tbSize).EndInit();
        tableLayoutPanel1.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)tbPadding).EndInit();
        ((System.ComponentModel.ISupportInitialize)tbRotation).EndInit();
        ((System.ComponentModel.ISupportInitialize)tbAlignment).EndInit();
        ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
        ResumeLayout(false);
    }

    #endregion
    private TrackBar tbSize;
    private TableLayoutPanel tableLayoutPanel1;
    private Label label5;
    private TrackBar tbPadding;
    private TrackBar tbRotation;
    private Label label3;
    private TrackBar tbAlignment;
    private Label label2;
    private Label label1;
    private PictureBox pictureBox1;
}