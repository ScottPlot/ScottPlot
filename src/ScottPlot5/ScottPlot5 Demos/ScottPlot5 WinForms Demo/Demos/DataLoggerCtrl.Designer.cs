namespace WinForms_Demo.Demos
{
    partial class DataLoggerCtrl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            formsPlot = new ScottPlot.WinForms.FormsPlot();
            btnSlide = new Button();
            btnJump = new Button();
            btnFull = new Button();
            cbInvertedX = new CheckBox();
            cbInvertedY = new CheckBox();
            lblInverted = new Label();
            flpControls = new FlowLayoutPanel();
            tbStatus = new TextBox();
            flpControls.SuspendLayout();
            SuspendLayout();
            // 
            // formsPlot
            // 
            formsPlot.DisplayScale = 1F;
            formsPlot.Dock = DockStyle.Fill;
            formsPlot.Location = new Point(0, 44);
            formsPlot.Name = "formsPlot";
            formsPlot.Size = new Size(701, 434);
            formsPlot.TabIndex = 6;
            formsPlot.MouseMove += formsPlot_MouseMove;
            // 
            // btnSlide
            // 
            btnSlide.Location = new Point(200, 3);
            btnSlide.Name = "btnSlide";
            btnSlide.Size = new Size(88, 34);
            btnSlide.TabIndex = 2;
            btnSlide.Text = "Slide";
            btnSlide.UseVisualStyleBackColor = true;
            btnSlide.Click += btnSlide_Click;
            // 
            // btnJump
            // 
            btnJump.Location = new Point(106, 3);
            btnJump.Name = "btnJump";
            btnJump.Size = new Size(88, 34);
            btnJump.TabIndex = 1;
            btnJump.Text = "Jump";
            btnJump.UseVisualStyleBackColor = true;
            btnJump.Click += btnJump_Click;
            // 
            // btnFull
            // 
            btnFull.Location = new Point(12, 3);
            btnFull.Name = "btnFull";
            btnFull.Size = new Size(88, 34);
            btnFull.TabIndex = 0;
            btnFull.Text = "Full";
            btnFull.UseVisualStyleBackColor = true;
            btnFull.Click += btnFull_Click;
            // 
            // cbInvertedX
            // 
            cbInvertedX.Anchor = AnchorStyles.Left;
            cbInvertedX.AutoSize = true;
            cbInvertedX.Location = new Point(353, 9);
            cbInvertedX.Name = "cbInvertedX";
            cbInvertedX.Padding = new Padding(0, 3, 0, 0);
            cbInvertedX.Size = new Size(33, 22);
            cbInvertedX.TabIndex = 4;
            cbInvertedX.Text = "X";
            cbInvertedX.UseVisualStyleBackColor = true;
            cbInvertedX.CheckedChanged += cbInverted_CheckedChanged;
            // 
            // cbInvertedY
            // 
            cbInvertedY.Anchor = AnchorStyles.Left;
            cbInvertedY.AutoSize = true;
            cbInvertedY.Location = new Point(392, 9);
            cbInvertedY.Name = "cbInvertedY";
            cbInvertedY.Padding = new Padding(0, 3, 0, 0);
            cbInvertedY.Size = new Size(33, 22);
            cbInvertedY.TabIndex = 5;
            cbInvertedY.Text = "Y";
            cbInvertedY.UseVisualStyleBackColor = true;
            cbInvertedY.CheckedChanged += cbInverted_CheckedChanged;
            // 
            // lblInverted
            // 
            lblInverted.Anchor = AnchorStyles.Left;
            lblInverted.AutoSize = true;
            lblInverted.Location = new Point(294, 12);
            lblInverted.Name = "lblInverted";
            lblInverted.Size = new Size(53, 15);
            lblInverted.TabIndex = 3;
            lblInverted.Text = "Inverted:";
            // 
            // flpControls
            // 
            flpControls.Controls.Add(btnFull);
            flpControls.Controls.Add(btnJump);
            flpControls.Controls.Add(btnSlide);
            flpControls.Controls.Add(lblInverted);
            flpControls.Controls.Add(cbInvertedX);
            flpControls.Controls.Add(cbInvertedY);
            flpControls.Dock = DockStyle.Top;
            flpControls.Location = new Point(0, 0);
            flpControls.Name = "flpControls";
            flpControls.Padding = new Padding(9, 0, 0, 0);
            flpControls.Size = new Size(701, 44);
            flpControls.TabIndex = 18;
            // 
            // tbStatus
            // 
            tbStatus.Dock = DockStyle.Bottom;
            tbStatus.Location = new Point(0, 478);
            tbStatus.Name = "tbStatus";
            tbStatus.Size = new Size(701, 23);
            tbStatus.TabIndex = 19;
            // 
            // DataLoggerCtrl
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(formsPlot);
            Controls.Add(flpControls);
            Controls.Add(tbStatus);
            Name = "DataLoggerCtrl";
            Size = new Size(701, 501);
            flpControls.ResumeLayout(false);
            flpControls.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ScottPlot.WinForms.FormsPlot formsPlot;
        private Button btnSlide;
        private Button btnJump;
        private Button btnFull;
        private CheckBox cbInvertedX;
        private CheckBox cbInvertedY;
        private Label lblInverted;
        private FlowLayoutPanel flpControls;
        private TextBox tbStatus;
    }
}
