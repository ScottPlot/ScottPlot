namespace user_control_settings
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
            this.cbMenu = new System.Windows.Forms.CheckBox();
            this.cbPan = new System.Windows.Forms.CheckBox();
            this.cbZoom = new System.Windows.Forms.CheckBox();
            this.formsPlot1 = new ScottPlot.FormsPlot();
            this.SuspendLayout();
            // 
            // cbMenu
            // 
            this.cbMenu.AutoSize = true;
            this.cbMenu.Checked = true;
            this.cbMenu.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbMenu.Location = new System.Drawing.Point(12, 12);
            this.cbMenu.Name = "cbMenu";
            this.cbMenu.Size = new System.Drawing.Size(135, 17);
            this.cbMenu.TabIndex = 1;
            this.cbMenu.Text = "default right-click menu";
            this.cbMenu.UseVisualStyleBackColor = true;
            this.cbMenu.CheckedChanged += new System.EventHandler(this.CbMenu_CheckedChanged);
            // 
            // cbPan
            // 
            this.cbPan.AutoSize = true;
            this.cbPan.Checked = true;
            this.cbPan.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbPan.Location = new System.Drawing.Point(153, 12);
            this.cbPan.Name = "cbPan";
            this.cbPan.Size = new System.Drawing.Size(99, 17);
            this.cbPan.TabIndex = 2;
            this.cbPan.Text = "enable panning";
            this.cbPan.UseVisualStyleBackColor = true;
            this.cbPan.CheckedChanged += new System.EventHandler(this.CbPan_CheckedChanged);
            // 
            // cbZoom
            // 
            this.cbZoom.AutoSize = true;
            this.cbZoom.Checked = true;
            this.cbZoom.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbZoom.Location = new System.Drawing.Point(258, 12);
            this.cbZoom.Name = "cbZoom";
            this.cbZoom.Size = new System.Drawing.Size(100, 17);
            this.cbZoom.TabIndex = 3;
            this.cbZoom.Text = "enable zooming";
            this.cbZoom.UseVisualStyleBackColor = true;
            this.cbZoom.CheckedChanged += new System.EventHandler(this.CbZoom_CheckedChanged);
            // 
            // formsPlot1
            // 
            this.formsPlot1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.formsPlot1.Location = new System.Drawing.Point(12, 35);
            this.formsPlot1.Name = "formsPlot1";
            this.formsPlot1.Size = new System.Drawing.Size(725, 345);
            this.formsPlot1.TabIndex = 0;
            this.formsPlot1.MouseClicked += new System.Windows.Forms.MouseEventHandler(this.FormsPlot1_MouseClicked_1);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(749, 392);
            this.Controls.Add(this.cbZoom);
            this.Controls.Add(this.cbPan);
            this.Controls.Add(this.cbMenu);
            this.Controls.Add(this.formsPlot1);
            this.Name = "Form1";
            this.Text = "User Control Settings Demo";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ScottPlot.FormsPlot formsPlot1;
        private System.Windows.Forms.CheckBox cbMenu;
        private System.Windows.Forms.CheckBox cbPan;
        private System.Windows.Forms.CheckBox cbZoom;
    }
}