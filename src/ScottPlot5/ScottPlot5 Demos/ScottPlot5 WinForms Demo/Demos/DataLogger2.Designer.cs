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
        splitContainer = new SplitContainer();
        loggerPlotHorz = new DataLoggerCtrl();
        loggerPlotVert = new DataLoggerCtrl();
        cbRunning = new CheckBox();
        ((System.ComponentModel.ISupportInitialize)splitContainer).BeginInit();
        splitContainer.Panel1.SuspendLayout();
        splitContainer.Panel2.SuspendLayout();
        splitContainer.SuspendLayout();
        SuspendLayout();
        // 
        // splitContainer
        // 
        splitContainer.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        splitContainer.Location = new Point(12, 37);
        splitContainer.Name = "splitContainer";
        // 
        // splitContainer.Panel1
        // 
        splitContainer.Panel1.Controls.Add(loggerPlotHorz);
        // 
        // splitContainer.Panel2
        // 
        splitContainer.Panel2.Controls.Add(loggerPlotVert);
        splitContainer.Size = new Size(1072, 580);
        splitContainer.SplitterDistance = 536;
        splitContainer.TabIndex = 3;
        // 
        // loggerPlotHorz
        // 
        loggerPlotHorz.Dock = DockStyle.Fill;
        loggerPlotHorz.Location = new Point(0, 0);
        loggerPlotHorz.Name = "loggerPlotHorz";
        loggerPlotHorz.Rotated = false;
        loggerPlotHorz.Size = new Size(536, 580);
        loggerPlotHorz.TabIndex = 1;
        // 
        // loggerPlotVert
        // 
        loggerPlotVert.Dock = DockStyle.Fill;
        loggerPlotVert.Location = new Point(0, 0);
        loggerPlotVert.Name = "loggerPlotVert";
        loggerPlotVert.Rotated = true;
        loggerPlotVert.Size = new Size(532, 580);
        loggerPlotVert.TabIndex = 2;
        // 
        // cbRunning
        // 
        cbRunning.AutoSize = true;
        cbRunning.Checked = true;
        cbRunning.CheckState = CheckState.Checked;
        cbRunning.Location = new Point(26, 12);
        cbRunning.Name = "cbRunning";
        cbRunning.Size = new Size(71, 19);
        cbRunning.TabIndex = 0;
        cbRunning.Text = "Running";
        cbRunning.UseVisualStyleBackColor = true;
        cbRunning.CheckedChanged += cbRunning_CheckedChanged;
        // 
        // DataLogger2
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(1096, 629);
        Controls.Add(cbRunning);
        Controls.Add(splitContainer);
        Name = "DataLogger2";
        Text = "DataLogger2";
        splitContainer.Panel1.ResumeLayout(false);
        splitContainer.Panel2.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)splitContainer).EndInit();
        splitContainer.ResumeLayout(false);
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion
    private SplitContainer splitContainer;
    private CheckBox cbRunning;
    private DataLoggerCtrl loggerPlotHorz;
    private DataLoggerCtrl loggerPlotVert;
}
