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
        imageComparer1 = new GraphicalTestRunner.ImageComparer();
        btnCB = new Button();
        btnUT = new Button();
        btn2 = new Button();
        btn1 = new Button();
        splitContainer1 = new SplitContainer();
        btnHelp = new Button();
        checkHideUnchanged = new CheckBox();
        ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
        ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
        splitContainer1.Panel1.SuspendLayout();
        splitContainer1.Panel2.SuspendLayout();
        splitContainer1.SuspendLayout();
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
        tbBefore.Size = new Size(748, 23);
        tbBefore.TabIndex = 3;
        // 
        // tbAfter
        // 
        tbAfter.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        tbAfter.Location = new Point(12, 41);
        tbAfter.Name = "tbAfter";
        tbAfter.Size = new Size(748, 23);
        tbAfter.TabIndex = 4;
        // 
        // progressBar1
        // 
        progressBar1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        progressBar1.Location = new Point(111, 70);
        progressBar1.Name = "progressBar1";
        progressBar1.Size = new Size(436, 23);
        progressBar1.TabIndex = 5;
        // 
        // dataGridView1
        // 
        dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        dataGridView1.Dock = DockStyle.Fill;
        dataGridView1.Location = new Point(0, 0);
        dataGridView1.Name = "dataGridView1";
        dataGridView1.Size = new Size(791, 291);
        dataGridView1.TabIndex = 6;
        // 
        // imageComparer1
        // 
        imageComparer1.Dock = DockStyle.Fill;
        imageComparer1.Location = new Point(0, 0);
        imageComparer1.Name = "imageComparer1";
        imageComparer1.Size = new Size(791, 294);
        imageComparer1.TabIndex = 1;
        // 
        // btnCB
        // 
        btnCB.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnCB.Location = new Point(766, 70);
        btnCB.Name = "btnCB";
        btnCB.Size = new Size(37, 23);
        btnCB.TabIndex = 12;
        btnCB.Text = "CB";
        btnCB.UseVisualStyleBackColor = true;
        // 
        // btnUT
        // 
        btnUT.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnUT.Location = new Point(723, 70);
        btnUT.Name = "btnUT";
        btnUT.Size = new Size(37, 23);
        btnUT.TabIndex = 13;
        btnUT.Text = "UT";
        btnUT.UseVisualStyleBackColor = true;
        // 
        // btn2
        // 
        btn2.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btn2.Location = new Point(766, 41);
        btn2.Name = "btn2";
        btn2.Size = new Size(37, 23);
        btn2.TabIndex = 14;
        btn2.Text = "2";
        btn2.UseVisualStyleBackColor = true;
        // 
        // btn1
        // 
        btn1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btn1.Location = new Point(766, 11);
        btn1.Name = "btn1";
        btn1.Size = new Size(37, 23);
        btn1.TabIndex = 15;
        btn1.Text = "1";
        btn1.UseVisualStyleBackColor = true;
        // 
        // splitContainer1
        // 
        splitContainer1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        splitContainer1.Location = new Point(12, 99);
        splitContainer1.Name = "splitContainer1";
        splitContainer1.Orientation = Orientation.Horizontal;
        // 
        // splitContainer1.Panel1
        // 
        splitContainer1.Panel1.Controls.Add(imageComparer1);
        // 
        // splitContainer1.Panel2
        // 
        splitContainer1.Panel2.Controls.Add(dataGridView1);
        splitContainer1.Size = new Size(791, 589);
        splitContainer1.SplitterDistance = 294;
        splitContainer1.TabIndex = 16;
        // 
        // btnHelp
        // 
        btnHelp.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnHelp.Location = new Point(674, 70);
        btnHelp.Name = "btnHelp";
        btnHelp.Size = new Size(43, 23);
        btnHelp.TabIndex = 2;
        btnHelp.Text = "Help";
        btnHelp.UseVisualStyleBackColor = true;
        // 
        // checkHideUnchanged
        // 
        checkHideUnchanged.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        checkHideUnchanged.AutoSize = true;
        checkHideUnchanged.Checked = true;
        checkHideUnchanged.CheckState = CheckState.Checked;
        checkHideUnchanged.Location = new Point(553, 73);
        checkHideUnchanged.Name = "checkHideUnchanged";
        checkHideUnchanged.Size = new Size(115, 19);
        checkHideUnchanged.TabIndex = 2;
        checkHideUnchanged.Text = "Hide Unchanged";
        checkHideUnchanged.UseVisualStyleBackColor = true;
        // 
        // CollectionCompareForm
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(815, 700);
        Controls.Add(checkHideUnchanged);
        Controls.Add(btnHelp);
        Controls.Add(splitContainer1);
        Controls.Add(btn1);
        Controls.Add(btn2);
        Controls.Add(btnUT);
        Controls.Add(btnCB);
        Controls.Add(progressBar1);
        Controls.Add(tbAfter);
        Controls.Add(tbBefore);
        Controls.Add(btnAnalyze);
        Name = "CollectionCompareForm";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Image Collection Comparison";
        ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
        splitContainer1.Panel1.ResumeLayout(false);
        splitContainer1.Panel2.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
        splitContainer1.ResumeLayout(false);
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private Button btnAnalyze;
    private TextBox tbBefore;
    private TextBox tbAfter;
    private ProgressBar progressBar1;
    private DataGridView dataGridView1;
    private Button btnCB;
    private Button btnUT;
    private Button btn2;
    private Button btn1;
    private GraphicalTestRunner.ImageComparer imageComparer1;
    private SplitContainer splitContainer1;
    private Button btnHelp;
    private CheckBox checkHideUnchanged;
}
