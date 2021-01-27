
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.tbCursorCoord = new System.Windows.Forms.TextBox();
            this.tbCursorPixel = new System.Windows.Forms.TextBox();
            this.tbPointPixel = new System.Windows.Forms.TextBox();
            this.tbPointCoord = new System.Windows.Forms.TextBox();
            this.btnSimilar = new System.Windows.Forms.Button();
            this.btnDifferent = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // formsPlot1
            // 
            this.formsPlot1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.formsPlot1.BackColor = System.Drawing.Color.Transparent;
            this.formsPlot1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.formsPlot1.Location = new System.Drawing.Point(12, 123);
            this.formsPlot1.Name = "formsPlot1";
            this.formsPlot1.Size = new System.Drawing.Size(577, 358);
            this.formsPlot1.TabIndex = 0;
            this.formsPlot1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.FormsPlot1_MouseMove);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(22, 45);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(119, 20);
            this.label1.TabIndex = 1;
            this.label1.Text = "Cursor position:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(33, 81);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(108, 20);
            this.label2.TabIndex = 2;
            this.label2.Text = "Nearest point:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(147, 15);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(87, 20);
            this.label5.TabIndex = 5;
            this.label5.Text = "Coordinate";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(249, 15);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(41, 20);
            this.label6.TabIndex = 8;
            this.label6.Text = "Pixel";
            // 
            // tbCursorCoord
            // 
            this.tbCursorCoord.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbCursorCoord.ForeColor = System.Drawing.Color.Blue;
            this.tbCursorCoord.Location = new System.Drawing.Point(147, 46);
            this.tbCursorCoord.Name = "tbCursorCoord";
            this.tbCursorCoord.Size = new System.Drawing.Size(100, 23);
            this.tbCursorCoord.TabIndex = 9;
            this.tbCursorCoord.Text = "12, 123";
            // 
            // tbCursorPixel
            // 
            this.tbCursorPixel.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbCursorPixel.ForeColor = System.Drawing.Color.Blue;
            this.tbCursorPixel.Location = new System.Drawing.Point(253, 46);
            this.tbCursorPixel.Name = "tbCursorPixel";
            this.tbCursorPixel.Size = new System.Drawing.Size(100, 23);
            this.tbCursorPixel.TabIndex = 10;
            this.tbCursorPixel.Text = "12, 123";
            // 
            // tbPointPixel
            // 
            this.tbPointPixel.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbPointPixel.ForeColor = System.Drawing.Color.Red;
            this.tbPointPixel.Location = new System.Drawing.Point(253, 82);
            this.tbPointPixel.Name = "tbPointPixel";
            this.tbPointPixel.Size = new System.Drawing.Size(100, 23);
            this.tbPointPixel.TabIndex = 12;
            this.tbPointPixel.Text = "12, 123";
            // 
            // tbPointCoord
            // 
            this.tbPointCoord.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbPointCoord.ForeColor = System.Drawing.Color.Red;
            this.tbPointCoord.Location = new System.Drawing.Point(147, 82);
            this.tbPointCoord.Name = "tbPointCoord";
            this.tbPointCoord.Size = new System.Drawing.Size(100, 23);
            this.tbPointCoord.TabIndex = 11;
            this.tbPointCoord.Text = "12, 123";
            // 
            // btnSimilar
            // 
            this.btnSimilar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSimilar.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSimilar.Location = new System.Drawing.Point(432, 40);
            this.btnSimilar.Name = "btnSimilar";
            this.btnSimilar.Size = new System.Drawing.Size(157, 34);
            this.btnSimilar.TabIndex = 15;
            this.btnSimilar.Text = "similar scales";
            this.btnSimilar.UseVisualStyleBackColor = true;
            this.btnSimilar.Click += new System.EventHandler(this.btnSimilar_Click);
            // 
            // btnDifferent
            // 
            this.btnDifferent.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDifferent.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDifferent.Location = new System.Drawing.Point(432, 76);
            this.btnDifferent.Name = "btnDifferent";
            this.btnDifferent.Size = new System.Drawing.Size(157, 34);
            this.btnDifferent.TabIndex = 16;
            this.btnDifferent.Text = "different scales";
            this.btnDifferent.UseVisualStyleBackColor = true;
            this.btnDifferent.Click += new System.EventHandler(this.btnDifferent_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(376, 215);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(0, 20);
            this.label4.TabIndex = 17;
            // 
            // label7
            // 
            this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(428, 15);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(141, 20);
            this.label7.TabIndex = 18;
            this.label7.Text = "Plot new data with:";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(601, 493);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.btnDifferent);
            this.Controls.Add(this.btnSimilar);
            this.Controls.Add(this.tbPointPixel);
            this.Controls.Add(this.tbPointCoord);
            this.Controls.Add(this.tbCursorPixel);
            this.Controls.Add(this.tbCursorCoord);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.formsPlot1);
            this.Name = "Form1";
            this.Text = "ScottPlot Sandbox - WinForms (.NET Framework)";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ScottPlot.FormsPlot formsPlot1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox tbCursorCoord;
        private System.Windows.Forms.TextBox tbCursorPixel;
        private System.Windows.Forms.TextBox tbPointPixel;
        private System.Windows.Forms.TextBox tbPointCoord;
        private System.Windows.Forms.Button btnSimilar;
        private System.Windows.Forms.Button btnDifferent;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label7;
    }
}

