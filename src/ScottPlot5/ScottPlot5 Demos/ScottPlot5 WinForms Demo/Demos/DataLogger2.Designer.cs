namespace WinForms_Demo.Demos;

partial class DataLogger2
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
        btnJump = new Button();
        btnFull = new Button();
        btnSlide = new Button();
        formsPlotHorz = new ScottPlot.WinForms.FormsPlot();
        formsPlotVert = new ScottPlot.WinForms.FormsPlot();
        splitContainer = new SplitContainer();
        ((System.ComponentModel.ISupportInitialize)splitContainer).BeginInit();
        splitContainer.Panel1.SuspendLayout();
        splitContainer.Panel2.SuspendLayout();
        splitContainer.SuspendLayout();
        SuspendLayout();
        // 
        // btnJump
        // 
        btnJump.Location = new Point(106, 12);
        btnJump.Name = "btnJump";
        btnJump.Size = new Size(88, 34);
        btnJump.TabIndex = 9;
        btnJump.Text = "Jump";
        btnJump.UseVisualStyleBackColor = true;
        // 
        // btnFull
        // 
        btnFull.Location = new Point(12, 12);
        btnFull.Name = "btnFull";
        btnFull.Size = new Size(88, 34);
        btnFull.TabIndex = 8;
        btnFull.Text = "Full";
        btnFull.UseVisualStyleBackColor = true;
        // 
        // btnSlide
        // 
        btnSlide.Location = new Point(200, 12);
        btnSlide.Name = "btnSlide";
        btnSlide.Size = new Size(88, 34);
        btnSlide.TabIndex = 10;
        btnSlide.Text = "Slide";
        btnSlide.UseVisualStyleBackColor = true;
        // 
        // formsPlotHorz
        // 
        formsPlotHorz.DisplayScale = 1F;
        formsPlotHorz.Dock = DockStyle.Fill;
        formsPlotHorz.Location = new Point(0, 0);
        formsPlotHorz.Name = "formsPlotHorz";
        formsPlotHorz.Size = new Size(536, 565);
        formsPlotHorz.TabIndex = 6;
        // 
        // formsPlotVert
        // 
        formsPlotVert.DisplayScale = 1F;
        formsPlotVert.Dock = DockStyle.Fill;
        formsPlotVert.Location = new Point(0, 0);
        formsPlotVert.Name = "formsPlotVert";
        formsPlotVert.Size = new Size(532, 565);
        formsPlotVert.TabIndex = 7;
        // 
        // splitContainer
        // 
        splitContainer.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        splitContainer.Location = new Point(12, 52);
        splitContainer.Name = "splitContainer";
        // 
        // splitContainer.Panel1
        // 
        splitContainer.Panel1.Controls.Add(formsPlotHorz);
        // 
        // splitContainer.Panel2
        // 
        splitContainer.Panel2.Controls.Add(formsPlotVert);
        splitContainer.Size = new Size(1072, 565);
        splitContainer.SplitterDistance = 536;
        splitContainer.TabIndex = 12;
        // 
        // DataLogger2
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(1096, 629);
        Controls.Add(splitContainer);
        Controls.Add(btnSlide);
        Controls.Add(btnJump);
        Controls.Add(btnFull);
        Name = "DataLogger2";
        Text = "DataLogger2";
        splitContainer.Panel1.ResumeLayout(false);
        splitContainer.Panel2.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)splitContainer).EndInit();
        splitContainer.ResumeLayout(false);
        ResumeLayout(false);
    }

    #endregion

    private Button btnJump;
    private Button btnFull;
    private Button btnSlide;
    private ScottPlot.WinForms.FormsPlot formsPlotHorz;
    private ScottPlot.WinForms.FormsPlot formsPlotVert;
    private SplitContainer splitContainer;
}
