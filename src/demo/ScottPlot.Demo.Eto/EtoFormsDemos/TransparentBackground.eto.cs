using Eto.Forms;
using Eto.Drawing;

namespace ScottPlot.Demo.Eto.EtoFormsDemos
{
    partial class TransparentBackground : Form
    {
        private void InitializeComponent()
        {
            this.button1 = new Button();
            this.button2 = new Button();
            this.button3 = new Button();
            this.formsPlot1 = new ScottPlot.Eto.PlotView();
            this.button4 = new Button();
            this.button5 = new Button();
            this.button6 = new Button();
            this.SuspendLayout();

            var layout = new DynamicLayout() { Padding = 5 };
            layout.AddSeparateRow(spacing: Size.Empty + 5, controls: new[] { button1, button2, button3, button4, button5, button6, null });
            layout.AddSeparateRow(formsPlot1);

            this.Content = layout;

            // 
            // button1
            // 
            //this.button1.Location = new System.Drawing.Point(12, 12);
            //this.button1.Name = "button1";
            this.button1.Size = new Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "red";
            //this.button1.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            //this.button2.Location = new System.Drawing.Point(93, 12);
            //this.button2.Name = "button2";
            this.button2.Size = new Size(75, 23);
            this.button2.TabIndex = 1;
            this.button2.Text = "green";
            //this.button2.UseVisualStyleBackColor = true;
            // 
            // button3
            // 
            //this.button3.Location = new System.Drawing.Point(174, 12);
            //this.button3.Name = "button3";
            this.button3.Size = new Size(75, 23);
            this.button3.TabIndex = 2;
            this.button3.Text = "blue";
            //this.button3.UseVisualStyleBackColor = true;
            // 
            // formsPlot1
            // 
            //this.formsPlot1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            //| System.Windows.Forms.AnchorStyles.Left) 
            //| System.Windows.Forms.AnchorStyles.Right)));
            //this.formsPlot1.Location = new System.Drawing.Point(12, 41);
            //this.formsPlot1.Name = "formsPlot1";
            //this.formsPlot1.Size = new Size(599, 275);
            this.formsPlot1.TabIndex = 3;
            // 
            // button4
            // 
            //this.button4.Location = new System.Drawing.Point(255, 12);
            //this.button4.Name = "button4";
            this.button4.Size = new Size(75, 23);
            this.button4.TabIndex = 4;
            this.button4.Text = "white";
            //this.button4.UseVisualStyleBackColor = true;
            // 
            // button5
            // 
            //this.button5.Location = new System.Drawing.Point(336, 12);
            //this.button5.Name = "button5";
            this.button5.Size = new Size(75, 23);
            this.button5.TabIndex = 5;
            this.button5.Text = "control";
            //this.button5.UseVisualStyleBackColor = true;
            // 
            // button6
            // 
            //this.button6.Location = new System.Drawing.Point(417, 12);
            //this.button6.Name = "button6";
            this.button6.Size = new Size(75, 23);
            this.button6.TabIndex = 6;
            this.button6.Text = "image";
            //this.button6.UseVisualStyleBackColor = true;
            // 
            // TransparentBackground
            // 
            //this.AutoScaleDimensions = new SizeF(6F, 13F);
            //this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new Size(623, 328);
            //this.Name = "TransparentBackground";
            this.Title = "Transparent Background";
            this.ResumeLayout();
        }

        private Button button1;
        private Button button2;
        private Button button3;
        private ScottPlot.Eto.PlotView formsPlot1;
        private Button button4;
        private Button button5;
        private Button button6;
    }
}
