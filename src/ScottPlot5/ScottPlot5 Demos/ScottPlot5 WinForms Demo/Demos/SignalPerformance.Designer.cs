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
            rbSignalConst = new RadioButton();
            cbPointCount = new ComboBox();
            label1 = new Label();
            SuspendLayout();
            // 
            // formsPlot1
            // 
            formsPlot1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            formsPlot1.DisplayScale = 1.5F;
            formsPlot1.Location = new Point(12, 37);
            formsPlot1.Name = "formsPlot1";
            formsPlot1.Size = new Size(776, 401);
            formsPlot1.TabIndex = 0;
            // 
            // rbSignal
            // 
            rbSignal.AutoSize = true;
            rbSignal.Checked = true;
            rbSignal.Location = new Point(103, 12);
            rbSignal.Name = "rbSignal";
            rbSignal.Size = new Size(81, 19);
            rbSignal.TabIndex = 1;
            rbSignal.TabStop = true;
            rbSignal.Text = "Signal Plot";
            rbSignal.UseVisualStyleBackColor = true;
            // 
            // rbScatter
            // 
            rbScatter.AutoSize = true;
            rbScatter.Location = new Point(12, 12);
            rbScatter.Name = "rbScatter";
            rbScatter.Size = new Size(85, 19);
            rbScatter.TabIndex = 2;
            rbScatter.Text = "Scatter Plot";
            rbScatter.UseVisualStyleBackColor = true;
            // 
            // rbSignalConst
            // 
            rbSignalConst.AutoSize = true;
            rbSignalConst.Location = new Point(190, 12);
            rbSignalConst.Name = "rbSignalConst";
            rbSignalConst.Size = new Size(88, 19);
            rbSignalConst.TabIndex = 5;
            rbSignalConst.Text = "SignalConst";
            rbSignalConst.UseVisualStyleBackColor = true;
            // 
            // comboBox1
            // 
            cbPointCount.FormattingEnabled = true;
            cbPointCount.Location = new Point(453, 11);
            cbPointCount.Name = "comboBox1";
            cbPointCount.Size = new Size(121, 23);
            cbPointCount.TabIndex = 6;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(343, 14);
            label1.Name = "label1";
            label1.Size = new Size(104, 15);
            label1.TabIndex = 7;
            label1.Text = "Number of points:";
            // 
            // SignalPerformance
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(label1);
            Controls.Add(cbPointCount);
            Controls.Add(rbSignalConst);
            Controls.Add(rbScatter);
            Controls.Add(rbSignal);
            Controls.Add(formsPlot1);
            Name = "SignalPerformance";
            Text = "ScottPlot 5 Performance Demo";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ScottPlot.WinForms.FormsPlot formsPlot1;
        private RadioButton rbSignal;
        private RadioButton rbScatter;
        private RadioButton rbSignalConst;
        private ComboBox cbPointCount;
        private Label label1;
    }
}