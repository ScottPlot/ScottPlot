namespace ScottPlot.Demo.WinForms.WinFormsDemos
{
    partial class LiveDataUpdate
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
            this.label1 = new System.Windows.Forms.Label();
            this.formsPlot1 = new ScottPlot.FormsPlot();
            this.timerUpdateData = new System.Windows.Forms.Timer(this.components);
            this.timerRender = new System.Windows.Forms.Timer(this.components);
            this.runCheckbox = new System.Windows.Forms.CheckBox();
            this.rollCheckbox = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(328, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "This example uses a fixed-size array and updates its values with time";
            // 
            // formsPlot1
            // 
            this.formsPlot1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.formsPlot1.Location = new System.Drawing.Point(12, 31);
            this.formsPlot1.Name = "formsPlot1";
            this.formsPlot1.Size = new System.Drawing.Size(776, 407);
            this.formsPlot1.TabIndex = 1;
            // 
            // timerUpdateData
            // 
            this.timerUpdateData.Enabled = true;
            this.timerUpdateData.Interval = 5;
            this.timerUpdateData.Tick += new System.EventHandler(this.timerUpdateData_Tick);
            // 
            // timerRender
            // 
            this.timerRender.Enabled = true;
            this.timerRender.Interval = 20;
            this.timerRender.Tick += new System.EventHandler(this.timerRender_Tick);
            // 
            // runCheckbox
            // 
            this.runCheckbox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.runCheckbox.AutoSize = true;
            this.runCheckbox.Checked = true;
            this.runCheckbox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.runCheckbox.Location = new System.Drawing.Point(742, 8);
            this.runCheckbox.Name = "runCheckbox";
            this.runCheckbox.Size = new System.Drawing.Size(46, 17);
            this.runCheckbox.TabIndex = 2;
            this.runCheckbox.Text = "Run";
            this.runCheckbox.UseVisualStyleBackColor = true;
            this.runCheckbox.CheckedChanged += new System.EventHandler(this.runCheckbox_CheckedChanged);
            // 
            // rollCheckbox
            // 
            this.rollCheckbox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.rollCheckbox.AutoSize = true;
            this.rollCheckbox.Location = new System.Drawing.Point(692, 8);
            this.rollCheckbox.Name = "rollCheckbox";
            this.rollCheckbox.Size = new System.Drawing.Size(44, 17);
            this.rollCheckbox.TabIndex = 3;
            this.rollCheckbox.Text = "Roll";
            this.rollCheckbox.UseVisualStyleBackColor = true;
            this.rollCheckbox.CheckedChanged += new System.EventHandler(this.rollCheckbox_CheckedChanged);
            // 
            // LiveDataUpdate
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.rollCheckbox);
            this.Controls.Add(this.runCheckbox);
            this.Controls.Add(this.formsPlot1);
            this.Controls.Add(this.label1);
            this.Name = "LiveDataUpdate";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Live Data (fixed array)";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private FormsPlot formsPlot1;
        private System.Windows.Forms.Timer timerUpdateData;
        private System.Windows.Forms.Timer timerRender;
        private System.Windows.Forms.CheckBox runCheckbox;
        private System.Windows.Forms.CheckBox rollCheckbox;
    }
}