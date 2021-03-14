
namespace WinFormsFrameworkApp
{
    partial class Form1
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
            this.formsPlot1 = new ScottPlot.FormsPlot();
            this.btnScatter10 = new System.Windows.Forms.Button();
            this.buttonScatter1k = new System.Windows.Forms.Button();
            this.buttonSignal1M = new System.Windows.Forms.Button();
            this.cbRenderQueue = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // formsPlot1
            // 
            this.formsPlot1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.formsPlot1.BackColor = System.Drawing.Color.Transparent;
            this.formsPlot1.Location = new System.Drawing.Point(12, 41);
            this.formsPlot1.Name = "formsPlot1";
            this.formsPlot1.Size = new System.Drawing.Size(776, 397);
            this.formsPlot1.TabIndex = 0;
            // 
            // btnScatter10
            // 
            this.btnScatter10.Location = new System.Drawing.Point(12, 12);
            this.btnScatter10.Name = "btnScatter10";
            this.btnScatter10.Size = new System.Drawing.Size(75, 23);
            this.btnScatter10.TabIndex = 1;
            this.btnScatter10.Text = "10 scatter";
            this.btnScatter10.UseVisualStyleBackColor = true;
            this.btnScatter10.Click += new System.EventHandler(this.btnScatter10_Click);
            // 
            // buttonScatter1k
            // 
            this.buttonScatter1k.Location = new System.Drawing.Point(93, 12);
            this.buttonScatter1k.Name = "buttonScatter1k";
            this.buttonScatter1k.Size = new System.Drawing.Size(75, 23);
            this.buttonScatter1k.TabIndex = 2;
            this.buttonScatter1k.Text = "1K scatter";
            this.buttonScatter1k.UseVisualStyleBackColor = true;
            this.buttonScatter1k.Click += new System.EventHandler(this.buttonScatter1k_Click);
            // 
            // buttonSignal1M
            // 
            this.buttonSignal1M.Location = new System.Drawing.Point(174, 12);
            this.buttonSignal1M.Name = "buttonSignal1M";
            this.buttonSignal1M.Size = new System.Drawing.Size(75, 23);
            this.buttonSignal1M.TabIndex = 3;
            this.buttonSignal1M.Text = "1M signal";
            this.buttonSignal1M.UseVisualStyleBackColor = true;
            this.buttonSignal1M.Click += new System.EventHandler(this.buttonSignal1M_Click);
            // 
            // cbRenderQueue
            // 
            this.cbRenderQueue.AutoSize = true;
            this.cbRenderQueue.Checked = true;
            this.cbRenderQueue.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbRenderQueue.Location = new System.Drawing.Point(255, 16);
            this.cbRenderQueue.Name = "cbRenderQueue";
            this.cbRenderQueue.Size = new System.Drawing.Size(112, 17);
            this.cbRenderQueue.TabIndex = 4;
            this.cbRenderQueue.Text = "UseRenderQueue";
            this.cbRenderQueue.UseVisualStyleBackColor = true;
            this.cbRenderQueue.CheckedChanged += new System.EventHandler(this.cbRenderQueue_CheckedChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.cbRenderQueue);
            this.Controls.Add(this.buttonSignal1M);
            this.Controls.Add(this.buttonScatter1k);
            this.Controls.Add(this.btnScatter10);
            this.Controls.Add(this.formsPlot1);
            this.Name = "Form1";
            this.Text = "ScottPlot Sandbox - WinForms (.NET Framework)";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ScottPlot.FormsPlot formsPlot1;
        private System.Windows.Forms.Button btnScatter10;
        private System.Windows.Forms.Button buttonScatter1k;
        private System.Windows.Forms.Button buttonSignal1M;
        private System.Windows.Forms.CheckBox cbRenderQueue;
    }
}

