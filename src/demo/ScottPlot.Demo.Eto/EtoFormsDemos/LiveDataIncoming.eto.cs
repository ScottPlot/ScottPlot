using Eto.Forms;
using Eto.Drawing;

namespace ScottPlot.Demo.Eto.EtoFormsDemos
{
    partial class LiveDataIncoming : Form
    {
        private void InitializeComponent()
        {
            this.formsPlot1 = new ScottPlot.Eto.PlotView();
            this.label1 = new Label();
            this.label2 = new Label();
            this.tbLatestValue = new TextBox();
            this.label3 = new Label();
            this.tbLastValue = new TextBox();
            this.dataTimer = new UITimer();
            this.renderTimer = new UITimer();
            this.cbAutoAxis = new CheckBox();
            this.SuspendLayout();

            var layout = new DynamicLayout() { Padding = 5 };
            layout.AddSeparateRow(label1);
            var layout2 = new StackLayout(label2, tbLatestValue, label3, tbLastValue, cbAutoAxis)
            {
                Spacing = 10,
                Orientation = global::Eto.Forms.Orientation.Horizontal
            };
            layout.AddSeparateRow(layout2);
            layout.AddSeparateRow(formsPlot1);

            this.Content = layout;

            // 
            // formsPlot1
            // 
            //this.formsPlot1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            //| System.Windows.Forms.AnchorStyles.Left) 
            //| System.Windows.Forms.AnchorStyles.Right)));
            //this.formsPlot1.Location = new System.Drawing.Point(12, 58);
            //this.formsPlot1.Name = "formsPlot1";
            this.formsPlot1.Size = new Size(-1, 380);
            this.formsPlot1.TabIndex = 0;
            // 
            // label1
            // 
            //this.label1.AutoSize = true;
            //this.label1.Location = new System.Drawing.Point(9, 9);
            //this.label1.Name = "label1";
            //this.label1.Size = new Size(268, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "This example simulates live display of a growing dataset";
            // 
            // label2
            // 
            //this.label2.AutoSize = true;
            //this.label2.Location = new System.Drawing.Point(9, 33);
            //this.label2.Name = "label2";
            //this.label2.Size = new Size(80, -1);
            this.label2.TabIndex = 2;
            this.label2.Text = "readings: ";
            // 
            // tbLatestValue
            // 
            this.tbLatestValue.Font = Fonts.Monospace(10);// new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            //this.tbLatestValue.Location = new System.Drawing.Point(68, 29);
            //this.tbLatestValue.Name = "tbLatestValue";
            this.tbLatestValue.ReadOnly = true;
            //this.tbLatestValue.Size = new Size(61, 23);
            this.tbLatestValue.TabIndex = 3;
            this.tbLatestValue.Text = "123";
            // 
            // label3
            // 
            //this.label3.AutoSize = true;
            //this.label3.Location = new System.Drawing.Point(163, 33);
            //this.label3.Name = "label3";
            //this.label3.Size = new Size(80, -1);
            this.label3.TabIndex = 4;
            this.label3.Text = "latest value:";
            // 
            // tbLastValue
            // 
            this.tbLastValue.Font = Fonts.Monospace(10);//new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            //this.tbLastValue.Location = new System.Drawing.Point(233, 29);
            //this.tbLastValue.Name = "tbLastValue";
            this.tbLastValue.ReadOnly = true;
            //this.tbLastValue.Size = new Size(74, 23);
            this.tbLastValue.TabIndex = 5;
            this.tbLastValue.Text = "+123.4";
            // 
            // dataTimer
            // 
            //this.dataTimer.Enabled = true;
            // 
            // renderTimer
            // 
            //this.renderTimer.Enabled = true;
            // 
            // cbAutoAxis
            // 
            //this.cbAutoAxis.AutoSize = true;
            this.cbAutoAxis.Checked = true;
            //this.cbAutoAxis.CheckState = System.Windows.Forms.CheckState.Checked;
            //this.cbAutoAxis.Location = new System.Drawing.Point(342, 32);
            //this.cbAutoAxis.Name = "cbAutoAxis";
            //this.cbAutoAxis.Size = new Size(146, 17);
            this.cbAutoAxis.TabIndex = 6;
            this.cbAutoAxis.Text = "auto-axis on each update";
            //this.cbAutoAxis.UseVisualStyleBackColor = true;
            // 
            // LiveDataIncoming
            // 
            //this.AutoScaleDimensions = new SizeF(6F, 13F);
            //this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new Size(800, -1);
            //this.Name = "LiveDataIncoming";
            //this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Title = "Live Data (growing)";
            this.ResumeLayout();
        }

        private ScottPlot.Eto.PlotView formsPlot1;
        private Label label1;
        private Label label2;
        private TextBox tbLatestValue;
        private Label label3;
        private TextBox tbLastValue;
        private UITimer dataTimer;
        private UITimer renderTimer;
        private CheckBox cbAutoAxis;
    }
}
