namespace Sandbox.WinForms;

partial class Form1
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
            this.formsPlot1 = new ScottPlot.WinForms.FormsPlot();
            this.cbGuides = new System.Windows.Forms.CheckBox();
            this.btnAddX = new System.Windows.Forms.Button();
            this.btnRemoveX = new System.Windows.Forms.Button();
            this.btnRemoveY = new System.Windows.Forms.Button();
            this.btnAddY = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // formsPlot1
            // 
            this.formsPlot1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.formsPlot1.Location = new System.Drawing.Point(12, 70);
            this.formsPlot1.Name = "formsPlot1";
            this.formsPlot1.Size = new System.Drawing.Size(776, 368);
            this.formsPlot1.TabIndex = 0;
            // 
            // cbGuides
            // 
            this.cbGuides.AutoSize = true;
            this.cbGuides.Location = new System.Drawing.Point(12, 12);
            this.cbGuides.Name = "cbGuides";
            this.cbGuides.Size = new System.Drawing.Size(62, 19);
            this.cbGuides.TabIndex = 1;
            this.cbGuides.Text = "Guides";
            this.cbGuides.UseVisualStyleBackColor = true;
            this.cbGuides.CheckedChanged += new System.EventHandler(this.cbGuides_CheckedChanged);
            // 
            // btnAddX
            // 
            this.btnAddX.Location = new System.Drawing.Point(80, 12);
            this.btnAddX.Name = "btnAddX";
            this.btnAddX.Size = new System.Drawing.Size(75, 23);
            this.btnAddX.TabIndex = 2;
            this.btnAddX.Text = "Add X";
            this.btnAddX.UseVisualStyleBackColor = true;
            this.btnAddX.Click += new System.EventHandler(this.btnAddX_Click);
            // 
            // btnRemoveX
            // 
            this.btnRemoveX.Location = new System.Drawing.Point(80, 41);
            this.btnRemoveX.Name = "btnRemoveX";
            this.btnRemoveX.Size = new System.Drawing.Size(75, 23);
            this.btnRemoveX.TabIndex = 3;
            this.btnRemoveX.Text = "Remove X";
            this.btnRemoveX.UseVisualStyleBackColor = true;
            this.btnRemoveX.Click += new System.EventHandler(this.btnRemoveX_Click);
            // 
            // btnRemoveY
            // 
            this.btnRemoveY.Location = new System.Drawing.Point(161, 41);
            this.btnRemoveY.Name = "btnRemoveY";
            this.btnRemoveY.Size = new System.Drawing.Size(75, 23);
            this.btnRemoveY.TabIndex = 5;
            this.btnRemoveY.Text = "Remove Y";
            this.btnRemoveY.UseVisualStyleBackColor = true;
            this.btnRemoveY.Click += new System.EventHandler(this.btnRemoveY_Click);
            // 
            // btnAddY
            // 
            this.btnAddY.Location = new System.Drawing.Point(161, 12);
            this.btnAddY.Name = "btnAddY";
            this.btnAddY.Size = new System.Drawing.Size(75, 23);
            this.btnAddY.TabIndex = 4;
            this.btnAddY.Text = "Add Y";
            this.btnAddY.UseVisualStyleBackColor = true;
            this.btnAddY.Click += new System.EventHandler(this.btnAddY_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btnRemoveY);
            this.Controls.Add(this.btnAddY);
            this.Controls.Add(this.btnRemoveX);
            this.Controls.Add(this.btnAddX);
            this.Controls.Add(this.cbGuides);
            this.Controls.Add(this.formsPlot1);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ScottPlot 5 - Windows Forms Sandbox";
            this.ResumeLayout(false);
            this.PerformLayout();

    }

    #endregion

    private ScottPlot.WinForms.FormsPlot formsPlot1;
    private CheckBox cbGuides;
    private Button btnAddX;
    private Button btnRemoveX;
    private Button btnRemoveY;
    private Button btnAddY;
}
