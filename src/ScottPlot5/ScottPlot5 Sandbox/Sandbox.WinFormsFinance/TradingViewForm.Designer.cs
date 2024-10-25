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
        label1 = new Label();
        label2 = new Label();
        buttonClearAll = new Button();
        checkBoxAddLine = new CheckBox();
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
        // buttonClearAll
        // 
        buttonClearAll.Location = new Point(12, 168);
        buttonClearAll.Name = "buttonClearAll";
        buttonClearAll.Size = new Size(75, 31);
        buttonClearAll.TabIndex = 12;
        buttonClearAll.Text = "Clear All";
        buttonClearAll.UseVisualStyleBackColor = true;
        // 
        // checkBoxAddLine
        // 
        checkBoxAddLine.Appearance = Appearance.Button;
        checkBoxAddLine.Location = new Point(12, 131);
        checkBoxAddLine.Name = "checkBoxAddLine";
        checkBoxAddLine.Size = new Size(75, 31);
        checkBoxAddLine.TabIndex = 14;
        checkBoxAddLine.Text = "Add Line";
        checkBoxAddLine.TextAlign = ContentAlignment.MiddleCenter;
        checkBoxAddLine.UseVisualStyleBackColor = true;
        // 
        // TradingViewForm
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(800, 450);
        Controls.Add(checkBoxAddLine);
        Controls.Add(buttonClearAll);
        Controls.Add(label2);
        Controls.Add(label1);
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
    private Label label1;
    private Label label2;
    private Button buttonClearAll;
    private CheckBox checkBoxAddLine;
}
