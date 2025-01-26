namespace WinForms_Demo.Demos;

partial class MultiplotDraggable
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
        btnAddRow = new Button();
        btnDeleteRow = new Button();
        SuspendLayout();
        // 
        // formsPlot1
        // 
        formsPlot1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        formsPlot1.DisplayScale = 1F;
        formsPlot1.Location = new Point(12, 55);
        formsPlot1.Name = "formsPlot1";
        formsPlot1.Size = new Size(992, 671);
        formsPlot1.TabIndex = 7;
        // 
        // btnAddRow
        // 
        btnAddRow.Location = new Point(12, 12);
        btnAddRow.Name = "btnAddRow";
        btnAddRow.Size = new Size(96, 37);
        btnAddRow.TabIndex = 8;
        btnAddRow.Text = "Add Row";
        btnAddRow.UseVisualStyleBackColor = true;
        // 
        // btnDeleteRow
        // 
        btnDeleteRow.Location = new Point(114, 12);
        btnDeleteRow.Name = "btnDeleteRow";
        btnDeleteRow.Size = new Size(96, 37);
        btnDeleteRow.TabIndex = 9;
        btnDeleteRow.Text = "Delete Row";
        btnDeleteRow.UseVisualStyleBackColor = true;
        // 
        // MultiplotDraggable
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(1016, 738);
        Controls.Add(btnDeleteRow);
        Controls.Add(btnAddRow);
        Controls.Add(formsPlot1);
        Name = "MultiplotDraggable";
        Text = "MultiplotCollapsed";
        ResumeLayout(false);
    }

    #endregion

    private ScottPlot.WinForms.FormsPlot formsPlot1;
    private Button btnAddRow;
    private Button btnDeleteRow;
}