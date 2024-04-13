namespace ScottPlot
{
    partial class FormsPlotViewer
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
            SuspendLayout();
            // 
            // formsPlot1
            // 
            formsPlot1.Dock = System.Windows.Forms.DockStyle.Fill;
            formsPlot1.Location = new System.Drawing.Point(0, 0);
            formsPlot1.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            formsPlot1.Name = "formsPlot1";
            formsPlot1.Size = new System.Drawing.Size(681, 417);
            formsPlot1.TabIndex = 0;
            // 
            // FormsPlotViewer
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(681, 417);
            Controls.Add(formsPlot1);
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Name = "FormsPlotViewer";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Text = "FormsPlotViewer";
            ResumeLayout(false);
        }

        #endregion

        public FormsPlot formsPlot1;
    }
}