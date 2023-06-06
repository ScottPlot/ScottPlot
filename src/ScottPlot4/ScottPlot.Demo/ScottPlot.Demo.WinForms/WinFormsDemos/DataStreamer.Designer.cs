namespace ScottPlot.Demo.WinForms.WinFormsDemos;

partial class DataStreamer
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
        formsPlot1 = new FormsPlot();
        btnWipeRight = new System.Windows.Forms.Button();
        btnWipeLeft = new System.Windows.Forms.Button();
        btnScrollRight = new System.Windows.Forms.Button();
        btnScrollLeft = new System.Windows.Forms.Button();
        label1 = new System.Windows.Forms.Label();
        label2 = new System.Windows.Forms.Label();
        SuspendLayout();
        // 
        // formsPlot1
        // 
        formsPlot1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
        formsPlot1.Location = new System.Drawing.Point(13, 50);
        formsPlot1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
        formsPlot1.Name = "formsPlot1";
        formsPlot1.Size = new System.Drawing.Size(774, 388);
        formsPlot1.TabIndex = 0;
        // 
        // btnWipeRight
        // 
        btnWipeRight.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
        btnWipeRight.Location = new System.Drawing.Point(470, 7);
        btnWipeRight.Name = "btnWipeRight";
        btnWipeRight.Size = new System.Drawing.Size(75, 37);
        btnWipeRight.TabIndex = 1;
        btnWipeRight.Text = "Wipe Right";
        btnWipeRight.UseVisualStyleBackColor = true;
        btnWipeRight.Click += btnWipeRight_Click;
        // 
        // btnWipeLeft
        // 
        btnWipeLeft.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
        btnWipeLeft.Location = new System.Drawing.Point(551, 7);
        btnWipeLeft.Name = "btnWipeLeft";
        btnWipeLeft.Size = new System.Drawing.Size(75, 37);
        btnWipeLeft.TabIndex = 2;
        btnWipeLeft.Text = "Wipe Left";
        btnWipeLeft.UseVisualStyleBackColor = true;
        btnWipeLeft.Click += btnWipeLeft_Click;
        // 
        // btnScrollRight
        // 
        btnScrollRight.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
        btnScrollRight.Location = new System.Drawing.Point(632, 7);
        btnScrollRight.Name = "btnScrollRight";
        btnScrollRight.Size = new System.Drawing.Size(75, 37);
        btnScrollRight.TabIndex = 3;
        btnScrollRight.Text = "Scroll Right";
        btnScrollRight.UseVisualStyleBackColor = true;
        btnScrollRight.Click += btnScrollRight_Click;
        // 
        // btnScrollLeft
        // 
        btnScrollLeft.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
        btnScrollLeft.Location = new System.Drawing.Point(713, 7);
        btnScrollLeft.Name = "btnScrollLeft";
        btnScrollLeft.Size = new System.Drawing.Size(75, 37);
        btnScrollLeft.TabIndex = 4;
        btnScrollLeft.Text = "Scroll Left";
        btnScrollLeft.UseVisualStyleBackColor = true;
        btnScrollLeft.Click += btnScrollLeft_Click;
        // 
        // label1
        // 
        label1.AutoSize = true;
        label1.Location = new System.Drawing.Point(445, 29);
        label1.Name = "label1";
        label1.Size = new System.Drawing.Size(0, 15);
        label1.TabIndex = 5;
        // 
        // label2
        // 
        label2.AutoSize = true;
        label2.Location = new System.Drawing.Point(13, 18);
        label2.Name = "label2";
        label2.Size = new System.Drawing.Size(405, 15);
        label2.TabIndex = 8;
        label2.Text = "The DataStreamer plot type displays the latest N points of a live data source.";
        // 
        // DataStreamer
        // 
        AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
        AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        ClientSize = new System.Drawing.Size(800, 450);
        Controls.Add(label2);
        Controls.Add(label1);
        Controls.Add(btnScrollLeft);
        Controls.Add(btnScrollRight);
        Controls.Add(btnWipeLeft);
        Controls.Add(btnWipeRight);
        Controls.Add(formsPlot1);
        Name = "DataStreamer";
        StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
        Text = "DataStreamer Demo";
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private FormsPlot formsPlot1;
    private System.Windows.Forms.GroupBox groupBox1;
    private System.Windows.Forms.Button btnWipeRight;
    private System.Windows.Forms.Button btnWipeLeft;
    private System.Windows.Forms.Button btnScrollRight;
    private System.Windows.Forms.Button btnScrollLeft;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Label label2;

    /* Unmerged change from project 'WinForms Demo (net6.0-windows)'
    Before:
        private System.Windows.Forms.Button button1;
    After:
        private System.Windows.Forms.Button btnWipe;
        private System.Windows.Forms.Button btnLeft;
        private System.Windows.Forms.Button button3;
    */
}