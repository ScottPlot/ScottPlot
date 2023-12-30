namespace WinForms_Demo;

partial class MainMenuForm
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
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainMenuForm));
        tableLayoutPanel1 = new TableLayoutPanel();
        panel1 = new Panel();
        pictureBox1 = new PictureBox();
        label2 = new Label();
        label1 = new Label();
        tableLayoutPanel1.SuspendLayout();
        panel1.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
        SuspendLayout();
        // 
        // tableLayoutPanel1
        // 
        tableLayoutPanel1.AutoScroll = true;
        tableLayoutPanel1.AutoSize = true;
        tableLayoutPanel1.ColumnCount = 1;
        tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        tableLayoutPanel1.Controls.Add(panel1, 0, 0);
        tableLayoutPanel1.Dock = DockStyle.Fill;
        tableLayoutPanel1.Location = new Point(0, 0);
        tableLayoutPanel1.Name = "tableLayoutPanel1";
        tableLayoutPanel1.RowCount = 1;
        tableLayoutPanel1.RowStyles.Add(new RowStyle());
        tableLayoutPanel1.Size = new Size(568, 645);
        tableLayoutPanel1.TabIndex = 3;
        // 
        // panel1
        // 
        panel1.BackColor = SystemColors.Control;
        panel1.Controls.Add(pictureBox1);
        panel1.Controls.Add(label2);
        panel1.Controls.Add(label1);
        panel1.Location = new Point(3, 3);
        panel1.Name = "panel1";
        panel1.Size = new Size(507, 81);
        panel1.TabIndex = 0;
        // 
        // pictureBox1
        // 
        pictureBox1.Image = (Image)resources.GetObject("pictureBox1.Image");
        pictureBox1.Location = new Point(8, 8);
        pictureBox1.Name = "pictureBox1";
        pictureBox1.Size = new Size(64, 64);
        pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
        pictureBox1.TabIndex = 0;
        pictureBox1.TabStop = false;
        // 
        // label2
        // 
        label2.AutoSize = true;
        label2.Font = new Font("Segoe UI Semilight", 12F, FontStyle.Regular, GraphicsUnit.Point);
        label2.Location = new Point(80, 41);
        label2.Name = "label2";
        label2.Size = new Size(181, 21);
        label2.TabIndex = 2;
        label2.Text = "ScottPlot.WinForms 5.0.0";
        // 
        // label1
        // 
        label1.AutoSize = true;
        label1.Font = new Font("Segoe UI", 20.25F, FontStyle.Regular, GraphicsUnit.Point);
        label1.Location = new Point(78, 9);
        label1.Name = "label1";
        label1.Size = new Size(203, 37);
        label1.TabIndex = 1;
        label1.Text = "ScottPlot Demo";
        // 
        // MainMenuForm
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        AutoScroll = true;
        ClientSize = new Size(568, 645);
        Controls.Add(tableLayoutPanel1);
        Icon = (Icon)resources.GetObject("$this.Icon");
        Name = "MainMenuForm";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "ScottPlot Demo";
        Load += MainMenuForm_Load;
        tableLayoutPanel1.ResumeLayout(false);
        panel1.ResumeLayout(false);
        panel1.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private TableLayoutPanel tableLayoutPanel1;
    private Panel panel1;
    private PictureBox pictureBox1;
    private Label label2;
    private Label label1;
}