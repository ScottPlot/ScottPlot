namespace Sandbox.WinFormsFinance;

partial class TradingViewForm
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
        comboBoxSymbol = new ComboBox();
        comboBoxInterval = new ComboBox();
        formsPlot1 = new ScottPlot.WinForms.FormsPlot();
        button1 = new Button();
        label1 = new Label();
        label2 = new Label();
        label3 = new Label();
        button2 = new Button();
        SuspendLayout();
        // 
        // comboBoxSymbol
        // 
        comboBoxSymbol.DropDownStyle = ComboBoxStyle.DropDownList;
        comboBoxSymbol.FormattingEnabled = true;
        comboBoxSymbol.Location = new Point(12, 30);
        comboBoxSymbol.Name = "comboBoxSymbol";
        comboBoxSymbol.Size = new Size(121, 23);
        comboBoxSymbol.TabIndex = 0;
        // 
        // comboBoxInterval
        // 
        comboBoxInterval.DropDownStyle = ComboBoxStyle.DropDownList;
        comboBoxInterval.FormattingEnabled = true;
        comboBoxInterval.Location = new Point(12, 86);
        comboBoxInterval.Name = "comboBoxInterval";
        comboBoxInterval.Size = new Size(121, 23);
        comboBoxInterval.TabIndex = 3;
        // 
        // formsPlot1
        // 
        formsPlot1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        formsPlot1.DisplayScale = 1F;
        formsPlot1.Location = new Point(139, 12);
        formsPlot1.Name = "formsPlot1";
        formsPlot1.Size = new Size(649, 426);
        formsPlot1.TabIndex = 7;
        // 
        // button1
        // 
        button1.Location = new Point(12, 150);
        button1.Name = "button1";
        button1.Size = new Size(45, 23);
        button1.TabIndex = 8;
        button1.Text = "Line";
        button1.UseVisualStyleBackColor = true;
        // 
        // label1
        // 
        label1.AutoSize = true;
        label1.Location = new Point(12, 12);
        label1.Name = "label1";
        label1.Size = new Size(50, 15);
        label1.TabIndex = 9;
        label1.Text = "Symbol:";
        // 
        // label2
        // 
        label2.AutoSize = true;
        label2.Location = new Point(12, 68);
        label2.Name = "label2";
        label2.Size = new Size(49, 15);
        label2.TabIndex = 10;
        label2.Text = "Interval:";
        // 
        // label3
        // 
        label3.AutoSize = true;
        label3.Location = new Point(12, 132);
        label3.Name = "label3";
        label3.Size = new Size(32, 15);
        label3.TabIndex = 11;
        label3.Text = "Add:";
        // 
        // button2
        // 
        button2.Enabled = false;
        button2.Location = new Point(63, 150);
        button2.Name = "button2";
        button2.Size = new Size(40, 23);
        button2.TabIndex = 12;
        button2.Text = "Ray";
        button2.UseVisualStyleBackColor = true;
        // 
        // TradingViewForm
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(800, 450);
        Controls.Add(button2);
        Controls.Add(label3);
        Controls.Add(label2);
        Controls.Add(label1);
        Controls.Add(button1);
        Controls.Add(formsPlot1);
        Controls.Add(comboBoxInterval);
        Controls.Add(comboBoxSymbol);
        Name = "TradingViewForm";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "ScottPlot Financial Charting Sandbox (TradingView)";
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private ComboBox comboBoxSymbol;
    private ComboBox comboBoxInterval;
    private ScottPlot.WinForms.FormsPlot formsPlot1;
    private Button button1;
    private Label label1;
    private Label label2;
    private Label label3;
    private Button button2;
}
