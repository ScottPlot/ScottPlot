namespace WinForms.Examples
{
    partial class MultipleAxes
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
            this.formsPlot1 = new ScottPlot.WinForms.FormsPlot();
            this.btnAddAxis = new System.Windows.Forms.Button();
            this.btnRemoveAxis = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // formsPlot1
            // 
            this.formsPlot1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.formsPlot1.Location = new System.Drawing.Point(12, 41);
            this.formsPlot1.Name = "formsPlot1";
            this.formsPlot1.Size = new System.Drawing.Size(1003, 632);
            this.formsPlot1.TabIndex = 0;
            // 
            // btnAddAxis
            // 
            this.btnAddAxis.Location = new System.Drawing.Point(12, 12);
            this.btnAddAxis.Name = "btnAddAxis";
            this.btnAddAxis.Size = new System.Drawing.Size(75, 23);
            this.btnAddAxis.TabIndex = 1;
            this.btnAddAxis.Text = "Add";
            this.btnAddAxis.UseVisualStyleBackColor = true;
            // 
            // btnRemoveAxis
            // 
            this.btnRemoveAxis.Location = new System.Drawing.Point(93, 12);
            this.btnRemoveAxis.Name = "btnRemoveAxis";
            this.btnRemoveAxis.Size = new System.Drawing.Size(75, 23);
            this.btnRemoveAxis.TabIndex = 2;
            this.btnRemoveAxis.Text = "Remove";
            this.btnRemoveAxis.UseVisualStyleBackColor = true;
            // 
            // MultipleAxes
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1027, 685);
            this.Controls.Add(this.btnRemoveAxis);
            this.Controls.Add(this.btnAddAxis);
            this.Controls.Add(this.formsPlot1);
            this.Name = "MultipleAxes";
            this.Text = "ScottPlot 5: Multiple Axes";
            this.ResumeLayout(false);

        }

        #endregion

        private ScottPlot.WinForms.FormsPlot formsPlot1;
        private System.Windows.Forms.Button btnAddAxis;
        private System.Windows.Forms.Button btnRemoveAxis;
    }
}