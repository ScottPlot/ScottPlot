namespace ScottPlotDemos
{
    partial class FormDraggableAxisLines
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
            this.btnAddHline = new System.Windows.Forms.Button();
            this.btnAddVline = new System.Windows.Forms.Button();
            this.btnClearLines = new System.Windows.Forms.Button();
            this.formsPlot1 = new ScottPlot.FormsPlot();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.btnAddHspan = new System.Windows.Forms.Button();
            this.btnAddVSpan = new System.Windows.Forms.Button();
            this.btnClearSpans = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnAddHline
            // 
            this.btnAddHline.Location = new System.Drawing.Point(12, 12);
            this.btnAddHline.Name = "btnAddHline";
            this.btnAddHline.Size = new System.Drawing.Size(107, 23);
            this.btnAddHline.TabIndex = 1;
            this.btnAddHline.Text = "Add HLine";
            this.btnAddHline.UseVisualStyleBackColor = true;
            this.btnAddHline.Click += new System.EventHandler(this.BtnAddHline_Click);
            // 
            // btnAddVline
            // 
            this.btnAddVline.Location = new System.Drawing.Point(12, 41);
            this.btnAddVline.Name = "btnAddVline";
            this.btnAddVline.Size = new System.Drawing.Size(107, 23);
            this.btnAddVline.TabIndex = 2;
            this.btnAddVline.Text = "Add VLine";
            this.btnAddVline.UseVisualStyleBackColor = true;
            this.btnAddVline.Click += new System.EventHandler(this.BtnAddVline_Click);
            // 
            // btnClearLines
            // 
            this.btnClearLines.Location = new System.Drawing.Point(12, 70);
            this.btnClearLines.Name = "btnClearLines";
            this.btnClearLines.Size = new System.Drawing.Size(107, 23);
            this.btnClearLines.TabIndex = 3;
            this.btnClearLines.Text = "Clear Lines";
            this.btnClearLines.UseVisualStyleBackColor = true;
            this.btnClearLines.Click += new System.EventHandler(this.BtnClearLines_Click);
            // 
            // formsPlot1
            // 
            this.formsPlot1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.formsPlot1.Location = new System.Drawing.Point(242, 12);
            this.formsPlot1.Name = "formsPlot1";
            this.formsPlot1.Size = new System.Drawing.Size(544, 343);
            this.formsPlot1.TabIndex = 5;
            this.formsPlot1.MouseDownOnPlottable += new System.EventHandler(this.ScottPlotUC1_MouseDownOnPlottable);
            this.formsPlot1.MouseDragPlottable += new System.EventHandler(this.ScottPlotUC1_MouseDragPlottable);
            this.formsPlot1.MouseDropPlottable += new System.EventHandler(this.ScottPlotUC1_MouseDropPlottable);
            // 
            // richTextBox1
            // 
            this.richTextBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.richTextBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.richTextBox1.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.richTextBox1.Location = new System.Drawing.Point(12, 99);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(224, 256);
            this.richTextBox1.TabIndex = 4;
            this.richTextBox1.Text = "";
            // 
            // btnAddHspan
            // 
            this.btnAddHspan.Location = new System.Drawing.Point(129, 41);
            this.btnAddHspan.Name = "btnAddHspan";
            this.btnAddHspan.Size = new System.Drawing.Size(107, 23);
            this.btnAddHspan.TabIndex = 6;
            this.btnAddHspan.Text = "Add HSpan";
            this.btnAddHspan.UseVisualStyleBackColor = true;
            this.btnAddHspan.Click += new System.EventHandler(this.BtnAddHspan_Click);
            // 
            // btnAddVSpan
            // 
            this.btnAddVSpan.Location = new System.Drawing.Point(129, 12);
            this.btnAddVSpan.Name = "btnAddVSpan";
            this.btnAddVSpan.Size = new System.Drawing.Size(107, 23);
            this.btnAddVSpan.TabIndex = 7;
            this.btnAddVSpan.Text = "Add VSpan";
            this.btnAddVSpan.UseVisualStyleBackColor = true;
            this.btnAddVSpan.Click += new System.EventHandler(this.BtnAddVSpan_Click);
            // 
            // btnClearSpans
            // 
            this.btnClearSpans.Location = new System.Drawing.Point(129, 70);
            this.btnClearSpans.Name = "btnClearSpans";
            this.btnClearSpans.Size = new System.Drawing.Size(107, 23);
            this.btnClearSpans.TabIndex = 8;
            this.btnClearSpans.Text = "Clear Spans";
            this.btnClearSpans.UseVisualStyleBackColor = true;
            this.btnClearSpans.Click += new System.EventHandler(this.BtnClearSpans_Click);
            // 
            // FormDraggableAxisLines
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(798, 367);
            this.Controls.Add(this.btnClearSpans);
            this.Controls.Add(this.btnAddVSpan);
            this.Controls.Add(this.btnAddHspan);
            this.Controls.Add(this.formsPlot1);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.btnClearLines);
            this.Controls.Add(this.btnAddVline);
            this.Controls.Add(this.btnAddHline);
            this.Name = "FormDraggableAxisLines";
            this.Text = "ScottPlot Draggable Markers";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button btnAddHline;
        private System.Windows.Forms.Button btnAddVline;
        private System.Windows.Forms.Button btnClearLines;
        private ScottPlot.FormsPlot formsPlot1;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.Button btnAddHspan;
        private System.Windows.Forms.Button btnAddVSpan;
        private System.Windows.Forms.Button btnClearSpans;
    }
}

