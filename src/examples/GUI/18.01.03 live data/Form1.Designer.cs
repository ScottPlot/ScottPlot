namespace _18._01._03_live_data
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
            this.components = new System.ComponentModel.Container();
            this.scottPlotUC1 = new ScottPlot.ScottPlotUC();
            this.btn_fill = new System.Windows.Forms.Button();
            this.btn_extend = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btn_update = new System.Windows.Forms.Button();
            this.btn_auto_axis = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.btn_continuous = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // scottPlotUC1
            // 
            this.scottPlotUC1.Location = new System.Drawing.Point(12, 53);
            this.scottPlotUC1.Name = "scottPlotUC1";
            this.scottPlotUC1.Size = new System.Drawing.Size(717, 449);
            this.scottPlotUC1.TabIndex = 0;
            // 
            // btn_fill
            // 
            this.btn_fill.Location = new System.Drawing.Point(234, 13);
            this.btn_fill.Name = "btn_fill";
            this.btn_fill.Size = new System.Drawing.Size(75, 34);
            this.btn_fill.TabIndex = 1;
            this.btn_fill.Text = "init";
            this.btn_fill.UseVisualStyleBackColor = true;
            this.btn_fill.Click += new System.EventHandler(this.btn_fill_Click);
            // 
            // btn_extend
            // 
            this.btn_extend.Location = new System.Drawing.Point(315, 13);
            this.btn_extend.Name = "btn_extend";
            this.btn_extend.Size = new System.Drawing.Size(75, 34);
            this.btn_extend.TabIndex = 2;
            this.btn_extend.Text = "extend";
            this.btn_extend.UseVisualStyleBackColor = true;
            this.btn_extend.Click += new System.EventHandler(this.btn_extend_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(73, 17);
            this.label1.TabIndex = 3;
            this.label1.Text = "ScottPlot";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 30);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(183, 17);
            this.label2.TabIndex = 4;
            this.label2.Text = "18.01.03 experimental code";
            // 
            // btn_update
            // 
            this.btn_update.Location = new System.Drawing.Point(396, 13);
            this.btn_update.Name = "btn_update";
            this.btn_update.Size = new System.Drawing.Size(75, 34);
            this.btn_update.TabIndex = 5;
            this.btn_update.Text = "update";
            this.btn_update.UseVisualStyleBackColor = true;
            this.btn_update.Click += new System.EventHandler(this.btn_update_Click);
            // 
            // btn_auto_axis
            // 
            this.btn_auto_axis.Location = new System.Drawing.Point(477, 13);
            this.btn_auto_axis.Name = "btn_auto_axis";
            this.btn_auto_axis.Size = new System.Drawing.Size(95, 34);
            this.btn_auto_axis.TabIndex = 7;
            this.btn_auto_axis.Text = "auto axis";
            this.btn_auto_axis.UseVisualStyleBackColor = true;
            this.btn_auto_axis.Click += new System.EventHandler(this.btn_auto_axis_Click);
            // 
            // timer1
            // 
            this.timer1.Interval = 20;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // btn_continuous
            // 
            this.btn_continuous.Location = new System.Drawing.Point(578, 13);
            this.btn_continuous.Name = "btn_continuous";
            this.btn_continuous.Size = new System.Drawing.Size(98, 34);
            this.btn_continuous.TabIndex = 8;
            this.btn_continuous.Text = "continuous";
            this.btn_continuous.UseVisualStyleBackColor = true;
            this.btn_continuous.Click += new System.EventHandler(this.btn_continuous_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(741, 514);
            this.Controls.Add(this.btn_continuous);
            this.Controls.Add(this.btn_auto_axis);
            this.Controls.Add(this.btn_update);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btn_extend);
            this.Controls.Add(this.btn_fill);
            this.Controls.Add(this.scottPlotUC1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ScottPlot.ScottPlotUC scottPlotUC1;
        private System.Windows.Forms.Button btn_fill;
        private System.Windows.Forms.Button btn_extend;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btn_update;
        private System.Windows.Forms.Button btn_auto_axis;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button btn_continuous;
    }
}

