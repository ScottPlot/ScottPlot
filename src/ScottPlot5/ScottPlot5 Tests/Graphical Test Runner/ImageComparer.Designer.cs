namespace GraphicalTestRunner;

partial class ImageComparer
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

    #region Component Designer generated code

    /// <summary> 
    /// Required method for Designer support - do not modify 
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        pictureBox1 = new PictureBox();
        timer1 = new System.Windows.Forms.Timer(components);
        tableLayoutPanel1 = new TableLayoutPanel();
        panel1 = new Panel();
        checkBox1 = new CheckBox();
        panel2 = new Panel();
        checkBox2 = new CheckBox();
        label3 = new Label();
        pictureBox2 = new PictureBox();
        label2 = new Label();
        label1 = new Label();
        ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
        tableLayoutPanel1.SuspendLayout();
        panel1.SuspendLayout();
        panel2.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
        SuspendLayout();
        // 
        // pictureBox1
        // 
        pictureBox1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        pictureBox1.BackColor = SystemColors.ControlDark;
        pictureBox1.Location = new Point(3, 31);
        pictureBox1.Name = "pictureBox1";
        pictureBox1.Size = new Size(613, 405);
        pictureBox1.TabIndex = 2;
        pictureBox1.TabStop = false;
        // 
        // timer1
        // 
        timer1.Interval = 50;
        // 
        // tableLayoutPanel1
        // 
        tableLayoutPanel1.ColumnCount = 2;
        tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        tableLayoutPanel1.Controls.Add(panel1, 0, 0);
        tableLayoutPanel1.Controls.Add(panel2, 1, 0);
        tableLayoutPanel1.Dock = DockStyle.Fill;
        tableLayoutPanel1.Location = new Point(0, 0);
        tableLayoutPanel1.Name = "tableLayoutPanel1";
        tableLayoutPanel1.RowCount = 1;
        tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
        tableLayoutPanel1.Size = new Size(1250, 445);
        tableLayoutPanel1.TabIndex = 8;
        // 
        // panel1
        // 
        panel1.Controls.Add(checkBox1);
        panel1.Controls.Add(pictureBox1);
        panel1.Controls.Add(label1);
        panel1.Controls.Add(label2);
        panel1.Dock = DockStyle.Fill;
        panel1.Location = new Point(3, 3);
        panel1.Name = "panel1";
        panel1.Size = new Size(619, 439);
        panel1.TabIndex = 0;
        // 
        // checkBox1
        // 
        checkBox1.AutoSize = true;
        checkBox1.Checked = true;
        checkBox1.CheckState = CheckState.Checked;
        checkBox1.Location = new Point(9, 8);
        checkBox1.Name = "checkBox1";
        checkBox1.Size = new Size(52, 19);
        checkBox1.TabIndex = 7;
        checkBox1.Text = "Auto";
        checkBox1.UseVisualStyleBackColor = true;
        // 
        // panel2
        // 
        panel2.Controls.Add(checkBox2);
        panel2.Controls.Add(label3);
        panel2.Controls.Add(pictureBox2);
        panel2.Dock = DockStyle.Fill;
        panel2.Location = new Point(628, 3);
        panel2.Name = "panel2";
        panel2.Size = new Size(619, 439);
        panel2.TabIndex = 1;
        // 
        // checkBox2
        // 
        checkBox2.AutoSize = true;
        checkBox2.Checked = true;
        checkBox2.CheckState = CheckState.Checked;
        checkBox2.Location = new Point(75, 8);
        checkBox2.Name = "checkBox2";
        checkBox2.Size = new Size(78, 19);
        checkBox2.TabIndex = 8;
        checkBox2.Text = "Autoscale";
        checkBox2.UseVisualStyleBackColor = true;
        // 
        // label3
        // 
        label3.AutoSize = true;
        label3.Location = new Point(8, 9);
        label3.Name = "label3";
        label3.Size = new Size(61, 15);
        label3.TabIndex = 7;
        label3.Text = "Difference";
        // 
        // pictureBox2
        // 
        pictureBox2.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        pictureBox2.BackColor = SystemColors.ControlDark;
        pictureBox2.Location = new Point(0, 31);
        pictureBox2.Name = "pictureBox2";
        pictureBox2.Size = new Size(613, 405);
        pictureBox2.TabIndex = 3;
        pictureBox2.TabStop = false;
        // 
        // label2
        // 
        label2.AutoSize = true;
        label2.Location = new Point(122, 9);
        label2.Name = "label2";
        label2.Size = new Size(49, 15);
        label2.TabIndex = 6;
        label2.Text = "Image 2";
        // 
        // label1
        // 
        label1.AutoSize = true;
        label1.Location = new Point(67, 9);
        label1.Name = "label1";
        label1.Size = new Size(49, 15);
        label1.TabIndex = 0;
        label1.Text = "Image 1";
        // 
        // ImageComparer
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        Controls.Add(tableLayoutPanel1);
        Name = "ImageComparer";
        Size = new Size(1250, 445);
        ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
        tableLayoutPanel1.ResumeLayout(false);
        panel1.ResumeLayout(false);
        panel1.PerformLayout();
        panel2.ResumeLayout(false);
        panel2.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
        ResumeLayout(false);
    }

    #endregion
    private PictureBox pictureBox1;
    private System.Windows.Forms.Timer timer1;
    private TableLayoutPanel tableLayoutPanel1;
    private Panel panel1;
    private Panel panel2;
    private CheckBox checkBox1;
    private CheckBox checkBox2;
    private Label label3;
    private PictureBox pictureBox2;
    private Label label1;
    private Label label2;
}
