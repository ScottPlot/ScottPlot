using Eto.Forms;
using Eto.Drawing;

namespace ScottPlot.Demo.Eto.EtoFormsDemos
{
    partial class MouseTracker : Form
    {
        private void InitializeComponent()
        {
            this.label1 = new Label();
            this.label2 = new Label();
            this.XPixelLabel = new Label();
            this.label4 = new Label();
            this.label5 = new Label();
            this.YPixelLabel = new Label();
            this.XCoordinateLabel = new Label();
            this.YCoordinateLabel = new Label();
            this.formsPlot1 = new ScottPlot.Eto.PlotView();
            this.lblMouse = new Label();
            this.SuspendLayout();

            var layout = new DynamicLayout();
            layout.AddRow(
                new DynamicLayout(new Label() { Text = "*", TextColor = BackgroundColor }, label2, label1),
                new DynamicLayout(label4, XPixelLabel, YPixelLabel),
                new DynamicLayout(label5, XCoordinateLabel, YCoordinateLabel),
                new DynamicLayout(lblMouse)
                );

            layout = new DynamicLayout();
            layout.AddRow(new Label() { Text = "*", TextColor = BackgroundColor }, label4, label5, lblMouse);
            layout.AddRow(label2, XPixelLabel, XCoordinateLabel);
            layout.AddRow(label1, YPixelLabel, YCoordinateLabel);

            layout.SetPaddingRecursive(4);

            this.Content = new DynamicLayout(layout, formsPlot1);

            // 
            // label1
            // 
            //this.label1.AutoSize = true;
            this.label1.Font = Fonts.Sans(12);// new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            //this.label1.Location = new Point(6, 67);
            //this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            //this.label1.Name = "label1";
            this.label1.Size = new Size(20, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "Y";
            // 
            // label2
            // 
            //this.label2.AutoSize = true;
            this.label2.Font = Fonts.Sans(12);//new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            //this.label2.Location = new System.Drawing.Point(6, 39);
            //this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            //this.label2.Name = "label2";
            this.label2.Size = new Size(20, 20);
            this.label2.TabIndex = 1;
            this.label2.Text = "X";
            // 
            // XPixelLabel
            // 
            //this.XPixelLabel.AutoSize = true;
            this.XPixelLabel.Font = Fonts.Monospace(12);//new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            //this.XPixelLabel.Location = new System.Drawing.Point(43, 40);
            //this.XPixelLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            //this.XPixelLabel.Name = "XPixelLabel";
            this.XPixelLabel.Size = new Size(100, 19);
            this.XPixelLabel.TabIndex = 2;
            this.XPixelLabel.Text = "?";
            // 
            // label4
            // 
            //this.label4.AutoSize = true;
            this.label4.Font = Fonts.Sans(12);//new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            //this.label4.Location = new System.Drawing.Point(43, 10);
            //this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            //this.label4.Name = "label4";
            this.label4.Size = new Size(41, 20);
            this.label4.TabIndex = 3;
            this.label4.Text = "Pixel";
            // 
            // label5
            // 
            //this.label5.AutoSize = true;
            this.label5.Font = Fonts.Sans(12);//new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            //this.label5.Location = new System.Drawing.Point(128, 10);
            //this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            //this.label5.Name = "label5";
            this.label5.Size = new Size(87, 20);
            this.label5.TabIndex = 4;
            this.label5.Text = "Coordinate";
            // 
            // YPixelLabel
            // 
            //this.YPixelLabel.AutoSize = true;
            this.YPixelLabel.Font = Fonts.Monospace(12);//new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            //this.YPixelLabel.Location = new System.Drawing.Point(43, 67);
            //this.YPixelLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            //this.YPixelLabel.Name = "YPixelLabel";
            this.YPixelLabel.Size = new Size(100, 19);
            this.YPixelLabel.TabIndex = 5;
            this.YPixelLabel.Text = "?";
            // 
            // XCoordinateLabel
            // 
            //this.XCoordinateLabel.AutoSize = true;
            this.XCoordinateLabel.Font = Fonts.Monospace(12);//new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            //this.XCoordinateLabel.Location = new System.Drawing.Point(128, 40);
            //this.XCoordinateLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            //this.XCoordinateLabel.Name = "XCoordinateLabel";
            this.XCoordinateLabel.Size = new Size(120, 19);
            this.XCoordinateLabel.TabIndex = 6;
            this.XCoordinateLabel.Text = "?";
            // 
            // YCoordinateLabel
            // 
            //this.YCoordinateLabel.AutoSize = true;
            this.YCoordinateLabel.Font = Fonts.Monospace(12);//new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            //this.YCoordinateLabel.Location = new System.Drawing.Point(128, 67);
            //this.YCoordinateLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            //this.YCoordinateLabel.Name = "YCoordinateLabel";
            this.YCoordinateLabel.Size = new Size(120, 19);
            this.YCoordinateLabel.TabIndex = 7;
            this.YCoordinateLabel.Text = "?";
            // 
            // formsPlot1
            // 
            //this.formsPlot1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            //| System.Windows.Forms.AnchorStyles.Left) 
            //| System.Windows.Forms.AnchorStyles.Right)));
            this.formsPlot1.BackgroundColor = Colors.Transparent;
            //this.formsPlot1.Location = new System.Drawing.Point(14, 102);
            //this.formsPlot1.Margin = new System.Windows.Forms.Padding(5);
            //this.formsPlot1.Name = "formsPlot1";
            //this.formsPlot1.Size = new Size(708, 290);
            this.formsPlot1.TabIndex = 8;
            // 
            // lblMouse
            // 
            //this.lblMouse.AutoSize = true;
            this.lblMouse.Font = Fonts.Sans(12);//new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            //this.lblMouse.Location = new System.Drawing.Point(269, 10);
            //this.lblMouse.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            //this.lblMouse.Name = "lblMouse";
            this.lblMouse.Size = new Size(149, 20);
            this.lblMouse.TabIndex = 9;
            this.lblMouse.Text = "Waiting for mouse...";
            // 
            // MouseTracker
            // 
            //this.AutoScaleDimensions = new SizeF(7F, 15F);
            //this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new Size(736, 405);
            //this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            //this.Name = "MouseTracker";
            this.Title = "MouseTracker";
            this.ResumeLayout();
        }

        private Label label1;
        private Label label2;
        private Label XPixelLabel;
        private Label label4;
        private Label label5;
        private Label YPixelLabel;
        private Label XCoordinateLabel;
        private Label YCoordinateLabel;
        private ScottPlot.Eto.PlotView formsPlot1;
        private Label lblMouse;
    }
}
