namespace ScottPlot.Demo.WinForms.WinFormsDemos
{
    partial class MouseTracker
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.XPixelLabel = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.YPixelLabel = new System.Windows.Forms.Label();
            this.XCoordinateLabel = new System.Windows.Forms.Label();
            this.YCoordinateLabel = new System.Windows.Forms.Label();
            this.formsPlot1 = new ScottPlot.FormsPlot();
            this.lblMouse = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label1.Location = new System.Drawing.Point(6, 67);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(20, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "Y";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label2.Location = new System.Drawing.Point(6, 39);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(20, 20);
            this.label2.TabIndex = 1;
            this.label2.Text = "X";
            // 
            // XPixelLabel
            // 
            this.XPixelLabel.AutoSize = true;
            this.XPixelLabel.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.XPixelLabel.Location = new System.Drawing.Point(43, 40);
            this.XPixelLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.XPixelLabel.Name = "XPixelLabel";
            this.XPixelLabel.Size = new System.Drawing.Size(18, 19);
            this.XPixelLabel.TabIndex = 2;
            this.XPixelLabel.Text = "?";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label4.Location = new System.Drawing.Point(43, 10);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(41, 20);
            this.label4.TabIndex = 3;
            this.label4.Text = "Pixel";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label5.Location = new System.Drawing.Point(128, 10);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(87, 20);
            this.label5.TabIndex = 4;
            this.label5.Text = "Coordinate";
            // 
            // YPixelLabel
            // 
            this.YPixelLabel.AutoSize = true;
            this.YPixelLabel.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.YPixelLabel.Location = new System.Drawing.Point(43, 67);
            this.YPixelLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.YPixelLabel.Name = "YPixelLabel";
            this.YPixelLabel.Size = new System.Drawing.Size(18, 19);
            this.YPixelLabel.TabIndex = 5;
            this.YPixelLabel.Text = "?";
            // 
            // XCoordinateLabel
            // 
            this.XCoordinateLabel.AutoSize = true;
            this.XCoordinateLabel.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.XCoordinateLabel.Location = new System.Drawing.Point(128, 40);
            this.XCoordinateLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.XCoordinateLabel.Name = "XCoordinateLabel";
            this.XCoordinateLabel.Size = new System.Drawing.Size(18, 19);
            this.XCoordinateLabel.TabIndex = 6;
            this.XCoordinateLabel.Text = "?";
            // 
            // YCoordinateLabel
            // 
            this.YCoordinateLabel.AutoSize = true;
            this.YCoordinateLabel.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.YCoordinateLabel.Location = new System.Drawing.Point(128, 67);
            this.YCoordinateLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.YCoordinateLabel.Name = "YCoordinateLabel";
            this.YCoordinateLabel.Size = new System.Drawing.Size(18, 19);
            this.YCoordinateLabel.TabIndex = 7;
            this.YCoordinateLabel.Text = "?";
            // 
            // formsPlot1
            // 
            this.formsPlot1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.formsPlot1.BackColor = System.Drawing.Color.Transparent;
            this.formsPlot1.Location = new System.Drawing.Point(14, 102);
            this.formsPlot1.Margin = new System.Windows.Forms.Padding(5);
            this.formsPlot1.Name = "formsPlot1";
            this.formsPlot1.Size = new System.Drawing.Size(708, 290);
            this.formsPlot1.TabIndex = 8;
            this.formsPlot1.MouseEnter += new System.EventHandler(this.formsPlot1_MouseEnter);
            this.formsPlot1.MouseLeave += new System.EventHandler(this.formsPlot1_MouseLeave);
            this.formsPlot1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.formsPlot1_MouseMoved_1);
            // 
            // lblMouse
            // 
            this.lblMouse.AutoSize = true;
            this.lblMouse.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblMouse.Location = new System.Drawing.Point(269, 10);
            this.lblMouse.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblMouse.Name = "lblMouse";
            this.lblMouse.Size = new System.Drawing.Size(149, 20);
            this.lblMouse.TabIndex = 9;
            this.lblMouse.Text = "Waiting for mouse...";
            // 
            // MouseTracker
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(736, 405);
            this.Controls.Add(this.lblMouse);
            this.Controls.Add(this.formsPlot1);
            this.Controls.Add(this.YCoordinateLabel);
            this.Controls.Add(this.XCoordinateLabel);
            this.Controls.Add(this.YPixelLabel);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.XPixelLabel);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "MouseTracker";
            this.Text = "MouseTracker";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label XPixelLabel;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label YPixelLabel;
        private System.Windows.Forms.Label XCoordinateLabel;
        private System.Windows.Forms.Label YCoordinateLabel;
        private FormsPlot formsPlot1;
        private System.Windows.Forms.Label lblMouse;
    }
}