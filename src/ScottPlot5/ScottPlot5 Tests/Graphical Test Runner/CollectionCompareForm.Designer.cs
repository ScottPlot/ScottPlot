namespace Graphical_Test_Runner;

partial class CollectionCompareForm
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
        btnAnalyze = new Button();
        tbBefore = new TextBox();
        tbAfter = new TextBox();
        progressBar1 = new ProgressBar();
        dataGridView1 = new DataGridView();
        cbChanged = new CheckBox();
        tableLayoutPanel2 = new TableLayoutPanel();
        pictureBox1 = new PictureBox();
        pictureBox2 = new PictureBox();
        pictureBox3 = new PictureBox();
        ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
        tableLayoutPanel2.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
        ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
        ((System.ComponentModel.ISupportInitialize)pictureBox3).BeginInit();
        SuspendLayout();
        // 
        // btnAnalyze
        // 
        btnAnalyze.Location = new Point(12, 70);
        btnAnalyze.Name = "btnAnalyze";
        btnAnalyze.Size = new Size(93, 23);
        btnAnalyze.TabIndex = 2;
        btnAnalyze.Text = "Analyze";
        btnAnalyze.UseVisualStyleBackColor = true;
        // 
        // tbBefore
        // 
        tbBefore.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        tbBefore.Location = new Point(12, 12);
        tbBefore.Name = "tbBefore";
        tbBefore.Size = new Size(1302, 23);
        tbBefore.TabIndex = 3;
        // 
        // tbAfter
        // 
        tbAfter.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        tbAfter.Location = new Point(12, 41);
        tbAfter.Name = "tbAfter";
        tbAfter.Size = new Size(1302, 23);
        tbAfter.TabIndex = 4;
        // 
        // progressBar1
        // 
        progressBar1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        progressBar1.Location = new Point(219, 70);
        progressBar1.Name = "progressBar1";
        progressBar1.Size = new Size(1095, 23);
        progressBar1.TabIndex = 5;
        // 
        // dataGridView1
        // 
        dataGridView1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
        dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        dataGridView1.Location = new Point(12, 99);
        dataGridView1.Name = "dataGridView1";
        dataGridView1.Size = new Size(537, 708);
        dataGridView1.TabIndex = 6;
        // 
        // cbChanged
        // 
        cbChanged.AutoSize = true;
        cbChanged.Checked = true;
        cbChanged.CheckState = CheckState.Checked;
        cbChanged.Location = new Point(111, 73);
        cbChanged.Name = "cbChanged";
        cbChanged.Size = new Size(102, 19);
        cbChanged.TabIndex = 10;
        cbChanged.Text = "Changed Only";
        cbChanged.UseVisualStyleBackColor = true;
        // 
        // tableLayoutPanel2
        // 
        tableLayoutPanel2.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        tableLayoutPanel2.ColumnCount = 1;
        tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        tableLayoutPanel2.Controls.Add(pictureBox1, 0, 0);
        tableLayoutPanel2.Controls.Add(pictureBox2, 0, 1);
        tableLayoutPanel2.Controls.Add(pictureBox3, 0, 2);
        tableLayoutPanel2.Location = new Point(555, 99);
        tableLayoutPanel2.Name = "tableLayoutPanel2";
        tableLayoutPanel2.RowCount = 3;
        tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 33.3333321F));
        tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 33.3333321F));
        tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 33.3333321F));
        tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
        tableLayoutPanel2.Size = new Size(759, 708);
        tableLayoutPanel2.TabIndex = 11;
        // 
        // pictureBox1
        // 
        pictureBox1.BackColor = SystemColors.ControlDark;
        pictureBox1.Dock = DockStyle.Fill;
        pictureBox1.Location = new Point(3, 3);
        pictureBox1.Name = "pictureBox1";
        pictureBox1.Size = new Size(753, 230);
        pictureBox1.SizeMode = PictureBoxSizeMode.AutoSize;
        pictureBox1.TabIndex = 0;
        pictureBox1.TabStop = false;
        // 
        // pictureBox2
        // 
        pictureBox2.BackColor = SystemColors.ControlDark;
        pictureBox2.Dock = DockStyle.Fill;
        pictureBox2.Location = new Point(3, 239);
        pictureBox2.Name = "pictureBox2";
        pictureBox2.Size = new Size(753, 230);
        pictureBox2.SizeMode = PictureBoxSizeMode.AutoSize;
        pictureBox2.TabIndex = 1;
        pictureBox2.TabStop = false;
        // 
        // pictureBox3
        // 
        pictureBox3.BackColor = SystemColors.ControlDark;
        pictureBox3.Dock = DockStyle.Fill;
        pictureBox3.Location = new Point(3, 475);
        pictureBox3.Name = "pictureBox3";
        pictureBox3.Size = new Size(753, 230);
        pictureBox3.SizeMode = PictureBoxSizeMode.AutoSize;
        pictureBox3.TabIndex = 2;
        pictureBox3.TabStop = false;
        // 
        // CollectionCompareForm
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(1326, 819);
        Controls.Add(tableLayoutPanel2);
        Controls.Add(cbChanged);
        Controls.Add(dataGridView1);
        Controls.Add(progressBar1);
        Controls.Add(tbAfter);
        Controls.Add(tbBefore);
        Controls.Add(btnAnalyze);
        Name = "CollectionCompareForm";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Image Collection Comparison";
        ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
        tableLayoutPanel2.ResumeLayout(false);
        tableLayoutPanel2.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
        ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
        ((System.ComponentModel.ISupportInitialize)pictureBox3).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private Button button1;
    private Button button2;
    private Button btnAnalyze;
    private TextBox tbBefore;
    private TextBox tbAfter;
    private ProgressBar progressBar1;
    private DataGridView dataGridView1;
    private CheckBox cbChanged;
    private TableLayoutPanel tableLayoutPanel2;
    private PictureBox pictureBox1;
    private PictureBox pictureBox2;
    private PictureBox pictureBox3;
}
