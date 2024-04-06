namespace WinForms_Demo.Demos;

partial class ImageBackgrounds
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
        cbData = new CheckBox();
        cbFigure = new CheckBox();
        label1 = new Label();
        cbMode = new ComboBox();
        formsPlot1 = new ScottPlot.WinForms.FormsPlot();
        SuspendLayout();
        // 
        // cbData
        // 
        cbData.AutoSize = true;
        cbData.Checked = true;
        cbData.CheckState = CheckState.Checked;
        cbData.Location = new Point(22, 25);
        cbData.Name = "cbData";
        cbData.Size = new Size(117, 19);
        cbData.TabIndex = 0;
        cbData.Text = "Data Background";
        cbData.UseVisualStyleBackColor = true;
        // 
        // cbFigure
        // 
        cbFigure.AutoSize = true;
        cbFigure.Checked = true;
        cbFigure.CheckState = CheckState.Checked;
        cbFigure.Location = new Point(167, 25);
        cbFigure.Name = "cbFigure";
        cbFigure.Size = new Size(126, 19);
        cbFigure.TabIndex = 1;
        cbFigure.Text = "Figure Background";
        cbFigure.UseVisualStyleBackColor = true;
        // 
        // label1
        // 
        label1.AutoSize = true;
        label1.Location = new Point(326, 26);
        label1.Name = "label1";
        label1.Size = new Size(50, 15);
        label1.TabIndex = 2;
        label1.Text = "Position";
        // 
        // cbMode
        // 
        cbMode.DropDownStyle = ComboBoxStyle.DropDownList;
        cbMode.FormattingEnabled = true;
        cbMode.Location = new Point(382, 23);
        cbMode.Name = "cbMode";
        cbMode.Size = new Size(121, 23);
        cbMode.TabIndex = 3;
        // 
        // formsPlot1
        // 
        formsPlot1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        formsPlot1.BorderStyle = BorderStyle.Fixed3D;
        formsPlot1.DisplayScale = 1F;
        formsPlot1.Location = new Point(12, 62);
        formsPlot1.Name = "formsPlot1";
        formsPlot1.Size = new Size(672, 367);
        formsPlot1.TabIndex = 4;
        // 
        // ImageBackgrounds
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(696, 441);
        Controls.Add(formsPlot1);
        Controls.Add(cbMode);
        Controls.Add(label1);
        Controls.Add(cbFigure);
        Controls.Add(cbData);
        Name = "ImageBackgrounds";
        Text = "Background Image Demo";
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private CheckBox cbData;
    private CheckBox cbFigure;
    private Label label1;
    private ComboBox cbMode;
    private ScottPlot.WinForms.FormsPlot formsPlot1;
}