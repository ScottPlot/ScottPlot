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
            SuspendLayout();
            // 
            // formsPlot1
            // 
            formsPlot1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            formsPlot1.DisplayScale = 1.5F;
            formsPlot1.Location = new Point(17, 117);
            formsPlot1.Margin = new Padding(4, 5, 4, 5);
            formsPlot1.Name = "formsPlot1";
            formsPlot1.Size = new Size(1109, 613);
            formsPlot1.TabIndex = 0;
            // 
            // rbSignal
            // 
            rbSignal.AutoSize = true;
            rbSignal.Checked = true;
            rbSignal.Location = new Point(17, 20);
            rbSignal.Margin = new Padding(4, 5, 4, 5);
            rbSignal.Name = "rbSignal";
            rbSignal.Size = new Size(121, 29);
            rbSignal.TabIndex = 1;
            rbSignal.TabStop = true;
            rbSignal.Text = "Signal Plot";
            rbSignal.UseVisualStyleBackColor = true;
            // 
            // rbScatter
            // 
            rbScatter.AutoSize = true;
            rbScatter.Location = new Point(141, 20);
            rbScatter.Margin = new Padding(4, 5, 4, 5);
            rbScatter.Name = "rbScatter";
            rbScatter.Size = new Size(127, 29);
            rbScatter.TabIndex = 2;
            rbScatter.Text = "Scatter Plot";
            rbScatter.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI Light", 12F, FontStyle.Regular, GraphicsUnit.Point);
            label1.Location = new Point(17, 68);
            label1.Margin = new Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new Size(70, 32);
            label1.TabIndex = 4;
            label1.Text = "label1";
            // 
            // SignalPerformance
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1143, 750);
            Controls.Add(label1);
            Controls.Add(rbScatter);
            Controls.Add(rbSignal);
            Controls.Add(formsPlot1);
            Margin = new Padding(4, 5, 4, 5);
            Name = "SignalPerformance";
            Text = "ScottPlot 5 Performance Demo";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ScottPlot.WinForms.FormsPlot formsPlot1;
        private RadioButton rbSignal;
        private RadioButton rbScatter;
        private Label label1;
    }
}