namespace WinForms_Demo.Demos;

partial class MultiplotAdvancedLayout
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
        formsPlot1 = new ScottPlot.WinForms.FormsPlot();
        btnMultiColumnSpan = new Button();
        btnRows = new Button();
        btnColumns = new Button();
        btnGrid = new Button();
        btnPixelSizing = new Button();
        SuspendLayout();
        // 
        // formsPlot1
        // 
        formsPlot1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        formsPlot1.DisplayScale = 1F;
        formsPlot1.Location = new Point(0, 49);
        formsPlot1.Name = "formsPlot1";
        formsPlot1.Size = new Size(800, 401);
        formsPlot1.TabIndex = 4;
        // 
        // btnMultiColumnSpan
        // 
        btnMultiColumnSpan.Location = new Point(223, 12);
        btnMultiColumnSpan.Name = "btnMultiColumnSpan";
        btnMultiColumnSpan.Size = new Size(137, 31);
        btnMultiColumnSpan.TabIndex = 8;
        btnMultiColumnSpan.Text = "Multi-Column Span";
        btnMultiColumnSpan.UseVisualStyleBackColor = true;
        // 
        // btnRows
        // 
        btnRows.Location = new Point(12, 12);
        btnRows.Name = "btnRows";
        btnRows.Size = new Size(66, 31);
        btnRows.TabIndex = 9;
        btnRows.Text = "Rows";
        btnRows.UseVisualStyleBackColor = true;
        // 
        // btnColumns
        // 
        btnColumns.Location = new Point(84, 12);
        btnColumns.Name = "btnColumns";
        btnColumns.Size = new Size(70, 31);
        btnColumns.TabIndex = 10;
        btnColumns.Text = "Columns";
        btnColumns.UseVisualStyleBackColor = true;
        // 
        // btnGrid
        // 
        btnGrid.Location = new Point(160, 12);
        btnGrid.Name = "btnGrid";
        btnGrid.Size = new Size(57, 31);
        btnGrid.TabIndex = 11;
        btnGrid.Text = "Grid";
        btnGrid.UseVisualStyleBackColor = true;
        // 
        // btnPixelSizing
        // 
        btnPixelSizing.Location = new Point(366, 12);
        btnPixelSizing.Name = "btnPixelSizing";
        btnPixelSizing.Size = new Size(91, 31);
        btnPixelSizing.TabIndex = 12;
        btnPixelSizing.Text = "Pixel Sizing";
        btnPixelSizing.UseVisualStyleBackColor = true;
        // 
        // MultiplotAdvancedLayout
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(800, 450);
        Controls.Add(btnPixelSizing);
        Controls.Add(btnGrid);
        Controls.Add(btnColumns);
        Controls.Add(btnRows);
        Controls.Add(btnMultiColumnSpan);
        Controls.Add(formsPlot1);
        Name = "MultiplotAdvancedLayout";
        Text = "MultiplotAdvancedLayout";
        ResumeLayout(false);
    }

    #endregion

    private ScottPlot.WinForms.FormsPlot formsPlot1;
    private Button btnMultiColumnSpan;
    private Button btnRows;
    private Button btnColumns;
    private Button btnGrid;
    private Button btnPixelSizing;
}