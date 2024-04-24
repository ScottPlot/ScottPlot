namespace WinForms_Demo.Demos
{
    partial class SignalPerformance
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
            formsPlot1 = new ScottPlot.WinForms.FormsPlot();
            rbSignal = new RadioButton();
            rbScatter = new RadioButton();
            label1 = new Label();
            rbFastSignal = new RadioButton();
            nUDCachePeriod = new NumericUpDown();
            cachePeriodApplyBtn = new Button();
            labelCachePeriod = new Label();
            ((System.ComponentModel.ISupportInitialize)nUDCachePeriod).BeginInit();
            SuspendLayout();
            // 
            // formsPlot1
            // 
            formsPlot1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            formsPlot1.DisplayScale = 1.5F;
            formsPlot1.Location = new Point(12, 70);
            formsPlot1.Name = "formsPlot1";
            formsPlot1.Size = new Size(776, 368);
            formsPlot1.TabIndex = 0;
            // 
            // rbSignal
            // 
            rbSignal.AutoSize = true;
            rbSignal.Checked = true;
            rbSignal.Location = new Point(12, 12);
            rbSignal.Name = "rbSignal";
            rbSignal.Size = new Size(83, 19);
            rbSignal.TabIndex = 1;
            rbSignal.TabStop = true;
            rbSignal.Text = "Signal Plot";
            rbSignal.UseVisualStyleBackColor = true;
            // 
            // rbScatter
            // 
            rbScatter.AutoSize = true;
            rbScatter.Location = new Point(99, 12);
            rbScatter.Name = "rbScatter";
            rbScatter.Size = new Size(87, 19);
            rbScatter.TabIndex = 2;
            rbScatter.Text = "Scatter Plot";
            rbScatter.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI Light", 12F, FontStyle.Regular, GraphicsUnit.Point);
            label1.Location = new Point(12, 41);
            label1.Name = "label1";
            label1.Size = new Size(47, 21);
            label1.TabIndex = 4;
            label1.Text = "label1";
            // 
            // rbFastSignal
            // 
            rbFastSignal.AutoSize = true;
            rbFastSignal.Location = new Point(192, 12);
            rbFastSignal.Name = "rbFastSignal";
            rbFastSignal.Size = new Size(104, 19);
            rbFastSignal.TabIndex = 5;
            rbFastSignal.Text = "FastSignal Plot";
            rbFastSignal.UseVisualStyleBackColor = true;
            // 
            // nUDCachePeriod
            // 
            nUDCachePeriod.Location = new Point(587, 12);
            nUDCachePeriod.Maximum = new decimal(new int[] { 10000000, 0, 0, 0 });
            nUDCachePeriod.Name = "nUDCachePeriod";
            nUDCachePeriod.Size = new Size(120, 23);
            nUDCachePeriod.TabIndex = 6;
            nUDCachePeriod.Value = new decimal(new int[] { 100, 0, 0, 0 });
            nUDCachePeriod.Visible = false;
            // 
            // cachePeriodApplyBtn
            // 
            cachePeriodApplyBtn.Location = new Point(713, 12);
            cachePeriodApplyBtn.Name = "cachePeriodApplyBtn";
            cachePeriodApplyBtn.Size = new Size(75, 23);
            cachePeriodApplyBtn.TabIndex = 7;
            cachePeriodApplyBtn.Text = "Apply";
            cachePeriodApplyBtn.UseVisualStyleBackColor = true;
            cachePeriodApplyBtn.Visible = false;
            // 
            // labelCachePeriod
            // 
            labelCachePeriod.AutoSize = true;
            labelCachePeriod.Font = new Font("맑은 고딕", 10F, FontStyle.Regular, GraphicsUnit.Point);
            labelCachePeriod.Location = new Point(482, 13);
            labelCachePeriod.Name = "labelCachePeriod";
            labelCachePeriod.Size = new Size(99, 19);
            labelCachePeriod.TabIndex = 8;
            labelCachePeriod.Text = "Cache Period: ";
            labelCachePeriod.Visible = false;
            // 
            // SignalPerformance
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(labelCachePeriod);
            Controls.Add(cachePeriodApplyBtn);
            Controls.Add(nUDCachePeriod);
            Controls.Add(rbFastSignal);
            Controls.Add(label1);
            Controls.Add(rbScatter);
            Controls.Add(rbSignal);
            Controls.Add(formsPlot1);
            Name = "SignalPerformance";
            Text = "ScottPlot 5 Performance Demo";
            ((System.ComponentModel.ISupportInitialize)nUDCachePeriod).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ScottPlot.WinForms.FormsPlot formsPlot1;
        private RadioButton rbSignal;
        private RadioButton rbScatter;
        private Label label1;
        private RadioButton rbFastSignal;
        private NumericUpDown nUDCachePeriod;
        private Button cachePeriodApplyBtn;
        private Label labelCachePeriod;
    }
}